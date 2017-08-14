using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace genBTC.FileTime.Classes
{
    //only used once on ExplorerTree.cs line 581 to scroll programmatically.
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

    ///http://www.pinvoke.net/default.aspx/shell32.shgetfileinfo
    /// <summary> Grabs the filesystem or system icon cache for a string filePath.</summary>
    public class ExtractIcon
    {
        /// <summary>Maximal Length of unmanaged Windows-Path-strings</summary>
        private const int MAX_PATH = 260;

        /// <summary>Maximal Length of unmanaged Typename</summary>
        private const int MAX_TYPE = 80;

        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        private static extern int SHGetFileInfo(
            string pszPath,
            int dwFileAttributes,
            out SHFILEINFO psfi,
            uint cbfileInfo,
            SHGFI uFlags);

        /// <summary>
        /// Get the associated Icon for a file or application, this method always returns
        /// an icon.  If the strPath is invalid or there is no idonc the default icon is returned
        /// </summary>
        /// <param name="strPath">full path to the file</param>
        /// <param name="bSmall">if true, the 16x16 icon is returned otherwise the 32x32</param>
        /// <returns></returns>
        public static Icon GetIcon(string strPath, bool bSmall)
        {
            var info = new SHFILEINFO();
            int cbFileInfo = Marshal.SizeOf(info);
            SHGFI flags = SHGFI.Icon | SHGFI.UseFileAttributes;
            if (bSmall)
                flags = flags | SHGFI.SmallIcon;
            else
                flags = flags | SHGFI.LargeIcon;

            SHGetFileInfo(strPath, 256, out info, (uint) cbFileInfo, flags);
            try
            {
                return Icon.FromHandle(info.hIcon);
            } catch (ArgumentException){return null;}
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct SHFILEINFO
        {
            public SHFILEINFO(bool b = true)
            {
                hIcon = IntPtr.Zero;
                int iIcon = 0;
                uint dwAttributes = 0;
                szDisplayName = "";
                szTypeName = "";
            }

            public readonly IntPtr hIcon;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_PATH)] private readonly string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_TYPE)] private readonly string szTypeName;
        };

        [Flags]
        private enum SHGFI
        {
            /// <summary>get icon</summary>
            Icon = 0x000000100,
            /// <summary>get display name</summary>
            DisplayName = 0x000000200,
            /// <summary>get type name</summary>
            TypeName = 0x000000400,
            /// <summary>get attributes</summary>
            Attributes = 0x000000800,
            /// <summary>get icon location</summary>
            IconLocation = 0x000001000,
            /// <summary>return exe type</summary>
            ExeType = 0x000002000,
            /// <summary>get system icon index</summary>
            SysIconIndex = 0x000004000,
            /// <summary>put a link overlay on icon</summary>
            LinkOverlay = 0x000008000,
            /// <summary>show icon in selected state</summary>
            Selected = 0x000010000,
            /// <summary>get only specified attributes</summary>
            Attr_Specified = 0x000020000,
            /// <summary>get large icon</summary>
            LargeIcon = 0x000000000,
            /// <summary>get small icon</summary>
            SmallIcon = 0x000000001,
            /// <summary>get open icon</summary>
            OpenIcon = 0x000000002,
            /// <summary>get shell size icon</summary>
            ShellIconSize = 0x000000004,
            /// <summary>pszPath is a pidl</summary>
            PIDL = 0x000000008,
            /// <summary>use passed dwFileAttribute</summary>
            UseFileAttributes = 0x000000010,
            /// <summary>apply the appropriate overlays</summary>
            AddOverlays = 0x000000020,
            /// <summary>Get the index of the overlay in the upper 8 bits of the iIcon</summary>
            OverlayIndex = 0x000000040,
        }
    }
}