using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using genBTC.FileTime.Classes;
using genBTC.FileTime.Classes.Native;

namespace genBTC.FileTime.Models
{
    public partial class DataModel
    {
        #region Vars

        public List<string> contentsDirList;
        public List<string> contentsFileList;
        public List<string> filextlist;
        /// <summary> Pass a list of files that had the read-only attribute fixed, so Form2 can display it </summary>
        public List<string> FilesReadOnlytoFix;
        /// <summary> List of Class to be passed to Form 2 for confirmation of files</summary>
        internal List<NameDateObj> FilestoConfirmList;

        public ListView listViewContents;

        public ImageList imageListFiles;

        internal SkippedHSR Skips;

        public Random random;

        #endregion Vars


        /// <summary>  Constructor  </summary>
        public DataModel()
        {
            contentsDirList = new List<string>();
            contentsFileList = new List<string>();
            filextlist = new List<string>();
            FilesReadOnlytoFix = new List<string>();
            FilestoConfirmList = new List<NameDateObj>();
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

        public List<string> PopulateFileList(string path)
        {
            foreach (string filename in Directory.GetFiles(path))
                contentsFileList.Add(Path.GetFileName(filename));
            return contentsFileList;
        }

        public List<string> PopulateDirList(string path)
        {
            foreach (string subDirectory in Directory.GetDirectories(path))
                contentsDirList.Add(Path.GetFileName(subDirectory));
            return contentsDirList;
        }



        /// <summary>
        /// Display subfiles and subdirectories in the right panel listview
        /// </summary>
        /// <param name="filesonly">Don't show the directories, only files.</param>
        /// reqs: listviewcontents, contentsDirList,contentsFileList, labelFpathText, checkbox_recurse, filextlist, imageList_Files
        public void DisplayContentsList(bool checkboxRecurse, string labelname, bool filesonly = true)
        {
            //Clear the contents UI + datamodel
            this.Clear();

            string directoryName = labelname;

            if (!Directory.Exists(directoryName))
                return;

            if (!filesonly && checkboxRecurse)
            {
                //part 1: list and store all the subdirectories
                try
                {
                    PopulateDirList(directoryName);
                }
                catch (UnauthorizedAccessException)
                { }
                //Sort them
                this.contentsDirList.Sort(SharedHelper.explorerStringComparer());
                //Add them to the listview.
                foreach (string subDirectory in this.contentsDirList)
                {
                    // Display all the sub directories using the directory icon (enum 1)
                    this.listViewContents.Items.Add(subDirectory, (int)ListViewIcon.Directory);
                }
            }

            // (Display all of the files and show a file icon)
            try
            {
                //part 2: list all subfiles, match the extension and find the icon.
                foreach (string file in Directory.GetFiles(directoryName))
                {
                    var fileAttribs = File.GetAttributes(file);
                    if ((fileAttribs & SharedHelper.SyncSettingstoInvisibleFlag()) != 0)
                        continue; //skip the rest if its supposed to be "invisible" based on the mask
                    var justName = Path.GetFileName(file);
                    SharedHelper.CurrExten = Path.GetExtension(file);
                    if ((SharedHelper.CurrExten != ".lnk")) //if its not a shortcut
                    {
                        //if not already in the list, then add it
                        if (this.filextlist.FindLastIndex(SharedHelper.FindCurExt) == -1)
                        {
                            this.filextlist.Add(SharedHelper.CurrExten);
                            //call NativeExtractIcon to get the filesystem icon of the filename
                            this.imageListFiles.Images.Add(SharedHelper.CurrExten, NativeExtractIcon.GetIcon(file, true));
                        }
                    }
                    else //if it is a shortcut, grab icon directly.
                        this.imageListFiles.Images.Add(justName, NativeExtractIcon.GetIcon(file, true));

                    this.contentsFileList.Add(justName);
                }
            }
            catch (UnauthorizedAccessException)
            { }
            //Sort them
            this.contentsFileList.Sort(SharedHelper.explorerStringComparer());
            //Add them to the listview.
            foreach (string file in this.contentsFileList)
            {
                string exten = Path.GetExtension(file);
                this.listViewContents.Items.Add(file, exten != ".lnk" ? exten : file);
            }
        }
    //
    }
}