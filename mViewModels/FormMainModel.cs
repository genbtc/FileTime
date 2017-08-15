using System;
using System.Collections.Generic;
using System.IO;
using genBTC.FileTime.Models;
using genBTC.FileTime.mViewModels;

namespace genBTC.FileTime
{
    public class DataModel
    {
        public readonly List<string> contentsDirList = new List<string>();
        public readonly List<string> contentsFileList = new List<string>();
        public readonly List<string> filextlist = new List<string>();

        public void ClearOnError()
        {
            contentsDirList.Clear();
            contentsFileList.Clear();
        }

    }

    public partial class Form_Main
    {

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
        /// Overloaded. Make a NameDateObject out of 1 filename; writes A SINGLE time to all 3 date properties.
        /// </summary>
        private NameDateObject Makedateobject(string filePath, DateTime fileTime, bool isDirectory)
        {
            var currentobject = new NameDateObject { Name = filePath, FileOrDirType = Bool2Int(isDirectory) };

            // Set the Creation date/time if selected
            if (checkBox_CreationDateTime.Checked)
                currentobject.Created = fileTime;
            // Set the Modified date/time is selected					
            if (checkBox_ModifiedDateTime.Checked)
                currentobject.Modified = fileTime;
            // Set the last access time if selected
            if (checkBox_AccessedDateTime.Checked)
                currentobject.Accessed = fileTime;
            return currentobject;
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
