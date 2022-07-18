namespace DBUpdater.UI
{
    partial class AboutForm
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
            this.edText = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // edText
            // 
            this.edText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.edText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.edText.Location = new System.Drawing.Point(12, 12);
            this.edText.Multiline = true;
            this.edText.Name = "edText";
            this.edText.ReadOnly = true;
            this.edText.Size = new System.Drawing.Size(407, 113);
            this.edText.TabIndex = 0;
            this.edText.TabStop = false;
            this.edText.Text = "Programos naujausią versiją galima rasti prisegtą prie užklausos N {0}.\r\n\r\nSource" +
    " origin:\r\nhttps://github.com/unix786/DBUpdater";
            this.edText.KeyUp += new System.Windows.Forms.KeyEventHandler(this.EdText_KeyUp);
            // 
            // AboutForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(431, 137);
            this.Controls.Add(this.edText);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(200, 111);
            this.Name = "AboutForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Apie";
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.AboutForm_KeyUp);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox edText;
    }
}