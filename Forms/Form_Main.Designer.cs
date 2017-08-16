using System.Windows.Forms;

namespace genBTC.FileTime
{
    public partial class Form_Main : Form
    {
        #region Helpers
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion

        #region Declarations

        private Button button_Browse;
        private MainMenu mainMenu_Main;
        private MenuItem menuItem_FileExit;
        private MenuItem menuItem_Help;
        private MenuItem menuItem_HelpAbout;
        private Button button_GoUpdate;
        private MenuItem menuItem_FileOpen;
        private CheckBox checkBox_CreationDateTime;
        private CheckBox checkBox_AccessedDateTime;
        private CheckBox checkBox_ModifiedDateTime;
        private Label label_FPath;
        private DateTimePicker dateTimePicker_Time;
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
        private CheckBox checkBoxShouldFiles;
        private GroupBox groupBoxCMA;
        private RadioButton radioButton1_CreationDate;
        private Label label_CreationTime;
        private RadioButton radioButton2_ModifiedDate;
        private Label label_ModificationTime;
        private RadioButton radioButton3_AccessedDate;
        private Label label_AccessedTime;
        private RadioButton radioButton3_setfromAccessed;
        private RadioButton radioButton2_setfromModified;
        private RadioButton radioButton1_setfromCreation;
        private UIToolbox.RadioButtonPanel radioButtonPanel1_DecideWhichTime;
        private UIToolbox.RadioGroupBox radioGroupBox3_UseTimeFrom;
        private UIToolbox.RadioGroupBox radioGroupBox1_SpecifyTime;
        private Panel panel1;
        private Panel panel2;
        private RadioButton radioButton3_Random;
        private RadioButton radioButton2_Newest;
        private RadioButton radioButton1_Oldest;
        private UIToolbox.RadioGroupBox radioGroupBox2_CurrentSelectionTime;
        private Label label3_AccessedDate;
        private Label label2_ModifiedDate;
        private Label label1_CreationDate;
        private UIToolbox.RadioButtonPanel radioButtonPanel2;
        private Panel panel3;
        private RadioButton radioButton2_useTimefromSubdir;
        private RadioButton radioButton1_useTimefromFile;
        private RadioButton radioButton4_setfromRandom;
        private Label labelHidden_PathName;

