using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using genBTC.FileTime.Classes;
using genBTC.FileTime.Models;
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

        #region Data Containers (Private)
        //Data Containers (Private)
        private static readonly char Seperator = Path.DirectorySeparatorChar;
        private static readonly string UserDesktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

        private readonly List<string> contentsDirList = new List<string>();
        private readonly List<string> contentsFileList = new List<string>();
        private readonly List<string> filextlist = new List<string>();

        #endregion

        #region Declarations
        //Declarations
        private readonly IComparer<string> explorerStringComparer = new ExplorerComparerstring();
        private readonly Timer itemSelectionChangedTimer = new Timer();

        private readonly Random random = new Random();

        /// <summary> Count of the number of hidden files skipped </summary>
        private int _skippedHiddenCount;
        /// <summary> Count of the number of Read-only files skipped </summary>
        private int _skippedReadOnlyCount;
        /// <summary> Count of the number of System files skipped </summary>
        private int _skippedSystemCount;

        #endregion

        private static FileAttributes SyncSettingstoInvisibleFlag()
        {
            FileAttributes reasonsToBeInvisible = (Settings.Default.ShowHidden ? 0 : FileAttributes.Hidden) |
                                                  (Settings.Default.ShowSystem ? 0 : FileAttributes.System) |
                                                  (Settings.Default.ShowReadOnly ? 0 : FileAttributes.ReadOnly);
            return reasonsToBeInvisible;
        }

        /// <summary>
        /// Overloaded. Make a NameDateObject out of 1 filename; writes each time time to the date attribute that was radiobutton selected.
        /// VIEWMODEL
        /// </summary>
        private NameDateObject Makedateobject(string folderPath, NameDateObject subObject)
        {
            var currentobject = new NameDateObjectListViewVm(subObject) { Name = folderPath, FileOrDirType = 1 };

            //If Checkbox is selected:
            if (!checkBox_CreationDateTime.Checked)
                currentobject.Created = "N/A"; // Set the Creation date/time if selected
            if (!checkBox_ModifiedDateTime.Checked)
                currentobject.Modified = "N/A"; // Set the Modified date/time if selected	
            if (!checkBox_AccessedDateTime.Checked)
                currentobject.Accessed = "N/A"; // Set the Last Access date/time if selected
            return new NameDateObject(currentobject.Converter());
        }
    }
}
