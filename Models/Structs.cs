/* All Internal Structs or Enums */

using System;

namespace genBTC.FileTime.Models
{
    internal struct BoolCMA
    {
        internal bool A;
        internal bool C;
        internal bool M;
    }

    internal struct SkippedHSR
    {
        /// <summary> Count of the number of hidden files skipped </summary>
        internal int H;

        /// <summary> Count of the number of Read-only files skipped </summary>
        internal int R;

        /// <summary> Count of the number of System files skipped </summary>
        internal int S;

        /// <summary> Default Constructor   </summary>
        public SkippedHSR(int h, int r, int s)
        {
            H = h;
            R = r;
            S = s;
            Reset();
        }

        /// <summary>  Reset the skip count of Hidden system and readonly files  </summary>
        public void Reset()
        {
            H = 0;
            R = 0;
            S = 0;
        }
    }

    /// <summary>  Store some the fields that the GUI uses. Small Version. </summary>
    internal struct NameDateStruct
    {
        internal string Accessed;
        internal string Created;

        internal string HiddenPathName;
        internal string Modified;
        internal string PathName;

        internal bool Selected;
    }

    /// <summary>  Store all(most) the fields that the GUI uses, for populating the viewmodel. </summary>
    internal struct guistatus
    {
        internal bool checkboxRecurse;
        internal bool checkboxShouldFiles;
        internal bool radioGroupBox1SpecifyTime;
        //specify time from date/time spinner picker.
        internal DateTime dateTimePickerDate, dateTimePickerTime;

        internal bool radioGroupBox2CurrentSelect;
        //use time from currently selected.
        internal string Created;
        internal string Modified;
        internal string Accessed;
        internal string PathName;
        private string HiddenPathName;

        internal bool radioGroupBox3UseTimeFrom;
        //use time from C, M, A text fields
        internal bool rg2rb1Creation;
        internal bool rg2rb2Modified;
        internal bool rg2rb3LastAccess;

        internal bool radioButton1_useTimefromFile;
        internal bool radioButton2_useTimefromSubdir;
        //use time from: (subdir/subfile)
        internal bool radioButton1Oldest;
        internal bool radioButton2Newest;
        internal bool radioButton3Random;
        internal bool radioButton1_setfromCreated,
                      radioButton2_setfromModified,
                      radioButton3_setfromAccessed,
                      radioButton4_setfromRandom;
    }

    /// <summary> Icon in listView image list </summary>
    internal enum ListViewIcon
    {
        /// <summary> File icon in listView image list </summary>
        File = 0,

        /// <summary> Directory icon in listView image list </summary>
        Directory = 1
    }
}
