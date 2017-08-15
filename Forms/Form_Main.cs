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
using System.Threading;
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
        #endregion //Main Startup Code

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
            label_Modified.Text = cma.Modified;
            label_LastAccess.Text = cma.Accessed;
            labelHidden_PathName.Text = cma.HiddenPathName;
            radioGroupBox2_CurrentSelectionTime.Enabled = cma.RadioGroupBox2_CurrentSelectionTime_Enabled;
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
            StartUpBOTHModes1and2(tabControl1.SelectedIndex);
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
                {}
                //Sort them
                _dataModel.contentsDirList.Sort(explorerStringComparer);
                //Add them to the listview.
                foreach (string subDirectory in _dataModel.contentsDirList)
                {
                    // Display all the sub directories using the directory icon (enum 1)
                    listView_Contents.Items.Add(subDirectory, (int) ListViewIcon.Directory);
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
            {}
            //Sort them
            _dataModel.contentsFileList.Sort(explorerStringComparer);
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
        private void StartUpBOTHModes1and2(int Mode2)
        {
            _dataModel.FilestoConfirmList.Clear();

            _dataModel.Skips.H = 0;
            _dataModel.Skips.R = 0;
            _dataModel.Skips.S = 0;

            switch (Mode2)
            {
                case (0):
                    if (Confirmation.active > 0)
                        Confirmation.FixReadonlyResults();
                    string startingdir = label_FPath.Text;
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

                        SetFileDateTimeMode1(_dataModel, QueryCMAcheckboxes(), startingdir, fileDateTime, true);
                    }

                    //TODO: where I should add worker process
                    if (Settings.Default.useRootDirAsContainer)
                        RecurseSubDirectoryMode1B(startingdir);
                    else
                        RecurseSubDirectoryMode1(startingdir);
                    //end worker process
                    break;
                case (1):
                    if (Settings.Default.useRootDirAsContainer)
                        RecurseSubDirectoryMode2B(label_FPath.Text);
                    else
                        RecurseSubDirectoryMode2(label_FPath.Text);
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

        #region Mode 1 Specific code
        /// <summary>
        /// Process One directory, with recursive sub-directory support. Calls SetFileDateTime() 
        /// (Only adds to the confirm list, Form 2 will actually write changes).
        /// </summary>
        /// <param name="directoryPath">Full path to the directory</param>
        /// req, checkBox_Recurse.Checked, checkBoxShouldFiles.Checked
        private void RecurseSubDirectoryMode1(string directoryPath)
        {
            DateTime? nullorfileDateTime = DecideWhichTimeMode1(directoryPath);
            if (nullorfileDateTime == null)
                return; //if nothing could be decided, exit. otherwise continue
            var fileDateTime = (DateTime)nullorfileDateTime;

            // Set the date/time for each sub directory but only if "Recurse Sub-Directories" is checkboxed.
            if (checkBox_Recurse.Checked)
                SetTimeDateEachDirectory(directoryPath, fileDateTime);
            // Set the date/time for each file, but only if "Perform operation on files" is checkboxed.
            if (checkBoxShouldFiles.Checked)
                SetTimeDateEachFile(directoryPath, fileDateTime);
        }

        //this looks strikingly similar 
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
            _dataModel.contentsDirList.Clear();
            _dataModel.contentsFileList.Clear();

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
                dateToUse = DecideTimeFromSubDirFile(path, _dataModel, radioButton1_useTimefromFile, radioButton2_useTimefromSubdir, radioButton1_setfromCreated, radioButton2_setfromModified, radioButton3_setfromAccessed, radioButton4_setfromRandom, radioButton1_Oldest, radioButton2_Newest, radioButton3_Random);
            }
            return dateToUse;
        }


        /// <summary>
        /// Set Directory Time
        /// </summary>
        private void SetTimeDateEachDirectory(string directoryPath, DateTime fileDateTime)
        {
            try
            {
                string[] subDirectories = Directory.GetDirectories(directoryPath);
                Array.Sort(subDirectories, explorerStringComparer);
                foreach (string eachdir in subDirectories)
                {
                    // Set the date/time for the sub directory
                    SetFileDateTimeMode1(_dataModel, QueryCMAcheckboxes(), eachdir, fileDateTime, true);
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
        private void SetTimeDateEachFile(string directoryPath, DateTime fileDateTime)
        {
            try
            {
                string[] subFiles = Directory.GetFiles(directoryPath);
                Array.Sort(subFiles, explorerStringComparer);
                foreach (string filename in subFiles)
                    SetFileDateTimeMode1(_dataModel, QueryCMAcheckboxes(), filename, fileDateTime, false);
            } //catch for GetFiles
            catch (UnauthorizedAccessException)
            { }
        }

        #endregion

        #region Mode 2 Specific code

        private void RecurseSubDirectoryMode2(string directoryPath)
        {
            NameDateObject timeInside = DecideWhichTimeMode2(directoryPath);
            
            SetFolderDateTimeMode2(_dataModel, QueryCMAcheckboxes(), directoryPath, timeInside);
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
                    int randomindex = _dataModel.random.Next(0, timelist.Count);
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

        #endregion

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

        #endregion //Listbox + Checkbox Functions

    }
}