using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using genBTC.FileTime.Classes;
using genBTC.FileTime.Classes.Native;
using genBTC.FileTime.mViewModels;
using genBTC.FileTime.Properties;

namespace genBTC.FileTime.Models
{
    public partial class DataModel
    {
        public List<string> contentsDirList;
        public List<string> contentsFileList;
        public List<string> filextlist;
        /// <summary> Pass a list of files that had the read-only attribute fixed, so Form2 can display it </summary>
        public List<string> FilesReadOnlytoFix;
        /// <summary> List of Class to be passed to Form 2 for confirmation of files</summary>
        public List<NameDateObject> FilestoConfirmList;

        public ListView listViewContents;

        public ImageList imageListFiles;

        internal SkippedHSR Skips;

        public Random random;

        /// <summary>  Constructor  </summary>
        public DataModel()
        {
            contentsDirList = new List<string>();
            contentsFileList = new List<string>();
            filextlist = new List<string>();
            FilesReadOnlytoFix = new List<string>();
            FilestoConfirmList = new List<NameDateObject>();
            Skips = new SkippedHSR();
            random = new Random();
        }

        /// <summary> Clear contents Dir + File lists </summary>
        public void Clear()
        {
            contentsDirList.Clear();
            contentsFileList.Clear();
            listViewContents.Items.Clear();
        }

        public static List<string> PopulateFileList(string path, DataModel dataModel)
        {
            foreach (string filename in Directory.GetFiles(path))
                dataModel.contentsFileList.Add(Path.GetFileName(filename));
            return dataModel.contentsFileList;
        }

        public static List<string> PopulateDirList(string path, DataModel dataModel)
        {
            foreach (string subDirectory in Directory.GetDirectories(path))
                dataModel.contentsDirList.Add(Path.GetFileName(subDirectory));
            return dataModel.contentsDirList;
        }

        /// <summary>
        /// Display subfiles and subdirectories in the right panel listview
        /// </summary>
        /// <param name="filesonly">Don't show the directories, only files.</param>
        /// reqs: listviewcontents, contentsDirList,contentsFileList, labelFpathText, checkbox_recurse, filextlist, imageList_Files
        public static void DisplayContentsList(DataModel dataModel, bool checkboxRecurse, string labelname, bool filesonly = true)
        {
            //Clear the contents UI + datamodel
            dataModel.Clear();

            string directoryName = labelname;

            if (!Directory.Exists(directoryName))
                return;

            if (!filesonly && checkboxRecurse)
            {
                //part 1: list and store all the subdirectories
                try
                {
                    PopulateDirList(directoryName, dataModel);
                }
                catch (UnauthorizedAccessException)
                { }
                //Sort them
                dataModel.contentsDirList.Sort(explorerStringComparer());
                //Add them to the listview.
                foreach (string subDirectory in dataModel.contentsDirList)
                {
                    // Display all the sub directories using the directory icon (enum 1)
                    dataModel.listViewContents.Items.Add(subDirectory, (int)ListViewIcon.Directory);
                }
            }

            // (Display all of the files and show a file icon)
            try
            {
                //part 2: list all subfiles, match the extension and find the icon.
                foreach (string file in Directory.GetFiles(directoryName))
                {
                    var fileAttribs = File.GetAttributes(file);
                    if ((fileAttribs & SyncSettingstoInvisibleFlag()) != 0)
                        continue; //skip the rest if its supposed to be "invisible" based on the mask
                    var justName = Path.GetFileName(file);
                    SharedHelper.CurrExten = Path.GetExtension(file);
                    if ((SharedHelper.CurrExten != ".lnk")) //if its not a shortcut
                    {
                        //if not already in the list, then add it
                        if (dataModel.filextlist.FindLastIndex(SharedHelper.FindCurExt) == -1)
                        {
                            dataModel.filextlist.Add(SharedHelper.CurrExten);
                            //call NativeExtractIcon to get the filesystem icon of the filename
                            dataModel.imageListFiles.Images.Add(SharedHelper.CurrExten, NativeExtractIcon.GetIcon(file, true));
                        }
                    }
                    else //if it is a shortcut, grab icon directly.
                        dataModel.imageListFiles.Images.Add(justName, NativeExtractIcon.GetIcon(file, true));

                    dataModel.contentsFileList.Add(justName);
                }
            }
            catch (UnauthorizedAccessException)
            { }
            //Sort them
            dataModel.contentsFileList.Sort(explorerStringComparer());
            //Add them to the listview.
            foreach (string file in dataModel.contentsFileList)
            {
                string exten = Path.GetExtension(file);
                dataModel.listViewContents.Items.Add(file, exten != ".lnk" ? exten : file);
            }
        }

        public static IComparer<string> explorerStringComparer()
        {
            return new ExplorerComparerstringHelper();
        }
        /// <summary>  Reasons to be invisible  </summary>
        private static FileAttributes SyncSettingstoInvisibleFlag()
        {
            FileAttributes reasonsToBeInvisible = (Settings.Default.ShowHidden ? 0 : FileAttributes.Hidden) |
                                                  (Settings.Default.ShowSystem ? 0 : FileAttributes.System) |
                                                  (Settings.Default.ShowReadOnly ? 0 : FileAttributes.ReadOnly);
            return reasonsToBeInvisible;
        }

        /// <summary> Return 1 if bool=true (Directory) otherwise 0=false (File) </summary>
        private static int Bool2Int(bool fileOrDir)
        {
            return fileOrDir ? 1 : 0;
        }


    //
    }
}