/* *************************************************
 * Programmer: Rajesh Lal(connectrajesh@hotmail.com)
 * Date: 06/25/06
 * Company Info: www.irajesh.com
 * *************************************************
 * Modified by genBTC 08/26/2014 for use in his own project.
 */

using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using genBTC.FileTime.Classes.Native;
using genBTC.FileTime.mViewModels;

namespace WindowsExplorer
{
    /// <summary>
    /// Summary description for ExplorerTree.
    /// </summary>
    public partial class ExplorerTree
    {
        private static readonly string Seperator = Path.DirectorySeparatorChar.ToString();
        private static readonly string Desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        private static readonly string MyDocuments = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        private static readonly string Favorites = Environment.GetFolderPath(Environment.SpecialFolder.Favorites);
        private TreeNode nodeEntireNetwork;
        private TreeNode nodeMyComputer;
        private TreeNode nodeMyDocuments;
        private TreeNode rootnodeDesktop;

        /// <summary> Explorer-Like TreeView control. </summary>
        public ExplorerTree()
        {
            //ShowMyDocuments = true;
            //ShowMyFavorites = true;
            //ShowMyNetwork = true;
            //ShowToolbar = true;
            //ShowAddressbar = true;
            //GoFlag = false;
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();
        }

        /// <summary> Startup Path, what is shown in text box. </summary>
        [Category("Appearance"), Description("The path the treeview is navigated to when it is first loaded.")]
        public string CurrentPath { get; set; }

        private bool GoFlag { get; set; }

        public bool ShowAddressbar { get; set; }

        public bool ShowMyDocuments { get; set; }

        public bool ShowMyFavorites { get; set; }

        public bool ShowMyNetwork { get; set; }

        public bool ShowToolbar { get; set; }

        private void ExplorerTree_Load(object sender, EventArgs e)
        {
            InitialPopulate();
            RefreshControlUi();
        }

        /// <summary>
        /// Refresh the Control itself (toolbar, etc) (public)
        /// </summary>
        public void RefreshControlUi()
        {
            if ((!ShowAddressbar) && (!ShowToolbar))
            {
                treeview1.Top = 0;
                txtPath.Visible = false;
                btnGo.Visible = false;
                grptoolbar.Visible = false;
                treeview1.Height = Height;
            }
            else
            {
                if (ShowToolbar && (!ShowAddressbar))
                {
                    treeview1.Top = 20;
                    txtPath.Visible = false;
                    btnGo.Visible = false;
                    treeview1.Height = Height - 20;
                    grptoolbar.Visible = true;
                }
                else if (ShowAddressbar && (!ShowToolbar))
                {
                    treeview1.Top = 20;
                    txtPath.Top = 1;
                    btnGo.Top = -2;
                    txtPath.Visible = true;
                    btnGo.Visible = true;
                    treeview1.Height = Height - 20;
                    grptoolbar.Visible = false;
                }
                else
                {
                    treeview1.Top = 40;
                    txtPath.Visible = true;
                    btnGo.Visible = true;
                    txtPath.Top = 19;
                    btnGo.Top = 16;
                    grptoolbar.Visible = true;
                    treeview1.Height = Height - 40;
                }
            }
        }

