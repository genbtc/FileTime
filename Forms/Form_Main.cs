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
using System.IO;
using System.Windows.Forms;
using genBTC.FileTime.Classes;
using genBTC.FileTime.Classes.Native;
using genBTC.FileTime.Models;
using genBTC.FileTime.Properties;

namespace genBTC.FileTime
{
    /// <summary>
    /// GUI : Main Form Window of the Program.
    /// </summary>
    public partial class Form_Main
    {
        public DataModel _dataModel;
        //Form 2 stuff we need to have in Form1.
        /// <summary> Stub for Form 2 to be accessed once it is opened. </summary>
        public Form_Confirm Confirmation;

        #region Main startup/load code
        /// <summary>Init the main startup form </summary>
        public Form_Main()
        {
            //The DataModel can be loaded here.
            _dataModel = new DataModel();
            //The form 2 can be created here.
            Confirmation = new Form_Confirm(this);

            //Required for Windows Form Designer support
            InitializeComponent();
            //Reason Explanation: This is needed AFTER InitializeComponent because SplitterDistance must be declared FIRST. The designer
            // sorts by alphabetical order and puts P for Panel2Minsize before S for SplitterDistance throwing an exception.
            splitContainer1.Panel2MinSize = 150;
        }

        /// <summary> After the InitializeComponent() loads the designer, the designer will fire an event handler signifying its done loading</summary>
        private void Form_Main_Load(object sender, EventArgs e)
        {
            Form_Main_Run();
        }

