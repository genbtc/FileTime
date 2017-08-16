using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using genBTC.FileTime.mViewModels;
using Timer = System.Windows.Forms.Timer;

namespace genBTC.FileTime
{
    public partial class Form_Main
    {
        //native call to do string compare like the OS
        [DllImport("shlwapi.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
        public static extern int StrCmpLogicalW(String x, String y);

        #region Vars

        private static readonly char Seperator = Path.DirectorySeparatorChar;
        private static readonly string UserDesktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        
        public static IComparer<string> explorerStringComparer()
        {
            return new ExplorerComparerstringHelper();
        }

        private readonly Timer itemSelectionChangedTimer = new Timer();

        #endregion Vars

    }
}