        /// <summary>
        /// Initially Populate the Treeview (public)
        /// </summary>
        public void InitialPopulate()
        {
            treeview1.Nodes.Clear();

            treeview1.Nodes.Add("Desktop", "Desktop", 10, 10);
            rootnodeDesktop = treeview1.Nodes["Desktop"];
            rootnodeDesktop.Tag = Desktop; //actual users desktop path

            nodeMyComputer = new TreeNode
            {
                Tag = "My Computer",
                Text = "My Computer",
                ImageIndex = 12,
                SelectedImageIndex = 12
            };
            rootnodeDesktop.Nodes.Add(nodeMyComputer);
            //a placeholder node to sit under the My Computer word so theres a + sign but hasnt yet been enumerated
            var nodeMyCompSubNodePlaceholder = new TreeNode
            {
                Tag = "My Computer Placeholder Node",
                Text = "My Computer Placeholder Node",
                ImageIndex = 12,
                SelectedImageIndex = 12
            };
            nodeMyComputer.Nodes.Add(nodeMyCompSubNodePlaceholder);

            if (ShowMyDocuments)
            {
                //Add My Documents folder outside
                nodeMyDocuments = new TreeNode
                {
                    Tag = MyDocuments,
                    Text = "My Documents",
                    ImageIndex = 9,
                    SelectedImageIndex = 9
                };
                rootnodeDesktop.Nodes.Add(nodeMyDocuments);
                GetDirectories(nodeMyDocuments);
            }

            if (ShowMyNetwork)
            {
                nodeEntireNetwork = new TreeNode
                {
                    Tag = "Entire Network",
                    Text = "Entire Network",
                    ImageIndex = 14,
                    SelectedImageIndex = 14
                };
                rootnodeDesktop.Nodes.Add(nodeEntireNetwork);
                //a placeholder node to sit under the Entire Network word so theres a + sign but hasnt yet been enumerated
                var nodeNetworkNode = new TreeNode
                {
                    Tag = "Network Placeholder Node",
                    Text = "Network Placeholder Node",
                    ImageIndex = 15,
                    SelectedImageIndex = 15
                };
                nodeEntireNetwork.Nodes.Add(nodeNetworkNode);

                nodeEntireNetwork.EnsureVisible();
            }

            if (ShowMyFavorites)
            {
                var nodemyfavs = new TreeNode
                {
                    Tag = Favorites,
                    Text = "My Favorites",
                    ImageIndex = 26,
                    SelectedImageIndex = 26
                };
                rootnodeDesktop.Nodes.Add(nodemyfavs);
                GetDirectories(nodemyfavs);
            }

            ExploreTreeNode(rootnodeDesktop, true);

            //setCurrentPath(Desktop);    //Desktop (user's folder) was the default.
            rootnodeDesktop.Expand();
            nodeMyComputer.Expand();
            //genBTC.FileTime.NativeScroll.ScrollH(nodeMyComputer, 20);
            nodeMyComputer.EnsureVisible();
        }

        private static void ExploreTreeNode(TreeNode n, bool isInitial = false)
        {
            Cursor.Current = Cursors.WaitCursor;
            //get dirs
            GetDirectories(n, isInitial);
            //get dirs one more level deep in current dir so user can see
            // that there are more dirs underneath current dir
            foreach (TreeNode node in n.Nodes)
                GetDirectories(node, isInitial);
            Cursor.Current = Cursors.Default;
        }

        private static void GetDirectories(TreeNode node, bool isInitial = false)
        {
            string[] dirList;
            try
            {
                dirList = Directory.GetDirectories(node.Tag.ToString());
            }
            catch (Exception)
            {
                return;
            }
            var comparer = new ExplorerComparerstringHelper();
            Array.Sort(dirList, comparer);

            //check if dir already exists in case click same dir twice
            if (isInitial)
            {
            }
            else if (dirList.Length == node.Nodes.Count)
                return;
            else if (node.Text == "Desktop")
                return;
            //add each dir in selected dir
            for (int i = 0; i < dirList.Length; i++)
            {
                var subnode = new TreeNode
                {
                    Tag = dirList[i],
                    Text = dirList[i].Substring(dirList[i].LastIndexOf(Seperator) + 1),
                    ImageIndex = 1
                };
                node.Nodes.Add(subnode);
            }
        }

        /// <summary>
        /// Select and scroll to the topmost node (Desktop), after it has been initialized.
        /// </summary>
        public void SelectRootNode()
        {
            treeview1.SelectedNode = rootnodeDesktop;
            rootnodeDesktop.EnsureVisible();
        }

        /// <summary>
        /// Set the path in the textbox, and fires the path changed event... Call BrowseTo() afterwards to go to it.
        /// </summary>
        public void SetCurrentPath(string strPath)
        {
            if (Directory.Exists(strPath))
                txtPath.Text = strPath;
            CurrentPath = strPath;
        }

        /// <summary>
        /// Fires after every character is typed, or an item is selected.
        /// </summary>
        private void txtPath_TextChanged(object sender, EventArgs e)
        {
            if (Directory.Exists(txtPath.Text))
            {
                CurrentPath = txtPath.Text;
                PathChangedEvent(this, EventArgs.Empty);
            }
        }

