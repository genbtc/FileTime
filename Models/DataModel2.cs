﻿/* DataModel file #2. Most of this file is the  portion that was static when i was refactoring. */
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using genBTC.FileTime.Classes;
using genBTC.FileTime.mViewModels;
using genBTC.FileTime.Properties;

namespace genBTC.FileTime.Models
{
    #region FORM_MAIN2
    public partial class DataModel
    {
        public static List<string> PopulateFileList(string path, DataModel dataModel)
        {
            foreach (string filename in Directory.GetFiles(path))
                dataModel.contentsFileList.Add(Path.GetFileName(filename));
            return dataModel.contentsFileList;
        }

        public static List<string> PopulateDirList(string path, DataModel dataModel)
        {
            foreach (string subDirectory in Directory.GetDirectories(path))
                dataModel.contentsDirList.Add(Path.GetFileName(subDirectory));
            return dataModel.contentsDirList;
        }

        /// <summary>
        /// Mode 1: Process One directory, with recursive sub-directory support. Calls SetFileDateTime()
        /// (Only adds to the confirm list, Form 2 will actually write changes).
        /// </summary>
        /// <param name="directoryPath">Full path to the directory</param>
        /// req, checkBox_Recurse.Checked, checkBoxShouldFiles.Checked
        internal static void RecurseSubDirectoryMode1(string directoryPath, bool checkedRecurse, bool checkedShouldFiles, BoolCMA checkboxes, DataModel dataModel, guistatus gui)
        {
            DateTime? nullorfileDateTime = dataModel.DecideWhichTimeMode1(directoryPath, gui);
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

        /// <summary>
        /// Mode 2: Recursive.
        /// </summary>
        /// <param name="directoryPath">Path to start in.</param>
        internal static void RecurseSubDirectoryMode2(string directoryPath, DataModel dataModel, BoolCMA checkboxes, guistatus gui)
        {
            NameDateObj timeInside = DecideWhichTimeMode2(dataModel, directoryPath, gui);
            SkipOrAddFile(dataModel, directoryPath, true);
            NameDateObj subFile = Makedateobject(checkboxes, directoryPath, timeInside);
            dataModel.FilestoConfirmList.Add(subFile);

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

        /// <summary>  Mode 1P (start at its Parent) </summary>
        internal static void RecurseSubDirectoryMode1Parent(string directoryPath, bool checkedRecurse, bool checkedShouldFiles, BoolCMA checkboxes, DataModel dataModel, guistatus gui)
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
        // These just point back to the above functions, using the parent folderinstead.
        /// <summary>  Mode 2P (start at its Parent)  </summary>
        internal static void RecurseSubDirectoryMode2Parent(string directoryPath, DataModel dataModel, BoolCMA checkboxes, guistatus gui)
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

        /// <summary>
        /// Set Directory Time
        /// </summary>
        internal static void SetTimeDateEachDirectory(string directoryPath, DateTime fileDateTime, BoolCMA checkboxes, DataModel dataModel, bool checkedRecurse, bool checkedShouldFiles, guistatus gui)
        {
            try
            {
                string[] subDirectories = Directory.GetDirectories(directoryPath);
                Array.Sort(subDirectories, SharedHelper.explorerStringComparer());
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
        internal static void SetTimeDateEachFile(string directoryPath, DateTime fileDateTime, BoolCMA checkboxes, DataModel dataModel)
        {
            try
            {
                string[] subFiles = Directory.GetFiles(directoryPath);
                Array.Sort(subFiles, SharedHelper.explorerStringComparer());
                foreach (string filename in subFiles)
                    SetFileDateTimeMode1(dataModel, checkboxes, filename, fileDateTime, false);
            } //catch for GetFiles
            catch (UnauthorizedAccessException)
            { }
        }

        /// <summary>
        /// Tracks H,S,R skipcount. Print a Messagebox Question if trying to skip read only files, and fix RO files if needed
        /// </summary>
        public static void SkipOrAddFile(DataModel dataModel, string path, bool isDirectory)
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
        internal static void SetFileDateTimeMode1(DataModel dataModel, BoolCMA checkboxes, string filePath, DateTime fileTime, bool isDirectory)
        {
            SkipOrAddFile(dataModel, filePath, isDirectory);

            NameDateObj currentobject = Makedateobject(checkboxes, filePath, fileTime, isDirectory);
            dataModel.FilestoConfirmList.Add(currentobject);
        }

        /// <summary>
        /// Static. Overloaded. Make a NameDateObj out of 1 filename; writes A SINGLE time to all 3 date properties.
        /// </summary>
        internal static NameDateObj Makedateobject(BoolCMA checkboxes, string filePath, DateTime fileTime, bool isDirectory)
        {
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
            return currentobject;
        }
        /// <summary>
        /// Static. Overloaded. Make a NameDateObj out of 1 filename; writes each time time to the date attribute that was radiobutton selected.
        /// </summary>
        internal static NameDateObj Makedateobject(BoolCMA checkboxes, string folderPath, NameDateObj subObj)
        {
            var currentobject = new NameDateObjListViewVMdl(subObj) { Name = folderPath, FileOrDirType = 1 };

            //If Checkbox is selected:
            if (!checkboxes.C)
                currentobject.Created = "N/A"; // Set the Creation date/time if selected
            if (!checkboxes.M)
                currentobject.Modified = "N/A"; // Set the Modified date/time if selected
            if (!checkboxes.A)
                currentobject.Accessed = "N/A"; // Set the Last Access date/time if selected
            return new NameDateObj(currentobject.Converter());
        }

        /// <summary>
        /// Display the date and time of the selected file (also works on Directories)
        /// </summary>
        internal static DisplayCmaTimeData GetCmaTimes(string pathName)
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
        /// Very long function that does a simple task. Read in the options the user set for the operation, and
        /// Decide on the timestamp it should use, by the end we will have a single object with 3 times.
        /// This will need to be hit with broad strokes if we attempt to do any more work on the program.
        /// </summary>
        internal static NameDateObj DecideWhichTimeMode2(DataModel dataModel, string directoryPath, guistatus gui)
        {
            var extractlist = new List<string>();

            var timelist = new List<NameDateObj>();
            var thingtoreturn = new NameDateObj();

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

    }
    #endregion FORM_MAIN2
}