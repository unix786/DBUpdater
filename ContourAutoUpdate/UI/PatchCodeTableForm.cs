using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;

namespace ContourAutoUpdate.UI
{
    internal partial class PatchCodeTableForm : Form
    {
        public PatchCodeTableForm()
        {
            InitializeComponent();
        }

        private PatchCodeTable patchCodes;
        public void SetObject(PatchCodeTable patchCodes) => this.patchCodes = patchCodes;

        protected override void OnShown(EventArgs e)
        {
            dtPatchCodes.Rows.Clear();
            if (patchCodes != null)
            {
                foreach (var item in patchCodes)
                {
                    var row = dtPatchCodes.NewRow();
                    row[colArchiveCode] = item.ArchiveCode;
                    row[colDBCode] = item.DBCode;
                    row[colSkip] = item.Ignore;
                    dtPatchCodes.Rows.Add(row);
                }
            }

            base.OnShown(e);
        }

        private void dataGrid_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
            MessageBox.Show(e.Exception.Message, e.Exception.GetType().Name);
        }

        private void SaveTable()
        {
            if (patchCodes == null)
                return;

            var newList = new Dictionary<string, PatchCodeInfo>();
            foreach (DataRow row in dtPatchCodes.Rows)
            {
                string archiveCode = (string)row[colArchiveCode];
                string dbCode = row[colDBCode] as string;
                if (String.IsNullOrWhiteSpace(dbCode)) dbCode = null;
                newList[archiveCode] = new PatchCodeInfo(archiveCode, dbCode, (bool)row[colSkip]);
            }

            patchCodes.ReplaceTable(newList);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            try
            {
                SaveTable();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().Name);
            }
            base.OnClosing(e);
        }
    }
}
