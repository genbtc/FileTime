using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;

namespace genBTC.FileTime.Classes
{
    /// <summary> Explorer-like Sort, for strings </summary>
    public class ExplorerLikeComparer : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            return SharedHelper.StrCmpLogicalW(x, y);
        }
    }

    /// <summary> Explorer-like Sort, for ListViewItem - used by listview.Sorter </summary>
    public class ExpLikeCmpHelperforListView : IComparer
    {
        public int Compare(object x, object y)
        {
            return SharedHelper.StrCmpLogicalW(((ListViewItem)x).Text, ((ListViewItem)y).Text);
        }
    }
}