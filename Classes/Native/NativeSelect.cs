using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace genBTC.FileTime.Classes.Native
{
    internal class NativeSelect
    {
        private const int LVM_FIRST = 0x1000;
        private const int LVM_SETITEMSTATE = LVM_FIRST + 43;

        [DllImport("user32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessageLVItem(IntPtr hWnd, int msg, int wParam, ref LVITEM lvi);

        /// <summary>  Select all rows on the given listview  </summary>
        /// <param name="list">The listview whose items are to be selected</param>
        public static void SelectAllItems(ListView list)
        {
            Cursor.Current = Cursors.WaitCursor;
            list.BeginUpdate();
                SetItemState(list, -1, 2, 2);
            list.EndUpdate();
            Cursor.Current = Cursors.Default;
        }

        /// <summary>  Deselect all rows on the given listview  </summary>
        /// <param name="list">The listview whose items are to be deselected</param>
        public static void DeselectAllItems(ListView list)
        {
            Cursor.Current = Cursors.WaitCursor;
            list.BeginUpdate();
                SetItemState(list, -1, 2, 0);
            list.EndUpdate();
            Cursor.Current = Cursors.Default;
        }

        /// <summary>  Invert selection on all rows on the listview and cleanup the focus line. </summary> 
        /// <param name="list">The listview whose items are to be deselected</param>
        /// <returns>int lastselectedindex (focus line index #)</returns>
        public static int InvertSelection(ListView list)
        {
            Cursor.Current = Cursors.WaitCursor;
            list.BeginUpdate();
            var lastselectedindex = 0;
                foreach (ListViewItem item in list.Items)
                {
                    item.Selected = !item.Selected;
                    if (item.Selected)
                        lastselectedindex = item.Index;
                }
                if (list.FocusedItem != null)
                    list.FocusedItem.Focused = false;
            list.Items[lastselectedindex].Focused = true;
            list.EndUpdate();
            Cursor.Current = Cursors.Default;
            return lastselectedindex;
        }

        /// <summary>
        /// Set the item state on the given item
        /// </summary>
        /// <param name="list">The listview whose item's state is to be changed</param>
        /// <param name="itemIndex">The index of the item to be changed</param>
        /// <param name="mask">Which bits of the value are to be set?</param>
        /// <param name="value">The value to be set</param>
        public static void SetItemState(ListView list, int itemIndex, int mask, int value)
        {
            var lvItem = new LVITEM
            {
                stateMask = mask,
                state = value
            };
            SendMessageLVItem(list.Handle, LVM_SETITEMSTATE, itemIndex, ref lvItem);
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct LVITEM
        {
            public int mask;
            public int iItem;
            public int iSubItem;
            public int state;
            public int stateMask;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string pszText;
            public int cchTextMax;
            public int iImage;
            public IntPtr lParam;
            public int iIndent;
            public int iGroupId;
            public int cColumns;
            public IntPtr puColumns;
        };
    }
}