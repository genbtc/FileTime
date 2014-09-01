using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;


namespace automationControls.FileTime
{
	/// <summary>
	/// Sets the creation, last write and last access date and time of user selection with various options
	/// Version: 1.0
	/// Date: 17 July 2014
	/// Author: genBTC
	/// </summary>


	public class Form_Main : Form
	{


        static char seperator =  System.IO.Path.DirectorySeparatorChar;
        static string homeFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + seperator;

        private List<string> folderList;
        private List<string> contentsDirList;
        private List<string> contentsFileList;

        private Button button_Browse;
		private MainMenu mainMenu_Main;
		private MenuItem menuItem_FileExit;
		private MenuItem menuItem_Help;
		private MenuItem menuItem_HelpAbout;
		private Button button_GoUpdate;
		private MenuItem menuItem_FileOpen;
		private CheckBox checkBox_CreationDateTime;
		private CheckBox checkBox_LastAccessDateTime;
		private CheckBox checkBox_LastWriteDateTime;
        private Label label_FPath;
        private DateTimePicker dateTimePicker_Time;
		private ListView listView_Folders;
		private ColumnHeader columnHeader_FileName;
		private ImageList imageList_Files;
		private CheckBox checkBox_Recurse;
		private MenuItem menuItem_File;
		private MenuItem menuItem_FileBar01;
		private MenuItem menuItem_HelpContents;
		private MenuItem menuItem_HelpBar01;
        private Label label_FilePathHeading;
        private DateTimePicker dateTimePicker_Date;
        private ListView listView_Contents;
        private ColumnHeader listview_ContentsColumHeader1;
        private Button buttonParentDir;
        private CheckBox checkBoxSimulation;
        private CheckBox checkBoxShouldFiles;
        private GroupBox groupBoxCMA;
        private GroupBox groupBoxConditions;
        private RadioButton radioButton3All;
        private RadioButton radioButton2Older;
        private RadioButton radioButton1Newer;
        private RadioButton radioButton1_CreationTimeHeading;
        private Label label_CreationTime;
        private RadioButton radioButton2_ModifiedHeading;
        private Label label_Modified;
        private RadioButton radioButton3_LastAccessHeading;
        private Label label_LastAccess;
        private RadioButton radioButton3_setfromAccessed;
        private RadioButton radioButton2_setfromModified;
        private RadioButton radioButton1_setfromCreated;
        private UIToolbox.RadioButtonPanel radioButtonPanel1;
        private UIToolbox.RadioGroupBox radioGroupBox3_UseTimeFrom;
        private UIToolbox.RadioGroupBox radioGroupBox1_SpecifyTime;
        private Panel panel1;
        private Panel panel2;
        private RadioButton radioButton5_Last;
        private RadioButton radioButton4_First;
        private RadioButton radioButton3_Newest;
        private RadioButton radioButton2_Oldest;
        private RadioButton radioButton1_Selected;
        private UIToolbox.RadioGroupBox radioGroupBox2_CurrentSelectionTime;
        private Label label3;
        private Label label2;
        private Label label1;
        private UIToolbox.RadioButtonPanel radioButtonPanel2;
		private System.ComponentModel.IContainer components;



