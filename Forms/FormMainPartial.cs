using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using genBTC.FileTime.mViewModels;
using genBTC.FileTime.Properties;
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
        
        private readonly IComparer<string> explorerStringComparer = new ExplorerComparerstring();
        private readonly Timer itemSelectionChangedTimer = new Timer();

        

        #endregion Vars

        /// <summary>  Reasons to be invisible  </summary>
        private static FileAttributes SyncSettingstoInvisibleFlag()
        {
            FileAttributes reasonsToBeInvisible = (Settings.Default.ShowHidden ? 0 : FileAttributes.Hidden) |
                                                  (Settings.Default.ShowSystem ? 0 : FileAttributes.System) |
                                                  (Settings.Default.ShowReadOnly ? 0 : FileAttributes.ReadOnly);
            return reasonsToBeInvisible;
        }

        /// <summary> Icon in listView image list </summary>
        private enum ListViewIcon
        {
            /// <summary> File icon in listView image list </summary>
            File = 0,
            /// <summary> Directory icon in listView image list </summary>
            Directory = 1
        }

        /// <summary> Return 1 if bool=true (Directory) otherwise 0=false (File) </summary>
        private static int Bool2Int(bool fileOrDir)
        {
            return fileOrDir ? 1 : 0;
        }

        //>
    }
}
