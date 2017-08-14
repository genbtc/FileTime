using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace genBTC.FileTime.Classes
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
            set { _c = (DateTime?) value; }
        }

        /// <summary>returns an object: DateTime or null </summary>
        public object Modified
        {
            get { return Nullordate(_m); }
            set { _m = (DateTime?) value; }
        }

        /// <summary>returns an object: DateTime or null </summary>
        public object Accessed
        {
            get { return Nullordate(_a); }
            set { _a = (DateTime?) value; }
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

//should this be in a Viewmodel file ?
//should this be repeated ? or can we base this class off the previous one? Combine them? refactor?

    /// <summary>ViewModel version of the NameDateObject(Name + 3 Dates)</summary>
    public class NameDateObjectListViewVm
    {
        private string _a, _c, _m;

        /// <summary> construct with a listviewitem </summary>
        public NameDateObjectListViewVm(ListViewItem thing)
        {
            FileOrDirType = thing.ImageIndex;
            Name = thing.SubItems[0].Text;
            Created = thing.SubItems[1].Text;
            Modified = thing.SubItems[2].Text;
            Accessed = thing.SubItems[3].Text;
        }

        /// <summary> construct with a NameDateObject </summary>
        public NameDateObjectListViewVm(NameDateObject thing)
        {
            FileOrDirType = thing.FileOrDirType;
            Name = thing.Name;
            Created = thing.Created.ToString();
            Modified = thing.Modified.ToString();
            Accessed = thing.Accessed.ToString();
        }

        /// <summary> Empty default constructor (initializer) </summary>
        public NameDateObjectListViewVm()
        {
            FileOrDirType = 0;
        }

        /// <summary>int FileOrDirType: 0 (File) or 1 (Dir)</summary>
        public int FileOrDirType { get; set; }

        /// <summary>string Name: filepathname</summary>
        public string Name { get; set; }

        /// <summary>returns a string of DateTime.ToString or null </summary>
        public string Created
        {
            get { return _c; }
            set { _c = Nullordatestring(value); }
        }

        /// <summary>returns a string of DateTime.ToString or null </summary>
        public string Modified
        {
            get { return _m; }
            set { _m = Nullordatestring(value); }
        }

        /// <summary>returns a string of DateTime.ToString or null </summary>
        public string Accessed
        {
            get { return _a; }
            set { _a = Nullordatestring(value); }
        }

        private static string Nullordatestring(string value)
        {
            return value == "N/A" ? null : value;
        }

        private static string DatestringorNa(string value)
        {
            return value ?? "N/A";
        }

        /// <summary>
        /// Compares two of these objects's DATES, Combines any dates that are null or not null and returns only one
        /// Looks super stupid, but makes the "Try to combine DUPlicate filenames" actually work. Help ?
        /// </summary>
        public bool Compare(NameDateObjectListViewVm thing1, NameDateObjectListViewVm thing2)
        {
            if (thing1.Name != thing2.Name) return false;
            //Created 
            if ((thing1.Created == null) && (thing2.Created != null))
                //if one is null and one isnt, use the one that isnt.
                Created = thing2.Created;
            if ((thing2.Created == null) && (thing1.Created != null))
                //if one is null and one isnt, use the one that isnt.
                Created = thing1.Created;
            if ((thing1.Created == thing2.Created) && (thing1.Created != null))
                //if they're both the same and not null, shrink it down into one.
                Created = thing1.Created;
            //Modified
            if ((thing1.Modified == null) && (thing2.Modified != null))
                Modified = thing2.Modified;
            if ((thing2.Modified == null) && (thing1.Modified != null))
                Modified = thing1.Modified;
            if ((thing1.Modified == thing2.Modified) && (thing1.Modified != null))
                Modified = thing1.Modified;
            //Accessed
            if ((thing1.Accessed == null) && (thing2.Accessed != null))
                Accessed = thing2.Accessed;
            if ((thing2.Accessed == null) && (thing1.Accessed != null))
                Accessed = thing1.Accessed;
            if ((thing1.Accessed == thing2.Accessed) && (thing1.Accessed != null))
                Accessed = thing1.Accessed;

            FileOrDirType = thing1.FileOrDirType;
            Name = thing1.Name;
            return true;
        }

        //Helper function to assosciate the model to the viewmodel. I think this gets run a lot (once per item)
        /// <summary> Converts this class into a ListViewItem. </summary>
        /// <returns>  Returns a listviewitem</returns>
        public ListViewItem Converter()
        {
            var theitem = new ListViewItem(Name, FileOrDirType);
            theitem.SubItems.Add(DatestringorNa(Created));
            theitem.SubItems.Add(DatestringorNa(Modified));
            theitem.SubItems.Add(DatestringorNa(Accessed));
            return theitem;
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

    /// <summary> Explorer-like Sort, for strings </summary>
    public class ExplorerComparerstring : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            return Form_Main.StrCmpLogicalW(x, y);
        }

    }

    /// <summary> Explorer-like Sort, for use by listview.Sorter </summary>
    public class ListViewItemExplorerLikeComparer : IComparer
    {
        public int Compare(object x, object y)
        {
            return Form_Main.StrCmpLogicalW(((ListViewItem) x).Text, ((ListViewItem) y).Text);
        }

    }

    //That native function is pretty important maybe it should be imported on load. its going to happen regardless.
}