		/// <summary>
		/// Init the main startup form
		/// </summary>
		public Form_Main()
		{
			//
			// Required for Windows Form Designer support
			//
            InitializeComponent();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_Main));
            this.label_FilePathHeading = new System.Windows.Forms.Label();
            this.button_Browse = new System.Windows.Forms.Button();
            this.dateTimePicker_Date = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker_Time = new System.Windows.Forms.DateTimePicker();
            this.checkBox_CreationDateTime = new System.Windows.Forms.CheckBox();
            this.checkBox_LastAccessDateTime = new System.Windows.Forms.CheckBox();
            this.checkBox_LastWriteDateTime = new System.Windows.Forms.CheckBox();
            this.mainMenu_Main = new System.Windows.Forms.MainMenu(this.components);
            this.menuItem_File = new System.Windows.Forms.MenuItem();
            this.menuItem_FileOpen = new System.Windows.Forms.MenuItem();
            this.menuItem_FileBar01 = new System.Windows.Forms.MenuItem();
            this.menuItem_FileExit = new System.Windows.Forms.MenuItem();
            this.menuItem_Help = new System.Windows.Forms.MenuItem();
            this.menuItem_HelpContents = new System.Windows.Forms.MenuItem();
            this.menuItem_HelpBar01 = new System.Windows.Forms.MenuItem();
            this.menuItem_HelpAbout = new System.Windows.Forms.MenuItem();
            this.button_GoUpdate = new System.Windows.Forms.Button();
            this.label_FPath = new System.Windows.Forms.Label();
            this.listView_Folders = new System.Windows.Forms.ListView();
            this.columnHeader_FileName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.imageList_Files = new System.Windows.Forms.ImageList(this.components);
            this.checkBox_Recurse = new System.Windows.Forms.CheckBox();
            this.listView_Contents = new System.Windows.Forms.ListView();
            this.listview_ContentsColumHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.buttonParentDir = new System.Windows.Forms.Button();
            this.checkBoxSimulation = new System.Windows.Forms.CheckBox();
            this.checkBoxShouldFiles = new System.Windows.Forms.CheckBox();
            this.groupBoxCMA = new System.Windows.Forms.GroupBox();
            this.groupBoxConditions = new System.Windows.Forms.GroupBox();
            this.radioButton3All = new System.Windows.Forms.RadioButton();
            this.radioButton2Older = new System.Windows.Forms.RadioButton();
            this.radioButton1Newer = new System.Windows.Forms.RadioButton();
            this.radioButton3_setfromAccessed = new System.Windows.Forms.RadioButton();
            this.radioButton2_setfromModified = new System.Windows.Forms.RadioButton();
            this.radioButton1_setfromCreated = new System.Windows.Forms.RadioButton();
            this.radioButton1_CreationTimeHeading = new System.Windows.Forms.RadioButton();
            this.label_CreationTime = new System.Windows.Forms.Label();
            this.radioButton2_ModifiedHeading = new System.Windows.Forms.RadioButton();
            this.label_Modified = new System.Windows.Forms.Label();
            this.radioButton3_LastAccessHeading = new System.Windows.Forms.RadioButton();
            this.label_LastAccess = new System.Windows.Forms.Label();
            this.radioButtonPanel1 = new UIToolbox.RadioButtonPanel();
            this.radioGroupBox2_CurrentSelectionTime = new UIToolbox.RadioGroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.radioGroupBox3_UseTimeFrom = new UIToolbox.RadioGroupBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.radioButton1_Selected = new System.Windows.Forms.RadioButton();
            this.radioButton5_Last = new System.Windows.Forms.RadioButton();
            this.radioButton4_First = new System.Windows.Forms.RadioButton();
            this.radioButton3_Newest = new System.Windows.Forms.RadioButton();
            this.radioButton2_Oldest = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.radioGroupBox1_SpecifyTime = new UIToolbox.RadioGroupBox();
            this.radioButtonPanel2 = new UIToolbox.RadioButtonPanel();
            this.groupBoxCMA.SuspendLayout();
            this.groupBoxConditions.SuspendLayout();
            this.radioButtonPanel1.SuspendLayout();
            this.radioGroupBox2_CurrentSelectionTime.SuspendLayout();
            this.radioGroupBox3_UseTimeFrom.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.radioGroupBox1_SpecifyTime.SuspendLayout();
            this.radioButtonPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label_FilePathHeading
            // 
            this.label_FilePathHeading.Location = new System.Drawing.Point(8, -1);
            this.label_FilePathHeading.Name = "label_FilePathHeading";
            this.label_FilePathHeading.Size = new System.Drawing.Size(116, 17);
            this.label_FilePathHeading.TabIndex = 0;
            this.label_FilePathHeading.Text = "Current Folder:";
            // 
            // button_Browse
            // 
            this.button_Browse.Location = new System.Drawing.Point(737, 16);
            this.button_Browse.Name = "button_Browse";
            this.button_Browse.Size = new System.Drawing.Size(78, 23);
            this.button_Browse.TabIndex = 2;
            this.button_Browse.Text = "Browse...";
            this.button_Browse.Click += new System.EventHandler(this.button_Browse_Click);
            // 
            // dateTimePicker_Date
            // 
            this.dateTimePicker_Date.CustomFormat = "  MM/dd/yyyy";
            this.dateTimePicker_Date.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePicker_Date.Location = new System.Drawing.Point(8, 20);
            this.dateTimePicker_Date.Name = "dateTimePicker_Date";
            this.dateTimePicker_Date.Size = new System.Drawing.Size(99, 21);
            this.dateTimePicker_Date.TabIndex = 2;
            // 
            // dateTimePicker_Time
            // 
            this.dateTimePicker_Time.CustomFormat = "HH";
            this.dateTimePicker_Time.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dateTimePicker_Time.Location = new System.Drawing.Point(8, 47);
            this.dateTimePicker_Time.Name = "dateTimePicker_Time";
            this.dateTimePicker_Time.ShowUpDown = true;
            this.dateTimePicker_Time.Size = new System.Drawing.Size(99, 21);
            this.dateTimePicker_Time.TabIndex = 3;
            // 
            // checkBox_CreationDateTime
            // 
            this.checkBox_CreationDateTime.AutoSize = true;
            this.checkBox_CreationDateTime.Location = new System.Drawing.Point(8, 14);
            this.checkBox_CreationDateTime.Name = "checkBox_CreationDateTime";
            this.checkBox_CreationDateTime.Size = new System.Drawing.Size(65, 17);
            this.checkBox_CreationDateTime.TabIndex = 1;
            this.checkBox_CreationDateTime.Text = "Created";
            this.checkBox_CreationDateTime.CheckedChanged += new System.EventHandler(this.checkBox_CreationDateTime_CheckedChanged);
            // 
            // checkBox_LastAccessDateTime
            // 
            this.checkBox_LastAccessDateTime.AutoSize = true;
            this.checkBox_LastAccessDateTime.Location = new System.Drawing.Point(8, 54);
            this.checkBox_LastAccessDateTime.Name = "checkBox_LastAccessDateTime";
            this.checkBox_LastAccessDateTime.Size = new System.Drawing.Size(71, 17);
            this.checkBox_LastAccessDateTime.TabIndex = 3;
            this.checkBox_LastAccessDateTime.Text = "Accessed";
            this.checkBox_LastAccessDateTime.CheckedChanged += new System.EventHandler(this.checkBox_LastAccessDateTime_CheckedChanged);
            // 
            // checkBox_LastWriteDateTime
            // 
            this.checkBox_LastWriteDateTime.AutoSize = true;
            this.checkBox_LastWriteDateTime.Location = new System.Drawing.Point(8, 34);
            this.checkBox_LastWriteDateTime.Name = "checkBox_LastWriteDateTime";
            this.checkBox_LastWriteDateTime.Size = new System.Drawing.Size(66, 17);
            this.checkBox_LastWriteDateTime.TabIndex = 2;
            this.checkBox_LastWriteDateTime.Text = "Modified";
            this.checkBox_LastWriteDateTime.CheckedChanged += new System.EventHandler(this.checkBox_LastWriteDateTime_CheckedChanged);
            // 
            // mainMenu_Main
            // 
            this.mainMenu_Main.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem_File,
            this.menuItem_Help});
            // 
            // menuItem_File
            // 
            this.menuItem_File.Index = 0;
            this.menuItem_File.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem_FileOpen,
            this.menuItem_FileBar01,
            this.menuItem_FileExit});
            this.menuItem_File.Text = "&File";
            // 
            // menuItem_FileOpen
            // 
            this.menuItem_FileOpen.Index = 0;
            this.menuItem_FileOpen.Shortcut = System.Windows.Forms.Shortcut.CtrlO;
            this.menuItem_FileOpen.Text = "&Open...";
            this.menuItem_FileOpen.Click += new System.EventHandler(this.menuItem_FileOpen_Click);
            // 
            // menuItem_FileBar01
            // 
            this.menuItem_FileBar01.Index = 1;
            this.menuItem_FileBar01.Text = "-";
            // 
            // menuItem_FileExit
            // 
            this.menuItem_FileExit.Index = 2;
            this.menuItem_FileExit.Text = "E&xit";
            this.menuItem_FileExit.Click += new System.EventHandler(this.menuItem_FileExit_Click);
            // 
            // menuItem_Help
            // 
            this.menuItem_Help.Index = 1;
            this.menuItem_Help.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem_HelpContents,
            this.menuItem_HelpBar01,
            this.menuItem_HelpAbout});
            this.menuItem_Help.Text = "&Help";
            // 
            // menuItem_HelpContents
            // 
            this.menuItem_HelpContents.Index = 0;
            this.menuItem_HelpContents.Shortcut = System.Windows.Forms.Shortcut.F1;
            this.menuItem_HelpContents.Text = "Contents...";
            this.menuItem_HelpContents.Click += new System.EventHandler(this.menuItem_HelpContents_Click);
            // 
            // menuItem_HelpBar01
            // 
            this.menuItem_HelpBar01.Index = 1;
            this.menuItem_HelpBar01.Text = "-";
            // 
            // menuItem_HelpAbout
            // 
            this.menuItem_HelpAbout.Index = 2;
            this.menuItem_HelpAbout.Text = "&About...";
            this.menuItem_HelpAbout.Click += new System.EventHandler(this.menuItem_HelpAbout_Click);
            // 
            // button_GoUpdate
            // 
            this.button_GoUpdate.Enabled = false;
            this.button_GoUpdate.Location = new System.Drawing.Point(8, 456);
            this.button_GoUpdate.Name = "button_GoUpdate";
            this.button_GoUpdate.Size = new System.Drawing.Size(144, 25);
            this.button_GoUpdate.TabIndex = 13;
            this.button_GoUpdate.Text = "Update File Date/Time";
            this.button_GoUpdate.Click += new System.EventHandler(this.button_Update_Click);
            // 
            // label_FPath
            // 
            this.label_FPath.AllowDrop = true;
            this.label_FPath.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.label_FPath.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label_FPath.Location = new System.Drawing.Point(10, 16);
            this.label_FPath.Name = "label_FPath";
            this.label_FPath.Size = new System.Drawing.Size(721, 21);
            this.label_FPath.TabIndex = 1;
            // 
            // listView_Folders
            // 
            this.listView_Folders.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView_Folders.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader_FileName});
            this.listView_Folders.FullRowSelect = true;
            this.listView_Folders.HideSelection = false;
            this.listView_Folders.LargeImageList = this.imageList_Files;
            this.listView_Folders.Location = new System.Drawing.Point(10, 69);
            this.listView_Folders.MultiSelect = false;
            this.listView_Folders.Name = "listView_Folders";
            this.listView_Folders.Size = new System.Drawing.Size(517, 187);
            this.listView_Folders.SmallImageList = this.imageList_Files;
            this.listView_Folders.TabIndex = 3;
            this.listView_Folders.UseCompatibleStateImageBehavior = false;
            this.listView_Folders.View = System.Windows.Forms.View.List;
            this.listView_Folders.SelectedIndexChanged += new System.EventHandler(this.listView_Folders_SelectedIndexChanged);
            this.listView_Folders.DoubleClick += new System.EventHandler(this.listView_Folders_DoubleClick);
            // 
            // columnHeader_FileName
            // 
            this.columnHeader_FileName.Text = "Subfolders";
            this.columnHeader_FileName.Width = 232;
            // 
            // imageList_Files
            // 
            this.imageList_Files.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList_Files.ImageStream")));
            this.imageList_Files.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList_Files.Images.SetKeyName(0, "ico1.ico");
            this.imageList_Files.Images.SetKeyName(1, "ico5.ico");
            // 
            // checkBox_Recurse
            // 
            this.checkBox_Recurse.Location = new System.Drawing.Point(537, 51);
            this.checkBox_Recurse.Name = "checkBox_Recurse";
            this.checkBox_Recurse.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.checkBox_Recurse.Size = new System.Drawing.Size(184, 18);
            this.checkBox_Recurse.TabIndex = 6;
            this.checkBox_Recurse.Text = "Recurse Sub-Directories";
            this.checkBox_Recurse.CheckedChanged += new System.EventHandler(this.checkBox_Recurse_CheckedChanged);
            // 
            // listView_Contents
            // 
            this.listView_Contents.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView_Contents.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.listview_ContentsColumHeader1});
            this.listView_Contents.FullRowSelect = true;
            this.listView_Contents.HideSelection = false;
            this.listView_Contents.LargeImageList = this.imageList_Files;
            this.listView_Contents.Location = new System.Drawing.Point(537, 69);
            this.listView_Contents.MultiSelect = false;
            this.listView_Contents.Name = "listView_Contents";
            this.listView_Contents.Size = new System.Drawing.Size(278, 335);
            this.listView_Contents.SmallImageList = this.imageList_Files;
            this.listView_Contents.TabIndex = 4;
            this.listView_Contents.UseCompatibleStateImageBehavior = false;
            this.listView_Contents.View = System.Windows.Forms.View.Details;
            this.listView_Contents.SelectedIndexChanged += new System.EventHandler(this.listView_Contents_SelectedIndexChanged);
            this.listView_Contents.DoubleClick += new System.EventHandler(this.listView_Contents_DoubleClick);
            // 
            // listview_ContentsColumHeader1
            // 
            this.listview_ContentsColumHeader1.Text = "Files";
            this.listview_ContentsColumHeader1.Width = 247;
            // 
            // buttonParentDir
            // 
            this.buttonParentDir.Location = new System.Drawing.Point(8, 40);
            this.buttonParentDir.Name = "buttonParentDir";
            this.buttonParentDir.Size = new System.Drawing.Size(78, 23);
            this.buttonParentDir.TabIndex = 5;
            this.buttonParentDir.Text = "^ Parent";
            this.buttonParentDir.UseVisualStyleBackColor = true;
            this.buttonParentDir.Click += new System.EventHandler(this.buttonParentDir_Click);
            // 
            // checkBoxSimulation
            // 
            this.checkBoxSimulation.AutoSize = true;
            this.checkBoxSimulation.Location = new System.Drawing.Point(10, 437);
            this.checkBoxSimulation.Name = "checkBoxSimulation";
            this.checkBoxSimulation.Size = new System.Drawing.Size(128, 17);
            this.checkBoxSimulation.TabIndex = 12;
            this.checkBoxSimulation.Text = "Simulation Mode Only";
            this.checkBoxSimulation.UseVisualStyleBackColor = true;
            // 
            // checkBoxShouldFiles
            // 
            this.checkBoxShouldFiles.AutoSize = true;
            this.checkBoxShouldFiles.Location = new System.Drawing.Point(537, 406);
            this.checkBoxShouldFiles.Name = "checkBoxShouldFiles";
            this.checkBoxShouldFiles.Size = new System.Drawing.Size(201, 17);
            this.checkBoxShouldFiles.TabIndex = 11;
            this.checkBoxShouldFiles.Text = "Perform the operation on Files inside";
            this.checkBoxShouldFiles.UseVisualStyleBackColor = true;
            // 
            // groupBoxCMA
            // 
            this.groupBoxCMA.Controls.Add(this.checkBox_CreationDateTime);
            this.groupBoxCMA.Controls.Add(this.checkBox_LastWriteDateTime);
            this.groupBoxCMA.Controls.Add(this.checkBox_LastAccessDateTime);
            this.groupBoxCMA.Location = new System.Drawing.Point(8, 262);
            this.groupBoxCMA.Name = "groupBoxCMA";
            this.groupBoxCMA.Size = new System.Drawing.Size(116, 75);
            this.groupBoxCMA.TabIndex = 7;
            this.groupBoxCMA.TabStop = false;
            this.groupBoxCMA.Text = "Set Timestamp of:";
            // 
            // groupBoxConditions
            // 
            this.groupBoxConditions.Controls.Add(this.radioButton3All);
            this.groupBoxConditions.Controls.Add(this.radioButton2Older);
            this.groupBoxConditions.Controls.Add(this.radioButton1Newer);
            this.groupBoxConditions.Location = new System.Drawing.Point(8, 343);
            this.groupBoxConditions.Name = "groupBoxConditions";
            this.groupBoxConditions.Size = new System.Drawing.Size(86, 81);
            this.groupBoxConditions.TabIndex = 9;
            this.groupBoxConditions.TabStop = false;
            this.groupBoxConditions.Text = "Update If:";
            // 
            // radioButton3All
            // 
            this.radioButton3All.AutoSize = true;
            this.radioButton3All.Checked = true;
            this.radioButton3All.Location = new System.Drawing.Point(6, 54);
            this.radioButton3All.Name = "radioButton3All";
            this.radioButton3All.Size = new System.Drawing.Size(59, 17);
            this.radioButton3All.TabIndex = 1;
            this.radioButton3All.TabStop = true;
            this.radioButton3All.Text = "Always";
            this.radioButton3All.UseVisualStyleBackColor = true;
            // 
            // radioButton2Older
            // 
            this.radioButton2Older.AutoSize = true;
            this.radioButton2Older.Location = new System.Drawing.Point(6, 34);
            this.radioButton2Older.Name = "radioButton2Older";
            this.radioButton2Older.Size = new System.Drawing.Size(51, 17);
            this.radioButton2Older.TabIndex = 2;
            this.radioButton2Older.Text = "Older";
            this.radioButton2Older.UseVisualStyleBackColor = true;
            // 
            // radioButton1Newer
            // 
            this.radioButton1Newer.AutoSize = true;
            this.radioButton1Newer.Location = new System.Drawing.Point(6, 14);
            this.radioButton1Newer.Name = "radioButton1Newer";
            this.radioButton1Newer.Size = new System.Drawing.Size(56, 17);
            this.radioButton1Newer.TabIndex = 3;
            this.radioButton1Newer.Text = "Newer";
            this.radioButton1Newer.UseVisualStyleBackColor = true;
            // 
            // radioButton3_setfromAccessed
            // 
            this.radioButton3_setfromAccessed.AutoSize = true;
            this.radioButton3_setfromAccessed.Location = new System.Drawing.Point(3, 44);
            this.radioButton3_setfromAccessed.Name = "radioButton3_setfromAccessed";
            this.radioButton3_setfromAccessed.Size = new System.Drawing.Size(89, 17);
            this.radioButton3_setfromAccessed.TabIndex = 2;
            this.radioButton3_setfromAccessed.Text = "File Accessed";
            this.radioButton3_setfromAccessed.UseVisualStyleBackColor = true;
            // 
            // radioButton2_setfromModified
            // 
            this.radioButton2_setfromModified.AutoSize = true;
            this.radioButton2_setfromModified.Location = new System.Drawing.Point(3, 24);
            this.radioButton2_setfromModified.Name = "radioButton2_setfromModified";
            this.radioButton2_setfromModified.Size = new System.Drawing.Size(84, 17);
            this.radioButton2_setfromModified.TabIndex = 1;
            this.radioButton2_setfromModified.Text = "File Modified";
            this.radioButton2_setfromModified.UseVisualStyleBackColor = true;
            // 
            // radioButton1_setfromCreated
            // 
            this.radioButton1_setfromCreated.AutoSize = true;
            this.radioButton1_setfromCreated.Checked = true;
            this.radioButton1_setfromCreated.Location = new System.Drawing.Point(3, 4);
            this.radioButton1_setfromCreated.Name = "radioButton1_setfromCreated";
            this.radioButton1_setfromCreated.Size = new System.Drawing.Size(83, 17);
            this.radioButton1_setfromCreated.TabIndex = 0;
            this.radioButton1_setfromCreated.TabStop = true;
            this.radioButton1_setfromCreated.Text = "File Created";
            this.radioButton1_setfromCreated.UseVisualStyleBackColor = true;
            // 
            // radioButton1_CreationTimeHeading
            // 
            this.radioButton1_CreationTimeHeading.AutoSize = true;
            this.radioButton1_CreationTimeHeading.Location = new System.Drawing.Point(3, 3);
            this.radioButton1_CreationTimeHeading.Name = "radioButton1_CreationTimeHeading";
            this.radioButton1_CreationTimeHeading.Size = new System.Drawing.Size(14, 13);
            this.radioButton1_CreationTimeHeading.TabIndex = 0;
            // 
            // label_CreationTime
            // 
            this.label_CreationTime.AutoSize = true;
            this.label_CreationTime.BackColor = System.Drawing.SystemColors.ControlLight;
            this.label_CreationTime.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label_CreationTime.Location = new System.Drawing.Point(258, 301);
            this.label_CreationTime.MinimumSize = new System.Drawing.Size(137, 21);
            this.label_CreationTime.Name = "label_CreationTime";
            this.label_CreationTime.Size = new System.Drawing.Size(137, 21);
            this.label_CreationTime.TabIndex = 0;
            // 
            // radioButton2_ModifiedHeading
            // 
            this.radioButton2_ModifiedHeading.AutoSize = true;
            this.radioButton2_ModifiedHeading.Location = new System.Drawing.Point(3, 47);
            this.radioButton2_ModifiedHeading.Name = "radioButton2_ModifiedHeading";
            this.radioButton2_ModifiedHeading.Size = new System.Drawing.Size(14, 13);
            this.radioButton2_ModifiedHeading.TabIndex = 0;
            // 
            // label_Modified
            // 
            this.label_Modified.AutoSize = true;
            this.label_Modified.BackColor = System.Drawing.SystemColors.ControlLight;
            this.label_Modified.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label_Modified.Location = new System.Drawing.Point(129, 83);
            this.label_Modified.MinimumSize = new System.Drawing.Size(137, 21);
            this.label_Modified.Name = "label_Modified";
            this.label_Modified.Size = new System.Drawing.Size(137, 21);
            this.label_Modified.TabIndex = 0;
            // 
            // radioButton3_LastAccessHeading
            // 
            this.radioButton3_LastAccessHeading.AutoSize = true;
            this.radioButton3_LastAccessHeading.Location = new System.Drawing.Point(3, 90);
            this.radioButton3_LastAccessHeading.Name = "radioButton3_LastAccessHeading";
            this.radioButton3_LastAccessHeading.Size = new System.Drawing.Size(14, 13);
            this.radioButton3_LastAccessHeading.TabIndex = 0;
            // 
            // label_LastAccess
            // 
            this.label_LastAccess.AutoSize = true;
            this.label_LastAccess.BackColor = System.Drawing.SystemColors.ControlLight;
            this.label_LastAccess.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label_LastAccess.Location = new System.Drawing.Point(128, 124);
            this.label_LastAccess.MinimumSize = new System.Drawing.Size(137, 21);
            this.label_LastAccess.Name = "label_LastAccess";
            this.label_LastAccess.Size = new System.Drawing.Size(137, 21);
            this.label_LastAccess.TabIndex = 0;
            // 
            // radioButtonPanel1
            // 
            this.radioButtonPanel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.radioButtonPanel1.Controls.Add(this.label_Modified);
            this.radioButtonPanel1.Controls.Add(this.label_LastAccess);
            this.radioButtonPanel1.Controls.Add(this.radioGroupBox3_UseTimeFrom);
            this.radioButtonPanel1.Controls.Add(this.radioGroupBox1_SpecifyTime);
            this.radioButtonPanel1.Controls.Add(this.radioGroupBox2_CurrentSelectionTime);
            this.radioButtonPanel1.Location = new System.Drawing.Point(127, 262);
            this.radioButtonPanel1.Name = "radioButtonPanel1";
            this.radioButtonPanel1.Size = new System.Drawing.Size(404, 169);
            this.radioButtonPanel1.TabIndex = 15;
            // 
            // radioGroupBox2_CurrentSelectionTime
            // 
            this.radioGroupBox2_CurrentSelectionTime.Controls.Add(this.radioButtonPanel2);
            this.radioGroupBox2_CurrentSelectionTime.DisableChildrenIfUnchecked = true;
            this.radioGroupBox2_CurrentSelectionTime.Location = new System.Drawing.Point(122, 0);
            this.radioGroupBox2_CurrentSelectionTime.Name = "radioGroupBox2_CurrentSelectionTime";
            this.radioGroupBox2_CurrentSelectionTime.Padding = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.radioGroupBox2_CurrentSelectionTime.Size = new System.Drawing.Size(148, 150);
            this.radioGroupBox2_CurrentSelectionTime.TabIndex = 3;
            this.radioGroupBox2_CurrentSelectionTime.TabStop = false;
            this.radioGroupBox2_CurrentSelectionTime.Text = "Currently Selected:";
            this.radioGroupBox2_CurrentSelectionTime.Enter += new System.EventHandler(this.radioGroupBox2_CurrentSelectionTime_Enter);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(273, 370);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(115, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Last Access Date/Time";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(273, 327);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(99, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Modified Date/Time";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(273, 283);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Creation Date/Time";
            // 
            // radioGroupBox3_UseTimeFrom
            // 
            this.radioGroupBox3_UseTimeFrom.Controls.Add(this.panel2);
            this.radioGroupBox3_UseTimeFrom.Controls.Add(this.panel1);
            this.radioGroupBox3_UseTimeFrom.DisableChildrenIfUnchecked = true;
            this.radioGroupBox3_UseTimeFrom.Location = new System.Drawing.Point(276, 0);
            this.radioGroupBox3_UseTimeFrom.Name = "radioGroupBox3_UseTimeFrom";
            this.radioGroupBox3_UseTimeFrom.Padding = new System.Windows.Forms.Padding(9, 3, 3, 3);
            this.radioGroupBox3_UseTimeFrom.Size = new System.Drawing.Size(124, 164);
            this.radioGroupBox3_UseTimeFrom.TabIndex = 1;
            this.radioGroupBox3_UseTimeFrom.TabStop = false;
            this.radioGroupBox3_UseTimeFrom.Text = "Use Time From:";
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.radioButton1_Selected);
            this.panel2.Controls.Add(this.radioButton5_Last);
            this.panel2.Controls.Add(this.radioButton4_First);
            this.panel2.Controls.Add(this.radioButton3_Newest);
            this.panel2.Controls.Add(this.radioButton2_Oldest);
            this.panel2.Enabled = false;
            this.panel2.Location = new System.Drawing.Point(24, 77);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(71, 83);
            this.panel2.TabIndex = 2;
            // 
            // radioButton1_Selected
            // 
            this.radioButton1_Selected.Checked = true;
            this.radioButton1_Selected.Location = new System.Drawing.Point(4, 0);
            this.radioButton1_Selected.Name = "radioButton1_Selected";
            this.radioButton1_Selected.Size = new System.Drawing.Size(66, 19);
            this.radioButton1_Selected.TabIndex = 4;
            this.radioButton1_Selected.TabStop = true;
            this.radioButton1_Selected.Text = "Selected";
            // 
            // radioButton5_Last
            // 
            this.radioButton5_Last.Location = new System.Drawing.Point(4, 65);
            this.radioButton5_Last.Name = "radioButton5_Last";
            this.radioButton5_Last.Size = new System.Drawing.Size(45, 16);
            this.radioButton5_Last.TabIndex = 3;
            this.radioButton5_Last.Text = "Last";
            // 
            // radioButton4_First
            // 
            this.radioButton4_First.Location = new System.Drawing.Point(4, 49);
            this.radioButton4_First.Name = "radioButton4_First";
            this.radioButton4_First.Size = new System.Drawing.Size(46, 15);
            this.radioButton4_First.TabIndex = 2;
            this.radioButton4_First.Text = "First";
            // 
            // radioButton3_Newest
            // 
            this.radioButton3_Newest.Location = new System.Drawing.Point(4, 33);
            this.radioButton3_Newest.Name = "radioButton3_Newest";
            this.radioButton3_Newest.Size = new System.Drawing.Size(61, 16);
            this.radioButton3_Newest.TabIndex = 1;
            this.radioButton3_Newest.Text = "Newest";
            // 
            // radioButton2_Oldest
            // 
            this.radioButton2_Oldest.Location = new System.Drawing.Point(4, 18);
            this.radioButton2_Oldest.Name = "radioButton2_Oldest";
            this.radioButton2_Oldest.Size = new System.Drawing.Size(56, 15);
            this.radioButton2_Oldest.TabIndex = 0;
            this.radioButton2_Oldest.Text = "Oldest";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.radioButton3_setfromAccessed);
            this.panel1.Controls.Add(this.radioButton1_setfromCreated);
            this.panel1.Controls.Add(this.radioButton2_setfromModified);
            this.panel1.Enabled = false;
            this.panel1.Location = new System.Drawing.Point(3, 13);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(117, 65);
            this.panel1.TabIndex = 1;
            // 
            // radioGroupBox1_SpecifyTime
            // 
            this.radioGroupBox1_SpecifyTime.Checked = true;
            this.radioGroupBox1_SpecifyTime.Controls.Add(this.dateTimePicker_Time);
            this.radioGroupBox1_SpecifyTime.Controls.Add(this.dateTimePicker_Date);
            this.radioGroupBox1_SpecifyTime.DisableChildrenIfUnchecked = true;
            this.radioGroupBox1_SpecifyTime.Location = new System.Drawing.Point(2, 0);
            this.radioGroupBox1_SpecifyTime.Name = "radioGroupBox1_SpecifyTime";
            this.radioGroupBox1_SpecifyTime.Size = new System.Drawing.Size(114, 89);
            this.radioGroupBox1_SpecifyTime.TabIndex = 0;
            this.radioGroupBox1_SpecifyTime.TabStop = false;
            this.radioGroupBox1_SpecifyTime.Text = "Specify Time:";
            // 
            // radioButtonPanel2
            // 
            this.radioButtonPanel2.Controls.Add(this.radioButton1_CreationTimeHeading);
            this.radioButtonPanel2.Controls.Add(this.radioButton3_LastAccessHeading);
            this.radioButtonPanel2.Controls.Add(this.radioButton2_ModifiedHeading);
            this.radioButtonPanel2.Enabled = false;
            this.radioButtonPanel2.Location = new System.Drawing.Point(4, 18);
            this.radioButtonPanel2.Name = "radioButtonPanel2";
            this.radioButtonPanel2.Size = new System.Drawing.Size(20, 111);
            this.radioButtonPanel2.TabIndex = 16;
            // 
            // Form_Main
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
            this.ClientSize = new System.Drawing.Size(827, 486);
            this.Controls.Add(this.label_CreationTime);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.radioButtonPanel1);
            this.Controls.Add(this.groupBoxConditions);
            this.Controls.Add(this.groupBoxCMA);
            this.Controls.Add(this.checkBoxShouldFiles);
            this.Controls.Add(this.checkBoxSimulation);
            this.Controls.Add(this.buttonParentDir);
            this.Controls.Add(this.listView_Contents);
            this.Controls.Add(this.listView_Folders);
            this.Controls.Add(this.label_FPath);
            this.Controls.Add(this.button_GoUpdate);
            this.Controls.Add(this.button_Browse);
            this.Controls.Add(this.label_FilePathHeading);
            this.Controls.Add(this.checkBox_Recurse);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Menu = this.mainMenu_Main;
            this.Name = "Form_Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Folder Timestamp Modifier";
            this.Load += new System.EventHandler(this.Form_Main_Load);
            this.groupBoxCMA.ResumeLayout(false);
            this.groupBoxCMA.PerformLayout();
            this.groupBoxConditions.ResumeLayout(false);
            this.groupBoxConditions.PerformLayout();
            this.radioButtonPanel1.ResumeLayout(false);
            this.radioButtonPanel1.PerformLayout();
            this.radioGroupBox2_CurrentSelectionTime.ResumeLayout(false);
            this.radioGroupBox2_CurrentSelectionTime.PerformLayout();
            this.radioGroupBox3_UseTimeFrom.ResumeLayout(false);
            this.radioGroupBox3_UseTimeFrom.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.radioGroupBox1_SpecifyTime.ResumeLayout(false);
            this.radioGroupBox1_SpecifyTime.PerformLayout();
            this.radioButtonPanel2.ResumeLayout(false);
            this.radioButtonPanel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
            Application.Run(new Form_Main());
		}

		#region Form Events...

		/// <summary>
		/// Load the main form
		/// </summary>
		/// <param name="sender">Form</param>
		/// <param name="e">EventArgs</param>
		private void Form_Main_Load(object sender, System.EventArgs e)
		{
			/*
            // Make the mins and seconds 0 when the form is loaded
			DateTime now = DateTime.Parse(DateTime.Now.ToString("d") + 
				" " + DateTime.Now.Hour + ":00:00");
			dateTimePicker_Time.Value = now;
            */
			try
			{
                //Create data containers for the UI to use
                this.folderList = new List<string>();
                this.contentsDirList = new List<string>();
                this.contentsFileList = new List<string>();
				// Display the folder path and the contents of it.
				DisplayFolderList(homeFolder);
                DisplayContentsList(initial: true);
			}
			catch
			{
				label_FPath.Text = "";
                clearonerror();
			}
		}

		#endregion
        #region Helper functions...

        // Clear both ListViews and the three data container lists
        private void clearonerror()
        {
            listView_Folders.Clear();
            listView_Contents.Clear();
            folderList.Clear();
            contentsDirList.Clear();
            contentsFileList.Clear();
        }
        /// <summary>
        /// Icon in listView image list
        /// </summary>
        private enum ListViewIcon : int
        {
            /// <summary>
            /// File icon in listView image list
            /// </summary>
            File = 0,

            /// <summary>
            /// Directory icon in listView image list
            /// </summary>
            Directory = 1
        }

        /// <summary>
        /// Count of the number of files/directories that have been set
        /// </summary>
        private int itemsSetCount;

        /// <summary>
        /// Count of the number of system or hidden files skipped
        /// </summary>
        private int itemsSkippedCount;

        /// <summary>
        /// Count of the number of files/directories that have errors setting the date/time
        /// </summary>
        private int itemsErrorsCount;

		#region Buttons...

		private void button_Browse_Click(object sender, System.EventArgs e)
		{
			OpenFile();
		}

		private void button_Update_Click(object sender, System.EventArgs e)
		{
			GoUpdateDateTime();
		}

		#endregion

		#region Menu...

		private void menuItem_FileOpen_Click(object sender, System.EventArgs e)
		{
			OpenFile();
		}

		private void menuItem_FileExit_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void menuItem_HelpContents_Click(object sender, System.EventArgs e)
		{
			Help.ShowHelp(this, "FileTime.chm", HelpNavigator.TableOfContents);
		}

		private void menuItem_HelpAbout_Click(object sender, System.EventArgs e)
		{
			Form_About form = new Form_About();
			form.ShowDialog(this);
		}

		#endregion

		#region ListBox and Checkbox functions...

		/// <summary>
		/// Creation Checkbox selection changed
		/// </summary>
		private void checkBox_CreationDateTime_CheckedChanged(object sender, System.EventArgs e)
		{
			UpdateButtonEnable();
		}

        /// <summary>
        /// Modified Checkbox selection changed
        /// </summary>
        private void checkBox_LastWriteDateTime_CheckedChanged(object sender, System.EventArgs e)
        {
            UpdateButtonEnable();
        }

        /// <summary>
		/// Accessed Checkbox selection changed
		/// </summary>
		private void checkBox_LastAccessDateTime_CheckedChanged(object sender, System.EventArgs e)
		{
			UpdateButtonEnable();
		}

		/// <summary>
		/// The Folder listview selection has changed
		/// </summary>
		private void listView_Folders_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			UpdateButtonEnable();
			UpdateDisplayFileDateTime();
            if (listView_Folders.SelectedItems.Count == 1)
                DisplayContentsList();
            else if (listView_Folders.SelectedItems.Count == 0)
                DisplayContentsList(filesonly: true);
		}

        /// <summary>
        /// The Folder listview was double clicked
        /// </summary>
        private void listView_Folders_DoubleClick(object sender, System.EventArgs e)
        {
            UpdateButtonEnable();
            UpdateDisplayFileDateTime();
            if (listView_Folders.SelectedItems.Count == 1)
            {
                string DirectoryName = Path.Combine(label_FPath.Text, listView_Folders.SelectedItems[0].Text) + seperator;
                DisplayFolderList(DirectoryName);
            }
            DisplayContentsList(filesonly: true);
        }

        /// <summary>
        /// The Contents listview selection has changed
        /// </summary>
        private void listView_Contents_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView_Contents.SelectedItems.Count == 1 && listView_Contents.SelectedItems[0].ImageIndex == 0)
            {
                if (listView_Folders.SelectedItems.Count == 1)
                {
                    string DirectoryName = Path.Combine(label_FPath.Text, listView_Folders.SelectedItems[0].Text) + seperator;
                    DisplayFolderList(DirectoryName);
                }
            }
            radioButton1_Selected.Enabled = Convert.ToBoolean(listView_Contents.SelectedItems.Count);
            radioButton1_Selected.Checked = Convert.ToBoolean(listView_Contents.SelectedItems.Count);
            UpdateDisplayFileDateTime();
        }

        /// <summary>
        /// The contents listview was double clicked
        /// </summary>
        private void listView_Contents_DoubleClick(object sender, System.EventArgs e)
        {
            if (listView_Contents.SelectedItems.Count == 1 && listView_Contents.SelectedItems[0].ImageIndex == 1)
            {
                string DirectoryName = Path.Combine(Path.Combine(label_FPath.Text, listView_Folders.SelectedItems[0].Text),
                    listView_Contents.SelectedItems[0].Text) + seperator;
                DisplayFolderList(DirectoryName);
                DisplayContentsList(filesonly: true);
            }
        }

		/// <summary>
		/// Only enable the update button if something is selected
		/// </summary>
		private void UpdateButtonEnable()
		{
			button_GoUpdate.Enabled = ((listView_Folders.SelectedItems.Count > 0) &
				(checkBox_CreationDateTime.Checked | 
				checkBox_LastAccessDateTime.Checked | 
				checkBox_LastWriteDateTime.Checked));
		}

		#endregion


		/// <summary>
		/// Display the Folder Browser Dialog and then display the selected
		/// file path and the directories and files in the folder.
		/// </summary>
		private void OpenFile()
		{
			FolderBrowserDialog openFile = new FolderBrowserDialog();
			openFile.ShowNewFolderButton = false;
			try
			{
				if (label_FPath.Text != "")
				{
					// Dislay the previous path in the dialog if one 
					// was selected
					openFile.SelectedPath = label_FPath.Text;
				}
				// Open the folder browser dialog
				openFile.Description = "Select the folder you want to view/change the subfolders of:";
				if (openFile.ShowDialog() == DialogResult.OK)
				{
					// Display the folder path
					string directoryName = openFile.SelectedPath;
					DisplayFolderList(directoryName);
                    DisplayContentsList();
				}
			}
			catch (Exception ex)
			{
				// if we have an error then clear the window
				label_FPath.Text = "";
                displayCMA("");
                clearonerror();
				// display an error message
				MessageBox.Show("Error selecting file: \r\n\r\n" + ex.Message, 
					"Open File Error", MessageBoxButtons.OK, 
					MessageBoxIcon.Exclamation);
			}
			finally
			{
				// In testing there we a few times where we seemed to need this
				openFile.Dispose();
			}
		}
        
		/// <summary>
		/// Display file and directories in the list box
		/// </summary>
		/// <param name="DirectoryName">Path to the directory</param>
		private void DisplayFolderList(string DirectoryName)
		{
            //UpdateDisplayFileDateTime();
            //set the Path textbox
            label_FPath.Text = DirectoryName;
            //Clear the Folder UI + container.
            listView_Folders.Items.Clear();
            folderList.Clear();

            try
            {
                foreach (string subDirectory in Directory.GetDirectories(DirectoryName))
                {
                    //Store the sub directories into the list container
                    folderList.Add(Path.GetFileName(subDirectory));
                }
                foreach (string subDirectory in folderList)
                {
                    // Display all the sub directories using the folder icon
                    listView_Folders.Items.Add(subDirectory, (int)ListViewIcon.Directory);
                }                
            }
            
            catch (System.UnauthorizedAccessException) { }

        }
        private void DisplayContentsList(bool initial = false, bool filesonly = false)
        {
            //Clear the contents UI + containers
			listView_Contents.Items.Clear();
            contentsDirList.Clear();
            contentsFileList.Clear();

            string DirectoryName = "";

            if (initial == true)
            {
                DirectoryName = homeFolder;
                filesonly = true;
            }
            else
            {
                string selecteditem = "";
                if (listView_Folders.SelectedItems.Count == 1)
                {
                    selecteditem = listView_Folders.SelectedItems[0].Text;
                }
                DirectoryName = Path.Combine(label_FPath.Text, selecteditem) + seperator;
            }

            // Display all the sub directories and show the folder icon
            if (checkBox_Recurse.Checked & initial == false & filesonly == false)
            {
                try
                {
                    foreach (string subDirectory in Directory.GetDirectories(DirectoryName))
                    {
                        contentsDirList.Add(Path.GetFileName(subDirectory));
                    }
                    foreach (string subDirectory in contentsDirList)
                    {
                        // Display all the sub directories using the folder icon
                        listView_Contents.Items.Add(subDirectory, (int)ListViewIcon.Directory);
                    }
                }
                catch (System.UnauthorizedAccessException) { }
            }

            // Display all of the files and show a file icon
            try
            {
                foreach (string file in Directory.GetFiles(DirectoryName))
                {
                    if (!((File.GetAttributes(file) & FileAttributes.Hidden) == FileAttributes.Hidden))
                    {
                        // When Path.GetFileName is called the last access date/time is updated on the file. 
                        // Hence we save the last access date/time and the reset it to the old last.
                        // access date/time after the call.
                        DateTime accessTime = File.GetLastAccessTime(file);
                        contentsFileList.Add(Path.GetFileName(file));

                        try
                        {
                            File.SetLastAccessTime(file, accessTime);
                        }
                        catch { } // Do nothing
                    }

                }
                foreach (string file in contentsFileList)
                {
                    // Display all the sub directories using the folder icon
                    listView_Contents.Items.Add(file, (int)ListViewIcon.File);
                }

            }
            catch (System.UnauthorizedAccessException) { }
		}

        private DateTime decidewhichtime()
        {
            var dateToUse = new DateTime();
            if (radioGroupBox1_SpecifyTime.Checked)
            {
                dateToUse = DateTime.Parse(dateTimePicker_Date.Value.Date.ToString("d") +
                    " " + dateTimePicker_Time.Value.Hour + ":" +
                    dateTimePicker_Time.Value.Minute + ":" +
                    dateTimePicker_Time.Value.Second);
            }
            else if (radioGroupBox2_CurrentSelectionTime.Checked)
            {
                if (radioButton1_CreationTimeHeading.Checked){
                    dateToUse = DateTime.Parse(label_CreationTime.Text);
                } else if (radioButton2_ModifiedHeading.Checked){
                    dateToUse = DateTime.Parse(label_Modified.Text);
                } else if (radioButton3_LastAccessHeading.Checked){
                    dateToUse = DateTime.Parse(label_LastAccess.Text);
                }
            }
            
            return dateToUse;
        }

		/// <summary>
		/// Modify/Update Timestamps on the filesystem or run a simulation
		/// </summary>
		private void GoUpdateDateTime()
		{
            DateTime fileDateTime = decidewhichtime();

			itemsSetCount = 0;
			itemsSkippedCount = 0;
			itemsErrorsCount = 0;
            //Save the selection index the user clicked so it can be restored
            int oldselection = listView_Folders.SelectedItems[0].Index;

			foreach (ListViewItem folder in listView_Folders.SelectedItems)
			{
				string fileName = Path.Combine(label_FPath.Text, folder.Text);

                // If the item is a directory (flagged by the image index) & recurse subdirectories is selected
                if ((folder.ImageIndex == (int)ListViewIcon.Directory) & checkBox_Recurse.Checked)
				{
					RecurseSubDirectory(fileName, fileDateTime);
				}

                SetFileDateTime(fileName, fileDateTime, (folder.ImageIndex == (int)ListViewIcon.Directory));
			}

			string message = itemsSetCount.ToString() + " file(s)/directorie(s) have had their date/time set";
			if (itemsErrorsCount > 0)
			{
				message += "\r\n\r\n There were " + itemsErrorsCount.ToString() + " error(s)";
			}
			if (itemsSkippedCount > 0)
			{
				message += "\r\n\r\n There were " + itemsSkippedCount.ToString() + " System or Hidden files/directories skipped.";
			}
			MessageBox.Show(message, "Info Log", MessageBoxButtons.OK, MessageBoxIcon.Information);
            DisplayFolderList(label_FPath.Text);
            //Restore the selection index
            listView_Folders.Items[oldselection].Selected = true;
		}

		/// <summary>
		/// Set the date/time to all files is a sub directory
		/// </summary>
		/// <param name="DirectoryPath">Full path to the directory</param>
		/// <param name="FileTime">File Date/Time</param>
		private void RecurseSubDirectory(string DirectoryPath, DateTime FileTime)
		{
			// Set the date/time for each sub directory
			foreach (string subDirectory in Directory.GetDirectories(DirectoryPath))
			{
				// Loop through each sub-sub directory
				RecurseSubDirectory(subDirectory, FileTime);
				// Set the date/time for the sub directory
				SetFileDateTime(subDirectory, FileTime, true);
			}
			// Set the date/time for each file only if "Perform operation on files" is checkboxed.
            if (checkBoxShouldFiles.Checked)
            {
                foreach (string file in Directory.GetFiles(DirectoryPath))
                {
                    SetFileDateTime(file, FileTime, false);
                }
            }
		}

		/// <summary>
		/// Set the date/time for a given file/directory
		/// (This works on files and directories)
		/// </summary>
		/// <param name="FilePath">Full path to the file/directory</param>
		/// <param name="FileTime">Date/Time to set the file/directory</param>
		/// <param name="IsDirectory">This path is a directory</param>
		private void SetFileDateTime(string FilePath, DateTime FileTime, bool IsDirectory)
		{
			try
			{
				FileAttributes fileAttributes = File.GetAttributes(FilePath);
				if ((fileAttributes & FileAttributes.Hidden) == FileAttributes.Hidden)
				{
					// Skip hidden files and directories 
					++itemsSkippedCount;
				}
				else if ((fileAttributes & FileAttributes.System) == FileAttributes.System)
				{
					// Skip system files and directories
					++itemsSkippedCount;
				}
				else if ((fileAttributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
				{
					MessageBox.Show("The file/directory '" + FilePath + "' is read only", 
						"Date/Time", MessageBoxButtons.OK, 
						MessageBoxIcon.Information);
				}
				else 
				{
                    if (!checkBoxSimulation.Checked)
                    {
                        // Set file creation date/time if selected
                        if (checkBox_CreationDateTime.Checked)
                        {
                            if (IsDirectory)
                            {
                                Directory.SetCreationTime(FilePath, FileTime);
                            }
                            else
                            {
                                File.SetCreationTime(FilePath, FileTime);
                            }
                        }
                        // Set the last write date/time is selected					
                        if (checkBox_LastWriteDateTime.Checked)
                        {
                            if (IsDirectory)
                            {
                                Directory.SetLastWriteTime(FilePath, FileTime);
                            }
                            else
                            {
                                File.SetLastWriteTime(FilePath, FileTime);
                            }
                        }
                        // Set the last access time if selected
                        // (Should be the last one we write to if not the
                        // last access date/time will update to now)
                        if (checkBox_LastAccessDateTime.Checked)
                        {
                            if (IsDirectory)
                            {
                                Directory.SetLastAccessTime(FilePath, FileTime);
                            }
                            else
                            {
                                File.SetLastAccessTime(FilePath, FileTime);
                            }
                        }
                    }
					++itemsSetCount;
				}
			}
			catch (Exception ex)
			{
				++itemsErrorsCount;
				MessageBox.Show("Error in setting date/time on '" + FilePath + "': \r\n\r\n" + ex.Message, 
					"Date/Time Error " + itemsErrorsCount.ToString(), MessageBoxButtons.OK, 
					MessageBoxIcon.Exclamation);
			}
		}

		/// <summary>
		/// Display the date and time of the selected file
		/// (Also works on directories)
		/// </summary>

        private void displayCMA(string pathName)
        {
            if (pathName == "")
            {
                // Maybe no file/directory is selected
                // Then Blank out the display of date/time.
                label_CreationTime.Text = "";
                label_LastAccess.Text = "";
                label_Modified.Text = "";
            }
            else
            {
                label_CreationTime.Text = File.GetCreationTime(pathName).ToString();
                label_LastAccess.Text = File.GetLastAccessTime(pathName).ToString();
                label_Modified.Text = File.GetLastWriteTime(pathName).ToString();
            }
        }

		private void UpdateDisplayFileDateTime()
		{
            //If none of the below conditions are true, "" means blank date/time during displayCMA()
            string pathName = "";   

            if (listView_Folders.SelectedItems.Count == 1)  // If we have one directory selected 
			{
                pathName = Path.Combine(label_FPath.Text,listView_Folders.SelectedItems[0].Text) + seperator;
			}
            if (listView_Contents.SelectedItems.Count == 1 && listView_Contents.SelectedItems[0].ImageIndex == 0) // If one file is selected
            {
                pathName = Path.Combine(label_FPath.Text, listView_Contents.SelectedItems[0].Text) + seperator;
            }
            //Always Call the display date/time function
            displayCMA(pathName);
		}

		#endregion
        
        private void buttonParentDir_Click(object sender, EventArgs e)
        {
            string directoryName = label_FPath.Text;
            if (directoryName.Length > 3)
            {
                directoryName = directoryName.TrimEnd(seperator);
                string newPath = directoryName.Substring(0, directoryName.LastIndexOf(seperator)+1);
                if (newPath.Length == 2)
                {
                    newPath += seperator;
                }
                DisplayFolderList(newPath);

                DisplayContentsList(filesonly: true);
            }
        }

        private void checkBox_Recurse_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_Recurse.Checked == true)
            {
                listview_ContentsColumHeader1.Text = "Contents (Sub-Directories/Files)";
            }
            else
            {
                listview_ContentsColumHeader1.Text = "Files";
            }
            if (listView_Folders.SelectedItems.Count == 1)
                DisplayContentsList(filesonly: !checkBox_Recurse.Checked);
            if (listView_Folders.SelectedItems.Count == 0)
                DisplayContentsList(filesonly: true);
        }

        private void radioGroupBox2_CurrentSelectionTime_Enter(object sender, EventArgs e)
        {

        }

	}
}
