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

        /// <summary>
        /// Returns a DateTime after examining the radiobuttons/checkboxes to specify the logic behavior.
        /// </summary>
        /// reqs: (string path), contentsDirList, contentsFileList, Thi
        /// This is a viewmodel thing sorta. Grab the viewmodel as data first, then act, then update viewmodel.
        /// This viewmodel needs to contain: radioGroupBox1_SpecifyTime.Che
        /// radioGroupBox1_SpecifyTime.Checked
        /// radioGroupBox2_CurrentSelectionTime.Checked
        /// radioGroupBox3_UseTimeFrom.Checked
        ///     =radioButton1_useTimefromFile.Checked
        ///     =radioButton2_useTimefromSubdir.Checked
        internal DateTime? DecideWhichTimeMode1(string path, guistatus gui)
        {
            contentsDirList.Clear();
            contentsFileList.Clear();

            var dateToUse = new DateTime?();
            if (gui.radioGroupBox1SpecifyTime)
            {
                dateToUse = DateTime.Parse(gui.dateTimePickerDate.Date.ToString("d") + " " +
                                           gui.dateTimePickerTime.Hour + ":" +
                                           gui.dateTimePickerTime.Minute + ":" +
                                           gui.dateTimePickerTime.Second);
            }
            else if (gui.radioGroupBox2CurrentSelect)
            {
                if (gui.rg2rb1Creation)
                    dateToUse = DateTime.Parse(gui.Created);
                else if (gui.rg2rb2Modified)
                    dateToUse = DateTime.Parse(gui.Modified);
                else if (gui.rg2rb3LastAccess)
                    dateToUse = DateTime.Parse(gui.Accessed);
            }
            else if (gui.radioGroupBox3UseTimeFrom)
            {
                dateToUse = DecideTimeFromSubDirFile(path, gui);
            }
            return dateToUse;
        }
        /// <summary>
        /// Static. Check all the subdirs or subfiles. And decide the time. Called from: DecideWhichTimeMode1() { radioGroupBox3_UseTimeFrom
        /// </summary>
        internal DateTime? DecideTimeFromSubDirFile(string path, guistatus gui)
        {
            var dateToUse = new DateTime?();
            var extractlist = new List<string>();
            if (gui.radioButton1_useTimefromFile)
            {
                extractlist = PopulateFileList(path);
            }
            else if (gui.radioButton2_useTimefromSubdir)
            {
                extractlist = PopulateDirList(path);
            }

            // if the list is blank due to no files actually existing then we have nothing to do, so stop here.
            if (extractlist.Count == 0)
                return null;
            //for Any/Random attribute mode, decide which attribute and stick with it.
            int randomNumber = random.Next(0, 3);
            var timelist = new List<DateTime?>();
            //start iterating through
            foreach (string subitem in extractlist)
            {
                var looptempDate = new DateTime();
                try
                {
                    string fullpath = Path.Combine(path, subitem);
                    string timelisttype = null; //this was made just in case.
                    if (gui.radioButton1_setfromCreated)
                    {
                        timelisttype = "Created";
                        looptempDate = File.GetCreationTime(fullpath);
                    }
                    else if (gui.radioButton2_setfromModified)
                    {
                        timelisttype = "Modified";
                        looptempDate = File.GetLastWriteTime(fullpath);
                    }
                    else if (gui.radioButton3_setfromAccessed)
                    {
                        timelisttype = "Accessed";
                        looptempDate = File.GetLastAccessTime(fullpath);
                    }
                    else if (gui.radioButton4_setfromRandom)
                    {
                        switch (randomNumber)
                        {
                            case 0:
                                timelisttype = "Created";
                                looptempDate = File.GetCreationTime(fullpath);
                                break;

                            case 1:
                                timelisttype = "Modified";
                                looptempDate = File.GetLastWriteTime(fullpath);
                                break;

                            case 2:
                                timelisttype = "Accessed";
                                looptempDate = File.GetLastAccessTime(fullpath);
                                break;
                        }
                    }
                    timelist.Add(looptempDate);
                    var stopbotheringme = timelisttype; //remove "unused var" warning
                }
                catch (UnauthorizedAccessException)
                { }
            }
            var minmax = new DateNewOldObj(timelist);
            if (gui.radioButton1Oldest)
            {
                if (minmax.MinDate != null)
                    dateToUse = minmax.MinDate; //explicit typecast from nullable
            }
            else if (gui.radioButton2Newest)
            {
                if (minmax.MaxDate != null)
                    dateToUse = minmax.MaxDate;
            }
            else if (gui.radioButton3Random)
            {
                int randomFile = random.Next(0, minmax.Index);
                if (timelist[randomFile] != null)
                    dateToUse = timelist[randomFile];
            }
            return dateToUse;
        }
    //
    }
}