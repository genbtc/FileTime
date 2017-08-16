namespace genBTC.FileTime.Forms
{
    partial class Form_Preferences
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
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_Preferences));
            this.button1_OK = new System.Windows.Forms.Button();
            this.button2_Cancel = new System.Windows.Forms.Button();
            this.button_UseCurrentDir = new System.Windows.Forms.Button();
            this.button_Browse = new System.Windows.Forms.Button();
            this.checkBox8_useRootDirAsContainer = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.checkBox7_Mode1addRootDir = new System.Windows.Forms.CheckBox();
            this.checkBox4a_ShowReadOnly = new System.Windows.Forms.CheckBox();
            this.checkBox2a_ShowSystem = new System.Windows.Forms.CheckBox();
            this.checkBox3a_ShowHidden = new System.Windows.Forms.CheckBox();
            this.textBox6_startupdir = new System.Windows.Forms.TextBox();
            this.checkBox6_StartupDir = new System.Windows.Forms.CheckBox();
            this.checkBox2b_SkipSystem = new System.Windows.Forms.CheckBox();
            this.checkBox3b_SkipHidden = new System.Windows.Forms.CheckBox();
            this.checkBox4b_SkipReadOnly = new System.Windows.Forms.CheckBox();
            this.checkBox1_ShowReadOnlyNotices = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // button1_OK
            // 
            this.button1_OK.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.button1_OK.Location = new System.Drawing.Point(279, 232);
            this.button1_OK.Name = "button1_OK";
            this.button1_OK.Size = new System.Drawing.Size(81, 29);
            this.button1_OK.TabIndex = 11;
            this.button1_OK.Text = "OK";
            this.button1_OK.UseVisualStyleBackColor = true;
            this.button1_OK.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2_Cancel
            // 
            this.button2_Cancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.button2_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2_Cancel.Location = new System.Drawing.Point(194, 235);
            this.button2_Cancel.Name = "button2_Cancel";
            this.button2_Cancel.Size = new System.Drawing.Size(64, 23);
            this.button2_Cancel.TabIndex = 12;
            this.button2_Cancel.Text = "Cancel";
            this.button2_Cancel.UseVisualStyleBackColor = true;
            this.button2_Cancel.Click += new System.EventHandler(this.button2_Cancel_Click);
            // 
            // button_UseCurrentDir
            // 
            this.button_UseCurrentDir.Location = new System.Drawing.Point(116, 155);
            this.button_UseCurrentDir.Name = "button_UseCurrentDir";
            this.button_UseCurrentDir.Size = new System.Drawing.Size(73, 23);
            this.button_UseCurrentDir.TabIndex = 16;
            this.button_UseCurrentDir.Text = "Use Current";
            this.button_UseCurrentDir.UseVisualStyleBackColor = true;
            this.button_UseCurrentDir.Click += new System.EventHandler(this.button_UseCurrentDir_Click);
            // 
            // button_Browse
            // 
            this.button_Browse.Location = new System.Drawing.Point(31, 155);
            this.button_Browse.Name = "button_Browse";
            this.button_Browse.Size = new System.Drawing.Size(75, 23);
            this.button_Browse.TabIndex = 17;
            this.button_Browse.Text = "Browse...";
            this.button_Browse.UseVisualStyleBackColor = true;
            this.button_Browse.Click += new System.EventHandler(this.button_Browse_Click);
            // 
            // checkBox8_useRootDirAsContainer
            // 
            this.checkBox8_useRootDirAsContainer.AutoSize = true;
            this.checkBox8_useRootDirAsContainer.Checked = global::genBTC.FileTime.Properties.Settings.Default.useRootDirAsContainer;
            this.checkBox8_useRootDirAsContainer.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::genBTC.FileTime.Properties.Settings.Default, "useRootDirAsContainer", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBox8_useRootDirAsContainer.Location = new System.Drawing.Point(13, 211);
            this.checkBox8_useRootDirAsContainer.Name = "checkBox8_useRootDirAsContainer";
            this.checkBox8_useRootDirAsContainer.Size = new System.Drawing.Size(318, 17);
            this.checkBox8_useRootDirAsContainer.TabIndex = 19;
            this.checkBox8_useRootDirAsContainer.Text = "Use root directory as container and operate on all subfolders";
            this.toolTip1.SetToolTip(this.checkBox8_useRootDirAsContainer, resources.GetString("checkBox8_useRootDirAsContainer.ToolTip"));
            this.checkBox8_useRootDirAsContainer.UseVisualStyleBackColor = true;
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 25000;
            this.toolTip1.InitialDelay = 500;
            this.toolTip1.ReshowDelay = 100;
            // 
            // checkBox7_Mode1addRootDir
            // 
            this.checkBox7_Mode1addRootDir.AutoSize = true;
            this.checkBox7_Mode1addRootDir.Checked = global::genBTC.FileTime.Properties.Settings.Default.mode1addrootdir;
            this.checkBox7_Mode1addRootDir.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::genBTC.FileTime.Properties.Settings.Default, "mode1addrootdir", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBox7_Mode1addRootDir.Location = new System.Drawing.Point(13, 187);
            this.checkBox7_Mode1addRootDir.Name = "checkBox7_Mode1addRootDir";
            this.checkBox7_Mode1addRootDir.Size = new System.Drawing.Size(205, 17);
            this.checkBox7_Mode1addRootDir.TabIndex = 18;
            this.checkBox7_Mode1addRootDir.Text = "Include Root Directory during Mode 1";
            this.checkBox7_Mode1addRootDir.UseVisualStyleBackColor = true;
            // 
            // checkBox4a_ShowReadOnly
            // 
            this.checkBox4a_ShowReadOnly.AutoSize = true;
            this.checkBox4a_ShowReadOnly.Checked = global::genBTC.FileTime.Properties.Settings.Default.ShowReadOnly;
            this.checkBox4a_ShowReadOnly.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox4a_ShowReadOnly.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::genBTC.FileTime.Properties.Settings.Default, "ShowReadOnly", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBox4a_ShowReadOnly.Location = new System.Drawing.Point(13, 82);
            this.checkBox4a_ShowReadOnly.Name = "checkBox4a_ShowReadOnly";
            this.checkBox4a_ShowReadOnly.Size = new System.Drawing.Size(130, 17);
            this.checkBox4a_ShowReadOnly.TabIndex = 15;
            this.checkBox4a_ShowReadOnly.Text = "Show Read-Only Files";
            this.checkBox4a_ShowReadOnly.UseVisualStyleBackColor = true;
            // 
            // checkBox2a_ShowSystem
            // 
            this.checkBox2a_ShowSystem.AutoSize = true;
            this.checkBox2a_ShowSystem.Checked = global::genBTC.FileTime.Properties.Settings.Default.ShowSystem;
            this.checkBox2a_ShowSystem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox2a_ShowSystem.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::genBTC.FileTime.Properties.Settings.Default, "ShowSystem", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBox2a_ShowSystem.Location = new System.Drawing.Point(13, 36);
            this.checkBox2a_ShowSystem.Name = "checkBox2a_ShowSystem";
            this.checkBox2a_ShowSystem.Size = new System.Drawing.Size(114, 17);
            this.checkBox2a_ShowSystem.TabIndex = 14;
            this.checkBox2a_ShowSystem.Text = "Show System Files";
            this.checkBox2a_ShowSystem.UseVisualStyleBackColor = true;
            // 
            // checkBox3a_ShowHidden
            // 
            this.checkBox3a_ShowHidden.AutoSize = true;
            this.checkBox3a_ShowHidden.Checked = global::genBTC.FileTime.Properties.Settings.Default.ShowHidden;
            this.checkBox3a_ShowHidden.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox3a_ShowHidden.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::genBTC.FileTime.Properties.Settings.Default, "ShowHidden", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBox3a_ShowHidden.Location = new System.Drawing.Point(13, 59);
            this.checkBox3a_ShowHidden.Name = "checkBox3a_ShowHidden";
            this.checkBox3a_ShowHidden.Size = new System.Drawing.Size(112, 17);
            this.checkBox3a_ShowHidden.TabIndex = 13;
            this.checkBox3a_ShowHidden.Text = "Show Hidden Files";
            this.checkBox3a_ShowHidden.UseVisualStyleBackColor = true;
            // 
            // textBox6_startupdir
            // 
            this.textBox6_startupdir.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::genBTC.FileTime.Properties.Settings.Default, "StartupDir", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBox6_startupdir.Location = new System.Drawing.Point(32, 128);
            this.textBox6_startupdir.Name = "textBox6_startupdir";
            this.textBox6_startupdir.Size = new System.Drawing.Size(326, 21);
            this.textBox6_startupdir.TabIndex = 10;
            this.textBox6_startupdir.Text = global::genBTC.FileTime.Properties.Settings.Default.StartupDir;
            // 
            // checkBox6_StartupDir
            // 
            this.checkBox6_StartupDir.AutoSize = true;
            this.checkBox6_StartupDir.Checked = global::genBTC.FileTime.Properties.Settings.Default.useStartupDir;
            this.checkBox6_StartupDir.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::genBTC.FileTime.Properties.Settings.Default, "useStartupDir", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBox6_StartupDir.Location = new System.Drawing.Point(13, 105);
            this.checkBox6_StartupDir.Name = "checkBox6_StartupDir";
            this.checkBox6_StartupDir.Size = new System.Drawing.Size(173, 17);
            this.checkBox6_StartupDir.TabIndex = 9;
            this.checkBox6_StartupDir.Text = "Default Startup Root Directory";
            this.checkBox6_StartupDir.UseVisualStyleBackColor = true;
            // 
            // checkBox2b_SkipSystem
            // 
            this.checkBox2b_SkipSystem.AutoSize = true;
            this.checkBox2b_SkipSystem.Checked = global::genBTC.FileTime.Properties.Settings.Default.SkipSystem;
            this.checkBox2b_SkipSystem.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::genBTC.FileTime.Properties.Settings.Default, "SkipSystem", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBox2b_SkipSystem.Location = new System.Drawing.Point(151, 36);
            this.checkBox2b_SkipSystem.Name = "checkBox2b_SkipSystem";
            this.checkBox2b_SkipSystem.Size = new System.Drawing.Size(107, 17);
            this.checkBox2b_SkipSystem.TabIndex = 4;
            this.checkBox2b_SkipSystem.Text = "Skip System Files";
            this.checkBox2b_SkipSystem.UseVisualStyleBackColor = true;
            // 
            // checkBox3b_SkipHidden
            // 
            this.checkBox3b_SkipHidden.AutoSize = true;
            this.checkBox3b_SkipHidden.Checked = global::genBTC.FileTime.Properties.Settings.Default.SkipHidden;
            this.checkBox3b_SkipHidden.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::genBTC.FileTime.Properties.Settings.Default, "SkipHidden", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBox3b_SkipHidden.Location = new System.Drawing.Point(151, 59);
            this.checkBox3b_SkipHidden.Name = "checkBox3b_SkipHidden";
            this.checkBox3b_SkipHidden.Size = new System.Drawing.Size(105, 17);
            this.checkBox3b_SkipHidden.TabIndex = 3;
            this.checkBox3b_SkipHidden.Text = "Skip Hidden Files";
            this.checkBox3b_SkipHidden.UseVisualStyleBackColor = true;
            // 
            // checkBox4b_SkipReadOnly
            // 
            this.checkBox4b_SkipReadOnly.AutoSize = true;
            this.checkBox4b_SkipReadOnly.Checked = global::genBTC.FileTime.Properties.Settings.Default.SkipReadOnly;
            this.checkBox4b_SkipReadOnly.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::genBTC.FileTime.Properties.Settings.Default, "SkipReadOnly", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBox4b_SkipReadOnly.Location = new System.Drawing.Point(151, 82);
            this.checkBox4b_SkipReadOnly.Name = "checkBox4b_SkipReadOnly";
            this.checkBox4b_SkipReadOnly.Size = new System.Drawing.Size(123, 17);
            this.checkBox4b_SkipReadOnly.TabIndex = 2;
            this.checkBox4b_SkipReadOnly.Text = "Skip Read-Only Files";
            this.checkBox4b_SkipReadOnly.UseVisualStyleBackColor = true;
            // 
            // checkBox1_ShowReadOnlyNotices
            // 
            this.checkBox1_ShowReadOnlyNotices.AutoSize = true;
            this.checkBox1_ShowReadOnlyNotices.Checked = global::genBTC.FileTime.Properties.Settings.Default.ShowNoticesReadOnly;
            this.checkBox1_ShowReadOnlyNotices.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1_ShowReadOnlyNotices.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::genBTC.FileTime.Properties.Settings.Default, "ShowNoticesReadOnly", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBox1_ShowReadOnlyNotices.Location = new System.Drawing.Point(13, 13);
            this.checkBox1_ShowReadOnlyNotices.Name = "checkBox1_ShowReadOnlyNotices";
            this.checkBox1_ShowReadOnlyNotices.Size = new System.Drawing.Size(176, 17);
            this.checkBox1_ShowReadOnlyNotices.TabIndex = 0;
            this.checkBox1_ShowReadOnlyNotices.Text = "Show Read-Only Notice Dialogs";
            this.checkBox1_ShowReadOnlyNotices.UseVisualStyleBackColor = true;
            // 
            // Form_Preferences
            // 
            this.AcceptButton = this.button1_OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button2_Cancel;
            this.ClientSize = new System.Drawing.Size(370, 271);
            this.Controls.Add(this.checkBox8_useRootDirAsContainer);
            this.Controls.Add(this.checkBox7_Mode1addRootDir);
            this.Controls.Add(this.button_Browse);
            this.Controls.Add(this.button_UseCurrentDir);
            this.Controls.Add(this.checkBox4a_ShowReadOnly);
            this.Controls.Add(this.checkBox2a_ShowSystem);
            this.Controls.Add(this.checkBox3a_ShowHidden);
            this.Controls.Add(this.button2_Cancel);
            this.Controls.Add(this.button1_OK);
            this.Controls.Add(this.textBox6_startupdir);
            this.Controls.Add(this.checkBox6_StartupDir);
            this.Controls.Add(this.checkBox2b_SkipSystem);
            this.Controls.Add(this.checkBox3b_SkipHidden);
            this.Controls.Add(this.checkBox4b_SkipReadOnly);
            this.Controls.Add(this.checkBox1_ShowReadOnlyNotices);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(386, 299);
            this.Name = "Form_Preferences";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Preferences";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.CheckBox checkBox1_ShowReadOnlyNotices;
        public System.Windows.Forms.CheckBox checkBox4b_SkipReadOnly;
        public System.Windows.Forms.CheckBox checkBox3b_SkipHidden;
        public System.Windows.Forms.CheckBox checkBox2b_SkipSystem;
        public System.Windows.Forms.CheckBox checkBox6_StartupDir;
        public System.Windows.Forms.TextBox textBox6_startupdir;
        private System.Windows.Forms.Button button1_OK;
        private System.Windows.Forms.Button button2_Cancel;
        public System.Windows.Forms.CheckBox checkBox3a_ShowHidden;
        private System.Windows.Forms.CheckBox checkBox2a_ShowSystem;
        private System.Windows.Forms.CheckBox checkBox4a_ShowReadOnly;
        private System.Windows.Forms.Button button_UseCurrentDir;
        private System.Windows.Forms.Button button_Browse;
        private System.Windows.Forms.CheckBox checkBox7_Mode1addRootDir;
        private System.Windows.Forms.CheckBox checkBox8_useRootDirAsContainer;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}