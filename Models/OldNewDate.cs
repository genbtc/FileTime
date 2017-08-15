using System;
using System.Collections.Generic;

namespace genBTC.FileTime.Models
{
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
