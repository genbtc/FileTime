namespace genBTC.FileTime.Models
{
    /// <summary>  Store some the fields that the GUI uses. Small Version. </summary>
    internal class NameDateQuick
    {
        internal string Accessed;
        internal string Created;

        internal string HiddenPathName;
        internal string Modified;
        internal string PathName;

        internal bool Selected;

        public NameDateQuick()
        {
            // Maybe no file/directory is selected
            // Then Blank out the display of date/time.
            PathName = "";
            Created = "";
            Modified = "";
            Accessed = "";
            HiddenPathName = "";
            Selected = false;
        }
    }
}