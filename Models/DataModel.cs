using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using genBTC.FileTime.Classes;
using genBTC.FileTime.Classes.Native;
using genBTC.FileTime.mViewModels;
using genBTC.FileTime.Properties;

namespace genBTC.FileTime.Models
{
    internal class DataModel
    {
        #region Vars
        /// <summary> Main Lists for filenames. Dir , File, Ext </summary>
        internal List<string> contentsDirList, contentsFileList, filextlist;
        /// <summary> Pass a resetList of files that had the read-only attribute fixed, so Form2 can display it </summary>
        internal List<string> FilesReadOnlytoFix;
        /// <summary> List of Class to be passed to Form 2 for confirmation of files</summary>
        internal List<NameDateObj> FilestoConfirmList;

        internal ListView listViewContents;

        internal ImageList imageListFiles;

        internal SkippedHSR Skips;

        internal Random random;
        internal string CwdPathName;

        internal BoolCMA checkboxes;
        internal guistatus gui;
        #endregion Vars
        
        /// <summary>  Constructor  </summary>
        internal DataModel()
        {
            contentsDirList = new List<string>();
            contentsFileList = new List<string>();
            filextlist = new List<string>();
            FilesReadOnlytoFix = new List<string>();
            FilestoConfirmList = new List<NameDateObj>();
            Skips = new SkippedHSR();
            random = new Random();
            CwdPathName = "";
        }

        /// <summary> Clear contents Dir + File lists </summary>
        internal void Clear(bool resetList)
        {
            contentsDirList.Clear();
            contentsFileList.Clear();
            if (resetList)
                listViewContents.Items.Clear();
        }

        internal List<string> PopulateFileList(string path)
        {
            foreach (string filename in Directory.GetFiles(path))
                contentsFileList.Add(Path.GetFileName(filename));
            return contentsFileList;
        }

        internal List<string> PopulateDirList(string path)
        {
            foreach (string subDirectory in Directory.GetDirectories(path))
                contentsDirList.Add(Path.GetFileName(subDirectory));
            return contentsDirList;
        }

        /// <summary>
        /// Display subfiles and subdirectories in the right panel listview  and show a file icon
        /// </summary>
        /// <param name="filesonly">Don't show the directories, only files.</param>
        internal void DisplayContentsList(bool checkboxRecurse, string labelname, bool filesonly = true)
        {
            //Clear the datamodel + contents UI
            Clear(true);

            string directoryName = labelname;

            if (!Directory.Exists(directoryName))
                return;

            //Directories:
            if (!filesonly && checkboxRecurse)
            {
                try
                {
                    PopulateDirList(directoryName);
                }
                catch (UnauthorizedAccessException)
                { }
                //Sort them
                contentsDirList.Sort(SharedHelper.explorerStringComparer());
                //Add them to the listview.
                foreach (string subDirectory in contentsDirList)
                {
                    // Display all the sub directories using the directory icon (enum 1)
                    listViewContents.Items.Add(subDirectory, (int)ListViewIcon.Directory);
                }
            }

            //Files: (Display all of the files and show a file icon)
            try
            {
                AddImagesExtsToFileLists(directoryName);
            }
            catch (UnauthorizedAccessException)
            { }
            //Sort them
            contentsFileList.Sort(SharedHelper.explorerStringComparer());
            //Add them to the listview.
            foreach (string file in contentsFileList)
            {
                string exten = Path.GetExtension(file);
                listViewContents.Items.Add(file, exten != ".lnk" ? exten : file);
            }
        }


        /// <summary> Fill the contents list window with files's own Icons, given a directory name path </summary>
        private void AddImagesExtsToFileLists(string directoryName)
        {
            contentsFileList.Clear();
            //part 2: resetList all subfiles, match the extension and find the icon.
            foreach (string file in Directory.GetFiles(directoryName))
            {
                var fileAttribs = File.GetAttributes(file);
                if ((fileAttribs & SharedHelper.SyncSettingstoInvisibleFlag()) != 0)
                    continue; //skip the rest if its supposed to be "invisible" based on the mask
                var justName = Path.GetFileName(file);
                SharedHelper.CurrExten = Path.GetExtension(file);
                if ((SharedHelper.CurrExten != ".lnk")) //if its not a shortcut
                {
                    //if not already in the resetList, then add it
                    if (filextlist.FindLastIndex(SharedHelper.FindCurExt) == -1)
                    {
                        filextlist.Add(SharedHelper.CurrExten);
                        //call NativeExtractIcon to get the filesystem icon of the filename
                        imageListFiles.Images.Add(SharedHelper.CurrExten, NativeExtractIcon.GetIcon(file, true));
                    }
                }
                else //if it is a shortcut, grab icon directly.
                    imageListFiles.Images.Add(justName, NativeExtractIcon.GetIcon(file, true));

                contentsFileList.Add(justName);
            }
        }

