using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

namespace genBTC.FileTime
{
    /// <summary>
    /// Form_About: Displays information about this application.
    /// Version: 1.0
    /// Company: genBTC
    /// Date Created: July 17 2014
    /// Date Modified: August 18 2014
    /// Author: genBTC, code snippets from elsewhere creative commons
    /// </summary>
    public class Form_About : Form
    {
        /// <summary>
        /// Required designer variables.
        /// </summary>
        private readonly Container _components = null;

        private Button button_Close;
        private Label label_About;
        private LinkLabel linkLabel_Address;
        private PictureBox pictureBox_Icon;
        private PictureBox pictureBox_automationControls;
        private TabControl tabControl_About;
        private TabPage tabPage_About;

        /// <summary>
        /// the About... form window
        /// </summary>
        public Form_About()
        {
            // Required for Windows Form Designer support
            InitializeComponent();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_components != null)
                    _components.Dispose();
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
            var resources = new System.ComponentModel.ComponentResourceManager(typeof (Form_About));
            this.button_Close = new System.Windows.Forms.Button();
            this.pictureBox_Icon = new System.Windows.Forms.PictureBox();
            this.label_About = new System.Windows.Forms.Label();
            this.tabControl_About = new System.Windows.Forms.TabControl();
            this.tabPage_About = new System.Windows.Forms.TabPage();
            this.linkLabel_Address = new System.Windows.Forms.LinkLabel();
            this.pictureBox_automationControls = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox_Icon)).BeginInit();
            this.tabControl_About.SuspendLayout();
            this.tabPage_About.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox_automationControls)).BeginInit();
            this.SuspendLayout();
            // 
            // button_Close
            // 
            this.button_Close.Location = new System.Drawing.Point(416, 465);
            this.button_Close.Name = "button_Close";
            this.button_Close.Size = new System.Drawing.Size(80, 26);
            this.button_Close.TabIndex = 0;
            this.button_Close.Text = "Cancel";
            this.button_Close.Click += new System.EventHandler(this.button_Close_Click);
            // 
            // pictureBox_Icon
            // 
            this.pictureBox_Icon.Location = new System.Drawing.Point(8, 69);
            this.pictureBox_Icon.Name = "pictureBox_Icon";
            this.pictureBox_Icon.Size = new System.Drawing.Size(56, 52);
            this.pictureBox_Icon.TabIndex = 2;
            this.pictureBox_Icon.TabStop = false;
            // 
            // label_About
            // 
            this.label_About.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_About.Location = new System.Drawing.Point(0, 0);
            this.label_About.Name = "label_About";
            this.label_About.Size = new System.Drawing.Size(432, 362);
            this.label_About.TabIndex = 3;
            this.label_About.Text = "Version:";
            // 
            // tabControl_About
            // 
            this.tabControl_About.Controls.Add(this.tabPage_About);
            this.tabControl_About.Location = new System.Drawing.Point(56, 69);
            this.tabControl_About.Name = "tabControl_About";
            this.tabControl_About.SelectedIndex = 0;
            this.tabControl_About.Size = new System.Drawing.Size(440, 388);
            this.tabControl_About.TabIndex = 5;
            // 
            // tabPage_About
            // 
            this.tabPage_About.Controls.Add(this.linkLabel_Address);
            this.tabPage_About.Controls.Add(this.label_About);
            this.tabPage_About.Location = new System.Drawing.Point(4, 22);
            this.tabPage_About.Name = "tabPage_About";
            this.tabPage_About.Size = new System.Drawing.Size(432, 362);
            this.tabPage_About.TabIndex = 0;
            this.tabPage_About.Text = "About";
            // 
            // linkLabel_Address
            // 
            this.linkLabel_Address.Location = new System.Drawing.Point(125, 0);
            this.linkLabel_Address.Name = "linkLabel_Address";
            this.linkLabel_Address.Size = new System.Drawing.Size(304, 25);
            this.linkLabel_Address.TabIndex = 6;
            this.linkLabel_Address.TabStop = true;
            this.linkLabel_Address.Text = "Send Mail - Submit Feedback";
            this.linkLabel_Address.LinkClicked +=
                new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel_Address_LinkClicked);
            // 
            // pictureBox_automationControls
            // 
            this.pictureBox_automationControls.BackColor = System.Drawing.Color.White;
            this.pictureBox_automationControls.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBox_automationControls.Image =
                ((System.Drawing.Image) (resources.GetObject("pictureBox_automationControls.Image")));
            this.pictureBox_automationControls.Location = new System.Drawing.Point(152, 9);
            this.pictureBox_automationControls.Name = "pictureBox_automationControls";
            this.pictureBox_automationControls.Size = new System.Drawing.Size(344, 69);
            this.pictureBox_automationControls.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox_automationControls.TabIndex = 7;
            this.pictureBox_automationControls.TabStop = false;
            // 
            // Form_About
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
            this.ClientSize = new System.Drawing.Size(506, 464);
            this.Controls.Add(this.pictureBox_automationControls);
            this.Controls.Add(this.tabControl_About);
            this.Controls.Add(this.pictureBox_Icon);
            this.Controls.Add(this.button_Close);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form_About";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About";
            this.Load += new System.EventHandler(this.Form_About_Load);
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox_Icon)).EndInit();
            this.tabControl_About.ResumeLayout(false);
            this.tabPage_About.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox_automationControls)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        #region Form Events...

        /// <summary>
        /// Load the About Form
        /// </summary>
        /// <param name="sender">Object</param>
        /// <param name="e">EventArgs</param>
        private void Form_About_Load(object sender, EventArgs e)
        {
            // Set the abouts forms icon from the owner form
            try
            {
                Icon = Owner.Icon;
                pictureBox_Icon.Image = Icon.ToBitmap();
                Text = "About: " + Owner.Text;
            } catch
            { /* Do nothing on error */
            }

            // Set the version information from the AssemblyInfo file
            // using the AssemblyVersion class.
            var version = new AssemblyVersion();
            label_About.Text = "\r\n" + version;
        }

        #endregion

        #region Buttons...

        /// <summary>
        /// Link to mailto: genbtc
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void linkLabel_Address_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("mailto:genbtc@gmx.com");
        }

        /// <summary>
        /// Close this form
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void button_Close_Click(object sender, EventArgs e)
        {
            Close();
        }

        #endregion

        //email address is in here
    }
}