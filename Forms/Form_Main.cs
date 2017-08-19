/*
 * ###  This file is codebehind for the Main program window ###
 *
 *   /// FileTime: Sets the creation, last write and last access date and time of user selection with ious options
 *   /// Version: 1.0
 *   /// Date: 17 July 2014, Last Modified: 8/26/2014
 *   ///               comments+Modified    8/13-14/2017
 *   ///                   Heavily Modified 8/15/2017
 *   ///                        Modified    8/16-17/2017
 * ###  Author: genBTC ###
 */


using System;
using System.IO;
using System.Windows.Forms;
using genBTC.FileTime.Classes;
using genBTC.FileTime.Models;
using genBTC.FileTime.Properties;

namespace genBTC.FileTime.Forms
{
    /// <summary>
    /// GUI : Main Form Window of the Program.
    /// </summary>
    #region FORM_MAIN1

    public partial class Form_Main
    {
        #region Vars., guistatus = GetGUIRadioButtonStatusData(), _dataModel
        private guistatus GetGUIRadioButtonStatusData()
        {
            guistatus radios = new guistatus
            {
                radioGroupBox1SpecifyTime = radioGroupBox1_SpecifyTime.Checked,
                radioGroupBox2CurrentSelect = radioGroupBox2_CurrentSelectionTime.Checked,
                rg2rb1Creation = radioButton1_CreationDate.Checked,
                rg2rb2Modified = radioButton2_ModifiedDate.Checked,
                rg2rb3LastAccess = radioButton3_AccessedDate.Checked,
                radioGroupBox3UseTimeFrom = radioGroupBox3_UseTimeFrom.Checked,
                Created = label_CreationTime.Text,
                Modified = label_ModificationTime.Text,
                Accessed = label_AccessedTime.Text,
                radioButton1_useTimefromFile = radioButton1_useTimefromFile.Checked,
                radioButton2_useTimefromSubdir = radioButton2_useTimefromSubdir.Checked,
                radioButton1_setfromCreated = radioButton1_setfromCreation.Checked,
                radioButton2_setfromModified = radioButton2_setfromModified.Checked,
                radioButton3_setfromAccessed = radioButton3_setfromAccessed.Checked,
                radioButton4_setfromRandom = radioButton4_setfromRandom.Checked,
                radioButton1Oldest = radioButton1_Oldest.Checked,
                radioButton2Newest = radioButton2_Newest.Checked,
                radioButton3Random = radioButton3_Random.Checked,
                dateTimePickerDate = dateTimePicker_Date.Value,
                dateTimePickerTime = dateTimePicker_Time.Value,
                PathName = labelHidden_PathName.Text,
                checkboxRecurse = checkBox_Recurse.Checked,
                checkboxShouldFiles = checkBoxShouldFiles.Checked
            };

            return radios;
        }
       
        /// <summary>  A model of the data. Self sufficient.   </summary>
        internal DataModel _dataModel;
 
        /// <summary> Stub for Form 2 to be accessed once it is opened. </summary>
        public Form_Confirm Confirmation;

        /// <summary>  a Timer to keep track of the selected line, prevents clicking too fast (timeout)</summary>
        private readonly Timer _itemSelectionChangedTimer = new Timer();
        /// <summary> Timeout in ms for ListView selection-box's repeat mouse click rejection </summary>
        private const int SELECTION_TIMEOUT = 100;

        #endregion

        #region Form_Main startup/load code, Main,Load,Run, +binding to CwdPathName
        /// <summary>Init the main startup form </summary>
        public Form_Main()
        {
            //Required for Windows Form Designer support
            InitializeComponent();
            //Reason Explanation: This is needed AFTER InitializeComponent because SplitterDistance must be declared FIRST. The designer
            // sorts by alphabetical order and puts P for Panel2Minsize before S for SplitterDistance throwing an exception.
            splitContainer1.Panel2MinSize = 150;
        }

        /// <summary> After the InitializeComponent() loads the designer, the designer will fire an event handler signifying its done loading</summary>
        private void Form_Main_Load(object sender, EventArgs e)
        {
            //Once the designer loads:
            //The DataModel can be loaded. (with 2 initializer params from .Designer.cs)
            _dataModel = new DataModel { listViewContents = listView_Contents, imageListFiles = imageList_Files};
            //The form 2 can now also be constructed.
            Confirmation = new Form_Confirm(_dataModel);

            Form_Main_Run();
        }

