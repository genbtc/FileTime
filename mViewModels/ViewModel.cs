using System;
using System.Collections.Generic;
using System.Text;

namespace genBTC.FileTime
{
    public partial class Form_Main
    {
        struct guistatus
        {
            public bool rg1SpecifyTime;
            public bool rg2CurrentSelectionTime;
            
            public bool rg2rb1Creation;
            public bool rg2rb2Modified;
            public bool rg2rb3LastAccess;

            public bool rg3UseTimeFrom;

            public string Created;
            public string Modified;
            public string Accessed;

            public bool radioButton1_useTimefromFile,
                radioButton2_useTimefromSubdir,
                radioButton1_setfromCreated,
                radioButton2_setfromModified,
                radioButton3_setfromAccessed,
                radioButton4_setfromRandom,
                radioButton1_Oldest,
                radioButton2_Newest,
                radioButton3_Random;

            public DateTime dateTimePicker_Date, dateTimePicker_Time;
        }



        //private guistatus radios;

        private guistatus GetGUIRadioButtonStatusData()
        {
            var radios = new guistatus
            {
                rg1SpecifyTime = radioGroupBox1_SpecifyTime.Checked,
                rg2CurrentSelectionTime = radioGroupBox2_CurrentSelectionTime.Checked,
                rg2rb1Creation = radioButton1_CreationDate.Checked,
                rg2rb2Modified = radioButton2_ModifiedDate.Checked,
                rg2rb3LastAccess = radioButton3_LastAccessDate.Checked,
                rg3UseTimeFrom = radioGroupBox3_UseTimeFrom.Checked,
                Created = label_CreationTime.Text,
                Modified = label_Modified.Text,
                Accessed = label_LastAccess.Text,
                radioButton1_useTimefromFile = radioButton1_useTimefromFile.Checked,
                radioButton2_useTimefromSubdir = radioButton2_useTimefromSubdir.Checked,
                radioButton1_setfromCreated = radioButton1_setfromCreated.Checked,
                radioButton2_setfromModified = radioButton2_setfromModified.Checked,
                radioButton3_setfromAccessed = radioButton3_setfromAccessed.Checked,
                radioButton4_setfromRandom = radioButton4_setfromRandom.Checked,
                radioButton1_Oldest = radioButton1_Oldest.Checked,
                radioButton2_Newest = radioButton2_Newest.Checked,
                radioButton3_Random = radioButton3_Random.Checked,
                dateTimePicker_Date = dateTimePicker_Date.Value,
                dateTimePicker_Time = dateTimePicker_Time.Value,
            };
            return radios;
        }
    }
}
