namespace AlphaMaskEffect
{
    partial class AlphaMaskImportEffectConfigDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AlphaMaskImportEffectConfigDialog));
            this.btnOK = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtPhotoFileName = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.btnBrowseFile = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.cbxInvert = new System.Windows.Forms.CheckBox();
            this.cbxAlphaMix = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(297, 90);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 8;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Mask File:";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(378, 90);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // txtPhotoFileName
            // 
            this.txtPhotoFileName.Location = new System.Drawing.Point(73, 6);
            this.txtPhotoFileName.Name = "txtPhotoFileName";
            this.txtPhotoFileName.Size = new System.Drawing.Size(299, 20);
            this.txtPhotoFileName.TabIndex = 34;
            this.txtPhotoFileName.TextChanged += new System.EventHandler(this.txtPhotoFileName_TextChanged);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = resources.GetString("openFileDialog1.Filter");
            this.openFileDialog1.Title = "Select Photo";
            // 
            // btnBrowseFile
            // 
            this.btnBrowseFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBrowseFile.Location = new System.Drawing.Point(378, 6);
            this.btnBrowseFile.Name = "btnBrowseFile";
            this.btnBrowseFile.Size = new System.Drawing.Size(75, 20);
            this.btnBrowseFile.TabIndex = 35;
            this.btnBrowseFile.Text = "Browse...";
            this.btnBrowseFile.UseVisualStyleBackColor = true;
            this.btnBrowseFile.Click += new System.EventHandler(this.btnBrowseFile_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label2.Location = new System.Drawing.Point(12, 93);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(230, 17);
            this.label2.TabIndex = 36;
            this.label2.Text = "All color information will be ignored.";
            // 
            // cbxInvert
            // 
            this.cbxInvert.AutoSize = true;
            this.cbxInvert.Location = new System.Drawing.Point(73, 32);
            this.cbxInvert.Name = "cbxInvert";
            this.cbxInvert.Size = new System.Drawing.Size(82, 17);
            this.cbxInvert.TabIndex = 37;
            this.cbxInvert.Text = "Invert Mask";
            this.cbxInvert.UseVisualStyleBackColor = true;
            this.cbxInvert.CheckedChanged += new System.EventHandler(this.cbxInvert_CheckedChanged);
            // 
            // cbxAlphaMix
            // 
            this.cbxAlphaMix.AutoSize = true;
            this.cbxAlphaMix.Location = new System.Drawing.Point(73, 50);
            this.cbxAlphaMix.Name = "cbxAlphaMix";
            this.cbxAlphaMix.Size = new System.Drawing.Size(72, 17);
            this.cbxAlphaMix.TabIndex = 38;
            this.cbxAlphaMix.Text = "Mix Alpha";
            this.cbxAlphaMix.UseVisualStyleBackColor = true;
            this.cbxAlphaMix.CheckedChanged += new System.EventHandler(this.cbxAlphaMix_CheckedChanged);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(73, 68);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(123, 17);
            this.checkBox1.TabIndex = 39;
            this.checkBox1.Text = "Paste from Clipboard";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // AlphaMaskImportEffectConfigDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(465, 125);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.cbxAlphaMix);
            this.Controls.Add(this.cbxInvert);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnBrowseFile);
            this.Controls.Add(this.txtPhotoFileName);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "AlphaMaskImportEffectConfigDialog";
            this.Text = "Alpha Mask Settings";
            this.Controls.SetChildIndex(this.btnOK, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.txtPhotoFileName, 0);
            this.Controls.SetChildIndex(this.btnBrowseFile, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.cbxInvert, 0);
            this.Controls.SetChildIndex(this.cbxAlphaMix, 0);
            this.Controls.SetChildIndex(this.checkBox1, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox txtPhotoFileName;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button btnBrowseFile;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox cbxInvert;
        private System.Windows.Forms.CheckBox cbxAlphaMix;
        private System.Windows.Forms.CheckBox checkBox1;

    }
}