        /// <summary>
        /// Returns a DateTime after examining the radiobuttons/checkboxes to specify the logic behavior.
        /// </summary>
        /// reqs: (string path), contentsDirList, contentsFileList, guistatus
        internal DateTime? DecideWhichTimeMode1(string path)
        {
            //Clear the datamodel only
            Clear(false);

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
                dateToUse = DecideTimeFromSubDirFile(path);
            }
            return dateToUse;
        }

        /// <summary>
        /// Check all the subdirs or subfiles. And decide the time. Called from above: DecideWhichTimeMode1() { radioGroupBox3_UseTimeFrom
        /// </summary>
        internal DateTime? DecideTimeFromSubDirFile(string path)
        {
            var dateToUse = new DateTime?();
            var extractlist = new List<string>();
            if (gui.radioButton1_useTimefromFile)
            {
                extractlist = PopulateFileList(path);
            }
            else if (gui.radioButton2_useTimefromSubdir)
            {
                extractlist = PopulateDirList(path);
            }

            // if the resetList is blank due to no files actually existing then we have nothing to do, so stop here.
            if (extractlist.Count == 0)
                return null;
            //for Any/Random attribute mode, decide which attribute and stick with it.
            int randomNumber = random.Next(0, 3);
            var timelist = new List<DateTime?>();
            //start iterating through
            foreach (string subitem in extractlist)
            {
                var looptempDate = new DateTime();
                try
                {
                    string fullpath = Path.Combine(path, subitem);
                    string timelisttype = null; //this was made just in case.
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
                    var stopbotheringme = timelisttype; //remove "unused var" warning
                }
                catch (UnauthorizedAccessException)
                { }
            }
            var minmax = new DateNewOldObj(timelist);
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
                int randomFile = random.Next(0, minmax.Index);
                if (timelist[randomFile] != null)
                    dateToUse = timelist[randomFile];
            }
            return dateToUse;
        }
         
        /// <summary>
        /// Mode 1: Process One directory, with recursive sub-directory support. Calls SetFileDateTime()
        /// (Only adds to the confirm list, Form 2 will actually write changes).
        /// </summary>
        /// <param name="directoryPath">Full path to the directory</param>
        /// req, checkBox_Recurse.Checked, checkBoxShouldFiles.Checked
        internal void RecurseSubDirectoryMode1(string directoryPath)
        {
            DateTime? nullorfileDateTime = DecideWhichTimeMode1(directoryPath);
            if (nullorfileDateTime == null)
                return; //if nothing could be decided, exit. otherwise continue
            var fileDateTime = (DateTime)nullorfileDateTime;

            // Set the date/time for each sub directory but only if "Recurse Sub-Directories" is checkboxed.
            if (gui.checkboxRecurse)
                SetTimeDateEachDirectory(directoryPath, fileDateTime);
            // Set the date/time for each file, but only if "Perform operation on files" is checkboxed.
            if (gui.checkboxShouldFiles)
                SetTimeDateEachFile(directoryPath, fileDateTime);
        }

        /// <summary>
        /// Mode 2: Recursive.
        /// </summary>
        /// <param name="directoryPath">Path to start in.</param>
        internal void RecurseSubDirectoryMode2(string directoryPath)
        {
            NameDateObj timeInside = DecideWhichTimeMode2(directoryPath);
            SkipOrAddFile(directoryPath, true);

            //Make a NameDateObj out of 1 filename; writes each time time to the date attribute that was radiobutton selected.
            var currentobject = new NameDateObjListViewVMdl(timeInside) { Name = directoryPath, FileOrDirType = 1 };

            //If Checkbox is selected:
            if (!checkboxes.C)
                currentobject.Created = "N/A"; // Set the Creation date/time if selected
            if (!checkboxes.M)
                currentobject.Modified = "N/A"; // Set the Modified date/time if selected
            if (!checkboxes.A)
                currentobject.Accessed = "N/A"; // Set the Last Access date/time if selected

            NameDateObj subFile = new NameDateObj(currentobject.Converter());

            FilestoConfirmList.Add(subFile);

            try
            {
                foreach (string subfolder in Directory.GetDirectories(directoryPath))
                    RecurseSubDirectoryMode2(Path.Combine(directoryPath, subfolder));
            }
            catch (UnauthorizedAccessException)
            { }
            catch (DirectoryNotFoundException)
            { }
        }

        /// <summary>  Mode 1P (start at its Parent) </summary>
        internal void RecurseSubDirectoryMode1Parent(string directoryPath)
        {
            try
            {
                foreach (string subfolder in Directory.GetDirectories(directoryPath))
                    RecurseSubDirectoryMode1(Path.Combine(directoryPath, subfolder));
            }
            catch (UnauthorizedAccessException)
            { }
            catch (DirectoryNotFoundException)
            { }
        }
        // These just point back to the above functions, using the parent folderinstead.
        /// <summary>  Mode 2P (start at its Parent)  </summary>
        internal void RecurseSubDirectoryMode2Parent(string directoryPath)
        {
            try
            {
                foreach (string subfolder in Directory.GetDirectories(directoryPath))
                    RecurseSubDirectoryMode2(Path.Combine(directoryPath, subfolder));
            }
            catch (UnauthorizedAccessException)
            { }
            catch (DirectoryNotFoundException)
            { }
        }

        /// <summary>
        /// Set Directory Time
        /// </summary>
        internal void SetTimeDateEachDirectory(string directoryPath, DateTime fileDateTime)
        {
            try
            {
                string[] subDirectories = Directory.GetDirectories(directoryPath);
                Array.Sort(subDirectories, SharedHelper.explorerStringComparer());
                foreach (string eachdir in subDirectories)
                {
                    // Set the date/time for the sub directory
                    SetFileDateTimeMode1(eachdir, fileDateTime, true);
                    // Recurse (loop) through each sub-sub directory
                    RecurseSubDirectoryMode1(eachdir);
                }
            } //catch for GetDirs
            catch (UnauthorizedAccessException)
            { }
        }

        /// <summary>
        /// Set File Time
        /// </summary>
        internal void SetTimeDateEachFile(string directoryPath, DateTime fileDateTime)
        {
            try
            {
                string[] subFiles = Directory.GetFiles(directoryPath);
                Array.Sort(subFiles, SharedHelper.explorerStringComparer());
                foreach (string filename in subFiles)
                    SetFileDateTimeMode1(filename, fileDateTime, false);
            } //catch for GetFiles
            catch (UnauthorizedAccessException)
            { }
        }

        /// <summary>
        /// Tracks H,S,R skipcount. Print a Messagebox Question if trying to skip read only files, and fix RO files if needed
        /// </summary>
        internal void SkipOrAddFile(string path, bool isDirectory)
        {
            FileAttributes fAttr = File.GetAttributes(path);

            if (((fAttr & FileAttributes.System) == FileAttributes.System) && Settings.Default.SkipSystem)
            {
                Skips.S++;
                return; // Skip system files and directories
            }
            if (((fAttr & FileAttributes.Hidden) == FileAttributes.Hidden) && Settings.Default.SkipHidden)
            {
                Skips.H++;
                return; // Skip hidden files and directories
            }
            if (((fAttr & FileAttributes.ReadOnly) == FileAttributes.ReadOnly) && !isDirectory)
            {
                if (Settings.Default.SkipReadOnly)
                {
                    Skips.R++;
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
                FilesReadOnlytoFix.Add(path);
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
        internal void SetFileDateTimeMode1(string filePath, DateTime fileTime, bool isDirectory)
        {
            SkipOrAddFile(filePath, isDirectory);
            //Make a NameDateObj out of 1 filename; Check all 3 date properties, and only write a time on match
            var currentobject = new NameDateObj { Name = filePath, FileOrDirType = SharedHelper.Bool2Int(isDirectory) };

            // Set the Creation date/time if selected
            if (checkboxes.C)
                currentobject.Created = fileTime;
            // Set the Modified date/time is selected
            if (checkboxes.M)
                currentobject.Modified = fileTime;
            // Set the last access time if selected
            if (checkboxes.A)
                currentobject.Accessed = fileTime;

            FilestoConfirmList.Add(currentobject);
        }

        /// <summary>
        /// Display the date and time of the selected file (also works on Directories)
        /// </summary>
        internal NameDateStruct GetCmaTimes(string pathName)
        {
            var cma = new NameDateStruct { PathName = pathName };
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

        private NameDateObj thingtoreturn;
        internal bool DecideTimeRG1and2(string directoryPath)
        {

            thingtoreturn = new NameDateObj();

            if (gui.radioGroupBox1SpecifyTime)
            {
                thingtoreturn.Name = gui.PathName;
                var specifiedDate = DateTime.Parse(gui.dateTimePickerDate.Date.ToString("d") + " " +
                                                   gui.dateTimePickerTime.Hour + ":" +
                                                   gui.dateTimePickerTime.Minute + ":" +
                                                   gui.dateTimePickerTime.Second);
                thingtoreturn.Created = specifiedDate;
                thingtoreturn.Modified = specifiedDate;
                thingtoreturn.Accessed = specifiedDate;
                return false;
            }
            else if (gui.radioGroupBox2CurrentSelect)
            {
                thingtoreturn.Name = gui.PathName;
                thingtoreturn.Created = DateTime.Parse(gui.Created);
                thingtoreturn.Modified = DateTime.Parse(gui.Modified);
                thingtoreturn.Accessed = DateTime.Parse(gui.Accessed);
                return false;
            }
            else
                return true;
        }
        /// <summary>
        /// Very long function that does a simple task. Read in the options the user set for the operation, and
        /// Decide on the timestamp it should use, by the end we will have a single object with 3 times.
        /// This will need to be hit with broad strokes if we attempt to do any more work on the program.
        /// </summary>
        internal NameDateObj DecideWhichTimeMode2(string directoryPath)
        {
            var extractlist = new List<string>();

            var timelist = new List<NameDateObj>();

            if (!DecideTimeRG1and2(directoryPath))
                ;
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
                            if ((fileAttribs & SharedHelper.SyncSettingstoInvisibleFlag()) != 0)
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
                    var decidetemp = new NameDateObj { Name = fullpath };
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
                //make 3 new lists, one for each date containing every NameDateObj
                var creationtimelist = new List<DateTime?>();
                var modtimelist = new List<DateTime?>();
                var accesstimelist = new List<DateTime?>();
                //populate the new seperated lists with the times from the combinedobject list (timelist)
                foreach (NameDateObj timeobject in timelist)
                {
                    creationtimelist.Add((DateTime?)timeobject.Created);
                    modtimelist.Add((DateTime?)timeobject.Modified);
                    accesstimelist.Add((DateTime?)timeobject.Accessed);
                }
                //Make a new list of the lists we just made (Collection initializer)
                var threetimelists = new List<List<DateTime?>> { creationtimelist, modtimelist, accesstimelist };
                //Instantiate 3 new vars as a new class that processes the min and max date from the 3 lists we just made
                var cre = new DateNewOldObj(creationtimelist);
                var mod = new DateNewOldObj(modtimelist);
                var acc = new DateNewOldObj(accesstimelist);

                ////Create 2 lists(min and max), containing the 3 min/max dates.
                //DateTime?[] minarray = { cre.minDate, mod.minDate, acc.minDate };
                //DateTime?[] maxarray = { cre.maxDate, mod.maxDate, acc.maxDate };
                ////Instantiate themin/themax as the new class that calculates the min and max date from the 3 dates above.
                //var themin = new DatesNewestOldest(new List<DateTime?>(minarray));
                //var themax = new DatesNewestOldest(new List<DateTime?>(maxarray));
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
                    int randomindex = random.Next(0, timelist.Count);
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
        //;

        /// <summary>
        /// Show a message box of any files that were cleared of their read-only tag.
        /// </summary>
        internal void FixReadonlyResults(int active)
        {
            if ((FilesReadOnlytoFix.Count > 0) && (active == 0))
            {
                string listoffixedreadonlyfiles = "";
                foreach (string file in FilesReadOnlytoFix)
                    listoffixedreadonlyfiles += file + "\n";
                DialogResult dr =
                    MessageBox.Show(listoffixedreadonlyfiles + "\nRead-Only must be un-set to change date. Continue?",
                        "Read-Only Files: ", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    foreach (string file in FilesReadOnlytoFix)
                    {
                        FileAttributes fileattribs = File.GetAttributes(file);
                        File.SetAttributes(file, SharedHelper.RemoveAttributes(fileattribs, FileAttributes.ReadOnly));
                    }
                    DialogResult dr2 = MessageBox.Show("Turn read-only back on when the confirm window is closed?",
                        "Make Files Read-Only again?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dr2 == DialogResult.No)
                        FilesReadOnlytoFix.Clear();
                    else
                        active = FilesReadOnlytoFix.Count;
                }
                else
                    FilesReadOnlytoFix.Clear();
            }
            else if (active > 0)
            {
                ResetReadOnly();
                active = 0;
            }
        }
        /// <summary> Adds the read-only attribute back after it was removed </summary>
        internal void ResetReadOnly()
        {
            if (FilesReadOnlytoFix.Count <= 0)
                return;
            foreach (string file in FilesReadOnlytoFix)
                File.SetAttributes(file, File.GetAttributes(file) | FileAttributes.ReadOnly);
            // the | is needed for boolean algebra of attribute flags (it means: add readonly)
            FilesReadOnlytoFix.Clear();
        }
    }
}