        private void PopulateMyComputer()
        {
            Cursor.Current = Cursors.WaitCursor;
            DriveInfo[] myDrives = DriveInfo.GetDrives();

            //if the only thing in here is the placeholder.
            if (nodeMyComputer.Nodes[0].Text == "My Computer Placeholder Node")
            {
                nodeMyComputer.FirstNode.Remove();

                foreach (DriveInfo drive in myDrives)
                {
                    var nodeDrive = new TreeNode { Tag = drive.Name };
                    try
                    {
                        nodeDrive.Text = drive.VolumeLabel + " (" + drive.Name.Substring(0, 2) + ")";
                    }
                    catch (IOException) //because !(drive.IsReady) - used for cdrom
                    {
                        nodeDrive.Text = drive.Name;
                    }
                    int drivetype;
                    switch (Kernel32.GetDriveType(drive.Name))
                    {
                        case 2: //Floppy/Removable
                            drivetype = 17;
                            break;

                        case 3: //Fixed Disk (Hard Drive)
                            drivetype = 0;
                            break;

                        case 4: //Network Drive
                            drivetype = 8;
                            break;

                        case 5: //CDRom (don't add these ones)
                            //drivetype = 7;
                            continue;
                        default: //Default to Fixed Disk
                            drivetype = 0;
                            break;
                    }
                    nodeDrive.ImageIndex = drivetype;
                    nodeDrive.SelectedImageIndex = drivetype;

                    nodeMyComputer.Nodes.Add(nodeDrive);

                    try
                    {
                        //add dirs under drive
                        if (Directory.Exists(drive.Name))
                        {
                            foreach (string dir in Directory.GetDirectories(drive.Name))
                            {
                                var node = new TreeNode
                                {
                                    Tag = dir,
                                    Text = dir.Substring(dir.LastIndexOf(Seperator) + 1),
                                    ImageIndex = 1
                                };
                                nodeDrive.Nodes.Add(node);
                            }
                        }
                    }
                    catch (Exception ex) //shouldn't really happen but just in case.
                    {
                        MessageBox.Show("Error while filling the My Computer node:" + ex.Message);
                    }
                }
            }
            //nodeMyComputer.Expand();
            Cursor.Current = Cursors.Default;
        }

        private void tvwMain_AfterExpand(object sender, TreeViewEventArgs e)
        {
            TreeNode cur = e.Node;

            if (cur.Tag.ToString().IndexOf(":", 1) > 0) //if its got a : it means its a C:\ like-type path etc
                ExploreTreeNode(cur);
            else if ((cur.Text == "My Computer") || (cur.Text == "Entire Network") ||
                     ((cur.Parent != null) && (cur.Parent.Text == "Entire Network")))
            {
                if (cur.Text == "My Computer")
                {
                    PopulateMyComputer(); //add each drive and files and dirs (if it hasnt already).
                    return;
                }
                Cursor.Current = Cursors.WaitCursor;
                if (cur.Text == "Entire Network")
                {
                    if (cur.FirstNode.Text == "Network Placeholder Node")
                    {
                        cur.FirstNode.Remove();

                        var servers = new ServerEnum(ResourceScope.RESOURCE_GLOBALNET,
                            ResourceType.RESOURCETYPE_DISK, ResourceUsage.RESOURCEUSAGE_ALL,
                            ResourceDisplayType.RESOURCEDISPLAYTYPE_NETWORK, "");

                        foreach (string s1 in servers)
                        {
                            string s2 = s1.Substring(0, s1.IndexOf("|", 1));

                            if ((s2 == "Microsoft Terminal Services") || (s2 == "Microsoft Windows Network"))
                                //ignore these names
                                continue;
                            //Domain or Workgroup
                            var nodeDorW = new TreeNode
                            {
                                Tag = cur.Text + Seperator + s2,
                                Text = s2,
                                ImageIndex = 16,
                                SelectedImageIndex = 16
                            };
                            cur.Nodes.Add(nodeDorW);

                            //Placeholder supposed to hold computers.
                            var nodeNcmp = new TreeNode
                            {
                                Tag = "NCMP",
                                Text = "NCMP",
                                ImageIndex = 12,
                                SelectedImageIndex = 12
                            };
                            nodeDorW.Nodes.Add(nodeNcmp);
                        }
                    }
                }
                if ((cur.Parent != null) && (cur.Parent.Text == "Entire Network"))
                {
                    if (cur.FirstNode.Text == "NCMP")
                    {
                        cur.FirstNode.Remove(); //remove the NCMP placeholder

                        string pS = cur.Text;

                        var allservers = new ServerEnum(ResourceScope.RESOURCE_GLOBALNET,
                            ResourceType.RESOURCETYPE_DISK, ResourceUsage.RESOURCEUSAGE_ALL,
                            ResourceDisplayType.RESOURCEDISPLAYTYPE_SERVER, pS);

                        IEnumerator e1 = allservers.GetEnumerator(); //e1 is enumerator.
                        //new array of computer nodes, with their subnode shares. (yes there will be blank indexes)
                        var computerlist = new TreeNode[allservers.Count];
                        int i = 0; //accumulator
                        int lasti = 0; //last location of a computer in the computerlist
                        while (e1.MoveNext())
                        {
                            if (e1.Current != null)
                            {
                                var enumtext = e1.Current.ToString();
                                // if NOT a _share, then its a Network Computer, and we add it
                                if (!enumtext.EndsWith("_share"))
                                {
                                    var aComp = new TreeNode
                                    {
                                        Tag = enumtext,
                                        Text = enumtext.Substring(2),
                                        ImageIndex = 12,
                                        SelectedImageIndex = 12
                                    };
                                    computerlist[i] = aComp;
                                    lasti = i;
                                }
                                else //Network Computer Shares subnodes.
                                {
                                    var pos = enumtext.LastIndexOf("\\") + 1;
                                    var aSubShare = new TreeNode
                                    {
                                        Tag = enumtext.Substring(0, enumtext.Length - 6),
                                        Text = enumtext.Substring(pos, enumtext.Length - pos - 6),
                                        ImageIndex = 28,
                                        SelectedImageIndex = 28
                                    };
                                    computerlist[lasti].Nodes.Add(aSubShare);
                                }
                            }
                            i++;
                        }
                        foreach (TreeNode eachcomp in computerlist)
                        {
                            if (eachcomp != null)
                                cur.Nodes.Add(eachcomp);
                        }
                    }
                }
            }
            Cursor.Current = Cursors.Default;
        }

