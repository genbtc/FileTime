﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using genBTC.FileTime.Properties;

namespace genBTC.FileTime.Classes
{
    //This needs to be renamed to something once we figure out why all this stuff is allowed to be seperate, Sort it. Refactor it.
    class SharedHelper
    {
        //native call to do string compare like the OS
        [DllImport("shlwapi.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
        public static extern int StrCmpLogicalW(String x, String y);
        public static IComparer<string> explorerStringComparer()
        {
            return new ExplorerLikeComparer();
        }
        public static IEqualityComparer<string> explorerStringEqualityComparer(string xPath, string yPath)
        {
            return new ExplorerLikeEqualityComparer(xPath, yPath);
        }

        public static readonly char Seperator = Path.DirectorySeparatorChar;
        public static readonly string SeperatorString = Path.DirectorySeparatorChar.ToString();
        public static readonly string UserDesktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

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

        /// <summary>  Reasons to be invisible  </summary>
        public static FileAttributes SyncSettingstoInvisibleFlag()
        {
            FileAttributes reasonsToBeInvisible = (Settings.Default.ShowHidden ? 0 : FileAttributes.Hidden) |
                                                  (Settings.Default.ShowSystem ? 0 : FileAttributes.System) |
                                                  (Settings.Default.ShowReadOnly ? 0 : FileAttributes.ReadOnly);
            return reasonsToBeInvisible;
        }

        /// <summary> Return 1 if bool=true (Directory) otherwise 0=false (File) </summary>
        public static int Bool2Int(bool fileOrDir)
        {
            return fileOrDir ? 1 : 0;
        }

        /// <summary>
        /// Display the Folder Browser Dialog and then display the selected
        /// file path and the directories and files in the folder.
        /// </summary>
        public static string OpenFilePicker(string inpath, string message)
        {
            //Feed in a path to start in or use current path as dialog path:
            if (inpath == null)
                return null;
            string outpath = "";
            //start a new windows api filebrowser dialog on its own thread.
            var t = new Thread(() =>
            {
                string description = message;
                var openFile = new FolderBrowserDialog
                {
                    ShowNewFolderButton = false,
                    SelectedPath = inpath,
                    Description = description
                };
                //use current path as dialog path

                if (openFile.ShowDialog() == DialogResult.Cancel)
                    return;
                //return what was selected (using inpath and outpath ensure thread safety)
                outpath = openFile.SelectedPath;
            });
            //STAThread is needed for OLE calls to dialog
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            t.Join();
            return outpath;
        }
    }
}