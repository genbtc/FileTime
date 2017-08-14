using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

//WTF is this just some poorly named and badly defined general helper class. 
using System.Windows.Forms;

namespace genBTC.FileTime.Classes
{
    class SharedHelper
    {
        public static string CurrExten;
        public List<string> Filextlist = new List<string>();

        public static bool FindCurExt(String s)
        {
            return s == CurrExten;
        }

        public static FileAttributes RemoveAttributes(FileAttributes attributes, FileAttributes attributesToRemove)
        {
            return attributes & ~attributesToRemove;
        }
    }
    /// <summary> Explorer-like Sort, for strings </summary>
    public class ExplorerComparerstring : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            return Form_Main.StrCmpLogicalW(x, y);
        }

    }

    /// <summary> Explorer-like Sort, for use by listview.Sorter </summary>
    public class ListViewItemExplorerLikeComparer : IComparer
    {
        public int Compare(object x, object y)
        {
            return Form_Main.StrCmpLogicalW(((ListViewItem)x).Text, ((ListViewItem)y).Text);
        }

    }
}