        /// <summary>
        /// At this point we can run our code (that wants to interact with our designer GUI).
        /// </summary>
        private void Form_Main_Run()
        {
            try
            {
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
        #endregion Main startup/load code

        #region Helper functions...

        /// <summary>  Return the value of the checkboxes "CMA Time"   </summary>
        private BoolCMA QueryCMAcheckboxes()
        {
            var checkboxes = new BoolCMA
            {
                C = checkBox_CreationDateTime.Checked,
                M = checkBox_ModifiedDateTime.Checked,
                A = checkBox_AccessedDateTime.Checked
            };
            return checkboxes;
        }

        /// <summary>
        /// Clear both ListViews and the three data container lists, blanks the selected date, and erases the top current dir textbox.
        /// </summary>
        private void ClearOnError()
        {
            _dataModel.Clear();
            listView_Contents.Items.Clear();

            DisplayCma("");
        }

        private void DisplayCma(string path)
        {
            var cma = GetCmaTimes(path);

            label_CreationTime.Text = cma.Created;
            label_ModificationTime.Text = cma.Modified;
            label_AccessedTime.Text = cma.Accessed;
            labelHidden_PathName.Text = cma.HiddenPathName;
            radioGroupBox2_CurrentSelectionTime.Enabled = cma.Selected;
            if (!radioGroupBox2_CurrentSelectionTime.Enabled)
                radioGroupBox1_SpecifyTime.Checked = true;
            label_FPath.Text = "";
            itemSelectionChangedTimer.Stop();
        }

        /// <summary> Only enable the update button if something is selected </summary>
        private void UpdateButtonEnable()
        {
            button_GoUpdate.Enabled =
                (checkBox_CreationDateTime.Checked |
                 checkBox_ModifiedDateTime.Checked |
                 checkBox_AccessedDateTime.Checked);
        }

        #endregion Helper functions...

        #region Buttons

        private void button_Browse_Click(object sender, EventArgs e)
        {
            string path = OpenFile(label_FPath.Text);
            //if (path == label_FPath.Text) //if nothing was changed
            explorerTree1.SetCurrentPath(path);
            explorerTree1.BrowseTo();
        }

        private void button_Update_Click(object sender, EventArgs e)
        {
            StartUpBothModes1And2(tabControl1.SelectedIndex, label_FPath.Text);
            string comparefolder;
            if (radioGroupBox1_pickFolderForCompare.Checked)
                comparefolder = OpenFile(label_FPath.Text);
        }

        #endregion Buttons

        #region Menu...
        //Menu items:
        private void menuItem_FileOpen_Click(object sender, EventArgs e)
        {
            OpenFile(label_FPath.Text);
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

        #endregion Menu...

        #region >>Main Logic Code<<

        /// <summary>
        /// Display subfiles and subdirectories in the right panel listview
        /// </summary>
        /// <param name="filesonly">Don't show the directories, only files.</param>
        /// reqs: listviewcontents, contentsDirList,contentsFileList, labelFpathText, checkbox_recurse, filextlist, imageList_Files
        private void DisplayContentsList(bool filesonly = true)
        {
            //Clear the contents UI + containers
            listView_Contents.Items.Clear();
            _dataModel.contentsDirList.Clear();
            _dataModel.contentsFileList.Clear();

            string directoryName = label_FPath.Text;

            if (!Directory.Exists(directoryName))
                return;

            if (!filesonly && checkBox_Recurse.Checked)
            {
                //part 1: list and store all the subdirectories
                try
                {
                    PopulateDirList(directoryName, _dataModel);
                }
                catch (UnauthorizedAccessException)
                { }
                //Sort them
                _dataModel.contentsDirList.Sort(explorerStringComparer());
                //Add them to the listview.
                foreach (string subDirectory in _dataModel.contentsDirList)
                {
                    // Display all the sub directories using the directory icon (enum 1)
                    listView_Contents.Items.Add(subDirectory, (int)ListViewIcon.Directory);
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
                        if (_dataModel.filextlist.FindLastIndex(SharedHelper.FindCurExt) == -1)
                        {
                            _dataModel.filextlist.Add(SharedHelper.CurrExten);
                            //call NativeExtractIcon to get the filesystem icon of the filename
                            imageList_Files.Images.Add(SharedHelper.CurrExten, NativeExtractIcon.GetIcon(file, true));
                        }
                    }
                    else //if it is a shortcut, grab icon directly.
                        imageList_Files.Images.Add(justName, NativeExtractIcon.GetIcon(file, true));

                    _dataModel.contentsFileList.Add(justName);
                }
            }
            catch (UnauthorizedAccessException)
            { }
            //Sort them
            _dataModel.contentsFileList.Sort(explorerStringComparer());
            //Add them to the listview.
            foreach (string file in _dataModel.contentsFileList)
            {
                string exten = Path.GetExtension(file);
                listView_Contents.Items.Add(file, exten != ".lnk" ? exten : file);
            }
        }

        /// <summary>
        /// Display file's times in the Tri-Textbox bottom UI for the currently selected file.
        /// </summary>
        /// <param name="listViewContents"></param>
        /// <param name="labelFPath"></param>
        private void UpdateDisplayFileDateTime(ListView listViewContents, Label labelFPath)
        {
            //If none of the below conditions are true, "" means blank date/time during displayCMA()
            string pathName = "";

            if (listViewContents.SelectedItems.Count == 1 && listViewContents.SelectedItems[0].ImageIndex != 1)
                // If one file is selected
                pathName = Path.Combine(labelFPath.Text, listViewContents.SelectedItems[0].Text);
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
                UpdateDisplayFileDateTime(listView_Contents, label_FPath);
        }

        /// <summary>
        /// The Contents listview (right) selection has changed, with a timer-based solution to decide if there was a re-selection within 100ms
        /// </summary>
        private void listView_Contents_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (listView_Contents.SelectedItems.Count == 1)
                UpdateDisplayFileDateTime(listView_Contents, label_FPath);
            else
            {
                itemSelectionChangedTimer.Interval = 100;
                itemSelectionChangedTimer.Tick += _ItemSelectionChangedTimer_Tick;
                itemSelectionChangedTimer.Start();
            }
        }

        /// <summary>
        /// Launches Mode 1 and Mode 2. This runs a LONG process on the folders/files. It decides which time to use,
        /// then adds them to the confirm list to be handled by the form_confirm window (part2).
        /// </summary>
        /// reqs: Datamodel , label_Fpathtext
        private void StartUpBothModes1And2(int mode2, string startingdir)
        {
            _dataModel.FilestoConfirmList.Clear();

            _dataModel.Skips.H = 0;
            _dataModel.Skips.R = 0;
            _dataModel.Skips.S = 0;

            var gui = GetGUIRadioButtonStatusData();

            switch (mode2)
            {
                case (0):
                    if (Confirmation.active > 0)
                        Confirmation.FixReadonlyResults();

                    if (Settings.Default.mode1addrootdir)
                    {
                        DateTime? nullorfileDateTime = DecideWhichTimeMode1(startingdir, gui, _dataModel);
                        if (nullorfileDateTime == null) //if nothing could be decided, exit, otherwise continue
                        {
                            MessageBox.Show("Error! Nothing to decide time from. \n" +
                                            "Sub-File or Sub-Dir button requires files/dirs to be SHOWN on the right-side Contents panel...",
                                "PEBKAC Error ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        var fileDateTime = (DateTime)nullorfileDateTime;

                        SetFileDateTimeMode1(_dataModel, QueryCMAcheckboxes(), startingdir, fileDateTime, true);
                    }

                    //TODO: where I should add worker process
                    if (Settings.Default.useRootDirAsContainer)
                        RecurseSubDirectoryMode1B(startingdir, checkBox_Recurse.Checked, checkBoxShouldFiles.Checked, QueryCMAcheckboxes(), _dataModel, gui);
                    else
                        RecurseSubDirectoryMode1(startingdir, checkBox_Recurse.Checked, checkBoxShouldFiles.Checked, QueryCMAcheckboxes(), _dataModel, gui);
                    //end worker process
                    break;

                case (1):
                    if (Settings.Default.useRootDirAsContainer)
                        RecurseSubDirectoryMode2B(startingdir, _dataModel, QueryCMAcheckboxes(), gui);
                    else
                        RecurseSubDirectoryMode2(startingdir, _dataModel, QueryCMAcheckboxes(), gui);
                    break;
            }

            var itemsSkippedCount = _dataModel.Skips.H + _dataModel.Skips.R + _dataModel.Skips.S;
            if (itemsSkippedCount > 0)
            {
                string skippedmessage = "";
                skippedmessage += "There were " + _dataModel.Skips.S + " System files/directories skipped.\n";
                skippedmessage += "There were " + _dataModel.Skips.H + " Hidden files/directories skipped.\n";
                skippedmessage += "There were " + _dataModel.Skips.R + " Read-Only files/directories skipped.";
                MessageBox.Show(skippedmessage, "Info Log", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            //Launch Form 2
            LaunchForm2Go();
        }

        /// <summary> Show files to be changed in the confirmation window (Show Form 2) </summary>
        private void LaunchForm2Go()
        {
            if (!Confirmation.Visible)
            {
                Confirmation = new Form_Confirm(this);
                Confirmation.Show();
            }
            else
                Confirmation.MakeListView();
        }

        #endregion >>Main Logic Code<<

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
            radioButton3_AccessedDate.Checked = radioGroupBox2_CurrentSelectionTime.Checked;
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

        #endregion Form EventHandlers Functions, tabcontrol and tooltip

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
        private void explorerTree1_PathChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            label_FPath.Text = explorerTree1.CurrentPath;
            if (!label_FPath.Text.EndsWith(Seperator.ToString()))
                label_FPath.Text += Seperator;
            DisplayContentsList();
            Cursor.Current = Cursors.Default;
        }

        #endregion Event functions: Onselected ListView main panels & checkbox
    }
}