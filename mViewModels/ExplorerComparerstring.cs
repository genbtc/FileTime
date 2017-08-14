using System.Collections.Generic;

namespace genBTC.FileTime.mViewModels
{
    /// <summary> Explorer-like Sort, for strings </summary>
    public class ExplorerComparerstring : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            return Form_Main.StrCmpLogicalW(x, y);
        }

    }
}