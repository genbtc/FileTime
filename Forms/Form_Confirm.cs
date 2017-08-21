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
using genBTC.FileTime.mViewModels;
using genBTC.FileTime.Models;

namespace genBTC.FileTime.Forms
{
    /// <summary>
    /// Form 2, confirms the action and shows filenames and dates
    /// </summary>
    public partial class Form_Confirm : Form
    {
        #region Form_Confirm declarations, startup Code

        //Stub to access Form_Main from this form
        private readonly List<string> _filextlist = new List<string>();
        private DataModel dataModel;

        /// <summary> list of listview items's checkbox state bool values </summary>
        private List<bool> _checklist;

        /// <summary> Count of the number of files/directories that have errors setting the date/time </summary>
        private int _itemsErrorsCount;

        /// <summary> Count of the number of files/directories that have been set </summary>
        private int _itemsSetCount;

        /// <summary> Form 2 creation and initialization code. </summary>
        /// <param name="f1DataModel"></param>
        /// 
        internal Form_Confirm(DataModel f1DataModel)
        {
            //Required for Windows Form Designer support
            InitializeComponent();

            //Initialize count variables
            _itemsErrorsCount = 0;
            _itemsSetCount = 0;

            dataModel = f1DataModel;

            MakeListView();
        }

        #endregion Form_Confirm declarations, startup Code

        #region Main logic and actions, Buttons, Behind-Code and Logic Functions


        /// <summary>
        /// turn the NameDateObj lists from Form_Main into the listview on Form_Confirm </summary>
        /// use the results as the new data model.
        public void MakeListView()
        {
            dataModel.FixReadonlyResults();
            _checklist = new List<bool>();
            listView1Confirm.BeginUpdate();
            listView1Confirm.ItemChecked -= listView1Confirm_ItemChecked; //event
            //this is wrapped in the event handlers -=, += because extremely modifying the collection with these handlers would cause lots of lag
            foreach (NameDateObj newobject in dataModel.FilestoConfirmList)
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
                listView1Confirm.Items.Add(theitem);
            }
            listView1Confirm.EndUpdate();
            listView1Confirm.ItemChecked += listView1Confirm_ItemChecked; //event
            UpdateStatusBar();
        }

        /// <summary> Show current number of files on the status bar line, and store all checkboxes each time 
        /// Most likely this is being called because the number of items have changed, so make a new list of stored checkboxes.  </summary>
        private void UpdateStatusBar()
        {
            toolStripStatusLabel1.Text = "Total Items: " + listView1Confirm.Items.Count;
            listView_StoreAllCheckboxes();  //TODO: seems costly.
        }

        /// <summary> Cleanup.
        ///Clear the main form's fileconfirm list if form2 is closed.
        ///If the user does not want to clear the list, they can leave the form_Confirm open
        /// and access form_Main multiple times to incrementally add to the form_Confirm list.
        /// </summary>
        private void Form_Confirm_FormClosing(object sender, FormClosingEventArgs e)
        {
            dataModel.FilestoConfirmList.Clear();
            dataModel.ResetReadOnly();
        }

        /// <summary>
        /// Check the radiobuttons, then check the filesystem's existing dates and
        /// returns true if files should be modified.
        /// </summary>
        /// TODO: Tuple radioButton1Newer radioButton2Older
        private static bool DoConditionalAgeCheck(int cmaType, string pathName, DateTime newDate, bool useNewer, bool useOlder)
        {
            try
            {
                var olddate = new DateTime();
                if (cmaType == 1)
                    olddate = File.GetCreationTime(pathName);
                if (cmaType == 2)
                    olddate = File.GetLastWriteTime(pathName);
                if (cmaType == 3)
                    olddate = File.GetLastAccessTime(pathName);
                if (useNewer)
                    return (newDate > olddate);
                if (useOlder)
                    return (newDate < olddate);
                return true;
            }
            //ArgumentException, ArgumentNullException, PathTooLongException, NotSupportedException
            catch (UnauthorizedAccessException) { return false; }
        }

        /// <summary> Function which touches the filesystem to set the dates </summary>
        /// Notes: Directory works on both files and dirs.
        public static bool SetTime(int cmaType, string pathName, DateTime newDate)
        {
            try
            {
                if (cmaType == 1)
                    Directory.SetCreationTime(pathName, newDate);
                else if (cmaType == 2)
                    Directory.SetLastWriteTime(pathName, newDate);
                else if (cmaType == 3)
                    Directory.SetLastAccessTime(pathName, newDate);
                return true;
            }
            catch (FileNotFoundException) { return false; }
            catch (UnauthorizedAccessException) { return false; }
            //ArgumentException, ArgumentNullException, PathTooLongException, NotSupportedException, PlatformNotSupportedException, ArgumentOutOfRangeException
        }

