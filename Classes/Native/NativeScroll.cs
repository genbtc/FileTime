using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace genBTC.FileTime.Classes.Native
{
    internal static class NativeScroll
    {
        private const int SB_HORZ = 0x0;
        //private const int SB_VERT = 0x1;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int GetScrollPos(IntPtr hWnd, int nBar);

        /// <summary> Manually scroll a control </summary>
        /// <param name="hWnd">Window Handle</param>
        /// <param name="nBar">Which scroll Bar?, 0x0 for horizontal or 0x1 for Vertical</param>
        /// <param name="nPos">New scroll position, in pixels</param>
        /// <param name="bRedraw">redraw is usually true</param>
        [DllImport("user32.dll")]
        public static extern int SetScrollPos(IntPtr hWnd, int nBar, int nPos, bool bRedraw);

        /// <summary> Only for treenodes. and only horizontal. </summary>
        /// <param name="thing">the TreeNode being passed, to scroll into view.</param>
        /// <param name="pixelsH">pixels to scroll by</param>
        /// <param name="abs">absolute? otherwise, get current position and add pixelsH to that</param>
        public static void ScrollH(TreeNode thing, int pixelsH, bool abs = true)
        {
            thing.TreeView.BeginUpdate();

            thing.EnsureVisible(); //always bring the node into view normally

            // now you can scroll back ALL the way to the left with:
            //SetScrollPos(thing.TreeView.Handle, SB_HORZ, 0, true);

            // ..or just a few pixels:
            if (!abs)
            {
                int spos = GetScrollPos(thing.TreeView.Handle, SB_HORZ);
                SetScrollPos(thing.TreeView.Handle, SB_HORZ, spos + pixelsH, true);
            }
            else
                SetScrollPos(thing.TreeView.Handle, SB_HORZ, pixelsH, true);

            thing.TreeView.EndUpdate();
        }
    }
}