using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;
using PaintDotNet.Effects;

namespace AlphaMaskEffect
{
    public partial class AlphaMaskImportEffectConfigDialog : EffectConfigDialog
    {
        private Stack ignoreChangedEvents = new Stack();

        public AlphaMaskImportEffectConfigDialog()
        {
            InitializeComponent();
        }


        #region Effect Dialog Implementation ( Overrides )
        /// <summary>
        /// Required for effects to initialize the options data structure
        /// </summary>
        protected override void InitialInitToken()
        {
            theEffectToken = new AlphaMaskImportEffectConfigToken();
        }

        /// <summary>
        /// Required for effects to set the options data structure based on the options dialog box
        /// </summary>
        protected override void InitTokenFromDialog()
        {
            base.InitTokenFromDialog();
            ((AlphaMaskImportEffectConfigToken)theEffectToken).PhotoFileName = txtPhotoFileName.Text;
            ((AlphaMaskImportEffectConfigToken)theEffectToken).Invert = cbxInvert.Checked;
            ((AlphaMaskImportEffectConfigToken)theEffectToken).AlphaMix = cbxAlphaMix.Checked;
            ((AlphaMaskImportEffectConfigToken)theEffectToken).inClipboard = checkBox1.Checked;
        }

        /// <summary>
        /// Required by effects to initialize the dialog box based on the options data structure
        /// </summary>
        /// <param name="effectTokenCopy"></param>
        /// 

        protected override void InitDialogFromToken(EffectConfigToken effectTokenCopy)
        {
            base.InitDialogFromToken(effectTokenCopy);
            AlphaMaskImportEffectConfigToken token = (AlphaMaskImportEffectConfigToken)effectTokenCopy;
            cbxInvert.Checked = ((AlphaMaskImportEffectConfigToken)theEffectToken).Invert;
            cbxAlphaMix.Checked = ((AlphaMaskImportEffectConfigToken)theEffectToken).AlphaMix;
            txtPhotoFileName.Text = ((AlphaMaskImportEffectConfigToken)theEffectToken).PhotoFileName;
            checkBox1.Checked = ((AlphaMaskImportEffectConfigToken)theEffectToken).inClipboard;
        }
        #endregion

        #region Control Event Handlers
        /// <summary>
        /// Browse Handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBrowseFile_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                FileInfo fi = new FileInfo(openFileDialog1.FileName);
                if (fi.Exists)
                {
                    txtPhotoFileName.Text = fi.FullName;
                    FinishTokenUpdate();
                }
            }
        }
        /// <summary>
        /// Text Changed Handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPhotoFileName_TextChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txtPhotoFileName.Text))
            {
                FileInfo fi = new FileInfo(txtPhotoFileName.Text);
                if (fi.Exists)
                {
                    FinishTokenUpdate();
                }
            }
        }
        /// <summary>
        /// Process the OK button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            FinishTokenUpdate();
            DialogResult = DialogResult.OK;
            Close();
        }

        /// <summary>
        /// Process the Cancel Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        /// <summary>
        /// Allows realtime display changes as the checkbox changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxInvert_CheckedChanged(object sender, EventArgs e)
        {
            FinishTokenUpdate();
        }

        private void cbxAlphaMix_CheckedChanged(object sender, EventArgs e)
        {
            FinishTokenUpdate();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            FinishTokenUpdate();
        }

        /*private void ClearLimits()
        {
        }
        private void ResetLimits()
        {
            /*if (!String.IsNullOrEmpty(txtPhotoFileName.Text))
            {
                Bitmap b = (Bitmap)Bitmap.FromFile(txtPhotoFileName.Text);
            }
            else
            {
                ClearLimits();
            }*/
        //}
        #endregion

    }
}