        /// <summary>
        /// Confirm Button - Actually finally set the files datetimes, if it matches conditions.
        /// Shows a progress bar.
        /// </summary>
        private void button1_Confirm_Click(object sender, EventArgs e)
        {
            toolStripProgressBar1.Maximum = listView1Confirm.Items.Count;
            toolStripProgressBar1.Value = 0;
            _itemsErrorsCount = 0;
            _itemsSetCount = 0;
            foreach (ListViewItem thing in listView1Confirm.Items)
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
                        var dateToUse = DateTime.Parse(thing.SubItems[i].Text);
                        //Skip files by date if we chose "Skip older" Or "Skip newer"
                        if (!DoConditionalAgeCheck(i, thing.Text, dateToUse, radioButton1Newer.Checked, radioButton2Older.Checked)) continue;
                        //Set the Time and Check the result, then increment the count
                        if (SetTime(i, thing.Text, dateToUse))
                            ++attributesetcount;
                        else
                        {
                            ++_itemsErrorsCount;
                            thing.BackColor = Color.FromName("Red");
                            MessageBox.Show(
                                $@"Error in setting date/time:  {dateToUse}   on '{thing.Text}': ",
                                $@"Date/Time Error #{_itemsErrorsCount}", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
            MessageBox.Show(message, @"Operation Complete!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        //Column Header ComboBox Event
        private void ThreeComboBox_SelectedChange(object sender, EventArgs e)
        {
            var currentbox = (ComboBox)sender;
            int boxnumber = 0;
            //current columnboxnumber that was clicked
            if (sender.Equals(comboBox1_Created)) //created column combobox
                boxnumber = 1;
            if (sender.Equals(comboBox2_Modified)) //modified column combobox
                boxnumber = 2;
            if (sender.Equals(comboBox3_Accessed)) //accessed column combobox
                boxnumber = 3;
            //two dimensional array. outer = current column, inner = column numbers to copy
            int[,] cols = { { 2, 3 }, { 1, 3 }, { 1, 2 } };
            listView1Confirm.BeginUpdate();
            foreach (ListViewItem thing in listView1Confirm.Items)
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
            listView1Confirm.EndUpdate();
            currentbox.SelectedIndex = 0;
        }

        #endregion Main logic and actions, Buttons, Behind-Code and Logic Functions

        #region the menubar (Edit... MenuStrip) (has ListView controls too)

        private void listView_selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NativeSelect.SelectAllItems(listView1Confirm);
        }

        private void listView_deSelectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NativeSelect.DeselectAllItems(listView1Confirm);
        }

        private void listView_invertSelectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NativeSelect.InvertSelection(listView1Confirm);
        }

        private void listView_removeNAOnlysToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView1Confirm.BeginUpdate();
            //
            listView1Confirm.SelectedIndices.Clear();
            foreach (ListViewItem thing in listView1Confirm.Items)
            {
                if ((thing.SubItems[1].Text == "N/A") &&
                    (thing.SubItems[2].Text == "N/A") &&
                    (thing.SubItems[3].Text == "N/A"))
                    listView1Confirm.Items.RemoveAt(thing.Index);
            }
            //
            listView1Confirm.EndUpdate();
            UpdateStatusBar();
        }

