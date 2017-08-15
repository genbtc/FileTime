using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using genBTC.FileTime.Models;
using genBTC.FileTime.mViewModels;
using genBTC.FileTime.Properties;
using UIToolbox;

namespace genBTC.FileTime
{

    struct BoolCMA
    {
        public bool C;
        public bool M;
        public bool A;
    }

    public struct SkippedHSR
    {
        /// <summary> Count of the number of hidden files skipped </summary>
        public int H;
        /// <summary> Count of the number of Read-only files skipped </summary>
        public int R;
        /// <summary> Count of the number of System files skipped </summary>
        public int S;
    }

    public class DataModel
    {
        public readonly List<string> contentsDirList = new List<string>();
        public readonly List<string> contentsFileList = new List<string>();
        public readonly List<string> filextlist = new List<string>();
        /// <summary> Pass a list of files that had the read-only attribute fixed, so Form2 can display it </summary>
        public List<string> FilesReadOnlytoFix = new List<string>();
        /// <summary> List of Class to be passed to Form 2 for confirmation of files</summary>
        public List<NameDateObject> FilestoConfirmList = new List<NameDateObject>();

        public SkippedHSR Skips = new SkippedHSR();

        public readonly Random random = new Random();


        /// <summary>
        /// Clear Function.
        /// </summary>
        public void Clear()
        {
            contentsDirList.Clear();
            contentsFileList.Clear();
        }

    }

