using System;

namespace genBTC.FileTime.Models
{
    internal struct BoolCMA
    {
        internal bool C;
        internal bool M;
        internal bool A;
    }

    internal struct SkippedHSR
    {
        /// <summary> Count of the number of hidden files skipped </summary>
        internal int H;
        /// <summary> Count of the number of Read-only files skipped </summary>
        internal int R;
        /// <summary> Count of the number of System files skipped </summary>
        internal int S;
    }

    internal struct DisplayCmaTimeData
    {
        internal string PathName;

        internal bool Selected;

        internal string Created;
        internal string Modified;
        internal string Accessed;

        internal string HiddenPathName;
    }

    internal struct guistatus
    {
        internal bool radioGroupBox1SpecifyTime;
        internal bool radioGroupBox2CurrentSelect;

        internal bool rg2rb1Creation;
        internal bool rg2rb2Modified;
        internal bool rg2rb3LastAccess;

        internal bool radioGroupBox3UseTimeFrom;

        internal string Created;
        internal string Modified;
        internal string Accessed;

        internal bool radioButton1_useTimefromFile,
            radioButton2_useTimefromSubdir,
            radioButton1_setfromCreated,
            radioButton2_setfromModified,
            radioButton3_setfromAccessed,
            radioButton4_setfromRandom,
            radioButton1Oldest,
            radioButton2Newest,
            radioButton3Random;

        internal DateTime dateTimePickerDate, dateTimePickerTime;
        internal string labelHiddenPathName;
    }

    /// <summary> Icon in listView image list </summary>
    internal enum ListViewIcon
    {
        /// <summary> File icon in listView image list </summary>
        File = 0,

        /// <summary> Directory icon in listView image list </summary>
        Directory = 1
    };
}