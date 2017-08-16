using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace genBTC.FileTime.mViewModels
{
    /// <summary>
    /// Custom List View control that supports context menu, explorer-like sorting, and explorer-like styles.
    /// </summary>
    public class CustomListView : ListView
    {
        #region Native Imports
        /// <summary> Send a P/Invoke to a listview </summary>
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);

        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
        #endregion Native Imports

        private static extern int SetWindowTheme(IntPtr hWnd, string textSubAppName, string textSubIdList);

        private const int WM_CHANGEUISTATE = 0x127;

        /// <summary> keeps track of how often the selection is firing </summary>
        public Timer ItemSelectionChangedTimer;

        /// <summary> Constructor. activate the listview with desired options. </summary>
        public CustomListView()
        {
            //Activate double buffering
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            UseCompatibleStateImageBehavior = true;

            View = View.Details;
            FullRowSelect = true;
            //Instanciate the listview with sorting activated:
            //            this.ListViewItemSorter = new ListViewItemExplorerLikeComparer();

            //this removes the ugly dotted line around focused item:
            SendMessage(Handle, WM_CHANGEUISTATE, 0x10001, 0);

            ItemSelectionChangedTimer = new Timer { Interval = 800 };
            ItemSelectionChangedTimer.Tick += _ItemSelectionChangedTimer_Tick;
            //Enable the OnNotifyMessage event so we get a chance to filter out
            // Windows messages before they get to the form's WndProc:
            //this.SetStyle(ControlStyles.EnableNotifyMessage, true);
        }

        /// <summary> Bool to determine if the context menu should be shown</summary>
        public bool ContextMenuAllowed { get; set; }

        private void _ItemSelectionChangedTimer_Tick(object sender, EventArgs e)
        {
            ItemSelectionChangedTimer.Stop();
        }

        /// <summary>
        /// Subscribe to MouseDown event for the ListView.
        /// Do a HitTest on the ListView using the coordinates of the mouse (e.X and e.Y).
        /// If they hit on an item AND it was a right click, set contextMenuAllowed to true.
        /// </summary>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ListViewHitTestInfo lvhti = HitTest(e.X, e.Y);
                ContextMenuAllowed = lvhti.Item != null;
            }
            base.OnMouseDown(e);
        }

        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            base.OnSelectedIndexChanged(e);
            if (ItemSelectionChangedTimer.Enabled)
                return;
            SendMessage(Handle, WM_CHANGEUISTATE, 0x10001, 0);
            ItemSelectionChangedTimer.Start();
        }

        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);
            SendMessage(Handle, WM_CHANGEUISTATE, 0x10001, 0);
        }

        protected override void CreateHandle()
        {
            base.CreateHandle();
            SetWindowTheme(Handle, "Explorer", null);
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            SetWindowTheme(Handle, "Explorer", null);
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            SendMessage(Handle, WM_CHANGEUISTATE, 0x10001, 0);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            SendMessage(Handle, WM_CHANGEUISTATE, 0x10001, 0);
        }
    }
}