        /// <summary> Updates path textbox after selectionchange </summary>
        private void tvwMain_AfterSelect(object sender, TreeViewEventArgs e)
        {
            txtPath.Text = e.Node.Tag.ToString();
        }

        /// <summary>
        /// Event for collapse/expand
        /// </summary>
        private void tvwMain_DoubleClick(object sender, EventArgs e)
        {
            TreeNode n = treeview1.SelectedNode;

            if (!treeview1.SelectedNode.IsExpanded)
                treeview1.SelectedNode.Collapse();
            else
            {
                if (n.Text != "Desktop")
                    ExploreTreeNode(n);
            }
        }

        /// <summary>
        /// Refresh Button - Re-Initialize the TreeView from the beginning (Desktop).
        /// </summary>
        public void RefreshFolders()
        {
            listView1.Items.Clear();
            treeview1.Nodes.Clear();
            SetCurrentPath(Desktop); //static var
            InitialPopulate();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            RefreshControlUi();
            RefreshFolders();
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// The event handler for when the actual button is clicked.
        /// </summary>
        private void btnGo_Click(object sender, EventArgs e)
        {
            BrowseTo();
        }

        /// <summary>
        /// Navigate the treeview to the path in the textbox. Must be called manually when CurrentPath changes.
        /// </summary>
        public void BrowseTo()
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                PopulateMyComputer();
                string textboxPath = txtPath.Text.ToLower();

                int sentinel = 1;
                TreeNode loopRootNode = nodeMyComputer;
                bool network = false;
                if (textboxPath.Contains(Desktop.ToLower()))
                {
                    loopRootNode = rootnodeDesktop;
                    if (textboxPath.Length == Desktop.Length)
                    {
                        loopRootNode.Expand();
                        loopRootNode.EnsureVisible();
                        return;
                    }
                }
                else if (textboxPath.Contains(MyDocuments.ToLower()))
                {
                    loopRootNode = nodeMyDocuments;
                    if (textboxPath.Length == MyDocuments.Length)
                    {
                        loopRootNode.Expand();
                        loopRootNode.EnsureVisible();
                        return;
                    }
                }
                else if (textboxPath == "my computer")
                {
                    loopRootNode.Expand();
                    loopRootNode.EnsureVisible();
                    return;
                }
                else if (textboxPath.StartsWith(@"\\"))
                {
                    loopRootNode = nodeEntireNetwork;
                    network = true;
                }
                treeview1.SelectedNode = loopRootNode;
                if (!textboxPath.EndsWith(Seperator))
                    textboxPath += Seperator;

            StartAgain:

                do
                {
                    foreach (TreeNode currNode in loopRootNode.Nodes)
                    {
                        string currNodePath = currNode.Tag.ToString().ToLower();
                        if (!currNodePath.EndsWith(Seperator))
                            currNodePath += Seperator;

                        if (textboxPath.StartsWith(currNodePath))
                        {
                            currNode.TreeView.Focus();
                            currNode.TreeView.SelectedNode = currNode;
                            //currNode.EnsureVisible();
                            NativeScroll.ScrollH(currNode, 37);
                            if (currNode.Nodes.Count > 0)
                            {
                                currNode.Expand();
                                loopRootNode = currNode;
                            }
                            else
                            {
                                if (textboxPath == currNodePath)
                                {
                                    sentinel = -1;
                                    break;
                                }
                                continue;
                            }
                            if (currNodePath.StartsWith(textboxPath))
                            {
                                sentinel = -1;
                                break;
                            }
                            goto StartAgain;
                        }
                        if (network)
                        {
                            loopRootNode = currNode;
                            goto StartAgain;
                        }
                    }
                    if (sentinel == -1)
                        break;
                    try
                    {
                        loopRootNode = loopRootNode.NextNode;
                    }
                    catch (Exception)
                    { } //no more nodes (not an error, so suppress)
                } while (sentinel >= 0);
            }
            catch (Exception e1)
            {
                MessageBox.Show("Error: " + e1.Message);
            }
            Cursor.Current = Cursors.Default;
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            SetCurrentPath(Desktop);
            PopulateMyComputer();
            btnGo_Click(sender, e);
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            GoFlag = true;
            string cpath = txtPath.Text;
            UpdateListGoFwd();

