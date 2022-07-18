namespace DBUpdater.UI
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btnStartStop = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.profileList = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkUsePassiveMode = new System.Windows.Forms.CheckBox();
            this.btnTestFtp = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.edFtpTimeout = new DBUpdater.UI.TextBoxExt();
            this.lblAddress = new System.Windows.Forms.Label();
            this.edPatchPass = new DBUpdater.UI.TextBoxExt();
            this.edPatchLogin = new DBUpdater.UI.TextBoxExt();
            this.edPatchAddress = new DBUpdater.UI.TextBoxExt();
            this.chkFtpTimeout = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkUseDBLogin = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.edTimeout = new DBUpdater.UI.TextBoxExt();
            this.edDatabaseAddress = new DBUpdater.UI.TextBoxExt();
            this.label4 = new System.Windows.Forms.Label();
            this.edDatabaseLogin = new DBUpdater.UI.TextBoxExt();
            this.label3 = new System.Windows.Forms.Label();
            this.chkTimeout = new System.Windows.Forms.CheckBox();
            this.edDatabasePassword = new DBUpdater.UI.TextBoxExt();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.btnPatchCodes = new System.Windows.Forms.Button();
            this.btnCopy = new System.Windows.Forms.Button();
            this.edDatabaseName = new DBUpdater.UI.TextBoxExt();
            this.edPatchGroup = new DBUpdater.UI.TextBoxExt();
            this.edLog = new System.Windows.Forms.TextBox();
            this.chkAutoScroll = new System.Windows.Forms.CheckBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnReload = new System.Windows.Forms.Button();
            this.btnTest = new System.Windows.Forms.Button();
            this.chkAutosave = new System.Windows.Forms.CheckBox();
            this.btnDownloads = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.btnHelp = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnStartStop
            // 
            this.btnStartStop.Location = new System.Drawing.Point(993, 370);
            this.btnStartStop.Margin = new System.Windows.Forms.Padding(4);
            this.btnStartStop.Name = "btnStartStop";
            this.btnStartStop.Size = new System.Drawing.Size(216, 28);
            this.btnStartStop.TabIndex = 10;
            this.btnStartStop.Text = "Run update";
            this.btnStartStop.UseVisualStyleBackColor = true;
            this.btnStartStop.Click += new System.EventHandler(this.btnStartStop_Click);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // profileList
            // 
            this.profileList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.profileList.FormattingEnabled = true;
            this.profileList.ItemHeight = 16;
            this.profileList.Location = new System.Drawing.Point(8, 23);
            this.profileList.Margin = new System.Windows.Forms.Padding(4);
            this.profileList.Name = "profileList";
            this.profileList.Size = new System.Drawing.Size(329, 212);
            this.profileList.TabIndex = 0;
            this.profileList.SelectedIndexChanged += new System.EventHandler(this.profileList_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkUsePassiveMode);
            this.groupBox1.Controls.Add(this.btnTestFtp);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.edFtpTimeout);
            this.groupBox1.Controls.Add(this.lblAddress);
            this.groupBox1.Controls.Add(this.edPatchPass);
            this.groupBox1.Controls.Add(this.edPatchLogin);
            this.groupBox1.Controls.Add(this.edPatchAddress);
            this.groupBox1.Controls.Add(this.chkFtpTimeout);
            this.groupBox1.Location = new System.Drawing.Point(16, 15);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(525, 185);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Patch server";
            // 
            // chkUsePassiveMode
            // 
            this.chkUsePassiveMode.Checked = true;
            this.chkUsePassiveMode.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkUsePassiveMode.Location = new System.Drawing.Point(8, 150);
            this.chkUsePassiveMode.Margin = new System.Windows.Forms.Padding(4);
            this.chkUsePassiveMode.Name = "chkUsePassiveMode";
            this.chkUsePassiveMode.Size = new System.Drawing.Size(352, 25);
            this.chkUsePassiveMode.TabIndex = 6;
            this.chkUsePassiveMode.Text = "Use passive mode";
            this.chkUsePassiveMode.UseVisualStyleBackColor = true;
            this.chkUsePassiveMode.CheckedChanged += new System.EventHandler(this.chkUsePassiveMode_CheckedChanged);
            // 
            // btnTestFtp
            // 
            this.btnTestFtp.Location = new System.Drawing.Point(393, 149);
            this.btnTestFtp.Margin = new System.Windows.Forms.Padding(4);
            this.btnTestFtp.Name = "btnTestFtp";
            this.btnTestFtp.Size = new System.Drawing.Size(124, 28);
            this.btnTestFtp.TabIndex = 7;
            this.btnTestFtp.Text = "Test FTP";
            this.btnTestFtp.UseVisualStyleBackColor = true;
            this.btnTestFtp.Click += new System.EventHandler(this.btnTestFtp_Click);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(8, 84);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(133, 25);
            this.label2.TabIndex = 0;
            this.label2.Text = "Password :";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(8, 52);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(133, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Login name :";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // edFtpTimeout
            // 
            this.edFtpTimeout.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.edFtpTimeout.Enabled = false;
            this.edFtpTimeout.Location = new System.Drawing.Point(192, 117);
            this.edFtpTimeout.Margin = new System.Windows.Forms.Padding(4);
            this.edFtpTimeout.Name = "edFtpTimeout";
            this.edFtpTimeout.Size = new System.Drawing.Size(324, 22);
            this.edFtpTimeout.TabIndex = 5;
            this.edFtpTimeout.TextChangedAction = null;
            this.edFtpTimeout.Validating += new System.ComponentModel.CancelEventHandler(this.edTimeout_Validating);
            this.edFtpTimeout.Validated += new System.EventHandler(this.edFtpTimeout_Validated);
            // 
            // lblAddress
            // 
            this.lblAddress.Location = new System.Drawing.Point(8, 20);
            this.lblAddress.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAddress.Name = "lblAddress";
            this.lblAddress.Size = new System.Drawing.Size(133, 25);
            this.lblAddress.TabIndex = 0;
            this.lblAddress.Text = "Address :";
            this.lblAddress.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // edPatchPass
            // 
            this.edPatchPass.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.edPatchPass.Location = new System.Drawing.Point(149, 85);
            this.edPatchPass.Margin = new System.Windows.Forms.Padding(4);
            this.edPatchPass.Name = "edPatchPass";
            this.edPatchPass.PasswordChar = '*';
            this.edPatchPass.Size = new System.Drawing.Size(367, 22);
            this.edPatchPass.TabIndex = 3;
            this.edPatchPass.TextChangedAction = null;
            // 
            // edPatchLogin
            // 
            this.edPatchLogin.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.edPatchLogin.Location = new System.Drawing.Point(149, 53);
            this.edPatchLogin.Margin = new System.Windows.Forms.Padding(4);
            this.edPatchLogin.Name = "edPatchLogin";
            this.edPatchLogin.Size = new System.Drawing.Size(367, 22);
            this.edPatchLogin.TabIndex = 2;
            this.edPatchLogin.TextChangedAction = null;
            // 
            // edPatchAddress
            // 
            this.edPatchAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.edPatchAddress.Location = new System.Drawing.Point(149, 21);
            this.edPatchAddress.Margin = new System.Windows.Forms.Padding(4);
            this.edPatchAddress.Name = "edPatchAddress";
            this.edPatchAddress.Size = new System.Drawing.Size(367, 22);
            this.edPatchAddress.TabIndex = 1;
            this.edPatchAddress.TextChangedAction = null;
            this.toolTip.SetToolTip(this.edPatchAddress, "Kaip dalį adreso galima nurodyti pradinį kelią serveryje, pvz.:\r\n127.0.0.1:21\\Con" +
        "tour\\C4\\Patches\\");
            // 
            // chkFtpTimeout
            // 
            this.chkFtpTimeout.Checked = true;
            this.chkFtpTimeout.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkFtpTimeout.Location = new System.Drawing.Point(8, 117);
            this.chkFtpTimeout.Margin = new System.Windows.Forms.Padding(4);
            this.chkFtpTimeout.Name = "chkFtpTimeout";
            this.chkFtpTimeout.Size = new System.Drawing.Size(176, 25);
            this.chkFtpTimeout.TabIndex = 4;
            this.chkFtpTimeout.Text = "Timeout (seconds) :";
            this.chkFtpTimeout.UseVisualStyleBackColor = true;
            this.chkFtpTimeout.CheckedChanged += new System.EventHandler(this.chkFtpTimeout_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chkUseDBLogin);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.edTimeout);
            this.groupBox2.Controls.Add(this.edDatabaseAddress);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.edDatabaseLogin);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.chkTimeout);
            this.groupBox2.Controls.Add(this.edDatabasePassword);
            this.groupBox2.Location = new System.Drawing.Point(16, 207);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox2.Size = new System.Drawing.Size(525, 192);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Database server";
            // 
            // chkUseDBLogin
            // 
            this.chkUseDBLogin.Checked = true;
            this.chkUseDBLogin.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkUseDBLogin.Location = new System.Drawing.Point(8, 55);
            this.chkUseDBLogin.Margin = new System.Windows.Forms.Padding(4);
            this.chkUseDBLogin.Name = "chkUseDBLogin";
            this.chkUseDBLogin.Size = new System.Drawing.Size(509, 25);
            this.chkUseDBLogin.TabIndex = 2;
            this.chkUseDBLogin.Text = "Use server authentication";
            this.chkUseDBLogin.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(8, 118);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(133, 25);
            this.label5.TabIndex = 0;
            this.label5.Text = "Password :";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // edTimeout
            // 
            this.edTimeout.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.edTimeout.Enabled = false;
            this.edTimeout.Location = new System.Drawing.Point(192, 151);
            this.edTimeout.Margin = new System.Windows.Forms.Padding(4);
            this.edTimeout.Name = "edTimeout";
            this.edTimeout.Size = new System.Drawing.Size(324, 22);
            this.edTimeout.TabIndex = 6;
            this.edTimeout.TextChangedAction = null;
            this.edTimeout.Validating += new System.ComponentModel.CancelEventHandler(this.edTimeout_Validating);
            this.edTimeout.Validated += new System.EventHandler(this.edTimeout_Validated);
            // 
            // edDatabaseAddress
            // 
            this.edDatabaseAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.edDatabaseAddress.Location = new System.Drawing.Point(149, 23);
            this.edDatabaseAddress.Margin = new System.Windows.Forms.Padding(4);
            this.edDatabaseAddress.Name = "edDatabaseAddress";
            this.edDatabaseAddress.Size = new System.Drawing.Size(367, 22);
            this.edDatabaseAddress.TabIndex = 1;
            this.edDatabaseAddress.TextChangedAction = null;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(8, 86);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(133, 25);
            this.label4.TabIndex = 0;
            this.label4.Text = "Login name :";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // edDatabaseLogin
            // 
            this.edDatabaseLogin.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.edDatabaseLogin.Location = new System.Drawing.Point(149, 87);
            this.edDatabaseLogin.Margin = new System.Windows.Forms.Padding(4);
            this.edDatabaseLogin.Name = "edDatabaseLogin";
            this.edDatabaseLogin.Size = new System.Drawing.Size(367, 22);
            this.edDatabaseLogin.TabIndex = 3;
            this.edDatabaseLogin.TextChangedAction = null;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(8, 22);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(133, 25);
            this.label3.TabIndex = 0;
            this.label3.Text = "Address :";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chkTimeout
            // 
            this.chkTimeout.Checked = true;
            this.chkTimeout.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTimeout.Location = new System.Drawing.Point(8, 151);
            this.chkTimeout.Margin = new System.Windows.Forms.Padding(4);
            this.chkTimeout.Name = "chkTimeout";
            this.chkTimeout.Size = new System.Drawing.Size(176, 25);
            this.chkTimeout.TabIndex = 5;
            this.chkTimeout.Text = "Timeout (seconds) :";
            this.chkTimeout.UseVisualStyleBackColor = true;
            this.chkTimeout.CheckedChanged += new System.EventHandler(this.chkTimeout_CheckedChanged);
            // 
            // edDatabasePassword
            // 
            this.edDatabasePassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.edDatabasePassword.Location = new System.Drawing.Point(149, 119);
            this.edDatabasePassword.Margin = new System.Windows.Forms.Padding(4);
            this.edDatabasePassword.Name = "edDatabasePassword";
            this.edDatabasePassword.PasswordChar = '*';
            this.edDatabasePassword.Size = new System.Drawing.Size(367, 22);
            this.edDatabasePassword.TabIndex = 4;
            this.edDatabasePassword.TextChangedAction = null;
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdd.Location = new System.Drawing.Point(128, 242);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(4);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(115, 28);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemove.Location = new System.Drawing.Point(251, 242);
            this.btnRemove.Margin = new System.Windows.Forms.Padding(4);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(88, 28);
            this.btnRemove.TabIndex = 3;
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.profileList);
            this.groupBox3.Controls.Add(this.btnPatchCodes);
            this.groupBox3.Controls.Add(this.btnRemove);
            this.groupBox3.Controls.Add(this.btnCopy);
            this.groupBox3.Controls.Add(this.btnAdd);
            this.groupBox3.Controls.Add(this.edDatabaseName);
            this.groupBox3.Controls.Add(this.edPatchGroup);
            this.groupBox3.Location = new System.Drawing.Point(549, 50);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox3.Size = new System.Drawing.Size(660, 278);
            this.groupBox3.TabIndex = 8;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Profiles";
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(347, 84);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(85, 16);
            this.label7.TabIndex = 0;
            this.label7.Text = "Patch group :";
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(347, 20);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(73, 16);
            this.label6.TabIndex = 0;
            this.label6.Text = "Database :";
            // 
            // btnPatchCodes
            // 
            this.btnPatchCodes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPatchCodes.Location = new System.Drawing.Point(347, 135);
            this.btnPatchCodes.Margin = new System.Windows.Forms.Padding(4);
            this.btnPatchCodes.Name = "btnPatchCodes";
            this.btnPatchCodes.Size = new System.Drawing.Size(119, 28);
            this.btnPatchCodes.TabIndex = 6;
            this.btnPatchCodes.Text = "Patch codes ...";
            this.btnPatchCodes.UseVisualStyleBackColor = true;
            this.btnPatchCodes.Click += new System.EventHandler(this.btnPatchCodes_Click);
            // 
            // btnCopy
            // 
            this.btnCopy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCopy.Location = new System.Drawing.Point(8, 242);
            this.btnCopy.Margin = new System.Windows.Forms.Padding(4);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(112, 28);
            this.btnCopy.TabIndex = 1;
            this.btnCopy.Text = "Copy";
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // edDatabaseName
            // 
            this.edDatabaseName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.edDatabaseName.Location = new System.Drawing.Point(347, 39);
            this.edDatabaseName.Margin = new System.Windows.Forms.Padding(4);
            this.edDatabaseName.Name = "edDatabaseName";
            this.edDatabaseName.Size = new System.Drawing.Size(304, 22);
            this.edDatabaseName.TabIndex = 4;
            this.edDatabaseName.TextChangedAction = null;
            this.edDatabaseName.Leave += new System.EventHandler(this.edDatabaseName_Leave);
            // 
            // edPatchGroup
            // 
            this.edPatchGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.edPatchGroup.Location = new System.Drawing.Point(347, 103);
            this.edPatchGroup.Margin = new System.Windows.Forms.Padding(4);
            this.edPatchGroup.Name = "edPatchGroup";
            this.edPatchGroup.Size = new System.Drawing.Size(304, 22);
            this.edPatchGroup.TabIndex = 5;
            this.edPatchGroup.TextChangedAction = null;
            this.toolTip.SetToolTip(this.edPatchGroup, "Kelias iki direktorijos su archyvais, pvz.:\r\nC4\\Patches\\CPRO\r\n\\C4\\Patches\\CPRO\r\nC" +
        "PRO");
            this.edPatchGroup.Leave += new System.EventHandler(this.edDatabaseName_Leave);
            // 
            // edLog
            // 
            this.edLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.edLog.Location = new System.Drawing.Point(16, 406);
            this.edLog.Margin = new System.Windows.Forms.Padding(4);
            this.edLog.Multiline = true;
            this.edLog.Name = "edLog";
            this.edLog.ReadOnly = true;
            this.edLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.edLog.Size = new System.Drawing.Size(1192, 318);
            this.edLog.TabIndex = 11;
            // 
            // chkAutoScroll
            // 
            this.chkAutoScroll.Checked = true;
            this.chkAutoScroll.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAutoScroll.Location = new System.Drawing.Point(557, 374);
            this.chkAutoScroll.Margin = new System.Windows.Forms.Padding(4);
            this.chkAutoScroll.Name = "chkAutoScroll";
            this.chkAutoScroll.Size = new System.Drawing.Size(189, 25);
            this.chkAutoScroll.TabIndex = 7;
            this.chkAutoScroll.Text = "Auto scroll";
            this.chkAutoScroll.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(557, 15);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(155, 28);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Save settings";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnReload
            // 
            this.btnReload.Location = new System.Drawing.Point(720, 15);
            this.btnReload.Margin = new System.Windows.Forms.Padding(4);
            this.btnReload.Name = "btnReload";
            this.btnReload.Size = new System.Drawing.Size(155, 28);
            this.btnReload.TabIndex = 1;
            this.btnReload.Text = "Reload settings";
            this.btnReload.UseVisualStyleBackColor = true;
            this.btnReload.Click += new System.EventHandler(this.btnReload_Click);
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(904, 370);
            this.btnTest.Margin = new System.Windows.Forms.Padding(4);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(81, 28);
            this.btnTest.TabIndex = 9;
            this.btnTest.Text = "Test";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // chkAutosave
            // 
            this.chkAutosave.Checked = true;
            this.chkAutosave.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAutosave.Location = new System.Drawing.Point(883, 17);
            this.chkAutosave.Margin = new System.Windows.Forms.Padding(4);
            this.chkAutosave.Name = "chkAutosave";
            this.chkAutosave.Size = new System.Drawing.Size(132, 25);
            this.chkAutosave.TabIndex = 2;
            this.chkAutosave.Text = "Save on close";
            this.chkAutosave.UseVisualStyleBackColor = true;
            // 
            // btnDownloads
            // 
            this.btnDownloads.Location = new System.Drawing.Point(1023, 15);
            this.btnDownloads.Margin = new System.Windows.Forms.Padding(4);
            this.btnDownloads.Name = "btnDownloads";
            this.btnDownloads.Size = new System.Drawing.Size(137, 28);
            this.btnDownloads.TabIndex = 3;
            this.btnDownloads.Text = "Downloads ...";
            this.btnDownloads.UseVisualStyleBackColor = true;
            this.btnDownloads.Click += new System.EventHandler(this.btnDownloads_Click);
            // 
            // btnHelp
            // 
            this.btnHelp.Location = new System.Drawing.Point(1168, 15);
            this.btnHelp.Margin = new System.Windows.Forms.Padding(4);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(41, 28);
            this.btnHelp.TabIndex = 4;
            this.btnHelp.Text = "?";
            this.btnHelp.UseVisualStyleBackColor = true;
            this.btnHelp.Click += new System.EventHandler(this.BtnHelp_Click);
            // 
            // MainForm
            // 
            this.AcceptButton = this.btnStartStop;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1225, 740);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.btnHelp);
            this.Controls.Add(this.btnDownloads);
            this.Controls.Add(this.btnReload);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.chkAutosave);
            this.Controls.Add(this.chkAutoScroll);
            this.Controls.Add(this.edLog);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnStartStop);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(1241, 543);
            this.Name = "MainForm";
            this.Text = "DB Updater";
            this.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.MainForm_HelpRequested);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStartStop;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ListBox profileList;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblAddress;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.GroupBox groupBox3;
        private UI.TextBoxExt edPatchPass;
        private UI.TextBoxExt edPatchLogin;
        private UI.TextBoxExt edPatchAddress;
        private UI.TextBoxExt edDatabaseAddress;
        private UI.TextBoxExt edDatabaseLogin;
        private UI.TextBoxExt edDatabasePassword;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private TextBoxExt edDatabaseName;
        private TextBoxExt edPatchGroup;
        private System.Windows.Forms.TextBox edLog;
        private System.Windows.Forms.CheckBox chkAutoScroll;
        private System.Windows.Forms.CheckBox chkUseDBLogin;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnReload;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Button btnPatchCodes;
        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.CheckBox chkAutosave;
        private System.Windows.Forms.Button btnDownloads;
        private TextBoxExt edTimeout;
        private System.Windows.Forms.CheckBox chkTimeout;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Button btnHelp;
        private System.Windows.Forms.Button btnTestFtp;
        private TextBoxExt edFtpTimeout;
        private System.Windows.Forms.CheckBox chkFtpTimeout;
        private System.Windows.Forms.CheckBox chkUsePassiveMode;
    }
}

