using System.Collections.Generic;

namespace genBTC.FileTime.Classes
{
    /// <summary> Explorer-like Sort, for strings </summary>
    public class ExplorerComparerstringHelper : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            return SharedHelper.StrCmpLogicalW(x, y);
        }
    }
}