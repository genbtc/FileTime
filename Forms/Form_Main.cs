/*
 * ###  This file is codebehind for the Main program window ###
 * 
 *   /// FileTime: Sets the creation, last write and last access date and time of user selection with various options
 *   /// Version: 1.0
 *   /// Date: 17 July 2014, Last Modified: 8/26/2014
 *   ///                        comments    8/13/2017
 * ###  Author: genBTC ###
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Windows.Forms;
using genBTC.FileTime.Classes;
using genBTC.FileTime.Classes.Native;
using genBTC.FileTime.Models;
using genBTC.FileTime.mViewModels;
using genBTC.FileTime.Properties;

namespace genBTC.FileTime
{
    /// <summary>
    /// GUI : Main Form Window of the Program. 
    /// </summary>
    public partial class Form_Main
    {
       //Form 2 stuff we need to have in Form1.
        /// <summary> Stub for Form 2 to be accessed once it is opened. </summary>
        public Form_Confirm Confirmation;

        /// <summary> Pass a list of files that had the read-only attribute fixed, so Form2 can display it </summary>
        public List<string> FilesReadOnlytoFix = new List<string>();

        /// <summary> List of Class to be passed to Form 2 for confirmation of files</summary>
        public List<NameDateObject> FilestoConfirmList = new List<NameDateObject>();

        #region Main startup/load code
        /// <summary>Init the main startup form </summary>
        public Form_Main()
        {
            //Required for Windows Form Designer support
            InitializeComponent();
            //Reason Explanation: This is needed AFTER InitializeComponent because SplitterDistance must be declared FIRST. The designer 
            // sorts by alphabetical order and puts P for Panel2Minsize before S for SplitterDistance throwing an exception.
            splitContainer1.Panel2MinSize = 150;
        }

        /// <summary> Load the main form (from an event handler on this.Load in Form_Main.Designer.cs)</summary>
        private void Form_Main_Load(object sender, EventArgs e)
        {
            try
            {
                Confirmation = new Form_Confirm(this);
                explorerTree1.SetCurrentPath(Settings.Default.useStartupDir
                    ? Settings.Default.StartupDir.TrimEnd(Seperator)
                    : UserDesktop);
                explorerTree1.BrowseTo();
                // Display the folder path and the contents of it.
                DisplayContentsList();
            }
            catch //very broad catch, with no reason or explanation?
            {
                ClearOnError();
            }
        }

        #endregion //Main Form Code

        #region Helper functions...

        /// <summary>
        /// Clear both ListViews and the three data container lists, blanks the selected date, and erases the top current dir textbox.
        /// </summary>
        private void ClearOnError()
        {
            listView_Contents.Items.Clear();
            contentsDirList.Clear();
            contentsFileList.Clear();
            DisplayCma("");
            label_FPath.Text = "";
        }


        /// <summary> Only enable the update button if something is selected </summary>
        private void UpdateButtonEnable()
        {
            button_GoUpdate.Enabled =
                (checkBox_CreationDateTime.Checked |
                 checkBox_ModifiedDateTime.Checked |
                 checkBox_AccessedDateTime.Checked);
        }


        #endregion //Helper functions

        #region Buttons

        private void button_Browse_Click(object sender, EventArgs e)
        {
            string path = OpenFile();
            //if (path == label_FPath.Text) //if nothing was changed
            explorerTree1.SetCurrentPath(path);
            explorerTree1.BrowseTo();
        }

        private void button_Update_Click(object sender, EventArgs e)
        {
            switch (tabControl1.SelectedIndex)
            {
                //Branch to launch the selected mode.
                case 0:
                    GoUpdateDateTimeMode1();
                    break;
                case 1:
                    GoUpdateDateTimeMode2();
                    break;
            }
            string comparefolder;
            if (radioGroupBox1_pickFolderForCompare.Checked)
                comparefolder = OpenFile();

        }

        #endregion //Buttons

        #region Menu...
        //Menu items:
        private void menuItem_FileOpen_Click(object sender, EventArgs e)
        {
            OpenFile();
        }

        private void menuItem_FileExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void menuItem_HelpContents_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, "FileTime.chm", HelpNavigator.TableOfContents);
        }

        private void menuItem_HelpAbout_Click(object sender, EventArgs e)
        {
            var form = new Form_About();
            form.ShowDialog(this);
        }

        /// <summary> Create and initialize preferences </summary>
        private void menuItem_Preferences_Click(object sender, EventArgs e)
        {
            var prefs = new Form_Preferences(label_FPath.Text);
            prefs.ShowDialog(this);
        }

        #endregion //Menu

        #region Event functions: Onselected ListView main panels & checkbox

        /// <summary> Creation,Modified,Accessed Checkbox selection changed </summary>
        private void checkBox_DateTime_CheckedChanged(object sender, EventArgs e)
        {
            UpdateButtonEnable();
        }

        /// <summary>
        /// Update the textlabel box for FilePath when the Explorer Tree path changes.
        /// (making sure it always ends in a \ seperator). Also causes a re-render of Contents!
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void explorerTree1_PathChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            label_FPath.Text = explorerTree1.CurrentPath;
            if (!label_FPath.Text.EndsWith(Seperator.ToString()))
                label_FPath.Text += Seperator;
            DisplayContentsList();
            Cursor.Current = Cursors.Default;
        }

        #endregion //Listbox + Checkbox Functions

        #region >>Main Logic Code<<

        /// <summary>
        /// Display the Folder Browser Dialog and then display the selected
        /// file path and the directories and files in the folder.
        /// </summary>
        private string OpenFile(string path = "")
        {
            //use current path as dialog path, or feed in a path to start in.:
            if (path == "")
                path = label_FPath.Text;
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

        /// <summary>
        /// Display subfiles and subdirectories in the right panel listview
        /// </summary>
        /// <param name="filesonly">Don't show the directories, only files.</param>
        /// reqs: listviewcontents, contentsDirList,contentsFileList, labelFpathText, checkbox_recurse, filextlist, imageList_Files
        private void DisplayContentsList(bool filesonly = true)
        {
            //Clear the contents UI + containers
            listView_Contents.Items.Clear();
            contentsDirList.Clear();
            contentsFileList.Clear();

            string directoryName = label_FPath.Text;

            if (!Directory.Exists(directoryName))
                return;

            if (!filesonly && checkBox_Recurse.Checked)
            {
                try
                {
                    //grab and store all the subdirectories
                    foreach (string subDirectory in Directory.GetDirectories(directoryName))
                        contentsDirList.Add(Path.GetFileName(subDirectory));
                }
                catch (UnauthorizedAccessException)
                {}
                contentsDirList.Sort(explorerStringComparer);
                foreach (string subDirectory in contentsDirList)
                {
                    // Display all the sub directories using the directory icon (enum 1)
                    listView_Contents.Items.Add(subDirectory, (int) ListViewIcon.Directory);
                }
            }

            // Display all of the files and show a file icon
            try
            {
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
                        if (filextlist.FindLastIndex(SharedHelper.FindCurExt) == -1)
                        {
                            filextlist.Add(SharedHelper.CurrExten);
                            //call NativeExtractIcon to get the filesystem icon of the filename
                            imageList_Files.Images.Add(SharedHelper.CurrExten, NativeExtractIcon.GetIcon(file, true));
                        }
                    }
                    else //if it is a shortcut, grab icon directly.
                        imageList_Files.Images.Add(justName, NativeExtractIcon.GetIcon(file, true));

                    contentsFileList.Add(justName);
                }
            }
            catch (UnauthorizedAccessException)
            {}
            contentsFileList.Sort(explorerStringComparer);
            foreach (string file in contentsFileList)
            {
                string exten = Path.GetExtension(file);
                listView_Contents.Items.Add(file, exten != ".lnk" ? exten : file);
            }
        }

        /// <summary>
        /// Mode 1 Update-Button Command. This runs a LONG process on the folders/files. It decides which time to use, 
        /// then adds them to the confirm list to be handled by the form_confirm window (part2).
        /// </summary>
        /// reqs: label_Fpathtext, FilestoConfirmList, ref to Confirmation, skipcount of H,R,S, 
        /// Launches the creation of Form2_Confirm, and interacts with it.
        private void GoUpdateDateTimeMode1()
        {
            string startingdir = label_FPath.Text;

            FilestoConfirmList.Clear();

            if (Confirmation.active > 0)
                Confirmation.FixReadonlyResults();

            _skippedHiddenCount = 0;
            _skippedReadOnlyCount = 0;
            _skippedSystemCount = 0;

            if (Settings.Default.mode1addrootdir)
            {
                DateTime? nullorfileDateTime = DecideWhichTimeMode1(startingdir);
                if (nullorfileDateTime == null) //if nothing could be decided, exit, otherwise continue
                {
                    MessageBox.Show("Error! Nothing to decide time from. \n" +
                                    "Sub-File or Sub-Dir button requires files/dirs to be SHOWN on the right-side Contents panel...",
                        "PEBKAC Error ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                var fileDateTime = (DateTime)nullorfileDateTime;

                SetFileDateTimeMode1(startingdir, fileDateTime, true);
            }
        //TODO: where I should add worker process
            if (Settings.Default.useRootDirAsContainer)
                RecurseSubDirectoryMode1B(startingdir);
            else
                RecurseSubDirectoryMode1(startingdir);
        //end worker process

            var itemsSkippedCount = _skippedHiddenCount + _skippedReadOnlyCount + _skippedSystemCount;
            if (itemsSkippedCount > 0)
            {
                string skippedmessage = "";
                skippedmessage += "There were " + _skippedSystemCount + " System files/directories skipped.\n";
                skippedmessage += "There were " + _skippedHiddenCount + " Hidden files/directories skipped.\n";
                skippedmessage += "There were " + _skippedReadOnlyCount + " Read-Only files/directories skipped.";
                MessageBox.Show(skippedmessage, "Info Log", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            //Code to show files to be changed in the confirmation window (Show Form 2)
            if (!Confirmation.Visible)
            {
                Confirmation = new Form_Confirm(this);
                Confirmation.Show();
            }
            else
                Confirmation.MakeListView();
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
        private DateTime? DecideWhichTimeMode1(string path)
        {
            contentsDirList.Clear();
            contentsFileList.Clear();

            var dateToUse = new DateTime?();
            if (radioGroupBox1_SpecifyTime.Checked)
            {
                dateToUse = DateTime.Parse(dateTimePicker_Date.Value.Date.ToString("d") + " " +
                                           dateTimePicker_Time.Value.Hour + ":" +
                                           dateTimePicker_Time.Value.Minute + ":" +
                                           dateTimePicker_Time.Value.Second);
            }
            else if (radioGroupBox2_CurrentSelectionTime.Checked)
            {
                if (radioButton1_CreationDate.Checked)
                    dateToUse = DateTime.Parse(label_CreationTime.Text);
                else if (radioButton2_ModifiedDate.Checked)
                    dateToUse = DateTime.Parse(label_Modified.Text);
                else if (radioButton3_LastAccessDate.Checked)
                    dateToUse = DateTime.Parse(label_LastAccess.Text);
            }
            else if (radioGroupBox3_UseTimeFrom.Checked)
            {
                dateToUse = RadioBox3FunctionName(path, dateToUse);
            }
            return dateToUse;
        }

        private DateTime? RadioBox3FunctionName(string path, DateTime? dateToUse)
        {
            {
                var extractlist = new List<string>();
                if (radioButton1_useTimefromFile.Checked)
                {
                    foreach (string filename in Directory.GetFiles(path))
                        contentsFileList.Add(Path.GetFileName(filename));
                    extractlist = contentsFileList;
                }
                else if (radioButton2_useTimefromSubdir.Checked)
                {
                    foreach (string subDirectory in Directory.GetDirectories(path))
                        contentsDirList.Add(Path.GetFileName(subDirectory));
                    extractlist = contentsDirList;
                }

                // if the list is blank due to no files actually existing then we have nothing to do, so stop here.
                if (extractlist.Count == 0)
                    return null;
                //for Any/Random attribute mode, decide which attribute and stick with it.
                int randomNumber = random.Next(0, 3);
                var timelist = new List<DateTime?>();
                //start iterating through
                foreach (string subitem in extractlist)
                {
                    string timelisttype; //this was made just in case.
                    var looptempDate = new DateTime();
                    try
                    {
                        string fullpath = Path.Combine(path, subitem);
                        if (radioButton1_setfromCreated.Checked)
                        {
                            timelisttype = "Created";
                            looptempDate = File.GetCreationTime(fullpath);
                        }
                        else if (radioButton2_setfromModified.Checked)
                        {
                            timelisttype = "Modified";
                            looptempDate = File.GetLastWriteTime(fullpath);
                        }
                        else if (radioButton3_setfromAccessed.Checked)
                        {
                            timelisttype = "Accessed";
                            looptempDate = File.GetLastAccessTime(fullpath);
                        }
                        else if (radioButton4_setfromRandom.Checked)
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
                    {}
                }
                var minmax = new OldNewDate(timelist);
                if (radioButton1_Oldest.Checked)
                {
                    if (minmax.MinDate != null)
                        dateToUse = (DateTime)minmax.MinDate; //explicit typecast from nullable
                }
                else if (radioButton2_Newest.Checked)
                {
                    if (minmax.MaxDate != null)
                        dateToUse = (DateTime)minmax.MaxDate;
                }
                else if (radioButton3_Random.Checked)
                {
                    int randomFile = random.Next(0, minmax.Index);
                    if (timelist[randomFile] != null)
                        dateToUse = (DateTime)timelist[randomFile];
                }
            }
            return dateToUse;
        }

        /// <summary>
        /// Process One directory, with recursive sub-directory support. Calls SetFileDateTime() 
        /// (Only adds to the confirm list, Form 2 will actually write changes).
        /// </summary>
        /// <param name="directoryPath">Full path to the directory</param>
        private void RecurseSubDirectoryMode1(string directoryPath)
        {
            DateTime? nullorfileDateTime = DecideWhichTimeMode1(directoryPath);
            if (nullorfileDateTime == null) 
            {
                return; //if nothing could be decided, exit. otherwise continue
            }
            var fileDateTime = (DateTime)nullorfileDateTime;

            // Set the date/time for each sub directory but only if "Recurse Sub-Directories" is checkboxed.
            if (checkBox_Recurse.Checked)
            {
                try
                {
                    string[] subDirectories = Directory.GetDirectories(directoryPath);
                    Array.Sort(subDirectories, explorerStringComparer);
                    foreach (string eachdir in subDirectories)
                    {
                        // Set the date/time for the sub directory
                        SetFileDateTimeMode1(eachdir, fileDateTime, true);
                        // Recurse (loop) through each sub-sub directory
                        RecurseSubDirectoryMode1(eachdir);
                    }
                } //catch for GetDirs
                catch (UnauthorizedAccessException)
                {}
            }
            // Set the date/time for each file, but only if "Perform operation on files" is checkboxed.
            if (checkBoxShouldFiles.Checked)
            {
                try
                {
                    string[] subFiles = Directory.GetFiles(directoryPath);
                    Array.Sort(subFiles, explorerStringComparer);
                    foreach (string filename in subFiles)
                        SetFileDateTimeMode1(filename, fileDateTime, false);
                } //catch for GetFiles
                catch (UnauthorizedAccessException)
                {}
            }
        }

        private void RecurseSubDirectoryMode1B(string startingdir)
        {
            try
            {
                foreach (string subfolder in Directory.GetDirectories(startingdir))
                    RecurseSubDirectoryMode1(Path.Combine(startingdir, subfolder));
            }
            catch (UnauthorizedAccessException)
            { }
            catch (DirectoryNotFoundException)
            { }
        }

        /// <summary>
        /// Set the date/time for a single file/directory (This works on files and directories)
        /// (Only adds to the confirm list, Form 2 (Confirm) will actually write changes).
        /// </summary>
        /// <param name="filePath">Full path to the file/directory</param>
        /// <param name="fileTime">Date/Time to set the file/directory</param>
        /// <param name="isDirectory">This path is a directory</param>
        private void SetFileDateTimeMode1(string filePath, DateTime fileTime, bool isDirectory)
        {
            FileAttributes fileAttributes = File.GetAttributes(filePath);

            if (((fileAttributes & FileAttributes.System) == FileAttributes.System) && Settings.Default.SkipSystem)
            {
                _skippedSystemCount++;
                return; // Skip system files and directories
            }
            if (((fileAttributes & FileAttributes.Hidden) == FileAttributes.Hidden) && Settings.Default.SkipHidden)
            {
                _skippedHiddenCount++;
                return; // Skip hidden files and directories 
            }
            if (((fileAttributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly) && !isDirectory)
            {
                if (Settings.Default.SkipReadOnly)
                {
                    _skippedReadOnlyCount++;
                    return;
                }
                if (Settings.Default.ShowNoticesReadOnly)
                {
                    DialogResult dr =
                        MessageBox.Show(
                            "The file '" + filePath + "' is Read-Only.\n\nContinue showing Read-Only notifications?",
                            "Read-Only", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    Settings.Default.ShowNoticesReadOnly = (dr == DialogResult.Yes);
                }
                FilesReadOnlytoFix.Add(filePath);
            }

            NameDateObject currentobject = Makedateobject(filePath, fileTime, isDirectory);
            FilestoConfirmList.Add(currentobject);
        }

        /// <summary>
        /// Overloaded. Make a NameDateObject out of 1 filename; writes A SINGLE time to all 3 date properties.
        /// </summary>
        private NameDateObject Makedateobject(string filePath, DateTime fileTime, bool isDirectory)
        {
            var currentobject = new NameDateObject {Name = filePath, FileOrDirType = Bool2Int(isDirectory)};

            // Set the Creation date/time if selected
            if (checkBox_CreationDateTime.Checked)
                currentobject.Created = fileTime;
            // Set the Modified date/time is selected					
            if (checkBox_ModifiedDateTime.Checked)
                currentobject.Modified = fileTime;
            // Set the last access time if selected
            if (checkBox_AccessedDateTime.Checked)
                currentobject.Accessed = fileTime;
            return currentobject;
        }
        /// <summary>
        /// Overloaded. Make a NameDateObject out of 1 filename; writes each time time to the date attribute that was radiobutton selected.
        /// VIEWMODEL
        /// </summary>
        private NameDateObject Makedateobject(string folderPath, NameDateObject subObject)
        {
            var currentobject = new NameDateObjectListViewVm(subObject) { Name = folderPath, FileOrDirType = 1 };

            //If Checkbox is selected:
            if (!checkBox_CreationDateTime.Checked)
                currentobject.Created = "N/A"; // Set the Creation date/time if selected
            if (!checkBox_ModifiedDateTime.Checked)
                currentobject.Modified = "N/A"; // Set the Modified date/time if selected	
            if (!checkBox_AccessedDateTime.Checked)
                currentobject.Accessed = "N/A"; // Set the Last Access date/time if selected
            return new NameDateObject(currentobject.Converter());
        }

        /// <summary>
        /// Display the date and time of the selected file (also works on Directories)
        /// </summary>
        private void DisplayCma(string pathName)
        {
            if (pathName != "")
            {
                label_CreationTime.Text = File.GetCreationTime(pathName).ToString();
                label_Modified.Text = File.GetLastWriteTime(pathName).ToString();
                label_LastAccess.Text = File.GetLastAccessTime(pathName).ToString();
                labelHidden_PathName.Text = pathName;
                radioGroupBox2_CurrentSelectionTime.Enabled = true;
            }

            else
            {
                // Maybe no file/directory is selected
                // Then Blank out the display of date/time.
                label_CreationTime.Text = "";
                label_Modified.Text = "";
                label_LastAccess.Text = "";
                labelHidden_PathName.Text = "";
                radioGroupBox2_CurrentSelectionTime.Enabled = false;
                radioGroupBox1_SpecifyTime.Checked = true;
            }
            itemSelectionChangedTimer.Stop();
        }

        /// <summary>
        /// Display file's times in the Tri-Textbox bottom UI for the currently selected file.
        /// </summary>
        private void UpdateDisplayFileDateTime()
        {
            //If none of the below conditions are true, "" means blank date/time during displayCMA()
            string pathName = "";

            if (listView_Contents.SelectedItems.Count == 1 && listView_Contents.SelectedItems[0].ImageIndex != 1)
                // If one file is selected
                pathName = Path.Combine(label_FPath.Text, listView_Contents.SelectedItems[0].Text);
            //Always Call the display date/time function
            DisplayCma(pathName);
        }

        /// <summary>
        /// Timer Event function. Used to prevent the currently selected Textboxes for the 3 Times from blanking
        /// when an item selection is changed. Assosciated with listView_Contents_ItemSelectionChanged() ---
        /// Fires after some delay (100ms) and if there still is nothing selected, calls UpdateDisplayFileDateTime().
        /// </summary>
        private void _ItemSelectionChangedTimer_Tick(object sender, EventArgs e)
        {
            if (listView_Contents.SelectedItems.Count == 0)
                UpdateDisplayFileDateTime();
        }

        /// <summary>
        /// The Contents listview (right) selection has changed, with a timer-based solution to decide if there was a re-selection within 100ms
        /// </summary>
        private void listView_Contents_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (listView_Contents.SelectedItems.Count == 1)
                UpdateDisplayFileDateTime();
            else
            {
                itemSelectionChangedTimer.Interval = 100;
                itemSelectionChangedTimer.Tick += _ItemSelectionChangedTimer_Tick;
                itemSelectionChangedTimer.Start();
            }
        }

        #endregion

        #region Form EventHandlers Functions, tabcontrol and tooltip
    //EVENT HANDLERS
        private void label1_CreationDate_Click(object sender, EventArgs e)
        {
            radioButton1_CreationDate.Checked = radioGroupBox2_CurrentSelectionTime.Checked;
        }

        private void label2_ModifiedDate_Click(object sender, EventArgs e)
        {
            radioButton2_ModifiedDate.Checked = radioGroupBox2_CurrentSelectionTime.Checked;
        }

        private void label3_AccessDate_Click(object sender, EventArgs e)
        {
            radioButton3_LastAccessDate.Checked = radioGroupBox2_CurrentSelectionTime.Checked;
        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            var page = Convert.ToBoolean(e.TabPageIndex);
            checkBoxShouldFiles.Visible = !page;
            checkBox_Recurse.Visible = !page;
            panel1.Visible = !page;
        }

        private void tabControl1_MouseHover(object sender, EventArgs e)
        {
            toolTip1.Show("Mode 1: Standard mode: Set child folders/files\n" +
                          "Mode 2: Upward mode: Set Parent Folders based on Files/Dirs Inside",
                tabControl1);
        }


        #endregion

        #region Mode2 code

        /// <summary>
        /// Mode 2 Update-Button Command. This runs a LONG process on the folders/files. It decides which time to use, 
        /// then adds them to the confirm list to be handled by the form_confirm window (part2).
        /// </summary>
        private void GoUpdateDateTimeMode2()
        {
            //initialize/clear
            FilestoConfirmList.Clear();

            _skippedHiddenCount = 0;
            _skippedReadOnlyCount = 0;
            _skippedSystemCount = 0;

            if (Settings.Default.useRootDirAsContainer)
                RecurseSubDirectoryMode2B(label_FPath.Text);
            else
            {
                RecurseSubDirectoryMode2(label_FPath.Text);
            }
            

            var itemsSkippedCount = _skippedHiddenCount + _skippedReadOnlyCount + _skippedSystemCount;
            string skippedmessage = "";
            if (itemsSkippedCount > 0)
            {
                skippedmessage += "There were " + _skippedSystemCount + " System files/directories skipped.\n";
                skippedmessage += "There were " + _skippedHiddenCount + " Hidden files/directories skipped.\n";
                skippedmessage += "There were " + _skippedReadOnlyCount + " Read-Only files/directories skipped.";
                MessageBox.Show(skippedmessage, "Info Log", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            //Code to show files to be changed in the confirmation window (Show Form 2)
            if (!Confirmation.Visible)
            {
                Confirmation = new Form_Confirm(this);
                Confirmation.Show();
            }
            else
                Confirmation.MakeListView();
        }



        private void RecurseSubDirectoryMode2(string directoryPath)
        {
            NameDateObject timeInside = DecideWhichTimeMode2(directoryPath);
            SetFolderDateTimeMode2(directoryPath, timeInside);
            try
            {
                foreach (string subfolder in Directory.GetDirectories(directoryPath))
                    RecurseSubDirectoryMode2(Path.Combine(directoryPath, subfolder));
            }
            catch (UnauthorizedAccessException)
            {}
            catch (DirectoryNotFoundException)
            {}
        }

        private void RecurseSubDirectoryMode2B(string directoryPath)
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
        /// Very long function that does a simple task. Read in the options the user set for the operation, and
        /// Decide on the timestamp it should use, by the end we will have a single object with 3 times.
        /// This will need to be hit with broad strokes if we attempt to do any more work on the program.
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <returns></returns>
        private NameDateObject DecideWhichTimeMode2(string directoryPath)
        {
            var extractlist = new List<string>();

            var timelist = new List<NameDateObject>();
            var thingtoreturn = new NameDateObject();

            if (radioGroupBox1_SpecifyTime.Checked)
            {
                thingtoreturn.Name = labelHidden_PathName.Text;
                var specifiedDate = DateTime.Parse(dateTimePicker_Date.Value.Date.ToString("d") + " " +
                                                   dateTimePicker_Time.Value.Hour + ":" +
                                                   dateTimePicker_Time.Value.Minute + ":" +
                                                   dateTimePicker_Time.Value.Second);
                thingtoreturn.Created = specifiedDate;
                thingtoreturn.Modified = specifiedDate;
                thingtoreturn.Accessed = specifiedDate;
            }

            else if (radioGroupBox2_CurrentSelectionTime.Checked)
            {
                thingtoreturn.Name = labelHidden_PathName.Text;
                thingtoreturn.Created = DateTime.Parse(label_CreationTime.Text);
                thingtoreturn.Modified = DateTime.Parse(label_Modified.Text);
                thingtoreturn.Accessed = DateTime.Parse(label_LastAccess.Text);
            }
                //Begin checking Conditional for which file is newest oldest etc
            else if (radioGroupBox3_UseTimeFrom.Checked)
            {
                //decide if they wanted to use time from subfile or subdir
                if (radioButton1_useTimefromFile.Checked)
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
                    {} //show nothing because this is normal for this method when encountering inaccessible files
                }
                else if (radioButton2_useTimefromSubdir.Checked)
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
                    {} //show nothing because this is normal for this method when encountering inaccessible dirs
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
                    var decidetemp = new NameDateObject {Name = fullpath};
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
                    {}
                }
                //make 3 new lists, one for each date containing every NameDateObject
                var creationtimelist = new List<DateTime?>();
                var modtimelist = new List<DateTime?>();
                var accesstimelist = new List<DateTime?>();
                //populate the new seperated lists with the times from the combinedobject list (timelist)
                foreach (NameDateObject timeobject in timelist)
                {
                    creationtimelist.Add((DateTime?) timeobject.Created);
                    modtimelist.Add((DateTime?) timeobject.Modified);
                    accesstimelist.Add((DateTime?) timeobject.Accessed);
                }
                //Make a new list of the lists we just made (Collection initializer)
                var threetimelists = new List<List<DateTime?>> {creationtimelist, modtimelist, accesstimelist};
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

                if (radioButton1_Oldest.Checked)
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
                else if (radioButton2_Newest.Checked)
                {
                    //set each attribute to NEWest date from EACH attribute
                    thingtoreturn.Name = "Mode 2: Three Different Filenames"; // note to self.
                    thingtoreturn.Created = cre.MaxDate;
                    thingtoreturn.Modified = mod.MaxDate;
                    thingtoreturn.Accessed = acc.MaxDate;
                }
                else if (radioButton3_Random.Checked)
                {
                    //Mode A: (old) - removed the following 4 radio buttons
                    //pick a subfile/dir at random, then pick an attribute(C,M,A) at random
                    //    int randomindex = random.Next(0, timelist.Count);
                    //    filenameused = timelist[randomindex].Name;

                    //    if (radioButton1_setfromCreated.Checked)
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


        /// <summary>
        /// Go through the list, skipping H,S,R files, and add all the file+date objects to the Confirmation List
        /// </summary>
        /// <param name="folderPath"></param>
        /// <param name="subFile"></param>
        private void SetFolderDateTimeMode2(string folderPath, NameDateObject subFile)
        {
            FileAttributes folderAttributes = File.GetAttributes(folderPath);

            if (((folderAttributes & FileAttributes.System) == FileAttributes.System) && Settings.Default.SkipSystem)
            {
                _skippedSystemCount++;
                return; // Skip system files and directories
            }
            if (((folderAttributes & FileAttributes.Hidden) == FileAttributes.Hidden) && Settings.Default.SkipHidden)
            {
                _skippedHiddenCount++;
                return; // Skip hidden files and directories 
            }
            if ((folderAttributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
            {
                if (Settings.Default.ShowNoticesReadOnly)
                {
                    DialogResult dr =
                        MessageBox.Show(
                            "The folder '" + folderPath +
                            "' is Read-Only and was skipped.\n\nContinue showing Read-Only notifications?",
                            "Read-Only", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    Settings.Default.ShowNoticesReadOnly = (dr == DialogResult.OK);
                }
                if (Settings.Default.SkipReadOnly)
                {
                    _skippedReadOnlyCount++;
                    return;
                }
            }

            subFile = Makedateobject(folderPath, subFile);
            FilestoConfirmList.Add(subFile);
        }

        #endregion

    }
}