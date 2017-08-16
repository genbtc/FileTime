namespace genBTC.FileTime.Models
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

    struct DisplayCmaTimeData
    {
        public string PathName;

        public bool Selected;

        public string Created;
        public string Modified;
        public string Accessed;

        public string HiddenPathName;
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