using System;
using System.Collections.Generic;

namespace genBTC.FileTime.Models
{
    public class DataModel
    {
        public List<string> contentsDirList;
        public List<string> contentsFileList;
        public List<string> filextlist;
        /// <summary> Pass a list of files that had the read-only attribute fixed, so Form2 can display it </summary>
        public List<string> FilesReadOnlytoFix;
        /// <summary> List of Class to be passed to Form 2 for confirmation of files</summary>
        public List<NameDateObject> FilestoConfirmList;

        public SkippedHSR Skips;

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
        }
    }
}