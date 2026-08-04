namespace BU50_API
{
    partial class FRM_Config
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
            this.txtapi = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtdb = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtcomcode = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.Sql = new System.Windows.Forms.RadioButton();
            this.but_windows = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_password = new System.Windows.Forms.TextBox();
            this.txt_user = new System.Windows.Forms.TextBox();
            this.txt_DataBase = new System.Windows.Forms.TextBox();
            this.txt_Server = new System.Windows.Forms.TextBox();
            this.btnsave = new System.Windows.Forms.Button();
            this.save = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.check = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.txtServerNameExternal = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtpass2 = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtapi
            // 
            this.txtapi.Location = new System.Drawing.Point(662, 112);
            this.txtapi.Name = "txtapi";
            this.txtapi.Size = new System.Drawing.Size(233, 20);
            this.txtapi.TabIndex = 98;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.Color.Black;
            this.label8.Location = new System.Drawing.Point(590, 112);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(63, 13);
            this.label8.TabIndex = 97;
            this.label8.Text = "API Toking:";
            // 
            // txtdb
            // 
            this.txtdb.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::BU50_API.Properties.Settings.Default, "DB", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txtdb.Location = new System.Drawing.Point(662, 86);
            this.txtdb.Name = "txtdb";
            this.txtdb.Size = new System.Drawing.Size(233, 20);
            this.txtdb.TabIndex = 96;
            this.txtdb.Text = global::BU50_API.Properties.Settings.Default.DB;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.Black;
            this.label7.Location = new System.Drawing.Point(597, 89);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(56, 13);
            this.label7.TabIndex = 95;
            this.label7.Text = "DB Name:";
            // 
            // txtcomcode
            // 
            this.txtcomcode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtcomcode.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::BU50_API.Properties.Settings.Default, "CompCode", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txtcomcode.Location = new System.Drawing.Point(181, 91);
            this.txtcomcode.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtcomcode.Name = "txtcomcode";
            this.txtcomcode.Size = new System.Drawing.Size(236, 20);
            this.txtcomcode.TabIndex = 92;
            this.txtcomcode.Text = global::BU50_API.Properties.Settings.Default.CompCode;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(86, 91);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 13);
            this.label3.TabIndex = 91;
            this.label3.Text = "Company Code:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(113, 236);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 13);
            this.label4.TabIndex = 88;
            this.label4.Text = "Password";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(110, 195);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(57, 13);
            this.label5.TabIndex = 87;
            this.label5.Text = "UserName";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Sql);
            this.groupBox1.Controls.Add(this.but_windows);
            this.groupBox1.Location = new System.Drawing.Point(95, 117);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Size = new System.Drawing.Size(434, 66);
            this.groupBox1.TabIndex = 86;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "LoginType";
            // 
            // Sql
            // 
            this.Sql.AutoSize = true;
            this.Sql.ForeColor = System.Drawing.Color.Black;
            this.Sql.Location = new System.Drawing.Point(208, 25);
            this.Sql.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Sql.Name = "Sql";
            this.Sql.Size = new System.Drawing.Size(151, 17);
            this.Sql.TabIndex = 1;
            this.Sql.TabStop = true;
            this.Sql.Text = "SQL Server Authentication";
            this.Sql.UseVisualStyleBackColor = true;
            // 
            // but_windows
            // 
            this.but_windows.AutoSize = true;
            this.but_windows.ForeColor = System.Drawing.Color.Black;
            this.but_windows.Location = new System.Drawing.Point(16, 24);
            this.but_windows.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.but_windows.Name = "but_windows";
            this.but_windows.Size = new System.Drawing.Size(140, 17);
            this.but_windows.TabIndex = 0;
            this.but_windows.TabStop = true;
            this.but_windows.Text = "Windows Authentication";
            this.but_windows.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(90, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 13);
            this.label2.TabIndex = 83;
            this.label2.Text = "DataBase Name";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(105, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 13);
            this.label1.TabIndex = 82;
            this.label1.Text = "Server Name";
            // 
            // txt_password
            // 
            this.txt_password.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_password.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::BU50_API.Properties.Settings.Default, "Password", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txt_password.Location = new System.Drawing.Point(184, 236);
            this.txt_password.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txt_password.Name = "txt_password";
            this.txt_password.Size = new System.Drawing.Size(233, 20);
            this.txt_password.TabIndex = 90;
            this.txt_password.Text = global::BU50_API.Properties.Settings.Default.Password;
            // 
            // txt_user
            // 
            this.txt_user.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_user.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::BU50_API.Properties.Settings.Default, "UserId", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txt_user.Location = new System.Drawing.Point(184, 193);
            this.txt_user.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txt_user.Name = "txt_user";
            this.txt_user.Size = new System.Drawing.Size(233, 20);
            this.txt_user.TabIndex = 89;
            this.txt_user.Text = global::BU50_API.Properties.Settings.Default.UserId;
            // 
            // txt_DataBase
            // 
            this.txt_DataBase.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_DataBase.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::BU50_API.Properties.Settings.Default, "DataBase", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txt_DataBase.Location = new System.Drawing.Point(184, 63);
            this.txt_DataBase.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txt_DataBase.Name = "txt_DataBase";
            this.txt_DataBase.Size = new System.Drawing.Size(233, 20);
            this.txt_DataBase.TabIndex = 85;
            this.txt_DataBase.Text = global::BU50_API.Properties.Settings.Default.DataBase;
            // 
            // txt_Server
            // 
            this.txt_Server.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_Server.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::BU50_API.Properties.Settings.Default, "ServerName", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txt_Server.Location = new System.Drawing.Point(184, 36);
            this.txt_Server.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txt_Server.Name = "txt_Server";
            this.txt_Server.Size = new System.Drawing.Size(233, 20);
            this.txt_Server.TabIndex = 84;
            this.txt_Server.Text = global::BU50_API.Properties.Settings.Default.ServerName;
            // 
            // btnsave
            // 
            this.btnsave.Location = new System.Drawing.Point(114, 278);
            this.btnsave.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnsave.Name = "btnsave";
            this.btnsave.Size = new System.Drawing.Size(64, 19);
            this.btnsave.TabIndex = 99;
            this.btnsave.Text = "save";
            this.btnsave.UseVisualStyleBackColor = true;
            this.btnsave.Click += new System.EventHandler(this.btnsave_Click);
            // 
            // save
            // 
            this.save.Location = new System.Drawing.Point(662, 136);
            this.save.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.save.Name = "save";
            this.save.Size = new System.Drawing.Size(64, 19);
            this.save.TabIndex = 100;
            this.save.Text = "save";
            this.save.UseVisualStyleBackColor = true;
            this.save.Click += new System.EventHandler(this.save_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(184, 278);
            this.button2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(64, 19);
            this.button2.TabIndex = 101;
            this.button2.Text = "check";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // check
            // 
            this.check.Location = new System.Drawing.Point(731, 136);
            this.check.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.check.Name = "check";
            this.check.Size = new System.Drawing.Size(64, 19);
            this.check.TabIndex = 102;
            this.check.Text = "check";
            this.check.UseVisualStyleBackColor = true;
            this.check.Click += new System.EventHandler(this.check_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.Location = new System.Drawing.Point(529, 33);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(116, 13);
            this.label6.TabIndex = 103;
            this.label6.Text = "Server Name Expernal:";
            // 
            // txtServerNameExternal
            // 
            this.txtServerNameExternal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtServerNameExternal.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::BU50_API.Properties.Settings.Default, "ServerNameExternal", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txtServerNameExternal.Location = new System.Drawing.Point(662, 32);
            this.txtServerNameExternal.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtServerNameExternal.Name = "txtServerNameExternal";
            this.txtServerNameExternal.Size = new System.Drawing.Size(233, 20);
            this.txtServerNameExternal.TabIndex = 104;
            this.txtServerNameExternal.Text = global::BU50_API.Properties.Settings.Default.ServerNameExternal;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.Color.Black;
            this.label9.Location = new System.Drawing.Point(600, 59);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 13);
            this.label9.TabIndex = 105;
            this.label9.Text = "Password";
            // 
            // txtpass2
            // 
            this.txtpass2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtpass2.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::BU50_API.Properties.Settings.Default, "password2", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txtpass2.Location = new System.Drawing.Point(662, 59);
            this.txtpass2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtpass2.Name = "txtpass2";
            this.txtpass2.Size = new System.Drawing.Size(233, 20);
            this.txtpass2.TabIndex = 106;
            this.txtpass2.Text = global::BU50_API.Properties.Settings.Default.password2;
            // 
            // FRM_Config
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(914, 320);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtpass2);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtServerNameExternal);
            this.Controls.Add(this.check);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.save);
            this.Controls.Add(this.btnsave);
            this.Controls.Add(this.txtapi);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txtdb);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtcomcode);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txt_password);
            this.Controls.Add(this.txt_user);
            this.Controls.Add(this.txt_DataBase);
            this.Controls.Add(this.txt_Server);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "FRM_Config";
            this.Text = "FRM_Config";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtapi;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtdb;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtcomcode;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton Sql;
        private System.Windows.Forms.RadioButton but_windows;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_password;
        private System.Windows.Forms.TextBox txt_user;
        private System.Windows.Forms.TextBox txt_DataBase;
        private System.Windows.Forms.TextBox txt_Server;
        private System.Windows.Forms.Button btnsave;
        private System.Windows.Forms.Button save;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button check;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtServerNameExternal;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtpass2;
    }
}