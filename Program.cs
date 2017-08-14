/* #
 * # This file is the Program launcher. It creates a new window of Form_Main
 * # go to Form_Main.cs to see the program there.
 */

using System;
using System.Windows.Forms;

namespace genBTC.FileTime
{
    internal static class Program
    {
        /// <summary> The main entry point for the application.</summary>
        [MTAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form_Main());
        }
    }
}