        #endregion

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
            this.checkBox_AccessedDateTime = new System.Windows.Forms.CheckBox();
            this.checkBox_ModifiedDateTime = new System.Windows.Forms.CheckBox();
            this.mainMenu_Main = new System.Windows.Forms.MainMenu(this.components);
            this.menuItem_File = new System.Windows.Forms.MenuItem();
            this.menuItem_FileOpen = new System.Windows.Forms.MenuItem();
            this.menuItem_FileBar01 = new System.Windows.Forms.MenuItem();
            this.menuItem_FileExit = new System.Windows.Forms.MenuItem();
            this.menuItem_Preferences = new System.Windows.Forms.MenuItem();
            this.menuItem_Help = new System.Windows.Forms.MenuItem();
            this.menuItem_HelpContents = new System.Windows.Forms.MenuItem();
            this.menuItem_HelpBar01 = new System.Windows.Forms.MenuItem();
            this.menuItem_HelpAbout = new System.Windows.Forms.MenuItem();
            this.button_GoUpdate = new System.Windows.Forms.Button();
            this.label_FPath = new System.Windows.Forms.Label();
            this.imageList_Files = new System.Windows.Forms.ImageList(this.components);
            this.checkBox_Recurse = new System.Windows.Forms.CheckBox();
            this.checkBoxShouldFiles = new System.Windows.Forms.CheckBox();
            this.groupBoxCMA = new System.Windows.Forms.GroupBox();
            this.radioButton3_setfromAccessed = new System.Windows.Forms.RadioButton();
            this.radioButton2_setfromModified = new System.Windows.Forms.RadioButton();
            this.radioButton1_setfromCreation = new System.Windows.Forms.RadioButton();
            this.radioButton1_CreationDate = new System.Windows.Forms.RadioButton();
            this.label_CreationTime = new System.Windows.Forms.Label();
            this.radioButton2_ModifiedDate = new System.Windows.Forms.RadioButton();
            this.label_ModificationTime = new System.Windows.Forms.Label();
            this.radioButton3_AccessedDate = new System.Windows.Forms.RadioButton();
            this.label_AccessedTime = new System.Windows.Forms.Label();
            this.radioButtonPanel1_DecideWhichTime = new UIToolbox.RadioButtonPanel();
            this.radioGroupBox1_pickFolderForCompare = new UIToolbox.RadioGroupBox();
            this.label3_AccessedDate = new System.Windows.Forms.Label();
            this.label2_ModifiedDate = new System.Windows.Forms.Label();
            this.label1_CreationDate = new System.Windows.Forms.Label();
            this.radioGroupBox3_UseTimeFrom = new UIToolbox.RadioGroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.radioButton4_setfromRandom = new System.Windows.Forms.RadioButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.radioButton3_Random = new System.Windows.Forms.RadioButton();
            this.radioButton2_Newest = new System.Windows.Forms.RadioButton();
            this.radioButton1_Oldest = new System.Windows.Forms.RadioButton();
            this.panel3 = new System.Windows.Forms.Panel();
            this.radioButton2_useTimefromSubdir = new System.Windows.Forms.RadioButton();
            this.radioButton1_useTimefromFile = new System.Windows.Forms.RadioButton();
            this.radioGroupBox1_SpecifyTime = new UIToolbox.RadioGroupBox();
            this.radioGroupBox2_CurrentSelectionTime = new UIToolbox.RadioGroupBox();
            this.labelHidden_PathName = new System.Windows.Forms.Label();
            this.radioButtonPanel2 = new UIToolbox.RadioButtonPanel();
            this.listView_Contents = new System.Windows.Forms.ListView();
            this.listview_ContentsColumHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.label1 = new System.Windows.Forms.Label();
            this.explorerTree1 = new WindowsExplorer.ExplorerTree();
            this.groupBoxCMA.SuspendLayout();
            this.radioButtonPanel1_DecideWhichTime.SuspendLayout();
            this.radioGroupBox1_pickFolderForCompare.SuspendLayout();
            this.radioGroupBox3_UseTimeFrom.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.radioGroupBox1_SpecifyTime.SuspendLayout();
            this.radioGroupBox2_CurrentSelectionTime.SuspendLayout();
            this.radioButtonPanel2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label_FilePathHeading
            // 
            this.label_FilePathHeading.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_FilePathHeading.Location = new System.Drawing.Point(7, 3);
            this.label_FilePathHeading.Name = "label_FilePathHeading";
            this.label_FilePathHeading.Size = new System.Drawing.Size(116, 17);
            this.label_FilePathHeading.TabIndex = 0;
            this.label_FilePathHeading.Text = "Current Root Folder:";
            // 
            // button_Browse
            // 
            this.button_Browse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Browse.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_Browse.Location = new System.Drawing.Point(477, 2);
            this.button_Browse.Name = "button_Browse";
            this.button_Browse.Size = new System.Drawing.Size(78, 23);
            this.button_Browse.TabIndex = 0;
            this.button_Browse.Text = "Browse...";
            this.button_Browse.Click += new System.EventHandler(this.button_Browse_Click);
            // 
            // dateTimePickerDate
            // 
            this.dateTimePicker_Date.CustomFormat = "  MM/dd/yyyy";
            this.dateTimePicker_Date.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePicker_Date.Location = new System.Drawing.Point(8, 20);
            this.dateTimePicker_Date.Name = "dateTimePicker_Date";
            this.dateTimePicker_Date.Size = new System.Drawing.Size(99, 21);
            this.dateTimePicker_Date.TabIndex = 2;
            // 
            // dateTimePickerTime
            // 
            this.dateTimePicker_Time.CustomFormat = "HH";
            this.dateTimePicker_Time.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dateTimePicker_Time.Location = new System.Drawing.Point(8, 48);
            this.dateTimePicker_Time.Name = "dateTimePicker_Time";
            this.dateTimePicker_Time.ShowUpDown = true;
            this.dateTimePicker_Time.Size = new System.Drawing.Size(99, 21);
            this.dateTimePicker_Time.TabIndex = 3;
            // 
            // checkBox_CreationDateTime
            // 
            this.checkBox_CreationDateTime.Location = new System.Drawing.Point(8, 31);
            this.checkBox_CreationDateTime.Name = "checkBox_CreationDateTime";
            this.checkBox_CreationDateTime.Size = new System.Drawing.Size(88, 17);
            this.checkBox_CreationDateTime.TabIndex = 1;
            this.checkBox_CreationDateTime.Text = "Created";
            this.checkBox_CreationDateTime.CheckedChanged += new System.EventHandler(this.checkBox_DateTime_CheckedChanged);
            // 
            // checkBox_AccessedDateTime
            // 
            this.checkBox_AccessedDateTime.Location = new System.Drawing.Point(8, 71);
            this.checkBox_AccessedDateTime.Name = "checkBox_AccessedDateTime";
            this.checkBox_AccessedDateTime.Size = new System.Drawing.Size(86, 17);
            this.checkBox_AccessedDateTime.TabIndex = 3;
            this.checkBox_AccessedDateTime.Text = "Accessed";
            this.checkBox_AccessedDateTime.CheckedChanged += new System.EventHandler(this.checkBox_DateTime_CheckedChanged);
            // 
            // checkBox_ModifiedDateTime
            // 
            this.checkBox_ModifiedDateTime.Location = new System.Drawing.Point(8, 51);
            this.checkBox_ModifiedDateTime.Name = "checkBox_ModifiedDateTime";
            this.checkBox_ModifiedDateTime.Size = new System.Drawing.Size(87, 17);
            this.checkBox_ModifiedDateTime.TabIndex = 2;
            this.checkBox_ModifiedDateTime.Text = "Modified";
            this.checkBox_ModifiedDateTime.CheckedChanged += new System.EventHandler(this.checkBox_DateTime_CheckedChanged);
            // 
            // mainMenu_Main
            // 
            this.mainMenu_Main.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem_File,
            this.menuItem_Preferences,
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
            // menuItem_Preferences
            // 
            this.menuItem_Preferences.Index = 1;
            this.menuItem_Preferences.Text = "&Preferences";
            this.menuItem_Preferences.Click += new System.EventHandler(this.menuItem_Preferences_Click);
            // 
            // menuItem_Help
            // 
            this.menuItem_Help.Index = 2;
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
            this.button_GoUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_GoUpdate.Enabled = false;
            this.button_GoUpdate.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_GoUpdate.Location = new System.Drawing.Point(7, 381);
            this.button_GoUpdate.Name = "button_GoUpdate";
            this.button_GoUpdate.Size = new System.Drawing.Size(110, 30);
            this.button_GoUpdate.TabIndex = 13;
            this.button_GoUpdate.Text = "Update Date/Time";
            this.button_GoUpdate.Click += new System.EventHandler(this.button_Update_Click);
            // 
            // label_FPath
            // 
            this.label_FPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label_FPath.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.label_FPath.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label_FPath.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_FPath.Location = new System.Drawing.Point(118, 2);
            this.label_FPath.Name = "label_FPath";
            this.label_FPath.Size = new System.Drawing.Size(353, 21);
            this.label_FPath.TabIndex = 1;
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
            this.checkBox_Recurse.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox_Recurse.Checked = true;
            this.checkBox_Recurse.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_Recurse.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox_Recurse.Location = new System.Drawing.Point(0, 0);
            this.checkBox_Recurse.Name = "checkBox_Recurse";
            this.checkBox_Recurse.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.checkBox_Recurse.Size = new System.Drawing.Size(172, 18);
            this.checkBox_Recurse.TabIndex = 9;
            this.checkBox_Recurse.Text = "Recurse Sub-Directories";
            // 
            // checkBoxShouldFiles
            // 
            this.checkBoxShouldFiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxShouldFiles.Checked = true;
            this.checkBoxShouldFiles.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxShouldFiles.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxShouldFiles.Location = new System.Drawing.Point(0, 203);
            this.checkBoxShouldFiles.Name = "checkBoxShouldFiles";
            this.checkBoxShouldFiles.Size = new System.Drawing.Size(210, 17);
            this.checkBoxShouldFiles.TabIndex = 8;
            this.checkBoxShouldFiles.Text = "Perform the operation on FILES inside also";
            this.checkBoxShouldFiles.UseVisualStyleBackColor = true;
            // 
            // groupBoxCMA
            // 
            this.groupBoxCMA.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBoxCMA.Controls.Add(this.checkBox_CreationDateTime);
            this.groupBoxCMA.Controls.Add(this.checkBox_ModifiedDateTime);
            this.groupBoxCMA.Controls.Add(this.checkBox_AccessedDateTime);
            this.groupBoxCMA.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxCMA.Location = new System.Drawing.Point(8, 250);
            this.groupBoxCMA.Name = "groupBoxCMA";
            this.groupBoxCMA.Size = new System.Drawing.Size(105, 101);
            this.groupBoxCMA.TabIndex = 1;
            this.groupBoxCMA.TabStop = false;
            this.groupBoxCMA.Text = "Set Timestamp of    Destination:";
            // 
            // radioButton3_setfromAccessed
            // 
            this.radioButton3_setfromAccessed.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton3_setfromAccessed.Location = new System.Drawing.Point(7, 34);
            this.radioButton3_setfromAccessed.Name = "radioButton3_setfromAccessed";
            this.radioButton3_setfromAccessed.Size = new System.Drawing.Size(70, 17);
            this.radioButton3_setfromAccessed.TabIndex = 2;
            this.radioButton3_setfromAccessed.Text = "Accessed";
            this.radioButton3_setfromAccessed.UseVisualStyleBackColor = true;
            // 
            // radioButton2_setfromModified
            // 
            this.radioButton2_setfromModified.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton2_setfromModified.Location = new System.Drawing.Point(7, 17);
            this.radioButton2_setfromModified.Name = "radioButton2_setfromModified";
            this.radioButton2_setfromModified.Size = new System.Drawing.Size(65, 17);
            this.radioButton2_setfromModified.TabIndex = 1;
            this.radioButton2_setfromModified.Text = "Modified";
            this.radioButton2_setfromModified.UseVisualStyleBackColor = true;
            // 
            // radioButton1_setfromCreation
            // 
            this.radioButton1_setfromCreation.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton1_setfromCreation.Location = new System.Drawing.Point(7, 0);
            this.radioButton1_setfromCreation.Name = "radioButton1_setfromCreation";
            this.radioButton1_setfromCreation.Size = new System.Drawing.Size(64, 17);
            this.radioButton1_setfromCreation.TabIndex = 0;
            this.radioButton1_setfromCreation.Text = "Created";
            this.radioButton1_setfromCreation.UseVisualStyleBackColor = true;
            // 
            // radioButton1_CreationDate
            // 
            this.radioButton1_CreationDate.AutoSize = true;
            this.radioButton1_CreationDate.Checked = true;
            this.radioButton1_CreationDate.Location = new System.Drawing.Point(3, 3);
            this.radioButton1_CreationDate.Name = "radioButton1_CreationDate";
            this.radioButton1_CreationDate.Size = new System.Drawing.Size(14, 13);
            this.radioButton1_CreationDate.TabIndex = 0;
            this.radioButton1_CreationDate.TabStop = true;
            // 
            // label_CreationTime
            // 
            this.label_CreationTime.AutoSize = true;
            this.label_CreationTime.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.label_CreationTime.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label_CreationTime.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_CreationTime.Location = new System.Drawing.Point(140, 38);
            this.label_CreationTime.MinimumSize = new System.Drawing.Size(137, 21);
            this.label_CreationTime.Name = "label_CreationTime";
            this.label_CreationTime.Size = new System.Drawing.Size(137, 21);
            this.label_CreationTime.TabIndex = 0;
            this.label_CreationTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // radioButton2_ModifiedDate
            // 
            this.radioButton2_ModifiedDate.AutoSize = true;
            this.radioButton2_ModifiedDate.Location = new System.Drawing.Point(3, 47);
            this.radioButton2_ModifiedDate.Name = "radioButton2_ModifiedDate";
            this.radioButton2_ModifiedDate.Size = new System.Drawing.Size(14, 13);
            this.radioButton2_ModifiedDate.TabIndex = 0;
            // 
            // label_ModificationTime
            // 
            this.label_ModificationTime.AutoSize = true;
            this.label_ModificationTime.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.label_ModificationTime.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label_ModificationTime.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_ModificationTime.Location = new System.Drawing.Point(140, 83);
            this.label_ModificationTime.MinimumSize = new System.Drawing.Size(137, 21);
            this.label_ModificationTime.Name = "label_ModificationTime";
            this.label_ModificationTime.Size = new System.Drawing.Size(137, 21);
            this.label_ModificationTime.TabIndex = 0;
            this.label_ModificationTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // radioButton3_AccessedDate
            // 
            this.radioButton3_AccessedDate.AutoSize = true;
            this.radioButton3_AccessedDate.Location = new System.Drawing.Point(3, 90);
            this.radioButton3_AccessedDate.Name = "radioButton3_AccessedDate";
            this.radioButton3_AccessedDate.Size = new System.Drawing.Size(14, 13);
            this.radioButton3_AccessedDate.TabIndex = 0;
            // 
            // label_AccessedTime
            // 
            this.label_AccessedTime.AutoSize = true;
            this.label_AccessedTime.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.label_AccessedTime.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label_AccessedTime.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_AccessedTime.Location = new System.Drawing.Point(139, 124);
            this.label_AccessedTime.MinimumSize = new System.Drawing.Size(137, 21);
            this.label_AccessedTime.Name = "label_AccessedTime";
            this.label_AccessedTime.Size = new System.Drawing.Size(137, 21);
            this.label_AccessedTime.TabIndex = 0;
            this.label_AccessedTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // radioButtonPanel1_DecideWhichTime
            // 
            this.radioButtonPanel1_DecideWhichTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.radioButtonPanel1_DecideWhichTime.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.radioButtonPanel1_DecideWhichTime.Controls.Add(this.radioGroupBox1_pickFolderForCompare);
            this.radioButtonPanel1_DecideWhichTime.Controls.Add(this.label_CreationTime);
            this.radioButtonPanel1_DecideWhichTime.Controls.Add(this.label3_AccessedDate);
            this.radioButtonPanel1_DecideWhichTime.Controls.Add(this.label_ModificationTime);
            this.radioButtonPanel1_DecideWhichTime.Controls.Add(this.label2_ModifiedDate);
            this.radioButtonPanel1_DecideWhichTime.Controls.Add(this.label1_CreationDate);
            this.radioButtonPanel1_DecideWhichTime.Controls.Add(this.label_AccessedTime);
            this.radioButtonPanel1_DecideWhichTime.Controls.Add(this.radioGroupBox3_UseTimeFrom);
            this.radioButtonPanel1_DecideWhichTime.Controls.Add(this.radioGroupBox1_SpecifyTime);
            this.radioButtonPanel1_DecideWhichTime.Controls.Add(this.radioGroupBox2_CurrentSelectionTime);
            this.radioButtonPanel1_DecideWhichTime.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButtonPanel1_DecideWhichTime.Location = new System.Drawing.Point(124, 249);
            this.radioButtonPanel1_DecideWhichTime.Name = "radioButtonPanel1_DecideWhichTime";
            this.radioButtonPanel1_DecideWhichTime.Size = new System.Drawing.Size(430, 161);
            this.radioButtonPanel1_DecideWhichTime.TabIndex = 1;
            // 
            // radioGroupBox1_pickFolderForCompare
            // 
            this.radioGroupBox1_pickFolderForCompare.Controls.Add(this.label1);
            this.radioGroupBox1_pickFolderForCompare.Location = new System.Drawing.Point(9, 82);
            this.radioGroupBox1_pickFolderForCompare.Name = "radioGroupBox1_pickFolderForCompare";
            this.radioGroupBox1_pickFolderForCompare.Size = new System.Drawing.Size(114, 73);
            this.radioGroupBox1_pickFolderForCompare.TabIndex = 13;
            this.radioGroupBox1_pickFolderForCompare.TabStop = false;
            this.radioGroupBox1_pickFolderForCompare.Text = " ";
            // 
            // label3_AccessedDate
            // 
            this.label3_AccessedDate.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3_AccessedDate.Location = new System.Drawing.Point(153, 108);
            this.label3_AccessedDate.Name = "label3_AccessedDate";
            this.label3_AccessedDate.Size = new System.Drawing.Size(122, 13);
            this.label3_AccessedDate.TabIndex = 3;
            this.label3_AccessedDate.Text = "Last Access Date/Time";
            this.label3_AccessedDate.Click += new System.EventHandler(this.label3_AccessDate_Click);
            // 
            // label2_ModifiedDate
            // 
            this.label2_ModifiedDate.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2_ModifiedDate.Location = new System.Drawing.Point(153, 65);
            this.label2_ModifiedDate.Name = "label2_ModifiedDate";
            this.label2_ModifiedDate.Size = new System.Drawing.Size(120, 13);
            this.label2_ModifiedDate.TabIndex = 2;
            this.label2_ModifiedDate.Text = "Modified Date/Time";
            this.label2_ModifiedDate.Click += new System.EventHandler(this.label2_ModifiedDate_Click);
            // 
            // label1_CreationDate
            // 
            this.label1_CreationDate.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1_CreationDate.Location = new System.Drawing.Point(153, 21);
            this.label1_CreationDate.Name = "label1_CreationDate";
            this.label1_CreationDate.Size = new System.Drawing.Size(121, 13);
            this.label1_CreationDate.TabIndex = 1;
            this.label1_CreationDate.Text = "Creation Date/Time";
            this.label1_CreationDate.Click += new System.EventHandler(this.label1_CreationDate_Click);
            // 
            // radioGroupBox3_UseTimeFrom
            // 
            this.radioGroupBox3_UseTimeFrom.Controls.Add(this.panel1);
            this.radioGroupBox3_UseTimeFrom.Controls.Add(this.panel2);
            this.radioGroupBox3_UseTimeFrom.Controls.Add(this.panel3);
            this.radioGroupBox3_UseTimeFrom.DisableChildrenIfUnchecked = true;
            this.radioGroupBox3_UseTimeFrom.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioGroupBox3_UseTimeFrom.Location = new System.Drawing.Point(293, 0);
            this.radioGroupBox3_UseTimeFrom.Name = "radioGroupBox3_UseTimeFrom";
            this.radioGroupBox3_UseTimeFrom.Padding = new System.Windows.Forms.Padding(9, 3, 3, 3);
            this.radioGroupBox3_UseTimeFrom.Size = new System.Drawing.Size(123, 156);
            this.radioGroupBox3_UseTimeFrom.TabIndex = 12;
            this.radioGroupBox3_UseTimeFrom.TabStop = false;
            this.radioGroupBox3_UseTimeFrom.Text = "Source Time From:";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.radioButton4_setfromRandom);
            this.panel1.Controls.Add(this.radioButton3_setfromAccessed);
            this.panel1.Controls.Add(this.radioButton1_setfromCreation);
            this.panel1.Controls.Add(this.radioButton2_setfromModified);
            this.panel1.Enabled = false;
            this.panel1.Location = new System.Drawing.Point(12, 81);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(99, 72);
            this.panel1.TabIndex = 1;
            // 
            // radioButton4_setfromRandom
            // 
            this.radioButton4_setfromRandom.Checked = true;
            this.radioButton4_setfromRandom.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton4_setfromRandom.Location = new System.Drawing.Point(7, 51);
            this.radioButton4_setfromRandom.Name = "radioButton4_setfromRandom";
            this.radioButton4_setfromRandom.Size = new System.Drawing.Size(88, 17);
            this.radioButton4_setfromRandom.TabIndex = 3;
            this.radioButton4_setfromRandom.TabStop = true;
            this.radioButton4_setfromRandom.Text = "Any/Random";
            this.radioButton4_setfromRandom.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.radioButton3_Random);
            this.panel2.Controls.Add(this.radioButton2_Newest);
            this.panel2.Controls.Add(this.radioButton1_Oldest);
            this.panel2.Enabled = false;
            this.panel2.Location = new System.Drawing.Point(25, 30);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(71, 52);
            this.panel2.TabIndex = 2;
            // 
            // radioButton3Random
            // 
            this.radioButton3_Random.Checked = true;
            this.radioButton3_Random.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton3_Random.Location = new System.Drawing.Point(4, 33);
            this.radioButton3_Random.Name = "radioButton3_Random";
            this.radioButton3_Random.Size = new System.Drawing.Size(66, 16);
            this.radioButton3_Random.TabIndex = 3;
            this.radioButton3_Random.TabStop = true;
            this.radioButton3_Random.Text = "Random";
            // 
            // radioButton2Newest
            // 
            this.radioButton2_Newest.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton2_Newest.Location = new System.Drawing.Point(4, 17);
            this.radioButton2_Newest.Name = "radioButton2_Newest";
            this.radioButton2_Newest.Size = new System.Drawing.Size(61, 16);
            this.radioButton2_Newest.TabIndex = 1;
            this.radioButton2_Newest.Text = "Newest";
            // 
            // radioButton1Oldest
            // 
            this.radioButton1_Oldest.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton1_Oldest.Location = new System.Drawing.Point(4, 1);
            this.radioButton1_Oldest.Name = "radioButton1_Oldest";
            this.radioButton1_Oldest.Size = new System.Drawing.Size(56, 15);
            this.radioButton1_Oldest.TabIndex = 0;
            this.radioButton1_Oldest.Text = "Oldest";
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.radioButton2_useTimefromSubdir);
            this.panel3.Controls.Add(this.radioButton1_useTimefromFile);
            this.panel3.Enabled = false;
            this.panel3.Location = new System.Drawing.Point(2, 12);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(118, 19);
            this.panel3.TabIndex = 3;
            // 
            // radioButton2_useTimefromSubdir
            // 
            this.radioButton2_useTimefromSubdir.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton2_useTimefromSubdir.Location = new System.Drawing.Point(62, 0);
            this.radioButton2_useTimefromSubdir.Name = "radioButton2_useTimefromSubdir";
            this.radioButton2_useTimefromSubdir.Size = new System.Drawing.Size(56, 17);
            this.radioButton2_useTimefromSubdir.TabIndex = 0;
            this.radioButton2_useTimefromSubdir.TabStop = true;
            this.radioButton2_useTimefromSubdir.Text = "SubDir";
            this.radioButton2_useTimefromSubdir.UseVisualStyleBackColor = true;
            // 
            // radioButton1_useTimefromFile
            // 
            this.radioButton1_useTimefromFile.Checked = true;
            this.radioButton1_useTimefromFile.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton1_useTimefromFile.Location = new System.Drawing.Point(2, 0);
            this.radioButton1_useTimefromFile.Name = "radioButton1_useTimefromFile";
            this.radioButton1_useTimefromFile.Size = new System.Drawing.Size(59, 17);
            this.radioButton1_useTimefromFile.TabIndex = 1;
            this.radioButton1_useTimefromFile.TabStop = true;
            this.radioButton1_useTimefromFile.Text = "SubFile";
            this.radioButton1_useTimefromFile.UseVisualStyleBackColor = true;
            // 
            // radioGroupBox1_SpecifyTime
            // 
            this.radioGroupBox1_SpecifyTime.Controls.Add(this.dateTimePicker_Time);
            this.radioGroupBox1_SpecifyTime.Controls.Add(this.dateTimePicker_Date);
            this.radioGroupBox1_SpecifyTime.DisableChildrenIfUnchecked = true;
            this.radioGroupBox1_SpecifyTime.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioGroupBox1_SpecifyTime.Location = new System.Drawing.Point(9, 0);
            this.radioGroupBox1_SpecifyTime.Name = "radioGroupBox1_SpecifyTime";
            this.radioGroupBox1_SpecifyTime.Size = new System.Drawing.Size(114, 78);
            this.radioGroupBox1_SpecifyTime.TabIndex = 10;
            this.radioGroupBox1_SpecifyTime.TabStop = false;
            this.radioGroupBox1_SpecifyTime.Text = "Specify Time:";
            // 
            // radioGroupBox2_CurrentSelectionTime
            // 
            this.radioGroupBox2_CurrentSelectionTime.Controls.Add(this.labelHidden_PathName);
            this.radioGroupBox2_CurrentSelectionTime.Controls.Add(this.radioButtonPanel2);
            this.radioGroupBox2_CurrentSelectionTime.DisableChildrenIfUnchecked = true;
            this.radioGroupBox2_CurrentSelectionTime.Enabled = false;
            this.radioGroupBox2_CurrentSelectionTime.Location = new System.Drawing.Point(132, 0);
            this.radioGroupBox2_CurrentSelectionTime.Name = "radioGroupBox2_CurrentSelectionTime";
            this.radioGroupBox2_CurrentSelectionTime.Padding = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.radioGroupBox2_CurrentSelectionTime.Size = new System.Drawing.Size(150, 155);
            this.radioGroupBox2_CurrentSelectionTime.TabIndex = 11;
            this.radioGroupBox2_CurrentSelectionTime.TabStop = false;
            this.radioGroupBox2_CurrentSelectionTime.Text = "From Currently Selected:";
            // 
            // labelHidden_PathName
            // 
            this.labelHidden_PathName.AutoSize = true;
            this.labelHidden_PathName.Location = new System.Drawing.Point(130, 12);
            this.labelHidden_PathName.Name = "labelHidden_PathName";
            this.labelHidden_PathName.Size = new System.Drawing.Size(11, 13);
            this.labelHidden_PathName.TabIndex = 4;
            this.labelHidden_PathName.Text = "-";
            this.labelHidden_PathName.Visible = false;
            // 
            // radioButtonPanel2
            // 
            this.radioButtonPanel2.Controls.Add(this.radioButton1_CreationDate);
            this.radioButtonPanel2.Controls.Add(this.radioButton3_AccessedDate);
            this.radioButtonPanel2.Controls.Add(this.radioButton2_ModifiedDate);
            this.radioButtonPanel2.Enabled = false;
            this.radioButtonPanel2.Location = new System.Drawing.Point(4, 18);
            this.radioButtonPanel2.Name = "radioButtonPanel2";
            this.radioButtonPanel2.Size = new System.Drawing.Size(20, 111);
            this.radioButtonPanel2.TabIndex = 16;
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
            this.listView_Contents.Location = new System.Drawing.Point(0, 20);
            this.listView_Contents.MultiSelect = false;
            this.listView_Contents.Name = "listView_Contents";
            this.listView_Contents.Size = new System.Drawing.Size(326, 181);
            this.listView_Contents.SmallImageList = this.imageList_Files;
            this.listView_Contents.TabIndex = 7;
            this.listView_Contents.UseCompatibleStateImageBehavior = false;
            this.listView_Contents.View = System.Windows.Forms.View.Details;
            this.listView_Contents.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.listView_Contents_ItemSelectionChanged);
            // 
            // listview_ContentsColumHeader1
            // 
            this.listview_ContentsColumHeader1.Text = "Files";
            this.listview_ContentsColumHeader1.Width = 291;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.tabControl1.Appearance = System.Windows.Forms.TabAppearance.Buttons;
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.Location = new System.Drawing.Point(11, 356);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(106, 24);
            this.tabControl1.TabIndex = 5;
            this.tabControl1.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabControl1_Selected);
            this.tabControl1.MouseHover += new System.EventHandler(this.tabControl1_MouseHover);
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(98, 0);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Mode 1";
            this.toolTip1.SetToolTip(this.tabPage1, "Simple Set");
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(98, 0);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Mode 2";
            this.toolTip1.SetToolTip(this.tabPage2, "Set Folders based on Files/Dirs Inside");
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(5, 26);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.explorerTree1);
            this.splitContainer1.Panel1MinSize = 150;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.checkBoxShouldFiles);
            this.splitContainer1.Panel2.Controls.Add(this.checkBox_Recurse);
            this.splitContainer1.Panel2.Controls.Add(this.listView_Contents);
            this.splitContainer1.Size = new System.Drawing.Size(549, 220);
            this.splitContainer1.SplitterDistance = 220;
            this.splitContainer1.SplitterWidth = 6;
            this.splitContainer1.TabIndex = 23;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(2, 19);
            this.label1.MaximumSize = new System.Drawing.Size(118, 71);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(109, 39);
            this.label1.TabIndex = 1;
            this.label1.Text = "Choose 2nd identical folder as source, at the prompt...";
            // 
            // explorerTree1
            // 
            this.explorerTree1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.explorerTree1.BackColor = System.Drawing.Color.White;
            this.explorerTree1.CurrentPath = null;
            this.explorerTree1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.explorerTree1.Location = new System.Drawing.Point(0, 0);
            this.explorerTree1.Name = "explorerTree1";
            this.explorerTree1.ShowAddressbar = true;
            this.explorerTree1.ShowMyDocuments = true;
            this.explorerTree1.ShowMyFavorites = false;
            this.explorerTree1.ShowMyNetwork = true;
            this.explorerTree1.ShowToolbar = false;
            this.explorerTree1.Size = new System.Drawing.Size(221, 220);
            this.explorerTree1.TabIndex = 6;
            this.explorerTree1.PathChanged += new WindowsExplorer.ExplorerTree.PathChangedEventHandler(this.explorerTree1_PathChanged);
            // 
            // Form_Main
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
            this.ClientSize = new System.Drawing.Size(560, 415);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.button_Browse);
            this.Controls.Add(this.button_GoUpdate);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.groupBoxCMA);
            this.Controls.Add(this.radioButtonPanel1_DecideWhichTime);
            this.Controls.Add(this.label_FPath);
            this.Controls.Add(this.label_FilePathHeading);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Menu = this.mainMenu_Main;
            this.MinimumSize = new System.Drawing.Size(576, 454);
            this.Name = "Form_Main";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Folder Timestamp Modifier by genBTC";
            this.Load += new System.EventHandler(this.Form_Main_Load);
            this.groupBoxCMA.ResumeLayout(false);
            this.radioButtonPanel1_DecideWhichTime.ResumeLayout(false);
            this.radioButtonPanel1_DecideWhichTime.PerformLayout();
            this.radioGroupBox1_pickFolderForCompare.ResumeLayout(false);
            this.radioGroupBox1_pickFolderForCompare.PerformLayout();
            this.radioGroupBox3_UseTimeFrom.ResumeLayout(false);
            this.radioGroupBox3_UseTimeFrom.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.radioGroupBox1_SpecifyTime.ResumeLayout(false);
            this.radioGroupBox1_SpecifyTime.PerformLayout();
            this.radioGroupBox2_CurrentSelectionTime.ResumeLayout(false);
            this.radioGroupBox2_CurrentSelectionTime.PerformLayout();
            this.radioButtonPanel2.ResumeLayout(false);
            this.radioButtonPanel2.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        private MenuItem menuItem_Preferences;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private ToolTip toolTip1;
        private WindowsExplorer.ExplorerTree explorerTree1;
        private SplitContainer splitContainer1;
        private UIToolbox.RadioGroupBox radioGroupBox1_pickFolderForCompare;
        private Label label1;

    }

}
