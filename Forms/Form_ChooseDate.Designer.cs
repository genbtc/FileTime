using System.Windows.Forms;

namespace genBTC.FileTime
{
    partial class Form_ChooseDate
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dateTimePicker_CreatedDate = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker_CreatedTime = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker_ModifiedTime = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker_ModifiedDate = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker_AccessedTime = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker_AccessedDate = new System.Windows.Forms.DateTimePicker();
            this.button1_OK = new System.Windows.Forms.Button();
            this.button2_Cancel = new System.Windows.Forms.Button();
            this.checkBox3_Accessed = new System.Windows.Forms.CheckBox();
            this.checkBox2_Modified = new System.Windows.Forms.CheckBox();
            this.checkBox1_Created = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // dateTimePicker_CreatedDate
            // 
            this.dateTimePicker_CreatedDate.Enabled = false;
            this.dateTimePicker_CreatedDate.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTimePicker_CreatedDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePicker_CreatedDate.Location = new System.Drawing.Point(18, 40);
            this.dateTimePicker_CreatedDate.Name = "dateTimePicker_CreatedDate";
            this.dateTimePicker_CreatedDate.Size = new System.Drawing.Size(106, 21);
            this.dateTimePicker_CreatedDate.TabIndex = 0;
            this.dateTimePicker_CreatedDate.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.dateTimePicker_MouseWheel);
            // 
            // dateTimePicker_CreatedTime
            // 
            this.dateTimePicker_CreatedTime.Enabled = false;
            this.dateTimePicker_CreatedTime.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTimePicker_CreatedTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dateTimePicker_CreatedTime.Location = new System.Drawing.Point(139, 40);
            this.dateTimePicker_CreatedTime.Name = "dateTimePicker_CreatedTime";
            this.dateTimePicker_CreatedTime.ShowUpDown = true;
            this.dateTimePicker_CreatedTime.Size = new System.Drawing.Size(105, 21);
            this.dateTimePicker_CreatedTime.TabIndex = 1;
            this.dateTimePicker_CreatedTime.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.dateTimePicker_MouseWheel);
            // 
            // dateTimePicker_ModifiedTime
            // 
            this.dateTimePicker_ModifiedTime.Enabled = false;
            this.dateTimePicker_ModifiedTime.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTimePicker_ModifiedTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dateTimePicker_ModifiedTime.Location = new System.Drawing.Point(139, 90);
            this.dateTimePicker_ModifiedTime.Name = "dateTimePicker_ModifiedTime";
            this.dateTimePicker_ModifiedTime.ShowUpDown = true;
            this.dateTimePicker_ModifiedTime.Size = new System.Drawing.Size(105, 21);
            this.dateTimePicker_ModifiedTime.TabIndex = 3;
            this.dateTimePicker_ModifiedTime.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.dateTimePicker_MouseWheel);
            // 
            // dateTimePicker_ModifiedDate
            // 
            this.dateTimePicker_ModifiedDate.Enabled = false;
            this.dateTimePicker_ModifiedDate.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTimePicker_ModifiedDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePicker_ModifiedDate.Location = new System.Drawing.Point(18, 90);
            this.dateTimePicker_ModifiedDate.Name = "dateTimePicker_ModifiedDate";
            this.dateTimePicker_ModifiedDate.Size = new System.Drawing.Size(106, 21);
            this.dateTimePicker_ModifiedDate.TabIndex = 2;
            this.dateTimePicker_ModifiedDate.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.dateTimePicker_MouseWheel);
            // 
            // dateTimePicker_AccessedTime
            // 
            this.dateTimePicker_AccessedTime.Enabled = false;
            this.dateTimePicker_AccessedTime.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTimePicker_AccessedTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dateTimePicker_AccessedTime.Location = new System.Drawing.Point(139, 140);
            this.dateTimePicker_AccessedTime.Name = "dateTimePicker_AccessedTime";
            this.dateTimePicker_AccessedTime.ShowUpDown = true;
            this.dateTimePicker_AccessedTime.Size = new System.Drawing.Size(105, 21);
            this.dateTimePicker_AccessedTime.TabIndex = 5;
            this.dateTimePicker_AccessedTime.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.dateTimePicker_MouseWheel);
            // 
            // dateTimePicker_AccessedDate
            // 
            this.dateTimePicker_AccessedDate.Enabled = false;
            this.dateTimePicker_AccessedDate.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTimePicker_AccessedDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePicker_AccessedDate.Location = new System.Drawing.Point(18, 140);
            this.dateTimePicker_AccessedDate.Name = "dateTimePicker_AccessedDate";
            this.dateTimePicker_AccessedDate.Size = new System.Drawing.Size(106, 21);
            this.dateTimePicker_AccessedDate.TabIndex = 4;
            this.dateTimePicker_AccessedDate.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.dateTimePicker_MouseWheel);
            // 
            // button1_OK
            // 
            this.button1_OK.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1_OK.Location = new System.Drawing.Point(163, 181);
            this.button1_OK.Name = "button1_OK";
            this.button1_OK.Size = new System.Drawing.Size(81, 29);
            this.button1_OK.TabIndex = 9;
            this.button1_OK.Text = "OK";
            this.button1_OK.UseVisualStyleBackColor = true;
            this.button1_OK.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2_Cancel
            // 
            this.button2_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2_Cancel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2_Cancel.Location = new System.Drawing.Point(60, 184);
            this.button2_Cancel.Name = "button2_Cancel";
            this.button2_Cancel.Size = new System.Drawing.Size(64, 23);
            this.button2_Cancel.TabIndex = 10;
            this.button2_Cancel.Text = "Cancel";
            this.button2_Cancel.UseVisualStyleBackColor = true;
            this.button2_Cancel.Click += new System.EventHandler(this.button2_Cancel_Click);
            // 
            // checkBox3_Accessed
            // 
            this.checkBox3_Accessed.AutoSize = true;
            this.checkBox3_Accessed.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox3_Accessed.Location = new System.Drawing.Point(18, 118);
            this.checkBox3_Accessed.Name = "checkBox3_Accessed";
            this.checkBox3_Accessed.Size = new System.Drawing.Size(75, 17);
            this.checkBox3_Accessed.TabIndex = 11;
            this.checkBox3_Accessed.Text = "Accessed:";
            this.checkBox3_Accessed.UseVisualStyleBackColor = true;
            this.checkBox3_Accessed.CheckedChanged += new System.EventHandler(this.checkBox3_Accessed_CheckedChanged);
            // 
            // checkBox2_Modified
            // 
            this.checkBox2_Modified.AutoSize = true;
            this.checkBox2_Modified.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox2_Modified.Location = new System.Drawing.Point(18, 68);
            this.checkBox2_Modified.Name = "checkBox2_Modified";
            this.checkBox2_Modified.Size = new System.Drawing.Size(70, 17);
            this.checkBox2_Modified.TabIndex = 12;
            this.checkBox2_Modified.Text = "Modified:";
            this.checkBox2_Modified.UseVisualStyleBackColor = true;
            this.checkBox2_Modified.CheckedChanged += new System.EventHandler(this.checkBox2_Modified_CheckedChanged);
            // 
            // checkBox1_Created
            // 
            this.checkBox1_Created.AutoSize = true;
            this.checkBox1_Created.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox1_Created.Location = new System.Drawing.Point(18, 18);
            this.checkBox1_Created.Name = "checkBox1_Created";
            this.checkBox1_Created.Size = new System.Drawing.Size(69, 17);
            this.checkBox1_Created.TabIndex = 13;
            this.checkBox1_Created.Text = "Created:";
            this.checkBox1_Created.UseVisualStyleBackColor = true;
            this.checkBox1_Created.CheckedChanged += new System.EventHandler(this.checkBox1_Created_CheckedChanged);
            // 
            // Form_ChooseDate
            // 
            this.AcceptButton = this.button1_OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button2_Cancel;
            this.ClientSize = new System.Drawing.Size(262, 222);
            this.Controls.Add(this.checkBox1_Created);
            this.Controls.Add(this.checkBox2_Modified);
            this.Controls.Add(this.checkBox3_Accessed);
            this.Controls.Add(this.button2_Cancel);
            this.Controls.Add(this.button1_OK);
            this.Controls.Add(this.dateTimePicker_AccessedTime);
            this.Controls.Add(this.dateTimePicker_AccessedDate);
            this.Controls.Add(this.dateTimePicker_ModifiedTime);
            this.Controls.Add(this.dateTimePicker_ModifiedDate);
            this.Controls.Add(this.dateTimePicker_CreatedTime);
            this.Controls.Add(this.dateTimePicker_CreatedDate);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(268, 250);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(268, 250);
            this.Name = "Form_ChooseDate";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Change Date:";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DateTimePicker dateTimePicker_CreatedDate;
        private DateTimePicker dateTimePicker_CreatedTime;
        private DateTimePicker dateTimePicker_ModifiedTime;
        private DateTimePicker dateTimePicker_ModifiedDate;
        private DateTimePicker dateTimePicker_AccessedTime;
        private DateTimePicker dateTimePicker_AccessedDate;
        private Button button1_OK;
        private Button button2_Cancel;
        private CheckBox checkBox3_Accessed;
        private CheckBox checkBox2_Modified;
        private CheckBox checkBox1_Created;
    }
}