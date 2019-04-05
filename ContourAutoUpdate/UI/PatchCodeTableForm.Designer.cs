namespace ContourAutoUpdate.UI
{
    partial class PatchCodeTableForm
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
            System.Windows.Forms.DataGridViewTextBoxColumn archiveCodeDataGridViewTextBoxColumn;
            System.Windows.Forms.DataGridViewTextBoxColumn dBCodeDataGridViewTextBoxColumn;
            System.Windows.Forms.DataGridViewCheckBoxColumn skipDataGridViewCheckBoxColumn;
            this.colArchiveCode = new System.Data.DataColumn();
            this.colDBCode = new System.Data.DataColumn();
            this.colSkip = new System.Data.DataColumn();
            this.dataGrid = new System.Windows.Forms.DataGridView();
            this.dataSet = new System.Data.DataSet();
            this.dtPatchCodes = new System.Data.DataTable();
            archiveCodeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            dBCodeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            skipDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtPatchCodes)).BeginInit();
            this.SuspendLayout();
            // 
            // archiveCodeDataGridViewTextBoxColumn
            // 
            archiveCodeDataGridViewTextBoxColumn.DataPropertyName = "ArchiveCode";
            archiveCodeDataGridViewTextBoxColumn.HeaderText = "Archive code";
            archiveCodeDataGridViewTextBoxColumn.Name = "archiveCodeDataGridViewTextBoxColumn";
            // 
            // dBCodeDataGridViewTextBoxColumn
            // 
            dBCodeDataGridViewTextBoxColumn.DataPropertyName = "DBCode";
            dBCodeDataGridViewTextBoxColumn.HeaderText = "DB code";
            dBCodeDataGridViewTextBoxColumn.Name = "dBCodeDataGridViewTextBoxColumn";
            // 
            // skipDataGridViewCheckBoxColumn
            // 
            skipDataGridViewCheckBoxColumn.DataPropertyName = "Skip";
            skipDataGridViewCheckBoxColumn.HeaderText = "Ignore";
            skipDataGridViewCheckBoxColumn.Name = "skipDataGridViewCheckBoxColumn";
            // 
            // colArchiveCode
            // 
            this.colArchiveCode.AllowDBNull = false;
            this.colArchiveCode.Caption = "Archive code";
            this.colArchiveCode.ColumnName = "ArchiveCode";
            // 
            // colDBCode
            // 
            this.colDBCode.Caption = "DB code";
            this.colDBCode.ColumnName = "DBCode";
            // 
            // colSkip
            // 
            this.colSkip.AllowDBNull = false;
            this.colSkip.ColumnName = "Skip";
            this.colSkip.DataType = typeof(bool);
            this.colSkip.DefaultValue = false;
            // 
            // dataGrid
            // 
            this.dataGrid.AutoGenerateColumns = false;
            this.dataGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            archiveCodeDataGridViewTextBoxColumn,
            dBCodeDataGridViewTextBoxColumn,
            skipDataGridViewCheckBoxColumn});
            this.dataGrid.DataMember = "Patch Codes";
            this.dataGrid.DataSource = this.dataSet;
            this.dataGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGrid.Location = new System.Drawing.Point(0, 0);
            this.dataGrid.Name = "dataGrid";
            this.dataGrid.Size = new System.Drawing.Size(372, 562);
            this.dataGrid.TabIndex = 0;
            this.dataGrid.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGrid_DataError);
            // 
            // dataSet
            // 
            this.dataSet.DataSetName = "Data";
            this.dataSet.Tables.AddRange(new System.Data.DataTable[] {
            this.dtPatchCodes});
            // 
            // dtPatchCodes
            // 
            this.dtPatchCodes.CaseSensitive = true;
            this.dtPatchCodes.Columns.AddRange(new System.Data.DataColumn[] {
            this.colArchiveCode,
            this.colDBCode,
            this.colSkip});
            this.dtPatchCodes.Constraints.AddRange(new System.Data.Constraint[] {
            new System.Data.UniqueConstraint("Constraint1", new string[] {
                        "ArchiveCode"}, true)});
            this.dtPatchCodes.PrimaryKey = new System.Data.DataColumn[] {
        this.colArchiveCode};
            this.dtPatchCodes.TableName = "Patch Codes";
            // 
            // PatchCodeTableForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(372, 562);
            this.Controls.Add(this.dataGrid);
            this.Name = "PatchCodeTableForm";
            this.Text = "Patch codes";
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtPatchCodes)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGrid;
        private System.Data.DataSet dataSet;
        private System.Data.DataTable dtPatchCodes;
        private System.Data.DataColumn colArchiveCode;
        private System.Data.DataColumn colDBCode;
        private System.Data.DataColumn colSkip;
    }
}