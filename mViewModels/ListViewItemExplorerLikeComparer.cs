using System.Collections;
using System.Windows.Forms;
using genBTC.FileTime.Models;

namespace genBTC.FileTime.mViewModels
{
    /// <summary> Explorer-like Sort, for use by listview.Sorter </summary>
    public class ListViewItemExplorerLikeComparer : IComparer
    {
        public int Compare(object x, object y)
        {
            return DataModel.StrCmpLogicalW(((ListViewItem)x).Text, ((ListViewItem)y).Text);
        }
    }
}