using System;
using System.Collections.Generic;
using System.Windows.Forms;
using genBTC.FileTime.Classes;

namespace genBTC.FileTime.Models
{
    /// <summary>Class object with 5 properties to refer to a file/dir</summary>
    internal class NameDateObj
    {
        //private time fields
        private DateTime? _a, _c, _m;

        /// <summary> constructor with a listviewitem </summary>
        public NameDateObj(ListViewItem thing)
        {
            FileOrDirType = thing.ImageIndex;
            Name = thing.SubItems[0].Text;
            Created = Listviewtodateornull(thing.SubItems[1].Text);
            Modified = Listviewtodateornull(thing.SubItems[2].Text);
            Accessed = Listviewtodateornull(thing.SubItems[3].Text);
        }

        /// <summary> default Constructor (Initializer)</summary>
        public NameDateObj()
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
        /// Explorer-like Sort, that orders two NameDateObj's by their name property
        /// </summary>
        public class ExplorerLikeSort : IComparer<NameDateObj>
        {
            public int Compare(NameDateObj obj1, NameDateObj obj2)
            {
                return SharedHelper.StrCmpLogicalW(obj1.Name, obj2.Name);
            }
        }
    }
}