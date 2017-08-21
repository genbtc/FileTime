using System.IO;
using System.Collections.Generic;


namespace genBTC.FileTime.Classes
{
    // This implementation defines a very simple comparison  
    // between two FileInfo objects. It only compares the name  
    // of the files being compared and their length in bytes.  
    internal class FileComparer : IEqualityComparer<FileInfo>
    {
        #region IEqualityComparer<FileInfo> Members

        public bool Equals(FileInfo f1, FileInfo f2)
        {
            return (f1.Name == f2.Name &&
                    f1.Length == f2.Length);
                    //length means file size.
                    //TODO: Make the comparison take relative path into account. Not as crucial as doing it on dirs.
        }

        // Return a hash that reflects the comparison criteria. According to the   
        // rules for IEqualityComparer<T>, if Equals is true, then the hash codes must  
        // also be equal. Because equality as defined here is a simple value equality, not  
        // reference identity, it is possible that two or more objects will produce the same  
        // hash code.  
        public int GetHashCode(FileInfo fi)
        {
            string s = $"{fi.Name}{fi.Length}";
            return s.GetHashCode();
        }

        #endregion
    }

    // This implementation defines a very simple comparison  
    // between two FileInfo objects. It only compares the name  
    // of the files being compared and their length in bytes.  
    internal class DirComparer : IEqualityComparer<DirectoryInfo>
    {
        #region IEqualityComparer<DirectoryInfo> Members

        public bool Equals(DirectoryInfo f1, DirectoryInfo f2)
        {
            return (f1.Name == f2.Name);
            // && f1.Parent == f2.Parent);
            //Cant use the parent, To compare the parent, we'd need to:
            //TODO: Make a thing that eliminates the relative part of the path out, (of both, each) . Important.
            //since that will be most likely fix what Parents breaks on currently.
        }

        // Return a hash that reflects the comparison criteria. According to the   
        // rules for IEqualityComparer<T>, if Equals is true, then the hash codes must  
        // also be equal. Because equality as defined here is a simple value equality, not  
        // reference identity, it is possible that two or more objects will produce the same  
        // hash code.  
        public int GetHashCode(DirectoryInfo fi)
        {
            string s = $"{fi.Name}{fi.Parent}";
            return s.GetHashCode();
        }

        #endregion
    }
}