            if (cpath != txtPath.Text)
                btnGo_Click(sender, e);
            GoFlag = false;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            GoFlag = true;
            string cpath = txtPath.Text;
            UpdateListGoBack();

            if (cpath != txtPath.Text)
                btnGo_Click(sender, e);
            GoFlag = false;
        }

        private void tvwMain_MouseUp(object sender, MouseEventArgs e)
        {
            UpdateList(txtPath.Text);
            if ((treeview1.SelectedNode != null) && (treeview1.SelectedNode.ImageIndex == 18) &&
                (e.Button == MouseButtons.Right))
                cMShortcut.Show(treeview1, new Point(e.X, e.Y));
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            try
            {
                var myinfo = new DirectoryInfo(txtPath.Text);

                if (myinfo.Parent != null && myinfo.Parent.Exists)
                    txtPath.Text = myinfo.Parent.FullName;
                UpdateList(txtPath.Text);
                btnGo_Click(sender, e);
            }
            catch (Exception)
            {
                //MessageBox.Show ("Parent directory does not exists","Directory Not Found",MessageBoxButtons.OK,MessageBoxIcon.Information );
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var dialog = new FolderBrowserDialog
            {
                Description = "Add Folder in Explorer Tree",
                ShowNewFolderButton = true,
                SelectedPath = txtPath.Text
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string mypath = dialog.SelectedPath;
                string myname = mypath.Substring(mypath.LastIndexOf(Seperator) + 1);

                AddFolderNode(myname, mypath);
            }
        }

        private void AddFolderNode(string name, string path)
        {
            try
            {
                var nodemyC = new TreeNode { Tag = path, Text = name, ImageIndex = 18, SelectedImageIndex = 18 };
                rootnodeDesktop.Nodes.Add(nodemyC);

                try
                {
                    //add dirs under drive
                    if (Directory.Exists(path))
                    {
                        foreach (string dir in Directory.GetDirectories(path))
                        {
                            var node = new TreeNode
                            {
                                Tag = dir,
                                Text = dir.Substring(dir.LastIndexOf(Seperator) + 1),
                                ImageIndex = 1
                            };
                            nodemyC.Nodes.Add(node);
                        }
                    }
                }
                catch (Exception ex) //error
                {
                    MessageBox.Show("Error while Filling the Explorer:" + ex.Message);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void mnuShortcut_Click(object sender, EventArgs e)
        {
            if (treeview1.SelectedNode.ImageIndex == 18)
                treeview1.SelectedNode.Remove();
        }

        private void txtPath_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                btnGo_Click(sender, e);
                txtPath.Focus();
            }
        }

        private void btnInfo_Click(object sender, EventArgs e)
        {
        }

        private void UpdateListAddCurrent()
        {
            for (int i = 0; i < listView1.Items.Count - 1; i++)
            {
                if (listView1.Items[i].SubItems[1].Text == "Selected")
                {
                    int j;
                    for (j = listView1.Items.Count - 1; j > i + 1; j--)
                        listView1.Items[j].Remove();
                    break;
                }
            }
        }

        private void UpdateListGoBack()
        {
            if ((listView1.Items.Count > 0) && (listView1.Items[0].SubItems[1].Text == "Selected"))
                return;
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                if (listView1.Items[i].SubItems[1].Text == "Selected")
                {
                    if (i != 0)
                    {
                        listView1.Items[i - 1].SubItems[1].Text = "Selected";
                        txtPath.Text = listView1.Items[i - 1].Text;
                    }
                }
                if (i != 0)
                    listView1.Items[i].SubItems[1].Text = " -/- ";
            }
        }

        private void UpdateListGoFwd()
        {
            if ((listView1.Items.Count > 0) &&
                (listView1.Items[listView1.Items.Count - 1].SubItems[1].Text == "Selected"))
                return;
            for (int i = listView1.Items.Count - 1; i >= 0; i--)
            {
                if (listView1.Items[i].SubItems[1].Text == "Selected")
                {
                    if (i != listView1.Items.Count)
                    {
                        listView1.Items[i + 1].SubItems[1].Text = "Selected";
                        txtPath.Text = listView1.Items[i + 1].Text;
                    }
                }

                if (i != listView1.Items.Count - 1) listView1.Items[i].SubItems[1].Text = " -/- ";
            }
        }

        private void UpdateList(string f)
        {
            UpdateListAddCurrent();
            try
            {
                if (listView1.Items.Count > 0)
                {
                    if (listView1.Items[listView1.Items.Count - 1].Text == f)
                        return;
                }
                for (int i = 0; i < listView1.Items.Count; i++)
                    listView1.Items[i].SubItems[1].Text = " -/- ";
                var listviewitem = new ListViewItem(f); // Used for creating listview items.
                listviewitem.SubItems.Add("Selected");
                listviewitem.Tag = f;
                listView1.Items.Add(listviewitem);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }

    /// <summary> a kernel Shell Function </summary>
    public class Kernel32
    {
        /// <summary>
        /// Return an int specifying the type of drive.
        /// </summary>
        [DllImport("kernel32")]
        public static extern uint GetDriveType(string lpRootPathName);
    }

    public enum ResourceScope
    {
        RESOURCE_CONNECTED = 1,
        RESOURCE_GLOBALNET,
        RESOURCE_REMEMBERED,
        RESOURCE_RECENT,
        RESOURCE_CONTEXT
    }

    public enum ResourceType
    {
        RESOURCETYPE_ANY,
        RESOURCETYPE_DISK,
        RESOURCETYPE_PRINT,
        RESOURCETYPE_RESERVED
    }

    [Flags]
    public enum ResourceUsage
    {
        RESOURCEUSAGE_CONNECTABLE = 0x00000001,
        RESOURCEUSAGE_CONTAINER = 0x00000002,
        RESOURCEUSAGE_NOLOCALDEVICE = 0x00000004,
        RESOURCEUSAGE_SIBLING = 0x00000008,
        RESOURCEUSAGE_ATTACHED = 0x00000010,
        RESOURCEUSAGE_ALL = (RESOURCEUSAGE_CONNECTABLE | RESOURCEUSAGE_CONTAINER | RESOURCEUSAGE_ATTACHED),
    }

    public enum ResourceDisplayType
    {
        RESOURCEDISPLAYTYPE_GENERIC,
        RESOURCEDISPLAYTYPE_DOMAIN,
        RESOURCEDISPLAYTYPE_SERVER,
        RESOURCEDISPLAYTYPE_SHARE,
        RESOURCEDISPLAYTYPE_FILE,
        RESOURCEDISPLAYTYPE_GROUP,
        RESOURCEDISPLAYTYPE_NETWORK,
        RESOURCEDISPLAYTYPE_ROOT,
        RESOURCEDISPLAYTYPE_SHAREADMIN,
        RESOURCEDISPLAYTYPE_DIRECTORY,
        RESOURCEDISPLAYTYPE_TREE,
        RESOURCEDISPLAYTYPE_NDSCONTAINER
    }

    public class ServerEnum : IEnumerable
    {
        private readonly ArrayList aData = new ArrayList();

        public ServerEnum(ResourceScope scope, ResourceType type, ResourceUsage usage, ResourceDisplayType displayType, string kPath)
        {
            var netRoot = new NETRESOURCE();
            EnumerateServers(netRoot, scope, type, usage, displayType, kPath);
        }

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return aData.GetEnumerator();
        }

        #endregion IEnumerable Members

        public int Count
        {
            get { return aData.Count; }
        }

        [DllImport("Mpr.dll", EntryPoint = "WNetOpenEnumA", CallingConvention = CallingConvention.Winapi)]
        private static extern ErrorCodes WNetOpenEnum(ResourceScope dwScope, ResourceType dwType, ResourceUsage dwUsage,
            NETRESOURCE p, out IntPtr lphEnum);

        [DllImport("Mpr.dll", EntryPoint = "WNetCloseEnum", CallingConvention = CallingConvention.Winapi)]
        private static extern ErrorCodes WNetCloseEnum(IntPtr hEnum);

        [DllImport("Mpr.dll", EntryPoint = "WNetEnumResourceA", CallingConvention = CallingConvention.Winapi)]
        private static extern ErrorCodes WNetEnumResource(IntPtr hEnum, ref uint lpcCount, IntPtr buffer,
            ref uint lpBufferSize);

        private void EnumerateServers(NETRESOURCE pRsrc, ResourceScope scope, ResourceType type, ResourceUsage usage,
            ResourceDisplayType displayType, string kPath)
        {
            uint bufferSize = 16384;
            IntPtr buffer = Marshal.AllocHGlobal((int)bufferSize);
            IntPtr handle;
            uint cEntries = 1;
            bool serverenum = false;

            ErrorCodes result = WNetOpenEnum(scope, type, usage, pRsrc, out handle);

            if (result == ErrorCodes.NO_ERROR)
            {
                do
                {
                    result = WNetEnumResource(handle, ref cEntries, buffer, ref bufferSize);

                    if ((result == ErrorCodes.NO_ERROR))
                    {
                        Marshal.PtrToStructure(buffer, pRsrc);

                        if (kPath == "")
                        {
                            if ((pRsrc.dwDisplayType == displayType) ||
                                (pRsrc.dwDisplayType == ResourceDisplayType.RESOURCEDISPLAYTYPE_DOMAIN))
                                aData.Add(pRsrc.lpRemoteName + "|" + pRsrc.dwDisplayType);

                            if ((pRsrc.dwUsage & ResourceUsage.RESOURCEUSAGE_CONTAINER) ==
                                ResourceUsage.RESOURCEUSAGE_CONTAINER)
                            {
                                if ((pRsrc.dwDisplayType == displayType))
                                    EnumerateServers(pRsrc, scope, type, usage, displayType, kPath);
                            }
                        }
                        else
                        {
                            if (pRsrc.dwDisplayType == displayType)
                            {
                                aData.Add(pRsrc.lpRemoteName);
                                EnumerateServers(pRsrc, scope, type, usage, displayType, kPath);
                                //return;
                                serverenum = true;
                            }
                            if (!serverenum)
                            {
                                if (pRsrc.dwDisplayType == ResourceDisplayType.RESOURCEDISPLAYTYPE_SHARE)
                                    aData.Add(pRsrc.lpRemoteName + "_share");
                            }
                            else
                                serverenum = false;
                            if ((kPath.IndexOf(pRsrc.lpRemoteName) >= 0) ||
                                (pRsrc.lpRemoteName == "Microsoft Windows Network"))
                                EnumerateServers(pRsrc, scope, type, usage, displayType, kPath);
                        }
                    }
                    else if (result != ErrorCodes.ERROR_NO_MORE_ITEMS)
                        break;
                } while (result != ErrorCodes.ERROR_NO_MORE_ITEMS);

                WNetCloseEnum(handle);
            }

            Marshal.FreeHGlobal(buffer);
        }

        private enum ErrorCodes
        {
            NO_ERROR = 0,
            ERROR_NO_MORE_ITEMS = 259
        }

        [StructLayout(LayoutKind.Sequential)]
        private class NETRESOURCE
        {
            public ResourceScope dwScope = 0;
            public ResourceType dwType = 0;
            public ResourceDisplayType dwDisplayType = 0;
            public ResourceUsage dwUsage = 0;
            public string lpLocalName = null;
            public readonly string lpRemoteName = null;
            public string lpComment = null;
            public string lpProvider = null;
        }
    }
}