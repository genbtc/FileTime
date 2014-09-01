using System;
using System.Windows.Forms;
using genBTC.FileTime.Classes;

namespace genBTC.FileTime
{

    partial class Form_Confirm 
    {
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
//            synclistView1_ConfirmToFilesToConfirmList();
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private ImageList imageList_Files;
        private Label label1;
        private Button button1_Confirm;

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_Confirm));
            this.label1 = new System.Windows.Forms.Label();
            this.imageList_Files = new System.Windows.Forms.ImageList(this.components);
            this.button1_Confirm = new System.Windows.Forms.Button();
            this.groupBoxConditions = new System.Windows.Forms.GroupBox();
            this.radioButton3All = new System.Windows.Forms.RadioButton();
            this.radioButton2Older = new System.Windows.Forms.RadioButton();
            this.radioButton1Newer = new System.Windows.Forms.RadioButton();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deSelectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.invertSelectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeNAOnlysToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.condenseMultipleLinesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ComboBox1 = new System.Windows.Forms.ComboBox();
            this.ComboBox2 = new System.Windows.Forms.ComboBox();
            this.ComboBox3 = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.listView1_Confirm = new genBTC.FileTime.Classes.CustomListView();
            this.ColumnHeader_Name = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ColumnHeader_Created = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ColumnHeader_Modified = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ColumnHeader_Accessed = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBoxConditions.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(13, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(110, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Un-Check to Exclude:";
            // 
            // imageList_Files
            // 
            this.imageList_Files.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList_Files.ImageStream")));
            this.imageList_Files.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList_Files.Images.SetKeyName(0, "ico1.ico");
            this.imageList_Files.Images.SetKeyName(1, "ico5.ico");
            // 
            // button1_Confirm
            // 
            this.button1_Confirm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button1_Confirm.Location = new System.Drawing.Point(742, 402);
            this.button1_Confirm.Name = "button1_Confirm";
            this.button1_Confirm.Size = new System.Drawing.Size(88, 40);
            this.button1_Confirm.TabIndex = 3;
            this.button1_Confirm.Text = "Confirm";
            this.button1_Confirm.UseVisualStyleBackColor = true;
            this.button1_Confirm.Click += new System.EventHandler(this.button1_Confirm_Click);
            // 
            // groupBoxConditions
            // 
            this.groupBoxConditions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBoxConditions.Controls.Add(this.radioButton3All);
            this.groupBoxConditions.Controls.Add(this.radioButton2Older);
            this.groupBoxConditions.Controls.Add(this.radioButton1Newer);
            this.groupBoxConditions.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxConditions.Location = new System.Drawing.Point(520, 400);
            this.groupBoxConditions.Name = "groupBoxConditions";
            this.groupBoxConditions.Size = new System.Drawing.Size(216, 42);
            this.groupBoxConditions.TabIndex = 10;
            this.groupBoxConditions.TabStop = false;
            this.groupBoxConditions.Text = "Update If New Date is:";
            // 
            // radioButton3All
            // 
            this.radioButton3All.Checked = true;
            this.radioButton3All.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton3All.Location = new System.Drawing.Point(140, 16);
            this.radioButton3All.Name = "radioButton3All";
            this.radioButton3All.Size = new System.Drawing.Size(71, 17);
            this.radioButton3All.TabIndex = 1;
            this.radioButton3All.TabStop = true;
            this.radioButton3All.Text = "Always";
            this.radioButton3All.UseVisualStyleBackColor = true;
            // 
            // radioButton2Older
            // 
            this.radioButton2Older.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton2Older.Location = new System.Drawing.Point(79, 16);
            this.radioButton2Older.Name = "radioButton2Older";
            this.radioButton2Older.Size = new System.Drawing.Size(72, 17);
            this.radioButton2Older.TabIndex = 2;
            this.radioButton2Older.Text = "Older";
            this.radioButton2Older.UseVisualStyleBackColor = true;
            // 
            // radioButton1Newer
            // 
            this.radioButton1Newer.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton1Newer.Location = new System.Drawing.Point(16, 16);
            this.radioButton1Newer.Name = "radioButton1Newer";
            this.radioButton1Newer.Size = new System.Drawing.Size(72, 17);
            this.radioButton1Newer.TabIndex = 3;
            this.radioButton1Newer.Text = "Newer";
            this.radioButton1Newer.UseVisualStyleBackColor = true;
            // 
            // menuStrip1
            // 
            this.menuStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(851, 24);
            this.menuStrip1.TabIndex = 11;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.Click += new System.EventHandler(this.menuStrip1_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectAllToolStripMenuItem,
            this.deSelectAllToolStripMenuItem,
            this.invertSelectionToolStripMenuItem,
            this.removeNAOnlysToolStripMenuItem,
            this.condenseMultipleLinesToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "&Edit";
            this.editToolStripMenuItem.Click += new System.EventHandler(this.menuStrip1_Click);
            // 
            // selectAllToolStripMenuItem
            // 
            this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
            this.selectAllToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.selectAllToolStripMenuItem.Size = new System.Drawing.Size(261, 22);
            this.selectAllToolStripMenuItem.Text = "Select All";
            this.selectAllToolStripMenuItem.Click += new System.EventHandler(this.selectAllToolStripMenuItem_Click);
            // 
            // deSelectAllToolStripMenuItem
            // 
            this.deSelectAllToolStripMenuItem.Name = "deSelectAllToolStripMenuItem";
            this.deSelectAllToolStripMenuItem.Size = new System.Drawing.Size(261, 22);
            this.deSelectAllToolStripMenuItem.Text = "De-Select All";
            this.deSelectAllToolStripMenuItem.Click += new System.EventHandler(this.deSelectAllToolStripMenuItem_Click);
            // 
            // invertSelectionToolStripMenuItem
            // 
            this.invertSelectionToolStripMenuItem.Name = "invertSelectionToolStripMenuItem";
            this.invertSelectionToolStripMenuItem.Size = new System.Drawing.Size(261, 22);
            this.invertSelectionToolStripMenuItem.Text = "Invert Selection";
            this.invertSelectionToolStripMenuItem.Click += new System.EventHandler(this.invertSelectionToolStripMenuItem_Click);
            // 
            // removeNAOnlysToolStripMenuItem
            // 
            this.removeNAOnlysToolStripMenuItem.Name = "removeNAOnlysToolStripMenuItem";
            this.removeNAOnlysToolStripMenuItem.Size = new System.Drawing.Size(261, 22);
            this.removeNAOnlysToolStripMenuItem.Text = "Remove N/A Onlys";
            this.removeNAOnlysToolStripMenuItem.Click += new System.EventHandler(this.removeNAOnlysToolStripMenuItem_Click);
            // 
            // condenseMultipleLinesToolStripMenuItem
            // 
            this.condenseMultipleLinesToolStripMenuItem.Name = "condenseMultipleLinesToolStripMenuItem";
            this.condenseMultipleLinesToolStripMenuItem.Size = new System.Drawing.Size(261, 22);
            this.condenseMultipleLinesToolStripMenuItem.Text = "Combine DUPlicate Filenames (Try)";
            this.condenseMultipleLinesToolStripMenuItem.Click += new System.EventHandler(this.condenseMultipleLinesToolStripMenuItem_Click);
            // 
            // ComboBox1
            // 
            this.ComboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBox1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ComboBox1.FormattingEnabled = true;
            this.ComboBox1.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.ComboBox1.Items.AddRange(new object[] {
            "",
            "Copy from Modified",
            "Copy from Accessed",
            "Unset Created"});
            this.ComboBox1.Location = new System.Drawing.Point(3, 2);
            this.ComboBox1.Name = "ComboBox1";
            this.ComboBox1.Size = new System.Drawing.Size(128, 21);
            this.ComboBox1.TabIndex = 12;
            this.ComboBox1.SelectedIndexChanged += new System.EventHandler(this.ThreeComboBox_SelectedChange);
            // 
            // ComboBox2
            // 
            this.ComboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBox2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ComboBox2.FormattingEnabled = true;
            this.ComboBox2.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.ComboBox2.Items.AddRange(new object[] {
            "",
            "Copy from Created",
            "Copy from Accessed",
            "Unset Modified"});
            this.ComboBox2.Location = new System.Drawing.Point(135, 2);
            this.ComboBox2.Name = "ComboBox2";
            this.ComboBox2.Size = new System.Drawing.Size(125, 21);
            this.ComboBox2.TabIndex = 13;
            this.ComboBox2.SelectedIndexChanged += new System.EventHandler(this.ThreeComboBox_SelectedChange);
            // 
            // ComboBox3
            // 
            this.ComboBox3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBox3.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ComboBox3.FormattingEnabled = true;
            this.ComboBox3.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.ComboBox3.Items.AddRange(new object[] {
            "",
            "Copy from Created",
            "Copy from Modified",
            "Unset Accessed"});
            this.ComboBox3.Location = new System.Drawing.Point(264, 2);
            this.ComboBox3.Name = "ComboBox3";
            this.ComboBox3.Size = new System.Drawing.Size(125, 21);
            this.ComboBox3.TabIndex = 14;
            this.ComboBox3.SelectedIndexChanged += new System.EventHandler(this.ThreeComboBox_SelectedChange);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(252, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(159, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "Perform on CheckBoxed Items: ";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ComboBox2);
            this.panel1.Controls.Add(this.ComboBox1);
            this.panel1.Controls.Add(this.ComboBox3);
            this.panel1.Location = new System.Drawing.Point(411, 23);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(392, 24);
            this.panel1.TabIndex = 16;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolStripMenuItem2,
            this.toolStripMenuItem3,
            this.toolStripMenuItem4,
            this.toolStripMenuItem6,
            this.toolStripSeparator1,
            this.toolStripMenuItem5});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(185, 142);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(184, 22);
            this.toolStripMenuItem1.Text = "Change &Date...";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(184, 22);
            this.toolStripMenuItem2.Text = "Un-Set &Created";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenu_ThreeRemoves);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(184, 22);
            this.toolStripMenuItem3.Text = "Un-Set &Modified";
            this.toolStripMenuItem3.Click += new System.EventHandler(this.toolStripMenu_ThreeRemoves);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(184, 22);
            this.toolStripMenuItem4.Text = "Un-Set &Accessed";
            this.toolStripMenuItem4.Click += new System.EventHandler(this.toolStripMenu_ThreeRemoves);
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(184, 22);
            this.toolStripMenuItem6.Text = "Delete Item from List";
            this.toolStripMenuItem6.Click += new System.EventHandler(this.toolStripMenuItem6_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(181, 6);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(184, 22);
            this.toolStripMenuItem5.Text = "&Open Explorer Here";
            this.toolStripMenuItem5.Click += new System.EventHandler(this.toolStripMenuItem5_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripProgressBar1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 446);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.ManagerRenderMode;
            this.statusStrip1.Size = new System.Drawing.Size(851, 22);
            this.statusStrip1.TabIndex = 17;
            this.statusStrip1.Text = "0123";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.AutoSize = false;
            this.toolStripStatusLabel1.AutoToolTip = true;
            this.toolStripStatusLabel1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(173, 17);
            this.toolStripStatusLabel1.Spring = true;
            this.toolStripStatusLabel1.ToolTipText = "Number of Items";
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.AutoToolTip = true;
            this.toolStripProgressBar1.Margin = new System.Windows.Forms.Padding(12, 3, 1, 3);
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.toolStripProgressBar1.Size = new System.Drawing.Size(650, 16);
            this.toolStripProgressBar1.Step = 1;
            this.toolStripProgressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.toolStripProgressBar1.ToolTipText = "Progress Bar";
            // 
            // listView1_Confirm
            // 
            this.listView1_Confirm.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1_Confirm.CheckBoxes = true;
            this.listView1_Confirm.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ColumnHeader_Name,
            this.ColumnHeader_Created,
            this.ColumnHeader_Modified,
            this.ColumnHeader_Accessed});
            this.listView1_Confirm.ContextMenuAllowed = false;
            this.listView1_Confirm.ContextMenuStrip = this.contextMenuStrip1;
            this.listView1_Confirm.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listView1_Confirm.FullRowSelect = true;
            this.listView1_Confirm.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listView1_Confirm.HideSelection = false;
            this.listView1_Confirm.Location = new System.Drawing.Point(12, 47);
            this.listView1_Confirm.Name = "listView1_Confirm";
            this.listView1_Confirm.ShowItemToolTips = true;
            this.listView1_Confirm.Size = new System.Drawing.Size(827, 350);
            this.listView1_Confirm.SmallImageList = this.imageList_Files;
            this.listView1_Confirm.TabIndex = 2;
            this.listView1_Confirm.UseCompatibleStateImageBehavior = false;
            this.listView1_Confirm.View = System.Windows.Forms.View.Details;
            this.listView1_Confirm.KeyUp += new System.Windows.Forms.KeyEventHandler(this.listView1_Confirm_KeyUp);
            // 
            // ColumnHeader_Name
            // 
            this.ColumnHeader_Name.Text = "Name";
            this.ColumnHeader_Name.Width = 400;
            // 
            // ColumnHeader_Created
            // 
            this.ColumnHeader_Created.Text = "Created";
            this.ColumnHeader_Created.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ColumnHeader_Created.Width = 130;
            // 
            // ColumnHeader_Modified
            // 
            this.ColumnHeader_Modified.Text = "Modified";
            this.ColumnHeader_Modified.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ColumnHeader_Modified.Width = 130;
            // 
            // ColumnHeader_Accessed
            // 
            this.ColumnHeader_Accessed.Text = "Accessed";
            this.ColumnHeader_Accessed.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ColumnHeader_Accessed.Width = 130;
            // 
            // Form_Confirm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(851, 468);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.groupBoxConditions);
            this.Controls.Add(this.button1_Confirm);
            this.Controls.Add(this.listView1_Confirm);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(857, 200);
            this.Name = "Form_Confirm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Confirm Changes";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_Confirm_FormClosing);
            this.groupBoxConditions.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        // Subscribe to Opening event for the ContextMenu/ContextMenuStrip. 
        // If contextMenuAllowed is false, set e.Cancel to true and return. This is stop the context menu from actually opening.
        void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.listView1_Confirm.ContextMenuAllowed == false)
                e.Cancel = true;
        }


        private CustomListView listView1_Confirm;
        private ContextMenuStrip contextMenuStrip1;

        private GroupBox groupBoxConditions;
        private RadioButton radioButton3All;
        private RadioButton radioButton2Older;
        private RadioButton radioButton1Newer;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem editToolStripMenuItem;
        private ToolStripMenuItem selectAllToolStripMenuItem;
        private ToolStripMenuItem deSelectAllToolStripMenuItem;
        private ToolStripMenuItem invertSelectionToolStripMenuItem;
        private ComboBox ComboBox1;
        private ComboBox ComboBox2;
        private ComboBox ComboBox3;
        private ToolStripMenuItem removeNAOnlysToolStripMenuItem;
        private Label label2;
        private Panel panel1;
        private ColumnHeader ColumnHeader_Name;
        private ColumnHeader ColumnHeader_Created;
        private ColumnHeader ColumnHeader_Modified;
        private ColumnHeader ColumnHeader_Accessed;
        private ToolStripMenuItem toolStripMenuItem1;
        private ToolStripMenuItem toolStripMenuItem2;
        private ToolStripMenuItem toolStripMenuItem3;
        private ToolStripMenuItem toolStripMenuItem4;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem toolStripMenuItem5;
        private StatusStrip statusStrip1;
        private ToolStripProgressBar toolStripProgressBar1;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private ToolStripMenuItem condenseMultipleLinesToolStripMenuItem;
        private ToolStripMenuItem toolStripMenuItem6;

    }
}