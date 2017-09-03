using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
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
        /// <summary> the fixreadonly fixreadonlyActive files count (checked in between fix and unfix) </summary>
        public int fixreadonlyActive = 0;
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

        /// <summary>
        /// Display the date and time of the selected file (also works on Directories)
        /// </summary>
        public static NameDateQuick GetCmaTimesFromFilesystem(string pathName)
        {
            NameDateQuick cma;
            if (pathName != "")
            {
                cma = new NameDateQuick
                {
                    PathName = pathName,
                    Created = File.GetCreationTime(pathName).ToString(CultureInfo.CurrentCulture),
                    Modified = File.GetLastWriteTime(pathName).ToString(CultureInfo.CurrentCulture),
                    Accessed = File.GetLastAccessTime(pathName).ToString(CultureInfo.CurrentCulture),
                    HiddenPathName = pathName,
                    Selected = true
                };
            }
            else
            {
                // Maybe no file/directory is selected
                // Then Blank out the display of date/time.
                cma = new NameDateQuick();
            }
            return cma;
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
            //Get a list of files in the selected folder (This can't be done in .NET 2.0)
            //IEnumerable files = Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories);
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
                string justName = Path.GetFileName(file);
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
                dateToUse = DecideTimeFromSubdirOrSubfile(path);
            }
            return dateToUse;
        }

        /// <summary>
        /// Check all the subdirs or subfiles. And decide the time. Called from above: DecideWhichTimeMode1() { radioGroupBox3_UseTimeFrom
        /// </summary>
        internal DateTime? DecideTimeFromSubdirOrSubfile(string path)
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
            //for Any/Random attribute mode, decide which attribute and stick with it. (create before loop)
            // (and seed already exists so we dont regenerate seed through every recursive call to this.
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
                    if (gui.radioButton1_setfromCreated || gui.radioButton4_setfromRandom && randomNumber == 0)
                    {
                        timelisttype = "Created";
                        looptempDate = File.GetCreationTime(fullpath);
                    }
                    else if (gui.radioButton2_setfromModified || gui.radioButton4_setfromRandom && randomNumber == 1)
                    {
                        timelisttype = "Modified";
                        looptempDate = File.GetLastWriteTime(fullpath);
                    }
                    else if (gui.radioButton3_setfromAccessed || gui.radioButton4_setfromRandom && randomNumber == 2)
                    {
                        timelisttype = "Accessed";
                        looptempDate = File.GetLastAccessTime(fullpath);
                    }
                    timelist.Add(looptempDate);
                    string stopbotheringme = timelisttype; //remove "unused var" warning
                }
                //TODO: Check exceptions all over the code, and make sure we have enough
                catch (UnauthorizedAccessException)
                { } //wraps GetFileTimes in error handling
            }
            var minmax = new DateMinMaxNewOld(timelist);
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

            StoreDateByCMACheckboxes(currentobject);

            var subFile = new NameDateObj(currentobject.Converter());

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
        
        private static void ShowAllFoldersUnder(string path, int indent)
        {
            try
            {
                if ((File.GetAttributes(path) & FileAttributes.ReparsePoint)
                    != FileAttributes.ReparsePoint)
                {
                    foreach (string folder in Directory.GetDirectories(path))
                    {
                        Console.WriteLine(
                            "{0}{1}", new string(' ', indent), Path.GetFileName(folder));
                        ShowAllFoldersUnder(folder, indent + 2);
                    }
                }
            }
            catch (UnauthorizedAccessException) { }
        }

        private static IEnumerable<string> GetFileSystemEntries(string directory, string searchPattern)
        {
            IEnumerable<string> files = null;
            try
            {
                files = Directory.EnumerateFileSystemEntries(directory, searchPattern, SearchOption.TopDirectoryOnly);
            }
            catch (UnauthorizedAccessException)
            {
            }

            if (files == null) yield break;
            foreach (string file in files)
            {
                yield return file;
            }
        }

        public IEnumerable<string> GetAllFilesRecursive(string path, string searchPattern)
        {
            return Directory.EnumerateFiles(path, searchPattern).Union(
                Directory.EnumerateDirectories(path).SelectMany(d =>
                {
                    try
                    {
                        return GetAllFilesRecursive(d, searchPattern);
                    }
                    catch (UnauthorizedAccessException) {
                        return Enumerable.Empty<string>();
                    }
                    catch (DirectoryNotFoundException) {
                        return Enumerable.Empty<string>();
                    }
                }));
        }
        public IEnumerable<string> SafeEnumerateFiles(string path, string searchPattern)
        {
            try
            {
                return Directory.EnumerateFiles(path, searchPattern);
            }
            catch (UnauthorizedAccessException)
            {
                return Enumerable.Empty<string>();
            }
            catch (DirectoryNotFoundException)
            {
                return Enumerable.Empty<string>();
            }
            catch (FileNotFoundException)
            {
                return Enumerable.Empty<string>();
            }
        }

        public IEnumerable<string> GetAllDirs(string path, string searchPattern)
        {
            return Directory.EnumerateDirectories(path, searchPattern);
        }
        private IEnumerable<String> GetFilesAndDirsSafely(string path, string filePattern, bool recurse)
        {
            IEnumerable<String> emptyList = new string[0];

            if (File.Exists(path))
                return new[] { path };

            if (!Directory.Exists(path))
                return emptyList;

            var topDirectory = new DirectoryInfo(path);

            // Enumerate the files just in the top directory.
            var files = topDirectory.EnumerateFiles(filePattern);
            var fileInfos = files as FileInfo[] ?? files.ToArray();
            int filesLength = fileInfos.Length;
            IEnumerable<String> topDirEnum = Enumerable.Repeat(path, 1);
            IEnumerable<String> filesList = Enumerable.Range(0, filesLength).Select(i =>
                {
                    string filename = null;
                    try
                    {
                        var file = fileInfos.ElementAt(i);
                        filename = file.FullName;
                    } catch (UnauthorizedAccessException)
                    { } catch (FileNotFoundException)
                    { } catch (InvalidOperationException)
                    { }
                    return filename;
                })
                .Where(i => null != i);

            if (!recurse)
                return topDirEnum.Concat(filesList);

            var dirs = topDirectory.EnumerateDirectories("*");
            var directoryInfos = dirs as DirectoryInfo[] ?? dirs.ToArray();
            int dirsLength = directoryInfos.Length;
            IEnumerable<String> dirsList = Enumerable.Range(0, dirsLength).SelectMany(i =>
            {
                try
                {
                    var dir = directoryInfos.ElementAt(i);
                    string dirname = dir.FullName;
                    return GetFilesAndDirsSafely(dirname, filePattern, recurse);
                } catch (UnauthorizedAccessException)
                { } catch (DirectoryNotFoundException)
                { } catch (InvalidOperationException)
                { }
                return emptyList;
            });

            return topDirEnum.Concat(filesList.Concat(dirsList));
        }

        /// <summary>
        /// Mode 3: Process One directory, Process 2nd Dir. with recursive sub-directory support. Calls SetFileDateTime()
        /// (Only adds to the confirm list, Form 2 will actually write changes).
        /// </summary>
        /// <param name="targetPath">Full path to the targetPath directory</param>
        /// <param name="comparePath">Full path to the comparePath directory</param>
        /// req, checkBox_Recurse.Checked, checkBoxShouldFiles.Checked
        internal void RecurseSubDirectoryMode3(string targetPath, string comparePath)
        {
            //no point continuing if we have nothing matching to compare to.
            if (!Directory.Exists(comparePath))
                return;
            if (!comparePath.EndsWith(SharedHelper.SeperatorString))
                comparePath += SharedHelper.Seperator;

            // Take a snapshot of the paths of the file system.  Makes an IEnumerable.
            IEnumerable<string> destFileList = GetFilesAndDirsSafely(targetPath, "*", true);
            IEnumerable<string> srcFileList = GetFilesAndDirsSafely(comparePath, "*", true);
            // Find the common files. It produces a sequence and doesn't execute until the foreach statement.  
            IEnumerable<string> queryCommonFiles = srcFileList.Intersect(destFileList, SharedHelper.explorerStringEqualityComparer(targetPath, comparePath));

            foreach (string f in queryCommonFiles)
            {
                NameDateQuick srcfiletime = GetCmaTimesFromFilesystem(f);
                string nameChanged = f.Replace(comparePath, targetPath);
                bool isDirectory = Directory.Exists(nameChanged);
                SkipOrAddFile(nameChanged, isDirectory);

                var currentobject = new NameDateObjListViewVMdl(srcfiletime) { Name = nameChanged, FileOrDirType = SharedHelper.Bool2Int(isDirectory) };
                //If Checkbox is selected: writes each time time to the date attribute that was radiobutton selected.
                StoreDateByCMACheckboxes(currentobject);

                var item = new NameDateObj(currentobject.Converter());

                FilestoConfirmList.Add(item);
            }
        }

        private void StoreDateByCMACheckboxes(NameDateObjListViewVMdl currentobject)
        {
            //TODO: stop using N/A as a placeholder for logic
            //If Checkbox is selected:
            if (!checkboxes.C)
                currentobject.Created = "N/A"; // Only Set the Creation date/time if selected
            if (!checkboxes.M)
                currentobject.Modified = "N/A"; // Only Set the Modified date/time if selected
            if (!checkboxes.A)
                currentobject.Accessed = "N/A"; // Only Set the Last Access date/time if selected
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
                    AddONEFiletoConfirmList(eachdir, fileDateTime, true);
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
                    AddONEFiletoConfirmList(filename, fileDateTime, false);
            } //catch for GetFiles
            catch (UnauthorizedAccessException)
            { }
        }

        /// <summary> STATIC.
        /// Set the date/time for a single file/directory (This works on files and directories)
        /// Go through the list, skipping H,S,R files, and add all the file+date objects to the Confirmation List
        /// (Only adds to the confirm list, Form 2 (Confirm) will actually write changes).
        /// </summary>
        /// <param name="filePath">Full path to the file/directory</param>
        /// <param name="fileTime">Date/Time to set the file/directory</param>
        /// <param name="isDirectory">Is this a directory???</param>
        internal void AddONEFiletoConfirmList(string filePath, DateTime fileTime, bool isDirectory)
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
        /// Tracks H,S,R skipcount. Print a Messagebox Question if trying to skip read only files, and fix RO files if needed
        /// </summary>
        internal void SkipOrAddFile(string path, bool isDirectory)
        {
            if (isDirectory) return;    //so, idk why I came in here?

            // NOTE: The following is where an error would occur if you try and scan a directory with Longpaths in it.
            // I have tried to fix it in the app.config and the app.manifest file with some XML tags, but they didnt work.
            // In the end, Windows 10 build 10240 is too old to support Paths > 260 chars...crazy right. ?
            // Since its a managed app I would prefer to keep native code out if possible.
            // For now the only thing i can do is Target .NET 4.6.2
            // https://blogs.msdn.microsoft.com/jeremykuhne/2016/07/30/net-4-6-2-and-long-paths-on-windows-10/
            //Should really assert something before we get this far, this isnt the right function to be erroring out in:
            // TODO: Revisit this when a few months go by to figure out if anything has changed with >260 Longpaths 
            // >
            FileAttributes fAttr = File.GetAttributes(path); //longpath unsafe

            if ((fAttr & FileAttributes.System) == FileAttributes.System && Settings.Default.SkipSystem)
            {
                Skips.S++;
                return; // Skip system files and directories
            }
            if ((fAttr & FileAttributes.Hidden) == FileAttributes.Hidden && Settings.Default.SkipHidden)
            {
                Skips.H++;
                return; // Skip hidden files and directories
            }
            if ((fAttr & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
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
                            "The file '" + path + "' is Read-Only.\n\n" +
                            "The Files Will be Fixed Silently ANYWAY, So hit No to stop these popups\nYou can change re-enable this warning in Preferences at any time.",
                            @" Error - File is Read Only. Continue showing Read-Only notifications?", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    Settings.Default.ShowNoticesReadOnly = (dr == DialogResult.Yes);
                }
                //add them to the list to be readOnly+Fixed before they are timewritten.
                FilesReadOnlytoFix.Add(path);
            }
        }

        private NameDateObj thingtoreturn;
        internal bool ObtainParseSpecifiedTimeGUIBoxes()
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
                return true;
            }
            if (!gui.radioGroupBox2CurrentSelect) return false;
            thingtoreturn.Name = gui.PathName;
            thingtoreturn.Created = DateTime.Parse(gui.Created);
            thingtoreturn.Modified = DateTime.Parse(gui.Modified);
            thingtoreturn.Accessed = DateTime.Parse(gui.Accessed);
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

            if (ObtainParseSpecifiedTimeGUIBoxes())
                return thingtoreturn;
            //Begin checking Conditional for which file is newest oldest etc
            if (!gui.radioGroupBox3UseTimeFrom)
                return thingtoreturn;
            //decide if they wanted to use time from subfile or subdir
            if (gui.radioButton1_useTimefromFile)
            {
                try
                {
                    // Get List of the subfiles (full path)
                    extractlist.AddRange(from subFile in Directory.GetFiles(directoryPath) let fileAttribs = File.GetAttributes(subFile)
                        where (fileAttribs & SharedHelper.SyncSettingstoInvisibleFlag()) == 0 select Path.Combine(directoryPath, subFile));
                } // catch failure of GetAttributes
                catch (FileNotFoundException ex)
                {
                    MessageBox.Show(
                        "Error getting attributes of a file in '" + directoryPath + "': \r\n\r\n" + ex.Message,
                        @"PEBKAC Error ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                } // catch failure of GetFiles
                catch (UnauthorizedAccessException)
                { } //show nothing because this is normal for this method when encountering inaccessible files
            }
            else if (gui.radioButton2_useTimefromSubdir)
            {
                try
                {
                    // Get List of the subdirs (full path)
                    extractlist.AddRange(Directory.GetDirectories(directoryPath).Select(subDirectory => Path.Combine(directoryPath, subDirectory)));
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
            var cre = new DateMinMaxNewOld(creationtimelist);
            var mod = new DateMinMaxNewOld(modtimelist);
            var acc = new DateMinMaxNewOld(accesstimelist);

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
            return thingtoreturn;
        }
        //;

        /// <summary>
        /// Show a message box of any files that were cleared of their read-only tag.
        /// </summary>
        internal void FixReadonlyResults()
        {
            if ((FilesReadOnlytoFix.Count > 0) && (fixreadonlyActive == 0))
            {
                string listoffixedreadonlyfiles = FilesReadOnlytoFix.Aggregate((current, file) => current + (file + "\n"));
                DialogResult dr =
                    MessageBox.Show(listoffixedreadonlyfiles + @"Read-Only must be un-set to change date. Continue?",
                        @"PEBKAC Error ", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    foreach (string file in FilesReadOnlytoFix)
                    {
                        FileAttributes fileattribs = File.GetAttributes(file);
                        File.SetAttributes(file, SharedHelper.RemoveAttributes(fileattribs, FileAttributes.ReadOnly));
                    }
                    DialogResult dr2 = MessageBox.Show(@"Turn read-only back on when the confirm window is closed?",
                        @"PEBKAC Error ", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dr2 == DialogResult.No)
                        FilesReadOnlytoFix.Clear();
                    else
                        fixreadonlyActive = FilesReadOnlytoFix.Count;
                }
                else
                    FilesReadOnlytoFix.Clear();
            }
            else if (fixreadonlyActive > 0)
            {
                ResetReadOnly();
                fixreadonlyActive = 0;
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