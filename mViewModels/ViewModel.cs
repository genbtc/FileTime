using System;

namespace genBTC.FileTime
{
    public partial class Form_Main
    {
        struct guistatus
        {
            public bool radioGroupBox1SpecifyTime;
            public bool radioGroupBox2CurrentSelect;
            
            public bool rg2rb1Creation;
            public bool rg2rb2Modified;
            public bool rg2rb3LastAccess;

            public bool radioGroupBox3UseTimeFrom;

            public string Created;
            public string Modified;
            public string Accessed;

            public bool radioButton1_useTimefromFile,
                radioButton2_useTimefromSubdir,
                radioButton1_setfromCreated,
                radioButton2_setfromModified,
                radioButton3_setfromAccessed,
                radioButton4_setfromRandom,
                radioButton1Oldest,
                radioButton2Newest,
                radioButton3Random;

            public DateTime dateTimePickerDate, dateTimePickerTime;
            public string labelHiddenPathName;
        }


        private guistatus GetGUIRadioButtonStatusData()
        {
            var radios = new guistatus
            {
                radioGroupBox1SpecifyTime = radioGroupBox1_SpecifyTime.Checked,
                radioGroupBox2CurrentSelect = radioGroupBox2_CurrentSelectionTime.Checked,
                rg2rb1Creation = radioButton1_CreationDate.Checked,
                rg2rb2Modified = radioButton2_ModifiedDate.Checked,
                rg2rb3LastAccess = radioButton3_AccessedDate.Checked,
                radioGroupBox3UseTimeFrom = radioGroupBox3_UseTimeFrom.Checked,
                Created = label_CreationTime.Text,
                Modified = label_ModificationTime.Text,
                Accessed = label_AccessedTime.Text,
                radioButton1_useTimefromFile = radioButton1_useTimefromFile.Checked,
                radioButton2_useTimefromSubdir = radioButton2_useTimefromSubdir.Checked,
                radioButton1_setfromCreated = radioButton1_setfromCreation.Checked,
                radioButton2_setfromModified = radioButton2_setfromModified.Checked,
                radioButton3_setfromAccessed = radioButton3_setfromAccessed.Checked,
                radioButton4_setfromRandom = radioButton4_setfromRandom.Checked,
                radioButton1Oldest = radioButton1_Oldest.Checked,
                radioButton2Newest = radioButton2_Newest.Checked,
                radioButton3Random = radioButton3_Random.Checked,
                dateTimePickerDate = dateTimePicker_Date.Value,
                dateTimePickerTime = dateTimePicker_Time.Value,
                labelHiddenPathName = labelHidden_PathName.Text
            };

            return radios;
        }
    }
}
