/*
 * ## This file is for the Preferences popup window
 */

using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using genBTC.FileTime.Properties;

namespace genBTC.FileTime.Forms
{
    /// <summary> Preferences window class </summary>
    public partial class Form_Preferences : Form
    {
        private readonly string _currentdir;

        /// <summary>Show preferences window</summary>
        public Form_Preferences(string currentdirParam)
        {
            InitializeComponent();
            this._currentdir = currentdirParam;
        }

        //OK button saves stuff
        private void button1_Click(object sender, EventArgs e)
        {
            if (checkBox6_StartupDir.Checked)
            {
                if (!Directory.Exists(textBox6_startupdir.Text))
                {
                    MessageBox.Show("Invalid Startup Directory. Please fix or delete.");
                    return;
                }
            }
            Settings.Default.Save();
            Close();
        }

        //cancel button Not-saves stuff
        private void button2_Cancel_Click(object sender, EventArgs e)
        {
            Settings.Default.Reload();
            Close();
        }

        //use dir from last window
        private void button_UseCurrentDir_Click(object sender, EventArgs e)
        {
            textBox6_startupdir.Text = _currentdir;
        }

        private void button_Browse_Click(object sender, EventArgs e)
        {
            //Code was needed for when running as MultiThreaded App. [MTAThread]
            string path = textBox6_startupdir.Text;
            var t = new Thread(() =>
            {
                var openFile = new FolderBrowserDialog
                {
                    ShowNewFolderButton = false,
                    SelectedPath = path,    //This sketches me out.
                    Description = "Select the folder you want to set as the Default Start-Up directory:"
                };
                //use current path as dialog path
                //openFile.RootFolder = System.Environment.SpecialFolder.MyComputer;
                //openFile.ShowNewFolderButton = true;
                if (openFile.ShowDialog() == DialogResult.Cancel)
                    return;
                //path is also the variable that returns what was selected
                path = openFile.SelectedPath;
            });
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            t.Join();
            if (path == textBox6_startupdir.Text)
                return; //nothing was changed
            if (!path.EndsWith("\\"))
                path += "\\";
            textBox6_startupdir.Text = path;
        }
    }
}