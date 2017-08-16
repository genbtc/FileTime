using System.Collections.Generic;
using genBTC.FileTime.Models;

namespace genBTC.FileTime.mViewModels
{
    /// <summary> Explorer-like Sort, for strings </summary>
    public class ExplorerComparerstringHelper : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            return DataModel.StrCmpLogicalW(x, y);
        }
    }
}