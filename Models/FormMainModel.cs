using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using genBTC.FileTime.Models;
using genBTC.FileTime.mViewModels;
using genBTC.FileTime.Properties;

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
        public int C;
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
        /// <summary>
        /// Clear Function.
        /// </summary>
        public void ClearOnError()
        {
            contentsDirList.Clear();
            contentsFileList.Clear();
        }

    }

    public partial class Form_Main
    {
        private static void SetFileDateTimeMode1(DataModel dataModel, BoolCMA checkboxes, string filePath, DateTime fileTime, bool isDirectory)
        {
            FileAttributes fileAttributes = File.GetAttributes(filePath);

            if (((fileAttributes & FileAttributes.System) == FileAttributes.System) && Settings.Default.SkipSystem)
            {
                dataModel.Skips.C++;
                return; // Skip system files and directories
            }
            if (((fileAttributes & FileAttributes.Hidden) == FileAttributes.Hidden) && Settings.Default.SkipHidden)
            {
                dataModel.Skips.H++;
                return; // Skip hidden files and directories 
            }
            if (((fileAttributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly) && !isDirectory)
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
                            "The file '" + filePath + "' is Read-Only.\n\nContinue showing Read-Only notifications?",
                            "Read-Only", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    Settings.Default.ShowNoticesReadOnly = (dr == DialogResult.Yes);
                }
                dataModel.FilesReadOnlytoFix.Add(filePath);
            }

            NameDateObject currentobject = Makedateobject(checkboxes, filePath, fileTime, isDirectory);
            dataModel.FilestoConfirmList.Add(currentobject);
        }

        /// <summary>
        /// Go through the list, skipping H,S,R files, and add all the file+date objects to the Confirmation List
        /// </summary>
        /// <param name="folderPath"></param>
        /// <param name="subFile"></param>
        private static void SetFolderDateTimeMode2(DataModel dataModel, BoolCMA checkboxes, string folderPath, NameDateObject subFile)
        {
            FileAttributes folderAttributes = File.GetAttributes(folderPath);

            if (((folderAttributes & FileAttributes.System) == FileAttributes.System) && Settings.Default.SkipSystem)
            {
                dataModel.Skips.C++;
                return; // Skip system files and directories
            }
            if (((folderAttributes & FileAttributes.Hidden) == FileAttributes.Hidden) && Settings.Default.SkipHidden)
            {
                dataModel.Skips.H++;
                return; // Skip hidden files and directories 
            }
            if ((folderAttributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
            {
                if (Settings.Default.ShowNoticesReadOnly)
                {
                    DialogResult dr =
                        MessageBox.Show(
                            "The folder '" + folderPath +
                            "' is Read-Only and was skipped.\n\nContinue showing Read-Only notifications?",
                            "Read-Only", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    Settings.Default.ShowNoticesReadOnly = (dr == DialogResult.OK);
                }
                if (Settings.Default.SkipReadOnly)
                {
                    dataModel.Skips.R++;
                    return;
                }
            }
            subFile = Makedateobject(checkboxes, folderPath, subFile);
            dataModel.FilestoConfirmList.Add(subFile);
        }

        private BoolCMA QueryCMAcheckboxes()
        {
            var checkboxes = new BoolCMA
            {
                C = checkBox_CreationDateTime.Checked,
                M = checkBox_ModifiedDateTime.Checked,
                A = checkBox_AccessedDateTime.Checked
            };
            return checkboxes;
        }

        /// <summary>
        /// Clear both ListViews and the three data container lists, blanks the selected date, and erases the top current dir textbox.
        /// </summary>
        private void ClearOnError()
        {
            _dataModel.ClearOnError();
            listView_Contents.Items.Clear();
            DisplayCma("");
            label_FPath.Text = "";
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
        private void DisplayCma(string pathName)
        {
            if (pathName != "")
            {
                label_CreationTime.Text = File.GetCreationTime(pathName).ToString();
                label_Modified.Text = File.GetLastWriteTime(pathName).ToString();
                label_LastAccess.Text = File.GetLastAccessTime(pathName).ToString();
                labelHidden_PathName.Text = pathName;
                radioGroupBox2_CurrentSelectionTime.Enabled = true;
            }

            else
            {
                // Maybe no file/directory is selected
                // Then Blank out the display of date/time.
                label_CreationTime.Text = "";
                label_Modified.Text = "";
                label_LastAccess.Text = "";
                labelHidden_PathName.Text = "";
                radioGroupBox2_CurrentSelectionTime.Enabled = false;
                radioGroupBox1_SpecifyTime.Checked = true;
            }
            itemSelectionChangedTimer.Stop();
        }
    }
}
