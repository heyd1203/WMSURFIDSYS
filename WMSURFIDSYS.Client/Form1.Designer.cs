namespace WMSURFIDSYS.Client
{
    partial class WMSURFIDSYS
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WMSURFIDSYS));
            this.CAbbv = new System.Windows.Forms.Label();
            this.MName = new System.Windows.Forms.Label();
            this.FName = new System.Windows.Forms.Label();
            this.LName = new System.Windows.Forms.Label();
            this.StudID = new System.Windows.Forms.Label();
            this.image = new System.Windows.Forms.PictureBox();
            this.ShowDate = new System.Windows.Forms.Label();
            this.ShowTime = new System.Windows.Forms.Label();
            this.ShowSY = new System.Windows.Forms.Label();
            this.ShowSem = new System.Windows.Forms.Label();
            this.Error = new System.Windows.Forms.Label();
            this.label = new System.Windows.Forms.Label();
            this.Message = new System.Windows.Forms.Label();
            this.labelCollege = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.image)).BeginInit();
            this.SuspendLayout();
            // 
            // CAbbv
            // 
            this.CAbbv.AutoSize = true;
            this.CAbbv.BackColor = System.Drawing.Color.Transparent;
            this.CAbbv.Font = new System.Drawing.Font("Georgia", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CAbbv.ForeColor = System.Drawing.Color.Gainsboro;
            this.CAbbv.Location = new System.Drawing.Point(300, 281);
            this.CAbbv.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.CAbbv.Name = "CAbbv";
            this.CAbbv.Size = new System.Drawing.Size(145, 41);
            this.CAbbv.TabIndex = 22;
            this.CAbbv.Text = "Course";
            // 
            // MName
            // 
            this.MName.AutoSize = true;
            this.MName.BackColor = System.Drawing.Color.Transparent;
            this.MName.Font = new System.Drawing.Font("Georgia", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MName.ForeColor = System.Drawing.Color.Gainsboro;
            this.MName.Location = new System.Drawing.Point(301, 198);
            this.MName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.MName.Name = "MName";
            this.MName.Size = new System.Drawing.Size(70, 41);
            this.MName.TabIndex = 21;
            this.MName.Text = "MI";
            this.MName.Click += new System.EventHandler(this.MName_Click);
            // 
            // FName
            // 
            this.FName.AutoSize = true;
            this.FName.BackColor = System.Drawing.Color.Gainsboro;
            this.FName.Font = new System.Drawing.Font("Georgia", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FName.ForeColor = System.Drawing.Color.Maroon;
            this.FName.Location = new System.Drawing.Point(300, 146);
            this.FName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.FName.Name = "FName";
            this.FName.Size = new System.Drawing.Size(222, 43);
            this.FName.TabIndex = 20;
            this.FName.Text = "FirstName";
            // 
            // LName
            // 
            this.LName.AutoSize = true;
            this.LName.BackColor = System.Drawing.Color.Gainsboro;
            this.LName.Font = new System.Drawing.Font("Georgia", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LName.ForeColor = System.Drawing.Color.DarkRed;
            this.LName.Location = new System.Drawing.Point(301, 91);
            this.LName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LName.Name = "LName";
            this.LName.Size = new System.Drawing.Size(221, 43);
            this.LName.TabIndex = 19;
            this.LName.Text = "Last Name";
            // 
            // StudID
            // 
            this.StudID.AutoSize = true;
            this.StudID.BackColor = System.Drawing.Color.Transparent;
            this.StudID.Font = new System.Drawing.Font("Georgia", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StudID.ForeColor = System.Drawing.Color.Gainsboro;
            this.StudID.Location = new System.Drawing.Point(43, 343);
            this.StudID.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.StudID.Name = "StudID";
            this.StudID.Size = new System.Drawing.Size(195, 34);
            this.StudID.TabIndex = 18;
            this.StudID.Text = "0123456789";
            this.StudID.Click += new System.EventHandler(this.StudID_Click);
            // 
            // image
            // 
            this.image.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.image.Image = global::WMSURFIDSYS.Client.Properties.Resources.photo_not_available;
            this.image.InitialImage = ((System.Drawing.Image)(resources.GetObject("image.InitialImage")));
            this.image.Location = new System.Drawing.Point(27, 90);
            this.image.Margin = new System.Windows.Forms.Padding(4);
            this.image.Name = "image";
            this.image.Size = new System.Drawing.Size(234, 238);
            this.image.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.image.TabIndex = 13;
            this.image.TabStop = false;
            // 
            // ShowDate
            // 
            this.ShowDate.AutoSize = true;
            this.ShowDate.BackColor = System.Drawing.Color.Transparent;
            this.ShowDate.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ShowDate.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.ShowDate.Location = new System.Drawing.Point(387, 30);
            this.ShowDate.Name = "ShowDate";
            this.ShowDate.Size = new System.Drawing.Size(179, 33);
            this.ShowDate.TabIndex = 24;
            this.ShowDate.Text = "01 Oct 2015";
            this.ShowDate.Click += new System.EventHandler(this.label7_Click);
            // 
            // ShowTime
            // 
            this.ShowTime.AutoSize = true;
            this.ShowTime.BackColor = System.Drawing.Color.Transparent;
            this.ShowTime.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ShowTime.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.ShowTime.Location = new System.Drawing.Point(583, 30);
            this.ShowTime.Name = "ShowTime";
            this.ShowTime.Size = new System.Drawing.Size(143, 33);
            this.ShowTime.TabIndex = 25;
            this.ShowTime.Text = "01:30 PM";
            // 
            // ShowSY
            // 
            this.ShowSY.AutoSize = true;
            this.ShowSY.BackColor = System.Drawing.Color.Transparent;
            this.ShowSY.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ShowSY.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.ShowSY.Location = new System.Drawing.Point(131, 33);
            this.ShowSY.Name = "ShowSY";
            this.ShowSY.Size = new System.Drawing.Size(163, 33);
            this.ShowSY.TabIndex = 27;
            this.ShowSY.Text = "2015-2016";
            // 
            // ShowSem
            // 
            this.ShowSem.AutoSize = true;
            this.ShowSem.BackColor = System.Drawing.Color.Transparent;
            this.ShowSem.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ShowSem.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.ShowSem.Location = new System.Drawing.Point(30, 30);
            this.ShowSem.Name = "ShowSem";
            this.ShowSem.Size = new System.Drawing.Size(76, 33);
            this.ShowSem.TabIndex = 26;
            this.ShowSem.Text = "First";
            // 
            // Error
            // 
            this.Error.AutoSize = true;
            this.Error.Font = new System.Drawing.Font("Tahoma", 30F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Error.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.Error.Location = new System.Drawing.Point(57, 485);
            this.Error.Name = "Error";
            this.Error.Size = new System.Drawing.Size(632, 48);
            this.Error.TabIndex = 28;
            this.Error.Text = "Student not currenlty enrolled";
            // 
            // label
            // 
            this.label.AutoSize = true;
            this.label.BackColor = System.Drawing.Color.Gainsboro;
            this.label.Font = new System.Drawing.Font("Georgia", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label.ForeColor = System.Drawing.Color.Maroon;
            this.label.Location = new System.Drawing.Point(303, 331);
            this.label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size(132, 31);
            this.label.TabIndex = 30;
            this.label.Text = "Message";
            this.label.Click += new System.EventHandler(this.label1_Click);
            // 
            // Message
            // 
            this.Message.AutoSize = true;
            this.Message.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Message.ForeColor = System.Drawing.Color.Gainsboro;
            this.Message.Location = new System.Drawing.Point(303, 362);
            this.Message.Name = "Message";
            this.Message.Size = new System.Drawing.Size(379, 95);
            this.Message.TabIndex = 33;
            this.Message.Text = "                                                                          \r\n\r\n\r\n\r" +
    "\n\r\n";
            this.Message.Click += new System.EventHandler(this.Message_Click);
            // 
            // labelCollege
            // 
            this.labelCollege.AutoSize = true;
            this.labelCollege.BackColor = System.Drawing.Color.Transparent;
            this.labelCollege.Font = new System.Drawing.Font("Georgia", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCollege.ForeColor = System.Drawing.Color.Gainsboro;
            this.labelCollege.Location = new System.Drawing.Point(300, 239);
            this.labelCollege.Name = "labelCollege";
            this.labelCollege.Size = new System.Drawing.Size(149, 41);
            this.labelCollege.TabIndex = 36;
            this.labelCollege.Text = "College";
            this.labelCollege.Click += new System.EventHandler(this.labelCollege_Click);
            // 
            // WMSURFIDSYS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Maroon;
            this.BackgroundImage = global::WMSURFIDSYS.Client.Properties.Resources.WinForm_BG_v2;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(739, 540);
            this.Controls.Add(this.labelCollege);
            this.Controls.Add(this.Message);
            this.Controls.Add(this.label);
            this.Controls.Add(this.Error);
            this.Controls.Add(this.ShowSY);
            this.Controls.Add(this.ShowSem);
            this.Controls.Add(this.ShowTime);
            this.Controls.Add(this.ShowDate);
            this.Controls.Add(this.CAbbv);
            this.Controls.Add(this.MName);
            this.Controls.Add(this.FName);
            this.Controls.Add(this.LName);
            this.Controls.Add(this.StudID);
            this.Controls.Add(this.image);
            this.Name = "WMSURFIDSYS";
            this.Text = "WMSURFIDSYS";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.image)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label CAbbv;
        private System.Windows.Forms.Label MName;
        private System.Windows.Forms.Label FName;
        private System.Windows.Forms.Label LName;
        private System.Windows.Forms.Label StudID;
        private System.Windows.Forms.PictureBox image;
        private System.Windows.Forms.Label ShowDate;
        private System.Windows.Forms.Label ShowTime;
        private System.Windows.Forms.Label ShowSY;
        private System.Windows.Forms.Label ShowSem;
        private System.Windows.Forms.Label Error;
        private System.Windows.Forms.Label label;
        private System.Windows.Forms.Label Message;
        private System.Windows.Forms.Label labelCollege;
    }
}