        /// <summary>
        /// Combine all the adjacent items which have the same name, and matching blank/nonblank or identical date attributes
        /// </summary>
        private void listView_condenseMultipleLinesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //first sort the list.
            listView1Confirm.BeginUpdate();
            //because it has to function on adjacent items, sort alphabetically first.
            listView1Confirm.ListViewItemSorter = new ExpLikeCmpHelperforListView();
            if (listView1Confirm.Items.Count >= 2)
            {
                for (var i = 0; i < listView1Confirm.Items.Count - 1; i++)
                {
                    var thing1 = new NameDateObjListViewVMdl(listView1Confirm.Items[i]);
                    var thing2 = new NameDateObjListViewVMdl(listView1Confirm.Items[i + 1]);
                    var newthing = new NameDateObjListViewVMdl();
                    //TODO: Pretty sure you are not supposed to do the following:
                    if (newthing.Compare(thing1, thing2))
                    {
                        listView1Confirm.Items.RemoveAt(i);
                        listView1Confirm.Items.RemoveAt(i);
                        listView1Confirm.Items.Add(newthing.Converter());
                        i--;
                    }
                }
            }
            listView1Confirm.ListViewItemSorter = null;
            listView1Confirm.EndUpdate();
            UpdateStatusBar();
        }

        private void listView_SetDatesOfAllSelecteds(NameDateObj dateToUse)
        {
            foreach (ListViewItem thing in listView1Confirm.SelectedItems)
            {
                if (dateToUse.Created.ToString() != "N/A")
                    thing.SubItems[1].Text = dateToUse.Created.ToString();
                if (dateToUse.Modified.ToString() != "N/A")
                    thing.SubItems[2].Text = dateToUse.Modified.ToString();
                if (dateToUse.Accessed.ToString() != "N/A")
                    thing.SubItems[3].Text = dateToUse.Accessed.ToString();
            }
        }

        private void listView_RemoveDatesOfAllSelecteds(int menunumber)
        {
            foreach (ListViewItem thing in listView1Confirm.SelectedItems)
                thing.SubItems[menunumber].Text = "N/A";
        }

        #endregion the menubar (Edit... MenuStrip)

        #region CustomListView RightClick ContextMenu
        /// <summary> Change Date... </summary>
        private void toolStripMenuItem1_ChangeDate_Click(object sender, EventArgs e)
        {
            var theitem = listView1Confirm.SelectedItems[0];
            var dateform = new Form_ChooseDate(theitem);
            dateform.ShowDialog();
            var dateToUse = new NameDateObj();
            if (dateform.DialogResult == DialogResult.OK)
                dateToUse = dateform.Datechosen;
            Cursor.Current = Cursors.WaitCursor;
            listView1Confirm.BeginUpdate();
            listView_SetDatesOfAllSelecteds(dateToUse);
            listView1Confirm.EndUpdate();
            Cursor.Current = Cursors.Default;
        }

        /// <summary> Remove 1=Created,2=Modified,3=Accessed... </summary>
        private void toolStripMenuItem234_ThreeRemoves_Click(object sender, EventArgs e)
        {
            int menunumber = 0;
            if (sender.Equals(toolStripMenuItem2_UnsetCreated))
                menunumber = 1;
            if (sender.Equals(toolStripMenuItem3_UnsetModified))
                menunumber = 2;
            if (sender.Equals(toolStripMenuItem4_UnsetAccessed))
                menunumber = 3;
            Cursor.Current = Cursors.WaitCursor;
            listView1Confirm.BeginUpdate();
            listView_RemoveDatesOfAllSelecteds(menunumber);
            listView1Confirm.EndUpdate();
            Cursor.Current = Cursors.Default;
        }

        private void toolStripMenuItem5_unSetALL_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            listView1Confirm.BeginUpdate();
            foreach (ListViewItem thing in listView1Confirm.SelectedItems)
            {
                thing.SubItems[1].Text = "N/A";
                thing.SubItems[2].Text = "N/A";
                thing.SubItems[3].Text = "N/A";
            }
            listView1Confirm.EndUpdate();
            Cursor.Current = Cursors.Default;
        }

        /// <summary> Remove Item from list... </summary>
        //TODO: make static
        private void toolStripMenuItem6_RemoveItem_Click(object sender, EventArgs e)
        {
            //easy but slow. 3000 removals per second with beginupdate/endupdate (850 without)
            //while (listView1Confirm.SelectedIndices.Count > 0)
            //{
            //    listView1Confirm.Items.RemoveAt(listView1Confirm.SelectedIndices[0]);
            //}
            //fast (13,500 per second) - but confusing code, and slightly more memory.
            //Make an int array out of the selected indices, then...
            var selectarray = new int[listView1Confirm.SelectedIndices.Count];
            listView1Confirm.SelectedIndices.CopyTo(selectarray, 0);
            // turn that into a list.
            var selectlist = new List<int>(selectarray);
            //Make a listviewitem array out of the entire listview, then...
            var allitemsArray = new ListViewItem[listView1Confirm.Items.Count];
            listView1Confirm.Items.CopyTo(allitemsArray, 0);
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
            listView1Confirm.BeginUpdate();
            listView1Confirm.Items.Clear(); //Clear the listview items
            listView1Confirm.Items.AddRange(allitemsList.ToArray()); //addrange the modified list.ToArray back
            //Finished Updating, allow repainting again.
            listView1Confirm.EndUpdate();
            Cursor.Current = Cursors.Default;
            UpdateStatusBar();
        }


        /// <summary> Open Explorer to selected path. Supports multi-selection up to 10 items </summary>
        private void toolStripMenuItem7_OpenExplorer_Click(object sender, EventArgs e)
        {
            if (listView1Confirm.SelectedItems.Count > 10)
            {
                MessageBox.Show(@"You selected more than 10 items and tried to open them all in explorer windows. Not a good idea.", @"PEBKAC Error ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            foreach (ListViewItem thing in listView1Confirm.SelectedItems)
            {
                string path = thing.ImageIndex == 1 ? thing.SubItems[0].Text : Path.GetDirectoryName(thing.SubItems[0].Text);
                Process.Start("explorer.exe", path);
            }
        }

        #endregion CustomListView RightClick ContextMenu

        #region Checkbox handler/protector and Event Handler Functions

        /// <summary> restore all the checkboxes at once (not used?) </summary>
        private void listView_RestoreAllCheckboxes()
        {
            listView1Confirm.ItemChecked -= listView1Confirm_ItemChecked;
            if (_checklist.Count <= 0)  //should never happen.
                return;
            var index = 0;
            foreach (ListViewItem thing in listView1Confirm.Items)
            {
                thing.Checked = _checklist[index];
                index++;
            }
            listView1Confirm.ItemChecked += listView1Confirm_ItemChecked;
        }

        /// <summary> restore only one checkbox.  </summary>
        private void listView_RestoreCheckbox(int index)
        {
            listView1Confirm.ItemChecked -= listView1Confirm_ItemChecked;
            if (_checklist.Count <= 0)  //should never happen.
                return;
            listView1Confirm.Items[index].Checked = _checklist[index];
            listView1Confirm.ItemChecked += listView1Confirm_ItemChecked;
        }

        /// <summary>
        /// Clear list if any, and store entire new list of checkboxes. 
        /// TODO: could not direct copy the internal array (see comments) - revisit.
        /// </summary>
        private void listView_StoreAllCheckboxes()
        {
            _checklist.Clear();
            //create the checkbox list for the whole listview
            foreach (ListViewItem thing in listView1Confirm.Items)
                _checklist.Add(thing.Checked); 
            //var checkedarray = new int[listView1Confirm.CheckedIndices.Count];
            //listView1Confirm.CheckedIndices.CopyTo(checkedarray, 0);
            //NOTE: this failed because CopyTo does not exeist on CheckedIndices like it does on SelectedIndices
        }

        /// <summary>
        /// Change stored value of one checkbox in the checklist storage array.
        ///  Called multiple times from listView1Confirm_ItemChecked, e is the item being changed.
        /// </summary>
        private void listView_StoreCheckbox(int itemIndex, bool itemChecked)
        {
            // if (_checklist.Count <= 0) return; //maybe not needed?
            //TODO: double-check your assumptions, error handling?
            //if checkbox list is not empty, then it must exist fully. (this is never empty now).
            // And we can just modify single items. We want this to be fast.
            // We are trying to eliminate code from this because it runs a lot and has a performance impact
            _checklist[itemIndex] = itemChecked; //update the list with the item that was changed.
        }

        /// <summary>
        /// Trick to store/cache item selection checkboxes.
        /// Should save time, and improve lag on large lists with large (>25) selections.
        /// TODO: revisit this, and invent a better trick for >25 items. 
        /// TODO: benchmark the store and restore, optimize.
        /// </summary>
        private void listView1Confirm_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            //if so many items are selected, dont use this trick.
            if (listView1Confirm.SelectedItems.Count > 25)
            {
                listView1Confirm.ItemChecked -= listView1Confirm_ItemChecked;
                return;
            }
            //Explanation of whats happening here:
            //The listview has a 100ms timeout because its a moron control, and was behaving badly.
            //If we have triggered a timeout already, restore the old checkbox.
            if (!listView1Confirm.ItemSelectionChangedTimer.Enabled)
            {
                listView_StoreCheckbox(e.Item.Index, e.Item.Checked);
            } else
            //or if everythings OK, just store the checkbox that was passed by parameter.
            //TODO: Maybe invert the if ?
            {
                listView_RestoreCheckbox(e.Item.Index);
            }
        }

        /// <summary>Trick to change focus back to the listview when the menu is clicked (so it doesnt go gray)</summary>
        private void menuStrip1_Click(object sender, EventArgs e)
        {
            listView1Confirm.Focus();
        }

        /// <summary> respond to the delete key, and call the delete context menu </summary>
        private void listView1Confirm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 46)
            {
                toolStripMenuItem6_RemoveItem_Click(sender, e);
            }
        }

        #endregion Checkbox handler/protector and Event Handler Functions


    }
}