/*
 * ## This file is the 2nd Window the confirmation window, "step 2" of the program
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using genBTC.FileTime.Classes;
using genBTC.FileTime.Classes.Native;
using genBTC.FileTime.Models;
using genBTC.FileTime.mViewModels;

namespace genBTC.FileTime
{
    /// <summary>
    /// Form 2, confirms the action and shows filenames and dates
    /// </summary>
    public partial class Form_Confirm : Form
    {
        #region Form_Confirm declarations, startup Code

        //Stub to access Form_Main from this form
        private readonly List<string> _filextlist = new List<string>();
        private readonly Form_Main _formmain;

        /// <summary> list of listview items's checkbox state bool values </summary>
        private List<bool> _checklist;

        /// <summary> Count of the number of files/directories that have errors setting the date/time </summary>
        private int _itemsErrorsCount;

        /// <summary> Count of the number of files/directories that have been set </summary>
        private int _itemsSetCount;

        /// <summary> the fixreadonly active files count (checked in between fix and unfix) </summary>
        public int active = 0;

        /// <summary> Form 2 creation and initialization code. </summary>
        /// <param name="f1">so we can modify publics in the main form</param>
        public Form_Confirm(Form_Main f1)
        {
            //Required for Windows Form Designer support
            InitializeComponent();

            //Initialize count variables
            _itemsErrorsCount = 0;
            _itemsSetCount = 0;

            _formmain = f1;

            MakeListView();
        }

        #endregion

        #region Main logic and actions, Buttons, Behind-Code and Logic Functions

        /// <summary>
        /// Show a message box of any files that were cleared of their read-only tag.
        /// </summary>
        public void FixReadonlyResults()
        {
            if ((_formmain.FilesReadOnlytoFix.Count > 0) && (active == 0))
            {
                string listoffixedreadonlyfiles = "";
                foreach (string file in _formmain.FilesReadOnlytoFix)
                    listoffixedreadonlyfiles += file + "\n";
                DialogResult dr =
                    MessageBox.Show(listoffixedreadonlyfiles + "\nRead-Only must be un-set to change date. Continue?",
                        "Read-Only Files: ", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    foreach (string file in _formmain.FilesReadOnlytoFix)
                    {
                        FileAttributes fileattribs = File.GetAttributes(file);
                        File.SetAttributes(file, SharedHelper.RemoveAttributes(fileattribs, FileAttributes.ReadOnly));
                    }
                    DialogResult dr2 = MessageBox.Show("Turn read-only back on when the confirm window is closed?",
                        "Make Files Read-Only again?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dr2 == DialogResult.No)
                        _formmain.FilesReadOnlytoFix.Clear();
                    else
                        active = _formmain.FilesReadOnlytoFix.Count;
                }
                else
                    _formmain.FilesReadOnlytoFix.Clear();
            }
            else if (active > 0)
            {
                ResetReadOnly();
                active = 0;
            }
        }

        /// <summary> 
        /// turn the NameDateObject lists from Form_Main into the listview on Form_Confirm </summary>
        /// use the results as the new data model.
        public void MakeListView()
        {
            FixReadonlyResults();
            _checklist = new List<bool>();
            listView1_Confirm.BeginUpdate();
            listView1_Confirm.ItemChecked -= listView1_Confirm_ItemChecked; //event
            //this is wrapped in the event handlers -=, += because extremely modifying the collection with these handlers would cause lots of lag
            foreach (NameDateObject newobject in _formmain.FilestoConfirmList)
            {
                var theitem = new ListViewItem(newobject.Name, newobject.FileOrDirType);
                theitem.SubItems.Add(newobject.Created.ToString());
                theitem.SubItems.Add(newobject.Modified.ToString());
                theitem.SubItems.Add(newobject.Accessed.ToString());
                theitem.Checked = true;
                if (newobject.FileOrDirType == 0)
                {
                    SharedHelper.CurrExten = Path.GetExtension(newobject.Name);
                    if (_filextlist.FindLastIndex(SharedHelper.FindCurExt) == -1)
                    {
                        _filextlist.Add(SharedHelper.CurrExten);
                        //call ExtractIcon to get the filesystem icon of the filename
                        imageList_Files.Images.Add(SharedHelper.CurrExten, NativeExtractIcon.GetIcon(newobject.Name, true));
                    }
                    theitem.ImageKey = SharedHelper.CurrExten;
                }
                listView1_Confirm.Items.Add(theitem);
            }
            listView1_Confirm.EndUpdate();
            listView1_Confirm.ItemChecked += listView1_Confirm_ItemChecked; //event
            UpdateStatusBar();
        }

        /// <summary> Show current number of files on the status bar line, and store all checkboxes each time </summary>
        private void UpdateStatusBar()
        {
            toolStripStatusLabel1.Text = "Total Items: " + listView1_Confirm.Items.Count;
            //most likely this is being called because the number of items have changed, so make a new list of stored checkboxes.
            StoreCheckboxes();
        }

        /// <summary> Adds the read-only attribute back after it was removed </summary>
        public void ResetReadOnly()
        {
            if (_formmain.FilesReadOnlytoFix.Count <= 0) 
                return;
            foreach (string file in _formmain.FilesReadOnlytoFix)
                File.SetAttributes(file, File.GetAttributes(file) | FileAttributes.ReadOnly);
            // the | is needed for boolean algebra of attribute flags (it means: add readonly)
            _formmain.FilesReadOnlytoFix.Clear();
        }

        /// <summary>
        ///Clear the main form's fileconfirm list if form2 is closed.
        ///If the user does not want to clear the list, they can leave the form_Confirm open
        /// and access form_Main multiple times to incrementally add to the form_Confirm list.
        /// </summary>
        private void Form_Confirm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _formmain.FilestoConfirmList.Clear();
            ResetReadOnly();
        }

        /// <summary>
        /// Check the radiobuttons, then check the filesystem's existing dates and
        /// returns true if files should be modified.
        /// </summary>
        private bool DoConditionalAgeCheck(DateTime newdate, string pathName, int cmAtype)
        {
            try
            {
                var olddate = new DateTime();
                if (cmAtype == 1)
                    olddate = File.GetCreationTime(pathName);
                if (cmAtype == 2)
                    olddate = File.GetLastWriteTime(pathName);
                if (cmAtype == 3)
                    olddate = File.GetLastAccessTime(pathName);
                if (radioButton1Newer.Checked)
                    return (newdate > olddate);
                if (radioButton2Older.Checked)
                    return (newdate < olddate);
                return true;
            }
            catch (UnauthorizedAccessException) { return false; }
        }


        /// <summary> Function which touches the filesystem to set the dates </summary>
        private static bool SetTime(int cma, string path, DateTime date)
        {
            try
            {
                switch (cma)
                {
                        //Directory works on both files and dirs.
                    case 1:
                        Directory.SetCreationTime(path, date);
                        break;
                    case 2:
                        Directory.SetLastWriteTime(path, date);
                        break;
                    case 3:
                        Directory.SetLastAccessTime(path, date);
                        break;
                    default:
                        return false;
                }
            }
            catch (UnauthorizedAccessException) { return false; }
            catch (FileNotFoundException) { return false; }
            return true;
        }

        /// <summary>
        /// Confirm Button - Actually finally set the files datetimes, if it matches conditions.
        /// Shows a progress bar.
        /// </summary>
        private void button1_Confirm_Click(object sender, EventArgs e)
        {
            toolStripProgressBar1.Maximum = listView1_Confirm.Items.Count;
            toolStripProgressBar1.Value = 0;
            _itemsErrorsCount = 0;
            _itemsSetCount = 0;
            foreach (ListViewItem thing in listView1_Confirm.Items)
            {
                int attributesetcount = 0;
                //only perform a change if it was checkboxed.
                if (thing.Checked)
                {
                    //for boxes, 1, 2 and 3
                    for (int i = 1; i <= 3; i++)
                    {
                        if (thing.SubItems[i].Text == "N/A") 
                            continue;   //skip if it was N/A
                        DateTime dateToUse = DateTime.Parse(thing.SubItems[i].Text);
                        if (DoConditionalAgeCheck(dateToUse, thing.Text, i))
                        {
                            var success = SetTime(i, thing.Text, dateToUse);
                            if (success)
                                ++attributesetcount;
                            else
                            {
                                ++_itemsErrorsCount;
                                thing.BackColor = Color.FromName("Red");
                                MessageBox.Show(
                                    "Error in setting date/time:  " + dateToUse + "   on '" + thing.Text + "': \r\n\r\n",
                                    "Date/Time Error " + _itemsErrorsCount, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);                                    
                            }
                        }
                    }
                    if (attributesetcount > 0)
                    {
                        ++_itemsSetCount;
                        thing.BackColor = Color.FromName("Lime");
                    }
                }
                toolStripProgressBar1.PerformStep();
            }

            string message = _itemsSetCount + " file(s)/directorie(s) have had their date/time set";
            if (_itemsErrorsCount > 0)
                message += "\r\n\r\n There were " + _itemsErrorsCount + " error(s)";
            MessageBox.Show(message, "Info Log", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        //Column Header ComboBox Event
        private void ThreeComboBox_SelectedChange(object sender, EventArgs e)
        {
            var currentbox = (ComboBox) sender;
            int boxnumber = 0;
            //current columnboxnumber that was clicked
            if (sender.Equals(ComboBox1)) //created column combobox
                boxnumber = 1;
            if (sender.Equals(ComboBox2)) //modified column combobox
                boxnumber = 2;
            if (sender.Equals(ComboBox3)) //accessed column combobox
                boxnumber = 3;
            //two dimensional array. outer = current column, inner = column numbers to copy
            int[,] cols = {{2, 3}, {1, 3}, {1, 2}};
            listView1_Confirm.BeginUpdate();
            foreach (ListViewItem thing in listView1_Confirm.Items)
            {
                if (!thing.Checked) 
                    continue; //if not checked, skip to next
                if (currentbox.SelectedIndex == 1) //copy cols[x][0] to this
                {
                    if (thing.SubItems[boxnumber].Text == "N/A")
                        thing.SubItems[boxnumber].Text = thing.SubItems[cols[boxnumber - 1, 0]].Text;
                }
                if (currentbox.SelectedIndex == 2) //copy cols[x][1] to this
                {
                    if (thing.SubItems[boxnumber].Text == "N/A")
                        thing.SubItems[boxnumber].Text = thing.SubItems[cols[boxnumber - 1, 1]].Text;
                }
                if (currentbox.SelectedIndex == 3) //unset
                    thing.SubItems[boxnumber].Text = "N/A";
            }
            listView1_Confirm.EndUpdate();
            currentbox.SelectedIndex = 0;
        }

        #endregion

        #region the menubar (Edit... MenuStrip)

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView1_Confirm.BeginUpdate();
            NativeSelect.SelectAllItems(listView1_Confirm);
            listView1_Confirm.EndUpdate();
        }

        private void deSelectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView1_Confirm.BeginUpdate();
            NativeSelect.DeselectAllItems(listView1_Confirm);
            listView1_Confirm.EndUpdate();
        }

        private void invertSelectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int lastselectedindex = 0;
            listView1_Confirm.BeginUpdate();
            foreach (ListViewItem item in listView1_Confirm.Items)
            {
                item.Selected = !item.Selected;
                if (item.Selected)
                    lastselectedindex = item.Index;
            }
            if (listView1_Confirm.FocusedItem != null) 
                listView1_Confirm.FocusedItem.Focused = false;
            listView1_Confirm.Items[lastselectedindex].Focused = true;
            listView1_Confirm.EndUpdate();
        }

        private void removeNAOnlysToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView1_Confirm.BeginUpdate();
            listView1_Confirm.SelectedIndices.Clear();
            foreach (ListViewItem thing in listView1_Confirm.Items)
            {
                if ((thing.SubItems[1].Text == "N/A") && 
                    (thing.SubItems[2].Text == "N/A") &&
                    (thing.SubItems[3].Text == "N/A"))
                    listView1_Confirm.Items.RemoveAt(thing.Index);
            }
            listView1_Confirm.EndUpdate();
            UpdateStatusBar();
        }

        /// <summary>
        /// Combine all the adjacent items which have the same name, and matching blank/nonblank or identical date attributes
        /// </summary>
        private void condenseMultipleLinesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //first sort the list.
            listView1_Confirm.BeginUpdate();
            //because it has to function on adjacent items, sort alphabetically first.
            listView1_Confirm.ListViewItemSorter = new ListViewItemExplorerLikeComparer();
            if (listView1_Confirm.Items.Count >= 2)
            {
                for (int i = 0; i < listView1_Confirm.Items.Count - 1; i++)
                {
                    var thing1 = new NameDateObjectListViewVm(listView1_Confirm.Items[i]);
                    var thing2 = new NameDateObjectListViewVm(listView1_Confirm.Items[i + 1]);
                    var newthing = new NameDateObjectListViewVm();
                    if (newthing.Compare(thing1, thing2))
                    {
                        listView1_Confirm.Items.RemoveAt(i);
                        listView1_Confirm.Items.RemoveAt(i);
                        listView1_Confirm.Items.Add(newthing.Converter());
                        i--;
                    }
                }
            }
            listView1_Confirm.ListViewItemSorter = null;
            listView1_Confirm.EndUpdate();
            UpdateStatusBar();
        }

        #endregion

        #region CustomListView RightClick ContextMenu

        /// <summary> Change Date... </summary>
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var theitem = listView1_Confirm.SelectedItems[0];
            var dateform = new Form_ChooseDate(theitem);
            dateform.ShowDialog();
            var dateToUse = new NameDateObject();
            if (dateform.DialogResult == DialogResult.OK)
                dateToUse = dateform.Datechosen;
            Cursor.Current = Cursors.WaitCursor;
            listView1_Confirm.BeginUpdate();
            foreach (ListViewItem thing in listView1_Confirm.SelectedItems)
            {
                if (dateToUse.Created.ToString() != "N/A")
                    thing.SubItems[1].Text = dateToUse.Created.ToString();
                if (dateToUse.Modified.ToString() != "N/A")
                    thing.SubItems[2].Text = dateToUse.Modified.ToString();
                if (dateToUse.Accessed.ToString() != "N/A")
                    thing.SubItems[3].Text = dateToUse.Accessed.ToString();
            }
            listView1_Confirm.EndUpdate();
            Cursor.Current = Cursors.Default;
        }

        /// <summary> Remove 1=Created,2=Modified,3=Accessed... </summary>
        private void toolStripMenu_ThreeRemoves(object sender, EventArgs e)
        {
            int menunumber = 0;
            if (sender.Equals(toolStripMenuItem2))
                menunumber = 1;
            if (sender.Equals(toolStripMenuItem3))
                menunumber = 2;
            if (sender.Equals(toolStripMenuItem4))
                menunumber = 3;
            Cursor.Current = Cursors.WaitCursor;
            listView1_Confirm.BeginUpdate();
            foreach (ListViewItem thing in listView1_Confirm.SelectedItems)
                thing.SubItems[menunumber].Text = "N/A";
            listView1_Confirm.EndUpdate();
            Cursor.Current = Cursors.Default;
        }

        /// <summary> Open Explorer here... </summary>
        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            string path = "";
            foreach (ListViewItem thing in listView1_Confirm.SelectedItems)
                path = thing.ImageIndex == 1 ? thing.SubItems[0].Text : Path.GetDirectoryName(thing.SubItems[0].Text);
            //old
/*              if (thing.ImageIndex == 1)  //if its a directory, 
                    path = thing.SubItems[0].Text;
                else                        //if its a file, get the directory of it
                    path = Path.GetDirectoryName(thing.SubItems[0].Text);
*/
            Process.Start("explorer.exe", @path);
        }

        /// <summary> Delete Item... </summary>
        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            //easy but slow. 3000 removals per second with beginupdate/endupdate (850 without)
            //while (listView1_Confirm.SelectedIndices.Count > 0)
            //{
            //    listView1_Confirm.Items.RemoveAt(listView1_Confirm.SelectedIndices[0]);
            //}
            //fast (13,500 per second) - but confusing code, and slightly more memory.
            //Make an int array out of the selected indices, then...
            var selectarray = new int[listView1_Confirm.SelectedIndices.Count];
            listView1_Confirm.SelectedIndices.CopyTo(selectarray, 0);
            // turn that into a list.
            var selectlist = new List<int>(selectarray);
            //Make a listviewitem array out of the entire listview, then...
            var allitemsArray = new ListViewItem[listView1_Confirm.Items.Count];
            listView1_Confirm.Items.CopyTo(allitemsArray, 0);
            // turn that into a list.
            var allitemsList = new List<ListViewItem>(allitemsArray);
            int i = 0; //iterator
            while (selectlist.Count > 0)
            {
                allitemsList.RemoveAt(selectlist[0] - i);
                    //remove the first index counteracted by the number of iterations
                selectlist.RemoveAt(0); //remove the first index (always)
                i++; //keep track of number of iterations
            }
            //Start updating without repainting.
            Cursor.Current = Cursors.WaitCursor;
            listView1_Confirm.BeginUpdate();
            listView1_Confirm.Items.Clear(); //Clear the listview items
            listView1_Confirm.Items.AddRange(allitemsList.ToArray()); //addrange the modified list.ToArray back
            //Finished Updating, allow repainting again.
            listView1_Confirm.EndUpdate();
            Cursor.Current = Cursors.Default;
            UpdateStatusBar();
        }

        #endregion

        #region Checkbox handler/protector and Event Handler Functions

        /// <summary> restore all the checkboxes at once (not used?) </summary>
        private void RestoreCheckboxes()
        {
            if (_checklist.Count <= 0) 
                return;
            int index = 0;            
            foreach (ListViewItem thing in listView1_Confirm.Items)
            {
                thing.Checked = _checklist[index];
                index++;
            }
        }

        /// <summary> restore only one checkbox.  </summary>
        private void RestoreCheckboxes(int index)
        {
            if (_checklist.Count <= 0)
                return; //maybe not needed.
            listView1_Confirm.Items[index].Checked = _checklist[index];
        }

        /// <summary>
        /// Clear list if any, and store entire new list of checkboxes. (could not direct copy the internal array (see comments))
        /// </summary>
        private void StoreCheckboxes()
        {
            _checklist.Clear();
            foreach (ListViewItem thing in listView1_Confirm.Items)
                _checklist.Add(thing.Checked); //create the checkbox list for the whole listview
            //int[] checkedarray = new int[listView1_Confirm.CheckedIndices.Count];
            //((ICollection<int>)listView1_Confirm.CheckedIndices).CopyTo(checkedarray, 0);
            //failed because CopyTo does not exeist on CheckedIndices like it does on SelectedIndices
        }

        /// <summary>
        /// Change stored value of one checkbox in the checklist storage array.
        ///  Called multiple times from listView1_Confirm_ItemChecked, e is the item being changed.
        /// </summary>
        private void StoreCheckboxes(ItemCheckedEventArgs e)
        {
            //if checkbox list is not empty, then it must exist fully. (this is never empty now).
            // And we can just modify single items.
            _checklist[e.Item.Index] = e.Item.Checked; //update the list with the item that was changed.
        }

        private void listView1_Confirm_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (listView1_Confirm.SelectedItems.Count > 25)
            {
                listView1_Confirm.ItemChecked -= listView1_Confirm_ItemChecked;
                //if so many items are selected, remove this handler forever..... (not ideal)
                return;
            }

            if (listView1_Confirm.ItemSelectionChangedTimer.Enabled)
            {
                int i = e.Item.Index;
                listView1_Confirm.ItemChecked -= listView1_Confirm_ItemChecked;
                RestoreCheckboxes(i);
                listView1_Confirm.ItemChecked += listView1_Confirm_ItemChecked;
                //q++;
            }
            else //store checkboxes incrementally
                StoreCheckboxes(e);
        }
        
        /// <summary>change focus back to the listview when the menu is clicked (so it doesnt go gray)</summary>
        private void menuStrip1_Click(object sender, EventArgs e)
        {
            listView1_Confirm.Focus();
        }

        /// <summary> respond to the delete key as if the delete context menu was clicked </summary>
        private void listView1_Confirm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 46)
            {
                toolStripMenuItem6_Click(sender, e);
            }
        }

        #endregion
    }
}