    public partial class Form_Main
    {
        private static void SkipOrAddFile(DataModel dataModel, string path, bool isDirectory)
        {
            FileAttributes fAttr = File.GetAttributes(path);

            if (((fAttr & FileAttributes.System) == FileAttributes.System) && Settings.Default.SkipSystem)
            {
                dataModel.Skips.S++;
                return; // Skip system files and directories
            }
            if (((fAttr & FileAttributes.Hidden) == FileAttributes.Hidden) && Settings.Default.SkipHidden)
            {
                dataModel.Skips.H++;
                return; // Skip hidden files and directories 
            }
            if (((fAttr & FileAttributes.ReadOnly) == FileAttributes.ReadOnly) && !isDirectory)
            {
                if (Settings.Default.SkipReadOnly)
                {
                    dataModel.Skips.R++;
                    return;
                }
                if (Settings.Default.ShowNoticesReadOnly)
                {
                    DialogResult dr =
                        MessageBox.Show(
                            "The file '" + path + "' is Read-Only.\n\nContinue showing Read-Only notifications?",
                            "Read-Only", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    Settings.Default.ShowNoticesReadOnly = (dr == DialogResult.Yes);
                }
                //add them to the list to be readOnly+Fixed before they are timewritten.
                dataModel.FilesReadOnlytoFix.Add(path);
            }
        }
    

        /// <summary> STATIC.
        /// Set the date/time for a single file/directory (This works on files and directories)
        /// Go through the list, skipping H,S,R files, and add all the file+date objects to the Confirmation List
        /// (Only adds to the confirm list, Form 2 (Confirm) will actually write changes).
        /// </summary>
        /// Requires: Pass in the datamodel and the checkbox states.
        /// <param name="filePath">Full path to the file/directory</param>
        /// <param name="fileTime">Date/Time to set the file/directory</param>
        /// <param name="isDirectory">Is this a directory???</param>
        private static void SetFileDateTimeMode1(DataModel dataModel, BoolCMA checkboxes, string filePath, DateTime fileTime, bool isDirectory)
        {
            SkipOrAddFile(dataModel,filePath,isDirectory);

            NameDateObject currentobject = Makedateobject(checkboxes, filePath, fileTime, isDirectory);
            dataModel.FilestoConfirmList.Add(currentobject);
        }

        //Mode #2 (same like above). Static.
        private static void SetFolderDateTimeMode2(DataModel dataModel, BoolCMA checkboxes, string folderPath, NameDateObject subFile)
        {
            SkipOrAddFile(dataModel, folderPath, true);

            subFile = Makedateobject(checkboxes, folderPath, subFile);
            dataModel.FilestoConfirmList.Add(subFile);
        }

        /// <summary>
        /// Static. Overloaded. Make a NameDateObject out of 1 filename; writes A SINGLE time to all 3 date properties.
        /// </summary>
        private static NameDateObject Makedateobject(BoolCMA checkboxes, string filePath, DateTime fileTime, bool isDirectory)
        {
            var currentobject = new NameDateObject { Name = filePath, FileOrDirType = Bool2Int(isDirectory) };

            // Set the Creation date/time if selected
            if (checkboxes.C)
                currentobject.Created = fileTime;
            // Set the Modified date/time is selected					
            if (checkboxes.M)
                currentobject.Modified = fileTime;
            // Set the last access time if selected
            if (checkboxes.A)
                currentobject.Accessed = fileTime;
            return currentobject;
        }
        /// <summary>
        /// Static. Overloaded. Make a NameDateObject out of 1 filename; writes each time time to the date attribute that was radiobutton selected.
        /// </summary>
        private static NameDateObject Makedateobject(BoolCMA checkboxes, string folderPath, NameDateObject subObject)
        {
            var currentobject = new NameDateObjectListViewVm(subObject) { Name = folderPath, FileOrDirType = 1 };

            //If Checkbox is selected:
            if (!checkboxes.C)
                currentobject.Created = "N/A"; // Set the Creation date/time if selected
            if (!checkboxes.M)
                currentobject.Modified = "N/A"; // Set the Modified date/time if selected	
            if (!checkboxes.A)
                currentobject.Accessed = "N/A"; // Set the Last Access date/time if selected
            return new NameDateObject(currentobject.Converter());
        }

        /// <summary>
        /// Display the date and time of the selected file (also works on Directories)
        /// </summary>

        struct DisplayCmaTimeData
        {
            public string pathName;
            
            public bool RadioGroupBox2_CurrentSelectionTime_Enabled;

            public string Created;
            public string Modified;
            public string Accessed;

            public string HiddenPathName;
        }

        private static DisplayCmaTimeData GetCmaTimes(string pathName)
        {
            var cma = new DisplayCmaTimeData {pathName = pathName};
            if (cma.pathName != "")
            {
                cma.Created = File.GetCreationTime(cma.pathName).ToString();
                cma.Modified = File.GetLastWriteTime(cma.pathName).ToString();
                cma.Accessed = File.GetLastAccessTime(cma.pathName).ToString();
                cma.HiddenPathName = cma.pathName;
                cma.RadioGroupBox2_CurrentSelectionTime_Enabled = true;
            }

            else
            {
                // Maybe no file/directory is selected
                // Then Blank out the display of date/time.
                cma.Created = "";
                cma.Modified = "";
                cma.Accessed = "";
                cma.HiddenPathName = "";
                cma.RadioGroupBox2_CurrentSelectionTime_Enabled = false;
            }
            return cma;
        }


        /// <summary>
        /// Static. Check all the subdirs or subfiles. And decide the time. Called from: DecideWhichTimeMode1() { radioGroupBox3_UseTimeFrom
        /// </summary>
        /// <param name="path"></param>
        /// <param name="dataModel"></param>
        /// <param name="radioButton1UseTimefromFile"></param>
        /// <param name="radioButton2UseTimefromSubdir"></param>
        /// <param name="radioButton1SetfromCreated"></param>
        /// <param name="radioButton2SetfromModified"></param>
        /// <param name="radioButton3SetfromAccessed"></param>
        /// <param name="radioButton4SetfromRandom"></param>
        /// <param name="radioButton1Oldest"></param>
        /// <param name="radioButton2Newest"></param>
        /// <param name="radioButton3Random"></param>
        /// <returns></returns>
        private static DateTime? DecideTimeFromSubDirFile(string path, DataModel dataModel, RadioButton radioButton1UseTimefromFile, RadioButton radioButton2UseTimefromSubdir, RadioButton radioButton1SetfromCreated, RadioButton radioButton2SetfromModified, RadioButton radioButton3SetfromAccessed, RadioButton radioButton4SetfromRandom, RadioButton radioButton1Oldest, RadioButton radioButton2Newest, RadioButton radioButton3Random)
        {
            var dateToUse = new DateTime?();
            var extractlist = new List<string>();
            if (radioButton1UseTimefromFile.Checked)
            {
                extractlist = PopulateFileList(path, dataModel);
            }
            else if (radioButton2UseTimefromSubdir.Checked)
            {
                extractlist = PopulateDirList(path, dataModel);
            }

            // if the list is blank due to no files actually existing then we have nothing to do, so stop here.
            if (extractlist.Count == 0)
                return null;
            //for Any/Random attribute mode, decide which attribute and stick with it.
            int randomNumber = dataModel.random.Next(0, 3);
            var timelist = new List<DateTime?>();
            //start iterating through
            foreach (string subitem in extractlist)
            {
                string timelisttype; //this was made just in case.
                var looptempDate = new DateTime();
                try
                {
                    string fullpath = Path.Combine(path, subitem);
                    if (radioButton1SetfromCreated.Checked)
                    {
                        timelisttype = "Created";
                        looptempDate = File.GetCreationTime(fullpath);
                    }
                    else if (radioButton2SetfromModified.Checked)
                    {
                        timelisttype = "Modified";
                        looptempDate = File.GetLastWriteTime(fullpath);
                    }
                    else if (radioButton3SetfromAccessed.Checked)
                    {
                        timelisttype = "Accessed";
                        looptempDate = File.GetLastAccessTime(fullpath);
                    }
                    else if (radioButton4SetfromRandom.Checked)
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
                }
                catch (UnauthorizedAccessException)
                { }
            }
            var minmax = new OldNewDate(timelist);
            if (radioButton1Oldest.Checked)
            {
                if (minmax.MinDate != null)
                    dateToUse = minmax.MinDate; //explicit typecast from nullable
            }
            else if (radioButton2Newest.Checked)
            {
                if (minmax.MaxDate != null)
                    dateToUse = minmax.MaxDate;
            }
            else if (radioButton3Random.Checked)
            {
                int randomFile = dataModel.random.Next(0, minmax.Index);
                if (timelist[randomFile] != null)
                    dateToUse = timelist[randomFile];
            }
            return dateToUse;
        }

        private static List<string> PopulateDirList(string path, DataModel dataModel)
        {
            foreach (string subDirectory in Directory.GetDirectories(path))
                dataModel.contentsDirList.Add(Path.GetFileName(subDirectory));
            return dataModel.contentsDirList; 
        }

        private static List<string> PopulateFileList(string path, DataModel dataModel)
        {
            foreach (string filename in Directory.GetFiles(path))
                dataModel.contentsFileList.Add(Path.GetFileName(filename));
            return dataModel.contentsFileList;
        }
    }
}