        /// <summary> Public Setter to the private variable in Datamodel.  </summary>
        public string CwdPathName
        {
            get => _dataModel.CwdPathName;
            set => _dataModel.CwdPathName = value;
        }
        /// <summary>
        /// The Text Box Data Binding is rebound.
        /// </summary>
        public void RefreshDataBinding_labelFPath()
        {
            label_FPath.DataBindings.Clear();
            label_FPath.DataBindings.Add("Text", this, "CwdPathName", false, DataSourceUpdateMode.OnPropertyChanged);
        }

        /// <summary>
        /// At this point we can run our code (that wants to interact with our designer GUI).
        /// </summary>
        private void Form_Main_Run()
        {
            try
            {
                explorerTree1.SetCurrentPath(Settings.Default.useStartupDir
                    ? Settings.Default.StartupDir.TrimEnd(SharedHelper.Seperator)
                    : SharedHelper.UserDesktop);
                explorerTree1.BrowseTo();
                RefreshContentsRightPanel();
            }
            catch //Catches any errors caused by starting up in a non existing directory, and make sure using the explorer tree is safe.
            {
                ClearOnError();
            }
        }

        #endregion

        #region Menu items...

        private void menuItem_FileOpen_Click(object sender, EventArgs e)
        {
            SharedHelper.OpenFilePicker(CwdPathName);
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
            var prefs = new Form_Preferences(CwdPathName);
            prefs.ShowDialog(this);
        }

        #endregion

        #region Form EventHandlers Functions, tabcontrol and tooltip
        
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
            bool page = Convert.ToBoolean(e.TabPageIndex);
            checkBoxShouldFiles.Visible = !page;
            checkBox_Recurse.Visible = !page;
            panel1.Visible = !page;
        }

        private void tabControl1_MouseHover(object sender, EventArgs e)
        {
            toolTip1.Show("Mode 1: Standard mode: Set child folders/files\n" +
                          "Mode 2: Folder mode: Set Parent Folders based on Files/Dirs Inside",
                tabControl1);
        }

        #endregion

        #region Event functions: Onselected ListView, Datetime checkbox, ExplorerTree reloads ContentsList

        /// <summary>
        /// Timer Event function. Used to prevent the currently selected Textboxes for the 3 Times from blanking
        /// when an item selection is changed. Assosciated with listView_Contents_ItemSelectionChanged() ---
        /// Fires after some delay (SELECTION_TIMEOUT ms) and if there still is nothing selected, calls CallDisplayCma().
        /// </summary>
        private void ItemSelectionChangedTimer_Tick(object sender, EventArgs e)
        {
            if (listView_Contents.SelectedItems.Count == 0)
                CallDisplayCma(listView_Contents, CwdPathName);
        }

