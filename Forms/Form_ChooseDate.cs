using System;
using System.Windows.Forms;
using genBTC.FileTime.Models;

namespace genBTC.FileTime.Forms
{
    public partial class Form_ChooseDate : Form
    {
        /// <summary> the date object that will be accessed by the other form </summary>
        public NameDateObject Datechosen = new NameDateObject();

        /// <summary>  a small form, used only for choosing a date. </summary>
        public Form_ChooseDate(ListViewItem theitem)
        {
            InitializeComponent();
            PopulateWith(theitem);
        }

        private void PopulateWith(ListViewItem theitem)
        {
            if (theitem.SubItems[1].Text != "N/A")
            {
                dateTimePicker_CreatedDate.Value = DateTime.Parse(theitem.SubItems[1].Text);
                dateTimePicker_CreatedTime.Value = DateTime.Parse(theitem.SubItems[1].Text);
            }
            if (theitem.SubItems[2].Text != "N/A")
            {
                dateTimePicker_ModifiedDate.Value = DateTime.Parse(theitem.SubItems[2].Text);
                dateTimePicker_ModifiedTime.Value = DateTime.Parse(theitem.SubItems[2].Text);
            }
            if (theitem.SubItems[3].Text != "N/A")
            {
                dateTimePicker_AccessedDate.Value = DateTime.Parse(theitem.SubItems[3].Text);
                dateTimePicker_AccessedTime.Value = DateTime.Parse(theitem.SubItems[3].Text);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (checkBox1_Created.Checked)
            {
                Datechosen.Created = DateTime.Parse(dateTimePicker_CreatedDate.Value.Date.ToString("d") +
                                                    " " + dateTimePicker_CreatedTime.Value.Hour + ":" +
                                                    dateTimePicker_CreatedTime.Value.Minute + ":" +
                                                    dateTimePicker_CreatedTime.Value.Second);
            }
            if (checkBox2_Modified.Checked)
            {
                Datechosen.Modified = DateTime.Parse(dateTimePicker_ModifiedDate.Value.Date.ToString("d") +
                                                     " " + dateTimePicker_ModifiedTime.Value.Hour + ":" +
                                                     dateTimePicker_ModifiedTime.Value.Minute + ":" +
                                                     dateTimePicker_ModifiedTime.Value.Second);
            }
            if (checkBox3_Accessed.Checked)
            {
                Datechosen.Accessed = DateTime.Parse(dateTimePicker_AccessedDate.Value.Date.ToString("d") +
                                                     " " + dateTimePicker_AccessedTime.Value.Hour + ":" +
                                                     dateTimePicker_AccessedTime.Value.Minute + ":" +
                                                     dateTimePicker_AccessedTime.Value.Second);
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void button2_Cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void checkBox1_Created_CheckedChanged(object sender, EventArgs e)
        {
            dateTimePicker_CreatedDate.Enabled = checkBox1_Created.Checked;
            dateTimePicker_CreatedTime.Enabled = checkBox1_Created.Checked;
        }

        private void checkBox2_Modified_CheckedChanged(object sender, EventArgs e)
        {
            dateTimePicker_ModifiedDate.Enabled = checkBox2_Modified.Checked;
            dateTimePicker_ModifiedTime.Enabled = checkBox2_Modified.Checked;
        }

        private void checkBox3_Accessed_CheckedChanged(object sender, EventArgs e)
        {
            dateTimePicker_AccessedDate.Enabled = checkBox3_Accessed.Checked;
            dateTimePicker_AccessedTime.Enabled = checkBox3_Accessed.Checked;
        }

        private void dateTimePicker_MouseWheel(object sender, MouseEventArgs e)
        {
            SendKeys.Send(e.Delta > 0 ? "{UP}" : "{DOWN}");
        }
    }
}