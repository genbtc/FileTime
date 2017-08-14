using System.Collections;
using System.Windows.Forms;

namespace genBTC.FileTime.mViewModels
{
    /// <summary> Explorer-like Sort, for use by listview.Sorter </summary>
    public class ListViewItemExplorerLikeComparer : IComparer
    {
        public int Compare(object x, object y)
        {
            return Form_Main.StrCmpLogicalW(((ListViewItem)x).Text, ((ListViewItem)y).Text);
        }

    }
}