        /// <summary>
        /// The Contents listview (right) selection has changed. Timer-based solution - block re-selection within SELECTION_TIMEOUTms
        /// </summary>
        private void listView_Contents_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (listView_Contents.SelectedItems.Count == 1)
                CallDisplayCma(listView_Contents, CwdPathName);
            else
            {
                _itemSelectionChangedTimer.Interval = SELECTION_TIMEOUT;
                _itemSelectionChangedTimer.Tick += ItemSelectionChangedTimer_Tick;
                _itemSelectionChangedTimer.Start();
            }
        }

        /// <summary> 
        /// Creation,Modified,Accessed Checkbox selection changed. Only enable the update button if something is selected 
        /// </summary>
        private void checkBox_DateTime_CheckedChanged(object sender, EventArgs e)
        {
            BoolCMA checkboxes = QueryCMAcheckboxes();
            button_GoUpdate.Enabled = (checkboxes.C | checkboxes.M | checkboxes.A);
            _dataModel.checkboxes = checkboxes;
        }

        /// <summary>
        /// Update the textlabel box for FilePath when the Explorer Tree path changes.
        /// (making sure it always ends in a \ seperator). Also causes a re-render of Contents!
        /// </summary>
        private void explorerTree1_PathChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            CwdPathName = explorerTree1.CurrentPath;
            if (!CwdPathName.EndsWith(SharedHelper.Seperator.ToString()))
                CwdPathName += SharedHelper.Seperator;
            // Display the folder path and the contents of it.
            RefreshContentsRightPanel();
            Cursor.Current = Cursors.Default;
            //Refresh Textbox Data Binding
            RefreshDataBinding_labelFPath();
        }

        #endregion Event functions: Onselected ListView main panels & checkbox

        #region Helper functions: RefreshContentsRightPanel, QueryCMAcheckboxes, ClearOnError, DisplayCma, CallDisplayCma

        /// <summary>
        /// Wrapper for DisplayContentsList that manages imageList and listView resources automatically.
        /// TODO: Can be moved to model later.
        /// </summary>
        internal void RefreshContentsRightPanel()
        { 
            // Display the folder path and the contents of it. 
            _dataModel.DisplayContentsList(checkBox_Recurse.Checked, CwdPathName);
            //Copy these 2 over manually. they need to get shoved into the UI
            //imageList_Files = _dataModel.imageListFiles;
            //listView_Contents = _dataModel.listViewContents;
        }

        /// <summary>  Return the value of the checkboxes "CMA Time"   </summary>
        private BoolCMA QueryCMAcheckboxes()
        {
            BoolCMA checkboxes = new BoolCMA
            {
                C = checkBox_CreationDateTime.Checked,
                M = checkBox_ModifiedDateTime.Checked,
                A = checkBox_AccessedDateTime.Checked
            };
            _dataModel.checkboxes = checkboxes;
            return checkboxes;
        }

        private void checkBox_Recurse_CheckedChanged(object sender, EventArgs e)
        {
            _dataModel.gui.checkboxRecurse = ((CheckBox)sender).Checked;
        }

        private void checkBoxShouldFiles_CheckedChanged(object sender, EventArgs e)
        {
            _dataModel.gui.checkboxShouldFiles = ((CheckBox)sender).Checked;
        }

        /// <summary>
        /// Clear both ListViews and the three data container lists, blanks the selected date, and erases the top current dir textbox.
        /// </summary>
        private void ClearOnError()
        {
            //Clear the datamodel + contents UI
            _dataModel.Clear(true);
            //Blank out the CMA, clear.
            DisplayCma("");
        }

        /// <summary>
        /// Display file's times in the Tri-Textbox bottom UI for the currently selected file.
        /// </summary>
        private void DisplayCma(string path)
        {
            NameDateStruct cma = DataModel.GetCmaTimesFromFilesystem(path);

            label_CreationTime.Text = cma.Created;
            label_ModificationTime.Text = cma.Modified;
            label_AccessedTime.Text = cma.Accessed;
            labelHidden_PathName.Text = cma.HiddenPathName;
            radioGroupBox2_CurrentSelectionTime.Enabled = cma.Selected;
            if (!radioGroupBox2_CurrentSelectionTime.Enabled)
                radioGroupBox1_SpecifyTime.Checked = true;
            _itemSelectionChangedTimer.Stop();
        }

        /// <summary>  Update the GUI - Display the DateTime for the currently selected file. Calls DisplayCma </summary>
        private void CallDisplayCma(ListView listViewContents, string srcPath)
        {
            //Default: "" means blank date/time during DisplayCma(); If none of the below conditions are true.
            string pathName = "";
            //If one file is selected:
            if (listViewContents.SelectedItems.Count == 1 && listViewContents.SelectedItems[0].ImageIndex != 1)
                pathName = Path.Combine(srcPath, listViewContents.SelectedItems[0].Text);
            //Always Call the display date/time function
            DisplayCma(pathName);
        }

        #endregion

        #region Buttons (Browse & Update)

        private void button_Browse_Click(object sender, EventArgs e)
        {
            string path = SharedHelper.OpenFilePicker(CwdPathName);
            //safer when we get the path out of a file picker.
            explorerTree1.SetCurrentPath(path);
            explorerTree1.BrowseTo();
        }

        private void button_Update_Click(object sender, EventArgs e)
        {
            if (!radioGroupBox4_pickFolderForCompare.Checked && !radioGroupBox3_UseTimeFrom.Checked && !radioGroupBox2_CurrentSelectionTime.Checked && !radioGroupBox1_SpecifyTime.Checked)
            {
                MessageBox.Show("Error! Nothing to decide time from!! \n" +
                                "Please choose an Option, to the right of the Mode.",
                    "PEBKAC Error ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string comparefolder;
            if (radioGroupBox4_pickFolderForCompare.Checked)
            {
                comparefolder = SharedHelper.OpenFilePicker(CwdPathName);
                StartUpBothModes1And2(tabControl1.SelectedIndex, CwdPathName);
            }
            else
                StartUpBothModes1And2(tabControl1.SelectedIndex, CwdPathName);
        }

        #endregion

        #region >>Main Logic Code<<

        /// <summary>
        /// Mode 3. Recursive, file Tree time compare mode.
        /// </summary>
        /// <param name="mode2"></param>
        /// <param name="startingdir"></param>
        private void StartUpMode3(int mode2, string startingdir)
        {
            PrepareDataModelForUse();
        }

        /// <summary>
        /// Launches Mode 1 and Mode 2. This runs a LONG process on the folders/files. It decides which time to use,
        /// then adds them to the confirm list to be handled by the form_confirm window (part2). Cant move into the model.
        /// This is a nice staging point place for making EVERYTHING happen.
        /// </summary>
        private void StartUpBothModes1And2(int mode2, string startingdir)
        {
            //Erase the datamodel and stuff. Very important.
            PrepareDataModelForUse();

            switch (mode2)
            {
                case (0):
                    if (_dataModel.fixreadonlyActive > 0)
                        _dataModel.FixReadonlyResults();

                    if (Settings.Default.mode1addrootdir)
                    {
                        DateTime? nullorfileDateTime = _dataModel.DecideWhichTimeMode1(startingdir);
                        if (nullorfileDateTime == null) //if nothing could be decided, exit, otherwise continue
                        {
                            MessageBox.Show("Error! Nothing to decide time from. \n" +
                                            "Sub-File or Sub-Dir button requires files/dirs to be SHOWN on the right-side Contents panel...",
                                "PEBKAC Error ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        _dataModel.AddONEFiletoConfirmList(startingdir, (DateTime)nullorfileDateTime, true);
                    }

                    //TODO: where I should add worker process
                    if (Settings.Default.useRootDirAsContainer)
                    {
                        try
                        {
                            foreach (string subfolder in Directory.GetDirectories(startingdir))
                                _dataModel.RecurseSubDirectoryMode1(Path.Combine(startingdir, subfolder));
                        }
                        catch (UnauthorizedAccessException)
                        { }
                        catch (DirectoryNotFoundException)
                        { }
                    } else
                        _dataModel.RecurseSubDirectoryMode1(startingdir);
                    //end worker process
                    break;

                case (1):
                    //TODO: here too:
                    if (Settings.Default.useRootDirAsContainer)
                    {
                        try
                        {
                            foreach (string subfolder in Directory.GetDirectories(startingdir))
                                _dataModel.RecurseSubDirectoryMode2(Path.Combine(startingdir, subfolder));
                        }
                        catch (UnauthorizedAccessException)
                        { }
                        catch (DirectoryNotFoundException)
                        { }
                    } else
                        _dataModel.RecurseSubDirectoryMode2(startingdir);
                    break;
                //TODO: THREEDO: combine both.
            }

            int itemsSkippedCount = _dataModel.Skips.H + _dataModel.Skips.R + _dataModel.Skips.S;
            if (itemsSkippedCount > 0)
            {
                string skippedmessage = "";
                skippedmessage += "There were " + _dataModel.Skips.S + " System files/directories skipped.\n" + 
                                  "There were " + _dataModel.Skips.H + " Hidden files/directories skipped.\n" + 
                                  "There were " + _dataModel.Skips.R + " Read-Only files/directories skipped.";
                MessageBox.Show(skippedmessage, "Info Log", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            //Launch Form 2
            LaunchForm2Go();
        }

        /// <summary>
        /// Resets the files-to-confirm list, Skips count, reads GUIstatus data (radios), reads CMA checkboxes (lazy/dupe gui fields)
        /// </summary>
        private void PrepareDataModelForUse()
        {
            _dataModel.FilestoConfirmList.Clear();
            _dataModel.Skips.Reset();
            _dataModel.gui = GetGUIRadioButtonStatusData();
            _dataModel.checkboxes = QueryCMAcheckboxes();
        }

        /// <summary> Show files to be changed in the confirmation window (Show Form 2) </summary>
        private void LaunchForm2Go()
        {
            if (Confirmation.Visible)
            {
                Confirmation.MakeListView();
            } else
            {
                Confirmation = new Form_Confirm(_dataModel);
                Confirmation.Show();
            }
        }

        #endregion >>Main Logic Code<<
        // Program Logic continues in FORM_CONFIRM.cs
    }
    #endregion
}