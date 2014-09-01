/* *************************************************
 * Programmer: Rajesh Lal(connectrajesh@hotmail.com)
 * Date: 06/25/06
 * Company Info: www.irajesh.com
 * *************************************************
 * Modified by genBTC 08/18/2014 for use in his own project.
 */

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Runtime.InteropServices;   //Used for .dll import
using System.IO;

namespace WindowsExplorer
{
    [ToolboxBitmapAttribute(typeof(ExplorerTree), "tree.gif"), DefaultEvent("PathChanged")]
    public partial class ExplorerTree : UserControl
    {
        private System.ComponentModel.IContainer components;
        private Button btnAdd;
        private Button btnGo;
        private Button btnRefresh;
        private Button btnHome;
        private Button btnBack;
        private Button btnNext;
        private Button btnUp;
        private Button btnInfo;
        private ToolTip toolTip1;
        private ListView listView1;
        private ColumnHeader column1_Path;
        private ColumnHeader column2_Status;
        private ImageList imageList1;
        private ContextMenu cMShortcut;
        private MenuItem mnuShortcut;
        private TextBox txtPath;
        private GroupBox grptoolbar;
        private TreeView treeview1;

        public delegate void PathChangedEventHandler(object sender, EventArgs e);
        private PathChangedEventHandler PathChangedEvent;
        /// <summary>
        /// Event that fires when the textbox path changes(including but not limited to after a node selection), but only if the directory is valid.
        /// </summary>
        [Description("Only fires if the directory is valid.")]
        public event PathChangedEventHandler PathChanged
        {
            add
            {
                PathChangedEvent = (PathChangedEventHandler)System.Delegate.Combine(PathChangedEvent, value);
            }
            remove
            {
                PathChangedEvent = (PathChangedEventHandler)System.Delegate.Remove(PathChangedEvent, value);
            }
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExplorerTree));
            this.btnRefresh = new System.Windows.Forms.Button();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.btnGo = new System.Windows.Forms.Button();
            this.treeview1 = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.btnHome = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnUp = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnInfo = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.column1_Path = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.column2_Status = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cMShortcut = new System.Windows.Forms.ContextMenu();
            this.mnuShortcut = new System.Windows.Forms.MenuItem();
            this.grptoolbar = new System.Windows.Forms.GroupBox();
            this.grptoolbar.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefresh.BackColor = System.Drawing.Color.White;
            this.btnRefresh.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRefresh.Image = ((System.Drawing.Image)(resources.GetObject("btnRefresh.Image")));
            this.btnRefresh.Location = new System.Drawing.Point(220, 2);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(18, 18);
            this.btnRefresh.TabIndex = 62;
            this.toolTip1.SetToolTip(this.btnRefresh, "Refresh (Initialize) Explorer Tree");
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // txtPath
            // 
            this.txtPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPath.Location = new System.Drawing.Point(18, 19);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(184, 21);
            this.txtPath.TabIndex = 61;
            this.toolTip1.SetToolTip(this.txtPath, "Current directory");
            this.txtPath.TextChanged += new System.EventHandler(this.txtPath_TextChanged);
            this.txtPath.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtPath_KeyUp);
            // 
            // btnGo
            // 
            this.btnGo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGo.ForeColor = System.Drawing.Color.White;
            this.btnGo.Image = ((System.Drawing.Image)(resources.GetObject("btnGo.Image")));
            this.btnGo.Location = new System.Drawing.Point(197, 17);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(24, 24);
            this.btnGo.TabIndex = 60;
            this.toolTip1.SetToolTip(this.btnGo, "Go to the directory");
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // treeview1
            // 
            this.treeview1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeview1.BackColor = System.Drawing.Color.White;
            this.treeview1.HideSelection = false;
            this.treeview1.ImageIndex = 0;
            this.treeview1.ImageList = this.imageList1;
            this.treeview1.Indent = 17;
            this.treeview1.Location = new System.Drawing.Point(0, 41);
            this.treeview1.Name = "treeview1";
            this.treeview1.SelectedImageIndex = 2;
            this.treeview1.ShowRootLines = false;
            this.treeview1.Size = new System.Drawing.Size(240, 295);
            this.treeview1.TabIndex = 10;
            this.treeview1.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.tvwMain_AfterExpand);
            this.treeview1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvwMain_AfterSelect);
            this.treeview1.DoubleClick += new System.EventHandler(this.tvwMain_DoubleClick);
            this.treeview1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tvwMain_MouseUp);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "");
            this.imageList1.Images.SetKeyName(1, "");
            this.imageList1.Images.SetKeyName(2, "");
            this.imageList1.Images.SetKeyName(3, "");
            this.imageList1.Images.SetKeyName(4, "");
            this.imageList1.Images.SetKeyName(5, "");
            this.imageList1.Images.SetKeyName(6, "");
            this.imageList1.Images.SetKeyName(7, "");
            this.imageList1.Images.SetKeyName(8, "");
            this.imageList1.Images.SetKeyName(9, "");
            this.imageList1.Images.SetKeyName(10, "");
            this.imageList1.Images.SetKeyName(11, "");
            this.imageList1.Images.SetKeyName(12, "");
            this.imageList1.Images.SetKeyName(13, "");
            this.imageList1.Images.SetKeyName(14, "");
            this.imageList1.Images.SetKeyName(15, "");
            this.imageList1.Images.SetKeyName(16, "");
            this.imageList1.Images.SetKeyName(17, "");
            this.imageList1.Images.SetKeyName(18, "");
            this.imageList1.Images.SetKeyName(19, "");
            this.imageList1.Images.SetKeyName(20, "");
            this.imageList1.Images.SetKeyName(21, "");
            this.imageList1.Images.SetKeyName(22, "");
            this.imageList1.Images.SetKeyName(23, "");
            this.imageList1.Images.SetKeyName(24, "");
            this.imageList1.Images.SetKeyName(25, "");
            this.imageList1.Images.SetKeyName(26, "");
            this.imageList1.Images.SetKeyName(27, "");
            this.imageList1.Images.SetKeyName(28, "");
            // 
            // btnHome
            // 
            this.btnHome.BackColor = System.Drawing.Color.White;
            this.btnHome.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnHome.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHome.ForeColor = System.Drawing.Color.Transparent;
            this.btnHome.Image = ((System.Drawing.Image)(resources.GetObject("btnHome.Image")));
            this.btnHome.Location = new System.Drawing.Point(70, 8);
            this.btnHome.Name = "btnHome";
            this.btnHome.Size = new System.Drawing.Size(17, 17);
            this.btnHome.TabIndex = 63;
            this.toolTip1.SetToolTip(this.btnHome, "Application Directory");
            this.btnHome.UseVisualStyleBackColor = false;
            this.btnHome.Click += new System.EventHandler(this.btnHome_Click);
            // 
            // btnBack
            // 
            this.btnBack.BackColor = System.Drawing.Color.White;
            this.btnBack.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBack.ForeColor = System.Drawing.Color.Transparent;
            this.btnBack.Image = ((System.Drawing.Image)(resources.GetObject("btnBack.Image")));
            this.btnBack.Location = new System.Drawing.Point(30, 8);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(17, 17);
            this.btnBack.TabIndex = 64;
            this.toolTip1.SetToolTip(this.btnBack, "Backward");
            this.btnBack.UseVisualStyleBackColor = false;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // btnNext
            // 
            this.btnNext.BackColor = System.Drawing.Color.White;
            this.btnNext.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNext.ForeColor = System.Drawing.Color.Transparent;
            this.btnNext.Image = ((System.Drawing.Image)(resources.GetObject("btnNext.Image")));
            this.btnNext.Location = new System.Drawing.Point(52, 8);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(17, 17);
            this.btnNext.TabIndex = 65;
            this.toolTip1.SetToolTip(this.btnNext, "Forward");
            this.btnNext.UseVisualStyleBackColor = false;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnUp
            // 
            this.btnUp.BackColor = System.Drawing.Color.White;
            this.btnUp.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUp.ForeColor = System.Drawing.Color.Transparent;
            this.btnUp.Image = ((System.Drawing.Image)(resources.GetObject("btnUp.Image")));
            this.btnUp.Location = new System.Drawing.Point(0, 1);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(18, 18);
            this.btnUp.TabIndex = 67;
            this.toolTip1.SetToolTip(this.btnUp, "Parent Directory");
            this.btnUp.UseVisualStyleBackColor = false;
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.White;
            this.btnAdd.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.ForeColor = System.Drawing.Color.Transparent;
            this.btnAdd.Image = ((System.Drawing.Image)(resources.GetObject("btnAdd.Image")));
            this.btnAdd.Location = new System.Drawing.Point(8, 8);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(17, 17);
            this.btnAdd.TabIndex = 70;
            this.toolTip1.SetToolTip(this.btnAdd, "Add shortcut to frequently used folders");
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnInfo
            // 
            this.btnInfo.BackColor = System.Drawing.Color.White;
            this.btnInfo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnInfo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInfo.ForeColor = System.Drawing.Color.Transparent;
            this.btnInfo.Image = ((System.Drawing.Image)(resources.GetObject("btnInfo.Image")));
            this.btnInfo.Location = new System.Drawing.Point(91, 8);
            this.btnInfo.Name = "btnInfo";
            this.btnInfo.Size = new System.Drawing.Size(17, 17);
            this.btnInfo.TabIndex = 71;
            this.toolTip1.SetToolTip(this.btnInfo, "Application Directory");
            this.btnInfo.UseVisualStyleBackColor = false;
            this.btnInfo.Visible = false;
            this.btnInfo.Click += new System.EventHandler(this.btnInfo_Click);
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.column1_Path,
            this.column2_Status});
            this.listView1.Location = new System.Drawing.Point(8, 208);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(224, 48);
            this.listView1.TabIndex = 68;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.Visible = false;
            // 
            // cMShortcut
            // 
            this.cMShortcut.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuShortcut});
            // 
            // mnuShortcut
            // 
            this.mnuShortcut.Index = 0;
            this.mnuShortcut.Text = "Remove Shortcut";
            this.mnuShortcut.Click += new System.EventHandler(this.mnuShortcut_Click);
            // 
            // grptoolbar
            // 
            this.grptoolbar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grptoolbar.Controls.Add(this.btnInfo);
            this.grptoolbar.Controls.Add(this.btnHome);
            this.grptoolbar.Controls.Add(this.btnBack);
            this.grptoolbar.Controls.Add(this.btnNext);
            this.grptoolbar.Controls.Add(this.btnAdd);
            this.grptoolbar.Location = new System.Drawing.Point(30, -8);
            this.grptoolbar.Name = "grptoolbar";
            this.grptoolbar.Size = new System.Drawing.Size(114, 32);
            this.grptoolbar.TabIndex = 71;
            this.grptoolbar.TabStop = false;
            // 
            // ExplorerTree
            // 
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.treeview1);
            this.Controls.Add(this.txtPath);
            this.Controls.Add(this.btnUp);
            this.Controls.Add(this.btnGo);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.grptoolbar);
            this.Name = "ExplorerTree";
            this.Size = new System.Drawing.Size(240, 336);
            this.Load += new System.EventHandler(this.ExplorerTree_Load);
            this.grptoolbar.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

    }
}
