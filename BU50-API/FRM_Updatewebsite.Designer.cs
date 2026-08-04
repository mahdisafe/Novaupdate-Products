namespace BU50_API
{
    partial class FRM_Updatewebsite
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
            this.grdproduct = new System.Windows.Forms.DataGridView();
            this.btnnumber = new System.Windows.Forms.Button();
            this.txtlblsku = new System.Windows.Forms.TextBox();
            this.linksku = new System.Windows.Forms.LinkLabel();
            this.lblnumber = new System.Windows.Forms.Label();
            this.txtpagenumber = new System.Windows.Forms.TextBox();
            this.btnupdate = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.txtsku = new System.Windows.Forms.TextBox();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.lblProducts = new System.Windows.Forms.LinkLabel();
            this.lblTotalProducts = new System.Windows.Forms.LinkLabel();
            this.lblStatus = new System.Windows.Forms.Label();
            this.progressBar2 = new System.Windows.Forms.ProgressBar();
            this.lblcurrentRow = new System.Windows.Forms.LinkLabel();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.grdproduct)).BeginInit();
            this.SuspendLayout();
            // 
            // grdproduct
            // 
            this.grdproduct.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdproduct.Location = new System.Drawing.Point(10, 42);
            this.grdproduct.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.grdproduct.Name = "grdproduct";
            this.grdproduct.RowHeadersWidth = 51;
            this.grdproduct.RowTemplate.Height = 26;
            this.grdproduct.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdproduct.Size = new System.Drawing.Size(1028, 436);
            this.grdproduct.TabIndex = 0;
            // 
            // btnnumber
            // 
            this.btnnumber.Location = new System.Drawing.Point(933, 481);
            this.btnnumber.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnnumber.Name = "btnnumber";
            this.btnnumber.Size = new System.Drawing.Size(105, 25);
            this.btnnumber.TabIndex = 20;
            this.btnnumber.Text = "page number";
            this.btnnumber.UseVisualStyleBackColor = true;
            this.btnnumber.Click += new System.EventHandler(this.btnnumber_Click);
            // 
            // txtlblsku
            // 
            this.txtlblsku.Location = new System.Drawing.Point(519, 481);
            this.txtlblsku.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtlblsku.Name = "txtlblsku";
            this.txtlblsku.Size = new System.Drawing.Size(109, 20);
            this.txtlblsku.TabIndex = 19;
            this.txtlblsku.Visible = false;
            // 
            // linksku
            // 
            this.linksku.AutoSize = true;
            this.linksku.Location = new System.Drawing.Point(710, 481);
            this.linksku.Name = "linksku";
            this.linksku.Size = new System.Drawing.Size(80, 13);
            this.linksku.TabIndex = 18;
            this.linksku.TabStop = true;
            this.linksku.Text = "duplicate (SKU)";
            // 
            // lblnumber
            // 
            this.lblnumber.AutoSize = true;
            this.lblnumber.Location = new System.Drawing.Point(643, 483);
            this.lblnumber.Name = "lblnumber";
            this.lblnumber.Size = new System.Drawing.Size(39, 13);
            this.lblnumber.TabIndex = 17;
            this.lblnumber.Text = "--------";
            this.lblnumber.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblnumber.Visible = false;
            // 
            // txtpagenumber
            // 
            this.txtpagenumber.Location = new System.Drawing.Point(819, 484);
            this.txtpagenumber.Name = "txtpagenumber";
            this.txtpagenumber.Size = new System.Drawing.Size(109, 20);
            this.txtpagenumber.TabIndex = 16;
            this.txtpagenumber.TextChanged += new System.EventHandler(this.txtpagenumber_TextChanged);
            // 
            // btnupdate
            // 
            this.btnupdate.Location = new System.Drawing.Point(13, 481);
            this.btnupdate.Name = "btnupdate";
            this.btnupdate.Size = new System.Drawing.Size(108, 22);
            this.btnupdate.TabIndex = 14;
            this.btnupdate.Text = "Run Now ..";
            this.btnupdate.UseVisualStyleBackColor = true;
            this.btnupdate.Click += new System.EventHandler(this.btnupdate_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(10, 8);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(1028, 18);
            this.progressBar1.TabIndex = 21;
            this.progressBar1.Click += new System.EventHandler(this.progressBar1_Click);
            // 
            // txtsku
            // 
            this.txtsku.Location = new System.Drawing.Point(127, 481);
            this.txtsku.Name = "txtsku";
            this.txtsku.Size = new System.Drawing.Size(180, 20);
            this.txtsku.TabIndex = 1;
            this.txtsku.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtsku_KeyDown);
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(127, 465);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(63, 13);
            this.linkLabel1.TabIndex = 28;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "SKU (Enter)";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // lblProducts
            // 
            this.lblProducts.AutoSize = true;
            this.lblProducts.Location = new System.Drawing.Point(1066, 279);
            this.lblProducts.Name = "lblProducts";
            this.lblProducts.Size = new System.Drawing.Size(44, 13);
            this.lblProducts.TabIndex = 29;
            this.lblProducts.TabStop = true;
            this.lblProducts.Text = "Product";
            // 
            // lblTotalProducts
            // 
            this.lblTotalProducts.AutoSize = true;
            this.lblTotalProducts.Location = new System.Drawing.Point(1092, 9);
            this.lblTotalProducts.Name = "lblTotalProducts";
            this.lblTotalProducts.Size = new System.Drawing.Size(44, 13);
            this.lblTotalProducts.TabIndex = 30;
            this.lblTotalProducts.TabStop = true;
            this.lblTotalProducts.Text = "Product";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(643, 27);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(35, 13);
            this.lblStatus.TabIndex = 31;
            this.lblStatus.Text = "label1";
            // 
            // progressBar2
            // 
            this.progressBar2.Location = new System.Drawing.Point(10, -4);
            this.progressBar2.Name = "progressBar2";
            this.progressBar2.Size = new System.Drawing.Size(1028, 18);
            this.progressBar2.TabIndex = 32;
            this.progressBar2.Click += new System.EventHandler(this.progressBar2_Click);
            // 
            // lblcurrentRow
            // 
            this.lblcurrentRow.AutoSize = true;
            this.lblcurrentRow.Location = new System.Drawing.Point(1057, 9);
            this.lblcurrentRow.Name = "lblcurrentRow";
            this.lblcurrentRow.Size = new System.Drawing.Size(28, 13);
            this.lblcurrentRow.TabIndex = 34;
            this.lblcurrentRow.TabStop = true;
            this.lblcurrentRow.Text = "Row";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1081, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(11, 13);
            this.label2.TabIndex = 36;
            this.label2.Text = "/";
            // 
            // FRM_Updatewebsite
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1157, 526);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblcurrentRow);
            this.Controls.Add(this.progressBar2);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.lblTotalProducts);
            this.Controls.Add(this.lblProducts);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.txtsku);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.btnnumber);
            this.Controls.Add(this.txtlblsku);
            this.Controls.Add(this.linksku);
            this.Controls.Add(this.lblnumber);
            this.Controls.Add(this.txtpagenumber);
            this.Controls.Add(this.btnupdate);
            this.Controls.Add(this.grdproduct);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FRM_Updatewebsite";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Update Website";
            this.Load += new System.EventHandler(this.FRM_Updatewebsite_Load);
            this.Shown += new System.EventHandler(this.FRM_Updatewebsite_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.grdproduct)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnnumber;
        private System.Windows.Forms.TextBox txtlblsku;
        private System.Windows.Forms.LinkLabel linksku;
        private System.Windows.Forms.Label lblnumber;
        private System.Windows.Forms.TextBox txtpagenumber;
        private System.Windows.Forms.Button btnupdate;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.TextBox txtsku;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.DataGridView grdproduct;
        private System.Windows.Forms.LinkLabel lblProducts;
        private System.Windows.Forms.LinkLabel lblTotalProducts;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ProgressBar progressBar2;
        private System.Windows.Forms.LinkLabel lblcurrentRow;
        private System.Windows.Forms.Label label2;
    }
}

