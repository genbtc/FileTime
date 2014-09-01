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