using System.Collections;
using System.Windows.Forms;

namespace genBTC.FileTime.Classes
{
    /// <summary> Explorer-like Sort, for use by listview.Sorter </summary>
    public class ListViewItemExplorerLikeComparer : IComparer
    {
        public int Compare(object x, object y)
        {
            return SharedHelper.StrCmpLogicalW(((ListViewItem)x).Text, ((ListViewItem)y).Text);
        }
    }
}