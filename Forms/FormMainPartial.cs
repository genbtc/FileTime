using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using genBTC.FileTime.Classes;
using genBTC.FileTime.Classes.Native;
using genBTC.FileTime.Models;
using genBTC.FileTime.mViewModels;
using genBTC.FileTime.Properties;
using Timer = System.Windows.Forms.Timer;

namespace genBTC.FileTime
{
    #region FORM_MAIN2
    public partial class Form_Main
    {
        //native call to do string compare like the OS
        [DllImport("shlwapi.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
        public static extern int StrCmpLogicalW(String x, String y);

        #region Vars & Dupe Helper Methods

        private static readonly char Seperator = Path.DirectorySeparatorChar;
        private static readonly string UserDesktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

        private readonly Timer itemSelectionChangedTimer = new Timer();
        public static IComparer<string> explorerStringComparer()
        {
            return new ExplorerComparerstringHelper();
        }
        /// <summary>  Reasons to be invisible  </summary>
        private static FileAttributes SyncSettingstoInvisibleFlag()
        {
            FileAttributes reasonsToBeInvisible = (Settings.Default.ShowHidden ? 0 : FileAttributes.Hidden) |
                                                  (Settings.Default.ShowSystem ? 0 : FileAttributes.System) |
                                                  (Settings.Default.ShowReadOnly ? 0 : FileAttributes.ReadOnly);
            return reasonsToBeInvisible;
        }

        /// <summary> Return 1 if bool=true (Directory) otherwise 0=false (File) </summary>
        private static int Bool2Int(bool fileOrDir)
        {
            return fileOrDir ? 1 : 0;
        }

        #endregion Vars

        /// <summary>
        /// Display subfiles and subdirectories in the right panel listview
        /// </summary>
        /// <param name="filesonly">Don't show the directories, only files.</param>
        /// reqs: listviewcontents, contentsDirList,contentsFileList, labelFpathText, checkbox_recurse, filextlist, imageList_Files
        private static void DisplayContentsList(DataModel dataModel, bool checkboxRecurse, string labelname, bool filesonly = true)
        {
            //Clear the contents UI + datamodel
            dataModel.Clear();
            
            string directoryName = labelname;

            if (!Directory.Exists(directoryName))
                return;

            if (!filesonly && checkboxRecurse)
            {
                //part 1: list and store all the subdirectories
                try
                {
                    DataModel.PopulateDirList(directoryName, dataModel);
                }
                catch (UnauthorizedAccessException)
                { }
                //Sort them
                dataModel.contentsDirList.Sort(explorerStringComparer());
                //Add them to the listview.
                foreach (string subDirectory in dataModel.contentsDirList)
                {
                    // Display all the sub directories using the directory icon (enum 1)
                    dataModel.listViewContents.Items.Add(subDirectory, (int)ListViewIcon.Directory);
                }
            }

            // (Display all of the files and show a file icon)
            try
            {
                //part 2: list all subfiles, match the extension and find the icon.
                foreach (string file in Directory.GetFiles(directoryName))
                {
                    var fileAttribs = File.GetAttributes(file);
                    if ((fileAttribs & SyncSettingstoInvisibleFlag()) != 0)
                        continue; //skip the rest if its supposed to be "invisible" based on the mask
                    var justName = Path.GetFileName(file);
                    SharedHelper.CurrExten = Path.GetExtension(file);
                    if ((SharedHelper.CurrExten != ".lnk")) //if its not a shortcut
                    {
                        //if not already in the list, then add it
                        if (dataModel.filextlist.FindLastIndex(SharedHelper.FindCurExt) == -1)
                        {
                            dataModel.filextlist.Add(SharedHelper.CurrExten);
                            //call NativeExtractIcon to get the filesystem icon of the filename
                            dataModel.imageListFiles.Images.Add(SharedHelper.CurrExten, NativeExtractIcon.GetIcon(file, true));
                        }
                    }
                    else //if it is a shortcut, grab icon directly.
                        dataModel.imageListFiles.Images.Add(justName, NativeExtractIcon.GetIcon(file, true));

                    dataModel.contentsFileList.Add(justName);
                }
            }
            catch (UnauthorizedAccessException)
            { }
            //Sort them
            dataModel.contentsFileList.Sort(explorerStringComparer());
            //Add them to the listview.
            foreach (string file in dataModel.contentsFileList)
            {
                string exten = Path.GetExtension(file);
                dataModel.listViewContents.Items.Add(file, exten != ".lnk" ? exten : file);
            }
        }

        /// <summary>
        /// Mode1: Process One directory, with recursive sub-directory support. Calls SetFileDateTime()
        /// (Only adds to the confirm list, Form 2 will actually write changes).
        /// </summary>
        /// <param name="directoryPath">Full path to the directory</param>
        /// <param name="checkedRecurse"></param>
        /// <param name="checkedShouldFiles"></param>
        /// <param name="checkboxes"></param>
        /// <param name="dataModel"></param>
        /// req, checkBox_Recurse.Checked, checkBoxShouldFiles.Checked
        private static void RecurseSubDirectoryMode1(string directoryPath, bool checkedRecurse, bool checkedShouldFiles, BoolCMA checkboxes, DataModel dataModel, guistatus gui)
        {
            DateTime? nullorfileDateTime = DecideWhichTimeMode1(directoryPath, gui, dataModel);
            if (nullorfileDateTime == null)
                return; //if nothing could be decided, exit. otherwise continue
            var fileDateTime = (DateTime)nullorfileDateTime;

            // Set the date/time for each sub directory but only if "Recurse Sub-Directories" is checkboxed.
            if (checkedRecurse)
                SetTimeDateEachDirectory(directoryPath, fileDateTime, checkboxes, dataModel, checkedRecurse, checkedShouldFiles, gui);
            // Set the date/time for each file, but only if "Perform operation on files" is checkboxed.
            if (checkedShouldFiles)
                SetTimeDateEachFile(directoryPath, fileDateTime, checkboxes, dataModel);
        }

        /// <summary>  Mode 1B </summary>
        private static void RecurseSubDirectoryMode1B(string directoryPath, bool checkedRecurse, bool checkedShouldFiles, BoolCMA checkboxes, DataModel dataModel, guistatus gui)
        {
            try
            {
                foreach (string subfolder in Directory.GetDirectories(directoryPath))
                    RecurseSubDirectoryMode1(Path.Combine(directoryPath, subfolder), checkedRecurse, checkedShouldFiles, checkboxes, dataModel, gui);
            }
            catch (UnauthorizedAccessException)
            { }
            catch (DirectoryNotFoundException)
            { }
        }

        /// <summary>
        /// Returns a DateTime after examining the radiobuttons/checkboxes to specify the logic behavior.
        /// </summary>
        /// reqs: (string path), contentsDirList, contentsFileList, Thi
        /// This is a viewmodel thing sorta. Grab the viewmodel as data first, then act, then update viewmodel.
        /// This viewmodel needs to contain: radioGroupBox1_SpecifyTime.Che
        /// radioGroupBox1_SpecifyTime.Checked
        /// radioGroupBox2_CurrentSelectionTime.Checked
        /// radioGroupBox3_UseTimeFrom.Checked
        ///     =radioButton1_useTimefromFile.Checked
        ///     =radioButton2_useTimefromSubdir.Checked
        private static DateTime? DecideWhichTimeMode1(string path, guistatus gui, DataModel dataModel)
        {
            dataModel.contentsDirList.Clear();
            dataModel.contentsFileList.Clear();

            var dateToUse = new DateTime?();
            if (gui.radioGroupBox1SpecifyTime)
            {
                dateToUse = DateTime.Parse(gui.dateTimePickerDate.Date.ToString("d") + " " +
                                           gui.dateTimePickerTime.Hour + ":" +
                                           gui.dateTimePickerTime.Minute + ":" +
                                           gui.dateTimePickerTime.Second);
            }
            else if (gui.radioGroupBox2CurrentSelect)
            {
                if (gui.rg2rb1Creation)
                    dateToUse = DateTime.Parse(gui.Created);
                else if (gui.rg2rb2Modified)
                    dateToUse = DateTime.Parse(gui.Modified);
                else if (gui.rg2rb3LastAccess)
                    dateToUse = DateTime.Parse(gui.Accessed);
            }
            else if (gui.radioGroupBox3UseTimeFrom)
            {
                dateToUse = DecideTimeFromSubDirFile(path, dataModel, gui);
            }
            return dateToUse;
        }

        /// <summary>
        /// Set Directory Time
        /// </summary>
        private static void SetTimeDateEachDirectory(string directoryPath, DateTime fileDateTime, BoolCMA checkboxes, DataModel dataModel, bool checkedRecurse, bool checkedShouldFiles, guistatus gui)
        {
            try
            {
                string[] subDirectories = Directory.GetDirectories(directoryPath);
                Array.Sort(subDirectories, explorerStringComparer());
                foreach (string eachdir in subDirectories)
                {
                    // Set the date/time for the sub directory
                    SetFileDateTimeMode1(dataModel, checkboxes, eachdir, fileDateTime, true);
                    // Recurse (loop) through each sub-sub directory
                    RecurseSubDirectoryMode1(eachdir, checkedRecurse, checkedShouldFiles, checkboxes, dataModel, gui);
                }
            } //catch for GetDirs
            catch (UnauthorizedAccessException)
            { }
        }

        /// <summary>
        /// Set File Time
        /// </summary>
        private static void SetTimeDateEachFile(string directoryPath, DateTime fileDateTime, BoolCMA checkboxes, DataModel dataModel)
        {
            try
            {
                string[] subFiles = Directory.GetFiles(directoryPath);
                Array.Sort(subFiles, explorerStringComparer());
                foreach (string filename in subFiles)
                    SetFileDateTimeMode1(dataModel, checkboxes, filename, fileDateTime, false);
            } //catch for GetFiles
            catch (UnauthorizedAccessException)
            { }
        }


        /// <summary>
        /// Mode 2. Recursive.
        /// </summary>
        /// <param name="directoryPath">Path to start in.</param>
        /// <param name="dataModel"></param>
        /// <param name="checkboxes"></param>
        private static void RecurseSubDirectoryMode2(string directoryPath, DataModel dataModel, BoolCMA checkboxes, guistatus gui)
        {
            //Actually does important stuff.
            NameDateObject timeInside = DecideWhichTimeMode2(dataModel, directoryPath, gui);

            SkipOrAddFile(dataModel, directoryPath, true);
            NameDateObject subFile = Makedateobject(checkboxes, directoryPath, timeInside);
            dataModel.FilestoConfirmList.Add(subFile);
            //.
            try
            {
                foreach (string subfolder in Directory.GetDirectories(directoryPath))
                    RecurseSubDirectoryMode2(Path.Combine(directoryPath, subfolder), dataModel, checkboxes, gui);
            }
            catch (UnauthorizedAccessException)
            { }
            catch (DirectoryNotFoundException)
            { }
        }

        // Just points back to the above function, uses the parent instead.
        private static void RecurseSubDirectoryMode2B(string directoryPath, DataModel dataModel, BoolCMA checkboxes, guistatus gui)
        {
            try
            {
                foreach (string subfolder in Directory.GetDirectories(directoryPath))
                    RecurseSubDirectoryMode2(Path.Combine(directoryPath, subfolder), dataModel, checkboxes, gui);
            }
            catch (UnauthorizedAccessException)
            { }
            catch (DirectoryNotFoundException)
            { }
        }


        private static void SkipOrAddFile(DataModel dataModel, string path, bool isDirectory)
        {
            FileAttributes fAttr = File.GetAttributes(path);

            if (((fAttr & FileAttributes.System) == FileAttributes.System) && Settings.Default.SkipSystem)
            {
                dataModel.Skips.S++;
                return; // Skip system files and directories
            }
            if (((fAttr & FileAttributes.Hidden) == FileAttributes.Hidden) && Settings.Default.SkipHidden)
            {
                dataModel.Skips.H++;
                return; // Skip hidden files and directories
            }
            if (((fAttr & FileAttributes.ReadOnly) == FileAttributes.ReadOnly) && !isDirectory)
            {
                if (Settings.Default.SkipReadOnly)
                {
                    dataModel.Skips.R++;
                    return;
                }
                if (Settings.Default.ShowNoticesReadOnly)
                {
                    DialogResult dr =
                        MessageBox.Show(
                            "The file '" + path + "' is Read-Only.\n\nContinue showing Read-Only notifications?",
                            "Read-Only", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    Settings.Default.ShowNoticesReadOnly = (dr == DialogResult.Yes);
                }
                //add them to the list to be readOnly+Fixed before they are timewritten.
                dataModel.FilesReadOnlytoFix.Add(path);
            }
        }

        /// <summary> STATIC.
        /// Set the date/time for a single file/directory (This works on files and directories)
        /// Go through the list, skipping H,S,R files, and add all the file+date objects to the Confirmation List
        /// (Only adds to the confirm list, Form 2 (Confirm) will actually write changes).
        /// </summary>
        /// Requires: Pass in the datamodel and the checkbox states.
        /// <param name="filePath">Full path to the file/directory</param>
        /// <param name="fileTime">Date/Time to set the file/directory</param>
        /// <param name="isDirectory">Is this a directory???</param>
        private static void SetFileDateTimeMode1(DataModel dataModel, BoolCMA checkboxes, string filePath, DateTime fileTime, bool isDirectory)
        {
            SkipOrAddFile(dataModel, filePath, isDirectory);

            NameDateObject currentobject = Makedateobject(checkboxes, filePath, fileTime, isDirectory);
            dataModel.FilestoConfirmList.Add(currentobject);
        }

        /// <summary>
        /// Static. Overloaded. Make a NameDateObject out of 1 filename; writes A SINGLE time to all 3 date properties.
        /// </summary>
        private static NameDateObject Makedateobject(BoolCMA checkboxes, string filePath, DateTime fileTime, bool isDirectory)
        {
            var currentobject = new NameDateObject { Name = filePath, FileOrDirType = Bool2Int(isDirectory) };

            // Set the Creation date/time if selected
            if (checkboxes.C)
                currentobject.Created = fileTime;
            // Set the Modified date/time is selected
            if (checkboxes.M)
                currentobject.Modified = fileTime;
            // Set the last access time if selected
            if (checkboxes.A)
                currentobject.Accessed = fileTime;
            return currentobject;
        }
        /// <summary>
        /// Static. Overloaded. Make a NameDateObject out of 1 filename; writes each time time to the date attribute that was radiobutton selected.
        /// </summary>
        private static NameDateObject Makedateobject(BoolCMA checkboxes, string folderPath, NameDateObject subObject)
        {
            var currentobject = new NameDateObjectListViewVm(subObject) { Name = folderPath, FileOrDirType = 1 };

            //If Checkbox is selected:
            if (!checkboxes.C)
                currentobject.Created = "N/A"; // Set the Creation date/time if selected
            if (!checkboxes.M)
                currentobject.Modified = "N/A"; // Set the Modified date/time if selected
            if (!checkboxes.A)
                currentobject.Accessed = "N/A"; // Set the Last Access date/time if selected
            return new NameDateObject(currentobject.Converter());
        }

        /// <summary>
        /// Display the date and time of the selected file (also works on Directories)
        /// </summary>
        private static DisplayCmaTimeData GetCmaTimes(string pathName)
        {
            var cma = new DisplayCmaTimeData { PathName = pathName };
            if (cma.PathName != "")
            {
                cma.Created = File.GetCreationTime(cma.PathName).ToString();
                cma.Modified = File.GetLastWriteTime(cma.PathName).ToString();
                cma.Accessed = File.GetLastAccessTime(cma.PathName).ToString();
                cma.HiddenPathName = cma.PathName;
                cma.Selected = true;
            }
            else
            {
                // Maybe no file/directory is selected
                // Then Blank out the display of date/time.
                cma.Created = "";
                cma.Modified = "";
                cma.Accessed = "";
                cma.HiddenPathName = "";
                cma.Selected = false;
            }
            return cma;
        }

        /// <summary>
        /// Static. Check all the subdirs or subfiles. And decide the time. Called from: DecideWhichTimeMode1() { radioGroupBox3_UseTimeFrom
        /// </summary>
        /// <param name="path"></param>
        /// <param name="dataModel"></param>
        /// <param name="radioButton1UseTimefromFile"></param>
        /// <param name="radioButton2UseTimefromSubdir"></param>
        /// <param name="radioButton1SetfromCreated"></param>
        /// <param name="radioButton2SetfromModified"></param>
        /// <param name="radioButton3SetfromAccessed"></param>
        /// <param name="radioButton4SetfromRandom"></param>
        /// <param name="radioButton1Oldest"></param>
        /// <param name="radioButton2Newest"></param>
        /// <param name="radioButton3Random"></param>
        /// <returns></returns>
        private static DateTime? DecideTimeFromSubDirFile(string path, DataModel dataModel, guistatus gui)
        {
            var dateToUse = new DateTime?();
            var extractlist = new List<string>();
            if (gui.radioButton1_useTimefromFile)
            {
                extractlist = DataModel.PopulateFileList(path, dataModel);
            }
            else if (gui.radioButton2_useTimefromSubdir)
            {
                extractlist = DataModel.PopulateDirList(path, dataModel);
            }

            // if the list is blank due to no files actually existing then we have nothing to do, so stop here.
            if (extractlist.Count == 0)
                return null;
            //for Any/Random attribute mode, decide which attribute and stick with it.
            int randomNumber = dataModel.random.Next(0, 3);
            var timelist = new List<DateTime?>();
            //start iterating through
            foreach (string subitem in extractlist)
            {
                string timelisttype; //this was made just in case.
                var looptempDate = new DateTime();
                try
                {
                    string fullpath = Path.Combine(path, subitem);
                    if (gui.radioButton1_setfromCreated)
                    {
                        timelisttype = "Created";
                        looptempDate = File.GetCreationTime(fullpath);
                    }
                    else if (gui.radioButton2_setfromModified)
                    {
                        timelisttype = "Modified";
                        looptempDate = File.GetLastWriteTime(fullpath);
                    }
                    else if (gui.radioButton3_setfromAccessed)
                    {
                        timelisttype = "Accessed";
                        looptempDate = File.GetLastAccessTime(fullpath);
                    }
                    else if (gui.radioButton4_setfromRandom)
                    {
                        switch (randomNumber)
                        {
                            case 0:
                                timelisttype = "Created";
                                looptempDate = File.GetCreationTime(fullpath);
                                break;

                            case 1:
                                timelisttype = "Modified";
                                looptempDate = File.GetLastWriteTime(fullpath);
                                break;

                            case 2:
                                timelisttype = "Accessed";
                                looptempDate = File.GetLastAccessTime(fullpath);
                                break;
                        }
                    }
                    timelist.Add(looptempDate);
                }
                catch (UnauthorizedAccessException)
                { }
            }
            var minmax = new OldNewDate(timelist);
            if (gui.radioButton1Oldest)
            {
                if (minmax.MinDate != null)
                    dateToUse = minmax.MinDate; //explicit typecast from nullable
            }
            else if (gui.radioButton2Newest)
            {
                if (minmax.MaxDate != null)
                    dateToUse = minmax.MaxDate;
            }
            else if (gui.radioButton3Random)
            {
                int randomFile = dataModel.random.Next(0, minmax.Index);
                if (timelist[randomFile] != null)
                    dateToUse = timelist[randomFile];
            }
            return dateToUse;
        }




        /// <summary>
        /// Very long function that does a simple task. Read in the options the user set for the operation, and
        /// Decide on the timestamp it should use, by the end we will have a single object with 3 times.
        /// This will need to be hit with broad strokes if we attempt to do any more work on the program.
        /// </summary>
        /// <param name="dataModel"></param>
        /// <param name="radioButton3Random"></param>
        /// <param name="radioButton2Newest"></param>
        /// <param name="radioButton1Oldest"></param>
        /// <param name="radioButton2UseTimefromSubdir"></param>
        /// <param name="radioButton1UseTimefromFile"></param>
        /// <param name="radioGroupBox3UseTimeFrom"></param>
        /// <param name="labelLastAccess"></param>
        /// <param name="labelModified"></param>
        /// <param name="labelCreationTime"></param>
        /// <param name="radioGroupBox2CurrentSelectionTime"></param>
        /// <param name="dateTimePickerTime"></param>
        /// <param name="dateTimePickerDate"></param>
        /// <param name="labelHiddenPathName"></param>
        /// <param name="radioGroupBox1SpecifyTime"></param>
        /// <param name="directoryPath"></param>
        /// <returns></returns>
        // radioButton3Random, radioButton2Newest, radioButton1Oldest, radioButton2_useTimefromSubdir, radioButton1_useTimefromFile, radioGroupBox3_UseTimeFrom, label_AccessedTime, label_ModificationTime, label_CreationTime, radioGroupBox2_CurrentSelectionTime, dateTimePickerTime, dateTimePickerDate, labelHidden_PathName, radioGroupBox1_SpecifyTime,
        // RadioButton radioButton3Random, RadioButton radioButton2Newest, RadioButton radioButton1Oldest, RadioButton radioButton2UseTimefromSubdir, RadioButton radioButton1UseTimefromFile, RadioGroupBox radioGroupBox3UseTimeFrom, Label labelLastAccess, Label labelModified, Label labelCreationTime, RadioGroupBox radioGroupBox2CurrentSelectionTime, DateTimePicker dateTimePickerTime, DateTimePicker dateTimePickerDate, Label labelHiddenPathName, RadioGroupBox radioGroupBox1SpecifyTime,
        private static NameDateObject DecideWhichTimeMode2(DataModel dataModel, string directoryPath, guistatus gui)
        {
            var extractlist = new List<string>();

            var timelist = new List<NameDateObject>();
            var thingtoreturn = new NameDateObject();

            if (gui.radioGroupBox1SpecifyTime)
            {
                thingtoreturn.Name = gui.labelHiddenPathName;
                var specifiedDate = DateTime.Parse(gui.dateTimePickerDate.Date.ToString("d") + " " +
                                                   gui.dateTimePickerTime.Hour + ":" +
                                                   gui.dateTimePickerTime.Minute + ":" +
                                                   gui.dateTimePickerTime.Second);
                thingtoreturn.Created = specifiedDate;
                thingtoreturn.Modified = specifiedDate;
                thingtoreturn.Accessed = specifiedDate;
            }
            else if (gui.radioGroupBox2CurrentSelect)
            {
                thingtoreturn.Name = gui.labelHiddenPathName;
                thingtoreturn.Created = DateTime.Parse(gui.Created);
                thingtoreturn.Modified = DateTime.Parse(gui.Modified);
                thingtoreturn.Accessed = DateTime.Parse(gui.Accessed);
            }
            //Begin checking Conditional for which file is newest oldest etc
            else if (gui.radioGroupBox3UseTimeFrom)
            {
                //decide if they wanted to use time from subfile or subdir
                if (gui.radioButton1_useTimefromFile)
                {
                    try
                    {
                        // Get List of the subfiles (full path)
                        foreach (string subFile in Directory.GetFiles(directoryPath))
                        {
                            var fileAttribs = File.GetAttributes(subFile);
                            if ((fileAttribs & SyncSettingstoInvisibleFlag()) != 0)
                                continue;
                            var fullPathtoFileName = Path.Combine(directoryPath, subFile);
                            extractlist.Add(fullPathtoFileName);
                        }
                    } // catch failure of GetAttributes
                    catch (FileNotFoundException ex)
                    {
                        MessageBox.Show(
                            "Error getting attributes of a file in '" + directoryPath + "': \r\n\r\n" + ex.Message,
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    } // catch failure of GetFiles
                    catch (UnauthorizedAccessException)
                    { } //show nothing because this is normal for this method when encountering inaccessible files
                }
                else if (gui.radioButton2_useTimefromSubdir)
                {
                    try
                    {
                        // Get List of the subdirs (full path)
                        foreach (string subDirectory in Directory.GetDirectories(directoryPath))
                        {
                            var directoryName = Path.Combine(directoryPath, subDirectory);
                            extractlist.Add(directoryName);
                        }
                    } // catch failure from GetDirs
                    catch (UnauthorizedAccessException)
                    { } //show nothing because this is normal for this method when encountering inaccessible dirs
                }

                // ## Exit out early:
                // if the list is empty, theres nothing to do, then return an empty object.?????
                if (extractlist.Count == 0)
                    return thingtoreturn;

                // ---------------------------------------------------------------------------------//
                // ## MID WAY POINT ##
                // We have our first list and now we apply it to the 3 other CMA lists per all the files
                // And where we actually decide whether to keep our time or use the new time.
                // ---------------------------------------------------------------------------------//

                foreach (string fullpath in extractlist)
                {
                    var decidetemp = new NameDateObject { Name = fullpath };
                    //grab all 3 times and put them in a decidetemp object
                    try
                    {
                        decidetemp.Created = File.GetCreationTime(fullpath); // File Class works on dirs also
                        decidetemp.Modified = File.GetLastWriteTime(fullpath);
                        decidetemp.Accessed = File.GetLastAccessTime(fullpath);
                        //add the temp object to the list of objects
                        timelist.Add(decidetemp);
                    }
                    catch (UnauthorizedAccessException)
                    { }
                }
                //make 3 new lists, one for each date containing every NameDateObject
                var creationtimelist = new List<DateTime?>();
                var modtimelist = new List<DateTime?>();
                var accesstimelist = new List<DateTime?>();
                //populate the new seperated lists with the times from the combinedobject list (timelist)
                foreach (NameDateObject timeobject in timelist)
                {
                    creationtimelist.Add((DateTime?)timeobject.Created);
                    modtimelist.Add((DateTime?)timeobject.Modified);
                    accesstimelist.Add((DateTime?)timeobject.Accessed);
                }
                //Make a new list of the lists we just made (Collection initializer)
                var threetimelists = new List<List<DateTime?>> { creationtimelist, modtimelist, accesstimelist };
                //Instantiate 3 new vars as a new class that processes the min and max date from the 3 lists we just made
                var cre = new OldNewDate(creationtimelist);
                var mod = new OldNewDate(modtimelist);
                var acc = new OldNewDate(accesstimelist);

                ////Create 2 lists(min and max), containing the 3 min/max dates.
                //DateTime?[] minarray = { cre.minDate, mod.minDate, acc.minDate };
                //DateTime?[] maxarray = { cre.maxDate, mod.maxDate, acc.maxDate };
                ////Instantiate themin/themax as the new class that calculates the min and max date from the 3 dates above.
                //var themin = new OldNewDate(new List<DateTime?>(minarray));
                //var themax = new OldNewDate(new List<DateTime?>(maxarray));
                ////Keep track of the min/max indexes in this 1,2,3 format too.
                //int[] minindexesarray = { cre.minIndex, mod.minIndex, acc.minIndex };
                //int[] maxindexesarray = { cre.maxIndex, mod.maxIndex, acc.maxIndex };

                string filenameused = "";
                //var dateToUse = new DateTime?();

                //Decide which to use.

                if (gui.radioButton1Oldest)
                {
                    //mode a: set ALL attributes to the oldest date of whichever attribute was oldest.
                    //                    dateToUse = (DateTime?)minarray[themin.minIndex];
                    //                    filenameused = timelist[minindexesarray[themin.minIndex]].Name;
                    //                    thingtoreturn.Created = dateToUse;
                    //                    thingtoreturn.Modified= dateToUse;
                    //                    thingtoreturn.Accessed = dateToUse;

                    //mode b: the more desirable mode:
                    //set each attribute to OLDest date from EACH attribute
                    thingtoreturn.Name = "Mode 2: Three Different Filenames"; // note to self.
                    thingtoreturn.Created = cre.MinDate;
                    thingtoreturn.Modified = mod.MinDate;
                    thingtoreturn.Accessed = acc.MinDate;
                }
                //the above comments obviously can apply to newer mode also with a small edit.
                else if (gui.radioButton2Newest)
                {
                    //set each attribute to NEWest date from EACH attribute
                    thingtoreturn.Name = "Mode 2: Three Different Filenames"; // note to self.
                    thingtoreturn.Created = cre.MaxDate;
                    thingtoreturn.Modified = mod.MaxDate;
                    thingtoreturn.Accessed = acc.MaxDate;
                }
                else if (gui.radioButton3Random)
                {
                    //Mode A: (old) - removed the following 4 radio buttons
                    //pick a subfile/dir at random, then pick an attribute(C,M,A) at random
                    //    int randomindex = random.Next(0, timelist.Count);
                    //    filenameused = timelist[randomindex].Name;

                    //    if (radioButton1_setfromCreation.Checked)
                    //        thingtoreturn.Created = (DateTime?)threetimelists[0][randomindex];
                    //    if (radioButton2_setfromModified.Checked)
                    //        thingtoreturn.Modified = (DateTime?)threetimelists[1][randomindex];
                    //    if (radioButton3_setfromAccessed.Checked)
                    //        thingtoreturn.Accessed = (DateTime?)threetimelists[2][randomindex];
                    //    if (radioButton4_setfromRandom.Checked)
                    //    {
                    //        int cmarandomize = random.Next(0, 3);
                    //        dateToUse = (DateTime?)threetimelists[cmarandomize][randomindex];

                    //        if (cmarandomize == 0)
                    //            thingtoreturn.Created = dateToUse;
                    //        else if (cmarandomize == 1)
                    //            thingtoreturn.Modified = dateToUse;
                    //        else if (cmarandomize == 2)
                    //            thingtoreturn.Accessed = dateToUse;
                    //    }
                    //Mode B: (current)
                    //Pick a subfile/dir at random, copy all 3 attributes from it, to the return object.
                    int randomindex = dataModel.random.Next(0, timelist.Count);
                    filenameused = timelist[randomindex].Name;
                    thingtoreturn.Created = threetimelists[0][randomindex];
                    thingtoreturn.Modified = threetimelists[1][randomindex];
                    thingtoreturn.Accessed = threetimelists[2][randomindex];
                }
                //Set the thingtoReturn Name to what we just determined.
                thingtoreturn.Name = filenameused;
            }
            return thingtoreturn;
        }
        /// <summary>
        /// Display the Folder Browser Dialog and then display the selected
        /// file path and the directories and files in the folder.
        /// </summary>
        private static string OpenFile(string path)
        {
            //Feed in a path to start in or use current path as dialog path:
            if (path == null)
                return null;
            //start a new filebrowser dialog thread.
            var t = new Thread(() =>
            {
                var openFile = new FolderBrowserDialog
                {
                    ShowNewFolderButton = false,
                    SelectedPath = path,
                    Description = "Select the folder you want to view/change the subfolders of:"
                };

                //openFile.RootFolder = System.Environment.SpecialFolder.MyComputer;
                //openFile.ShowNewFolderButton = true;
                if (openFile.ShowDialog() == DialogResult.Cancel)
                    return;
                //re-use the path variable to return what was selected
                path = openFile.SelectedPath;
            });
            //TODO: check if Code is needed for when running as MultiThreaded App. [MTAThread]
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            t.Join();
            return path;
        }
    }
    #endregion FORM_MAIN2
}