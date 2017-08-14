using System;
using System.Collections.Generic;
using System.IO;

//WTF is this just some poorly named and badly defined general helper class. 

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
}