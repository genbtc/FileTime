using System.Windows.Forms;
using genBTC.FileTime.Models;

namespace genBTC.FileTime.mViewModels
{
    /// <summary>ViewModel version of the NameDateObj(Name + 3 Dates)</summary>
    internal class NameDateObjListViewVMdl
    {
        private string _a, _c, _m;

        /// <summary> construct with a listviewitem </summary>
        public NameDateObjListViewVMdl(ListViewItem thing)
        {
            FileOrDirType = thing.ImageIndex;
            Name = thing.SubItems[0].Text;
            Created = thing.SubItems[1].Text;
            Modified = thing.SubItems[2].Text;
            Accessed = thing.SubItems[3].Text;
        }

        /// <summary> construct with a NameDateObj </summary>
        public NameDateObjListViewVMdl(NameDateObj thing)
        {
            FileOrDirType = thing.FileOrDirType;
            Name = thing.Name;
            Created = thing.Created.ToString();
            Modified = thing.Modified.ToString();
            Accessed = thing.Accessed.ToString();
        }

        /// <summary> Empty default constructor (initializer) </summary>
        public NameDateObjListViewVMdl()
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
        public bool Compare(NameDateObjListViewVMdl thing1, NameDateObjListViewVMdl thing2)
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
}