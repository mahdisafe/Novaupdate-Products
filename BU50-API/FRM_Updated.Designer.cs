namespace BU50_API
{
    partial class FRM_Updated
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtsearch = new System.Windows.Forms.TextBox();
            this.grdproduct = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.grdproduct)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 13);
            this.label1.TabIndex = 29;
            this.label1.Text = "Search By Name";
            // 
            // txtsearch
            // 
            this.txtsearch.Location = new System.Drawing.Point(103, 6);
            this.txtsearch.Name = "txtsearch";
            this.txtsearch.Size = new System.Drawing.Size(210, 20);
            this.txtsearch.TabIndex = 28;
            this.txtsearch.TextChanged += new System.EventHandler(this.txtsearch_TextChanged);
            // 
            // grdproduct
            // 
            this.grdproduct.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdproduct.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdproduct.Location = new System.Drawing.Point(0, 0);
            this.grdproduct.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.grdproduct.Name = "grdproduct";
            this.grdproduct.RowHeadersWidth = 51;
            this.grdproduct.RowTemplate.Height = 26;
            this.grdproduct.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdproduct.Size = new System.Drawing.Size(1061, 607);
            this.grdproduct.TabIndex = 27;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.grdproduct);
            this.panel1.Location = new System.Drawing.Point(12, 31);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1061, 607);
            this.panel1.TabIndex = 30;
            // 
            // FRM_Updated
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1085, 650);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtsearch);
            this.Name = "FRM_Updated";
            this.Text = "FRM_Updated";
            ((System.ComponentModel.ISupportInitialize)(this.grdproduct)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtsearch;
        private System.Windows.Forms.DataGridView grdproduct;
        private System.Windows.Forms.Panel panel1;
    }
}