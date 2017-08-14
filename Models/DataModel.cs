using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace genBTC.FileTime.Models
{
    /// <summary>Class object with 5 properties to refer to a file/dir</summary>
    public class NameDateObject
    {
        //private time fields
        private DateTime? _a, _c, _m;

        /// <summary> constructor with a listviewitem </summary>
        public NameDateObject(ListViewItem thing)
        {
            FileOrDirType = thing.ImageIndex;
            Name = thing.SubItems[0].Text;
            Created = Listviewtodateornull(thing.SubItems[1].Text);
            Modified = Listviewtodateornull(thing.SubItems[2].Text);
            Accessed = Listviewtodateornull(thing.SubItems[3].Text);
        }

        /// <summary> default Constructor (Initializer)</summary>
        public NameDateObject()
        {
            Name = "";
            FileOrDirType = 0;
        }

        /// <summary>quick flag to know whether we're a file or a dir
        /// int FileOrDirType: 0 (File) or 1 (Dir)
        /// </summary>
        public int FileOrDirType { get; set; }

        /// <summary>string Name: the file name</summary>
        public string Name { get; set; }

        //3 Objects for the public Time fields (C,M,A)
        /// <summary>returns an object: DateTime or null </summary>
        public object Created
        {
            get { return Nullordate(_c); }
            set { _c = (DateTime?)value; }
        }

        /// <summary>returns an object: DateTime or null </summary>
        public object Modified
        {
            get { return Nullordate(_m); }
            set { _m = (DateTime?)value; }
        }

        /// <summary>returns an object: DateTime or null </summary>
        public object Accessed
        {
            get { return Nullordate(_a); }
            set { _a = (DateTime?)value; }
        }

        //helper function to check date for null - this helps the GUI display "N/A"
        private static object Nullordate(DateTime? x)
        {
            if (x == null)
                return "N/A";
            return x;
        }

        //helper function to convert back to null - this helps the GUI
        private static DateTime? Listviewtodateornull(string x)
        {
            if (x == "N/A")
                return null;
            return DateTime.Parse(x);
        }

        /// <summary>
        /// Explorer-like Sort, that orders two NameDateObject's by their name property
        /// </summary>
        public class ExplorerLikeSort : IComparer<NameDateObject>
        {
            public int Compare(NameDateObject obj1, NameDateObject obj2)
            {
                return Form_Main.StrCmpLogicalW(obj1.Name, obj2.Name);
            }

        }
    }

    /// <summary>
    /// Class: given a list of DateTime?s, get oldest and newest date and the index of each
    /// This is for the "Source time from" option - it looks at an entire subfolder of files.
    /// </summary>
    public class OldNewDate
    {
        /// <summary> total indexes </summary>
        public int Index;

        /// <summary> maxdate </summary>
        public DateTime? MaxDate = null;

        /// <summary> maxindex are indexes of the maxdate </summary>
        public int MaxIndex;

        /// <summary> mindate </summary>
        public DateTime? MinDate = null;

        /// <summary> minindex are indexes of the mindate </summary>
        public int MinIndex;

        /// <summary>
        /// Takes a list of datetimes(which came from a list of files) and
        /// calculates the newest and oldest dates and stores the indexes too.
        /// Constructor for the class. Also does all the work.
        /// </summary>
        /// <param name="timelist">a List of DateTime?s</param>
        public OldNewDate(List<DateTime?> timelist)
        {
            foreach (DateTime? dt in timelist)
            {
                if ((MinDate == null) || (dt < MinDate.Value))
                {
                    MinDate = dt;
                    MinIndex = Index;
                }

                if ((MaxDate == null) || (dt > MaxDate.Value))
                {
                    MaxDate = dt;
                    MaxIndex = Index;
                }
                Index++;
            }
        }
    }

}
