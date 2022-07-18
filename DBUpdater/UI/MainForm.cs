using System;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DBUpdater.State;

namespace DBUpdater.UI
{
    public partial class MainForm : Form
    {
        private readonly ProfileManager profileManager;
        private readonly PatchServerInfo patchServer;
        private readonly DatabaseServerInfo databaseServer;

        public MainForm()
        {
            InitializeComponent();
            timer1.Enabled = true;

            profileManager = new ProfileManager();
            patchServer = new PatchServerInfo();
            databaseServer = new DatabaseServerInfo();
            profileManager.PatchServers.Add(patchServer);
            profileManager.Databases.Add(databaseServer);

            Icon = App.Icon;
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null) components.Dispose();
            }
            base.Dispose(disposing);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            SaveOrLoad(false);
        }

        #region Profiles
        private void SaveOrLoad(bool isSave)
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime) return;
            int selectProfileIdx = NoSelection;
            using (IWriter regRoot = new RegistryWriter(Application.UserAppDataRegistry))
            {
                const string rootSectionName = "DBUpdater";
                try
                {
                    string bakSection = null;
                    if (isSave) regRoot.RenameSection(rootSectionName, bakSection = rootSectionName + "_bak");
                    else if (!regRoot.SectionExists(rootSectionName)) regRoot.RenameSection("ContourAutoUpdate", rootSectionName);
                    using (var root = regRoot.Section(rootSectionName))
                    {
                        var f = isSave ? root.GetSaveAction() : root.GetLoadAction();
                        f("PatchServer", patchServer);
                        f("DatabaseServer", databaseServer);
                        f("Profiles", profileManager);

                        const string keySelectedProfile = "selectedProfileRef";
                        const string keyAutosave = "Autosave";
                        if (isSave)
                        {
                            root.WriteRef(keySelectedProfile, GetSelectedProfile());
                            root.WriteBoolean(keyAutosave, chkAutosave.Checked);
                        }
                        else
                        {
                            var profile = root.ReadRef(keySelectedProfile) as Profile;
                            if (profile != null && profileManager.Profiles.Contains(profile)) selectProfileIdx = profileManager.Profiles.IndexOf(profile);
                            chkAutosave.Checked = root.ReadBoolean(keyAutosave, chkAutosave.Checked);
                        }

                        using (var bounds = root.Section("Bounds"))
                        {
                            if (isSave)
                            {
                                bounds.Write(nameof(Bounds.X), Bounds.X.ToString());
                                bounds.Write(nameof(Bounds.Y), Bounds.Y.ToString());
                                bounds.Write(nameof(Bounds.Width), Bounds.Width.ToString());
                                bounds.Write(nameof(Bounds.Height), Bounds.Height.ToString());
                            }
                            else
                            {
                                string
                                    strX = bounds.Read(nameof(Bounds.X)),
                                    strY = bounds.Read(nameof(Bounds.Y)),
                                    strW = bounds.Read(nameof(Bounds.Width)),
                                    strH = bounds.Read(nameof(Bounds.Height));

                                if (strX != null) Bounds = new Rectangle(
                                     int.Parse(strX),
                                     int.Parse(strY),
                                     int.Parse(strW),
                                     int.Parse(strH));
                            }
                        }
                    }
                    if (bakSection != null) regRoot.DeleteSection(bakSection);
                }
                catch
                {
                    if (!isSave) regRoot.RenameSection(rootSectionName, rootSectionName + "_error");
                    throw;
                }
            }
            if (!isSave)
            {
                RebindControls();
                if (selectProfileIdx != NoSelection && selectProfileIdx < profileList.Items.Count) profileList.SelectedIndex = selectProfileIdx;
            }
        }

        private static void BindBaseServerInfo(TextBoxExt edAddress, TextBoxExt edLogin, TextBoxExt edPassword, BaseServerInfo info)
        {
            edAddress.TextChangedAction = (text) => info.Address = text;
            edAddress.UpdateText(info.Address);
            edLogin.TextChangedAction = (text) => info.UserName = text;
            edLogin.UpdateText(info.UserName);
            edPassword.TextChangedAction = (text) => info.Password = text;
            edPassword.UpdateText(info.Password);
        }

        private void UpdateDBLoginControls()
        {
            if (chkUseDBLogin.Checked)
            {
                edDatabaseLogin.Enable();
                edDatabasePassword.Enable();
            }
            else
            {
                edDatabaseLogin.Disable();
                edDatabasePassword.Disable();
            }
        }

        private void ChkUseDBLogin_CheckedChanged(object sender, EventArgs e)
        {
            databaseServer.UseDBLogin = chkUseDBLogin.Checked;
            UpdateDBLoginControls();
        }

        private void RebindControls()
        {
            BindBaseServerInfo(edDatabaseAddress, edDatabaseLogin, edDatabasePassword, databaseServer);
            BindBaseServerInfo(edPatchAddress, edPatchLogin, edPatchPass, patchServer);
            BindCheckbox(chkUseDBLogin, databaseServer.UseDBLogin, ChkUseDBLogin_CheckedChanged);
            UpdateDBLoginControls();

            BindCheckbox(chkTimeout, databaseServer.UseTimeout, chkTimeout_CheckedChanged);
            edTimeout.Text = databaseServer.Timeout.ToString();

            BindCheckbox(chkFtpTimeout, patchServer.UseTimeout, chkFtpTimeout_CheckedChanged);
            edFtpTimeout.Text = patchServer.Timeout.ToString();
            BindCheckbox(chkUsePassiveMode, patchServer.UsePassive, chkUsePassiveMode_CheckedChanged);

            RefreshProfileListData();
        }

        private static void BindCheckbox(CheckBox control, bool value, EventHandler valueChangedHandler)
        {
            try
            {
                control.CheckedChanged -= valueChangedHandler;
                control.Checked = value;
            }
            finally
            {
                control.CheckedChanged += valueChangedHandler;
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            if (chkAutosave.Checked) SaveOrLoad(true);
            base.OnClosed(e);
        }

        private const int NoSelection = -1;

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddNewProfile();
        }

        private void AddNewProfile()
        {
            AddProfile(new Profile(patchServer, databaseServer));
        }

        private void AddProfile(Profile prof)
        {
            var selected = profileList.SelectedIndex;
            if (selected == NoSelection || selected + 1 >= profileManager.Profiles.Count)
            {
                // Надо бы больше логики в ProfileManager перенести.
                profileManager.Profiles.Add(prof);
                selected = profileManager.Profiles.Count - 1;
            }
            else
            {
                selected++;
                profileManager.Profiles.Insert(selected, prof);
            }

            RefreshProfileListData();
            profileList.SelectedIndex = selected;
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            var prof = GetSelectedProfile();
            if (prof == null) AddNewProfile();
            else AddProfile(prof.Clone());
        }

        private void RefreshProfileListData()
        {
            profileList.DataSource = profileManager.Profiles.Select(
                (p) => String.IsNullOrEmpty(p.DatabaseName) && String.IsNullOrEmpty(p.PatchGroupName) ? "<blank>" : $"{p.DatabaseName} - {p.PatchGroupName}")
                .ToArray();
            if (profileList.DataSource == null || profileManager.Profiles.Count == 0) SyncSelection();
        }

        private void profileList_SelectedIndexChanged(object sender, EventArgs e)
        {
            SyncSelection();
        }

        private void SyncSelection()
        {
            var idx = profileList.SelectedIndex;
            if (idx == NoSelection)
            {
                edDatabaseName.Enabled = false;
                edPatchGroup.Enabled = false;
            }
            else
            {
                edDatabaseName.Enabled = true;
                edPatchGroup.Enabled = true;
                var profile = profileManager.Profiles[idx];
                edDatabaseName.TextChangedAction = (text) => profile.DatabaseName = text;
                edDatabaseName.UpdateText(profile.DatabaseName);
                edPatchGroup.TextChangedAction = (text) => profile.PatchGroupName = text;
                edPatchGroup.UpdateText(profile.PatchGroupName);
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            var idx = profileList.SelectedIndex;
            if (idx != NoSelection && idx < profileManager.Profiles.Count)
            {
                profileManager.Profiles.RemoveAt(idx);
                RefreshProfileListData();
                if (idx < profileManager.Profiles.Count) profileList.SelectedIndex = idx;
                else profileList.SelectedIndex = profileManager.Profiles.Count - 1;
            }
        }

        private Profile GetSelectedProfile()
        {
            var idx = profileList.SelectedIndex;
            return idx == NoSelection || profileManager.Profiles.Count <= idx ? null : profileManager[idx];
        }

        private void edDatabaseName_Leave(object sender, EventArgs e)
        {
            var idx = profileList.SelectedIndex;
            RefreshProfileListData();
            profileList.SelectedIndex = idx;
        }
        #endregion

        #region async test
        int counter;
        private async void button_Click(object sender, EventArgs e)
        {
            string textPrev = btnStartStop.Text;
            IProgress<int> p = new Progress<int>((x) => btnStartStop.Text = $"Counter: {x}");
            await Task.Run(() =>
            {
                while (true)
                {
                    Thread.Sleep(1000);
                    counter++;
                    p.Report(counter);
                }
            });
            btnStartStop.Text = textPrev;
        }
        #endregion

        private void timer1_Tick(object sender, EventArgs e)
        {
            OnRefreshForm();
        }

        private void OnRefreshForm()
        {
            //button1.Text = $"Counter: {counter}";
        }

        private static UserActionException UserEx(string message) => new UserActionException(message);
        private static UserActionException User(Exception innerException) => new UserActionException(innerException.Message, innerException);

        private CancellationTokenSource runningTaskToken;
        private ManualResetEventSlim canContinue;
        private volatile bool checkToken = false;

        /// <summary>
        /// Окружает вызов в работающем потоке. Обычный handler срабатывает только
        /// в UI потоке (либо паралельно, либо после завершения процесса).
        /// </summary>
        private class ProgressProxy : IProgress<string>
        {
            private readonly IProgress<string> progress;
            private readonly Func<string, string> onReport;

            public ProgressProxy(IProgress<string> progress, Func<string, string> onReport)
            {
                this.progress = progress;
                this.onReport = onReport;
            }

            void IProgress<string>.Report(string value) => progress.Report(onReport(value));
        }

        private async void btnStartStop_Click(object sender, EventArgs e)
        {
            await StartStop(false);
        }

        private async void btnTest_Click(object sender, EventArgs e)
        {
            await StartStop(true);
        }

        private async Task StartStop(bool testMode)
        {
            if (runningTaskToken != null)
            {
                checkToken = true;
                canContinue?.Reset();
                if (MessageBox.Show("Stop update?", "Update paused", MessageBoxButtons.YesNo, MessageBoxIcon.None, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    runningTaskToken.Cancel();
                    btnStartStop.Enabled = false;
                    //try { await task; } finally { }
                }
                else
                {
                    checkToken = false;
                    canContinue?.Set();
                }
                return;
            }
            IProgress<string> progress = CreateProgress();
            const string progressSeparator = "============================";
            void reportException(Exception ex) => progress.Report(ex.GetType().Name + ": " + ex.Message);
            string strStart = btnStartStop.Text;
            var sw = Stopwatch.StartNew();
            try
            {
                var idx = profileList.SelectedIndex;
                if (idx == NoSelection) throw UserEx("No profile selected");
                runningTaskToken = new CancellationTokenSource();
                canContinue = new ManualResetEventSlim(true);
                var cancellationToken = runningTaskToken.Token;
                var taskProgress = new ProgressProxy(progress,
                    (message) =>
                    {
                        //if (stopRequest == true) throw User(new TaskCanceledException());
                        if (checkToken)
                        {
                            if (InvokeRequired)
                            {
                                canContinue.Wait(cancellationToken);
                                cancellationToken.ThrowIfCancellationRequested();
                            }
                            else
                            {
                                throw new OperationCanceledException();
                                // User(new TaskCanceledException());
                            }
                        }
                        return $"[{DateTime.Now.ToLongTimeString()}] {message}";
                    });
                progress.Report(progressSeparator);
                var runningTask = RunUpdate(profileManager[idx], taskProgress, testMode);
                btnStartStop.Text = "Stop update";
                btnTest.Enabled = false;
                try
                {
                    await runningTask;
                }
                catch (OperationCanceledException ex)
                {
                    progress.Report(ex.Message);
                    throw User(ex);
                }
                catch (Exception ex)
                {
                    reportException(ex);
                    if (ex is NullReferenceException) progress.Report(ex.StackTrace);
                }
            }
            catch (UserActionException ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
            finally
            {
                btnStartStop.Text = strStart;
                btnStartStop.Enabled = true;
                btnTest.Enabled = true;
                checkToken = false;
                runningTaskToken?.Dispose();
                runningTaskToken = null;
                canContinue?.Dispose();
                canContinue = null;
                try
                {
                    if (sw.ElapsedTicks > 20 * TimeSpan.TicksPerSecond) this.FlashNotification();
                }
                catch (Exception ex)
                {
                    progress.Report(progressSeparator);
                    reportException(ex);
                }
            }
        }

        private IProgress<string> CreateProgress() =>
            new Progress<string>((message) =>
            {
                var selection = new
                {
                    Start = edLog.SelectionStart,
                    Length = edLog.SelectionLength,
                };
                if (edLog.TextLength > 0) edLog.AppendText(Environment.NewLine + message);
                else edLog.Text = message;
                if (!chkAutoScroll.Checked)
                {
                    edLog.Select(selection.Start, selection.Length);
                    edLog.ScrollToCaret();
                }
            });

        /// <summary>
        /// Исключение из-за деиствия пользователя.
        /// </summary>
        private class UserActionException : InvalidOperationException
        {
            public UserActionException(string message, Exception innerException = null) : base(message, innerException) { }
        }

        private const string StrNonEmptyRequirement = "Must be nonempty string";

        private Task RunUpdate(Profile profile, IProgress<string> progress, bool testMode = false)
        {
            var patchProvider = new PatchProvider(profile.PatchServer, profile.PatchCodes); // Идеально надо бы PatchCodes разделить на два списка: словарь; другие параметры. Другие параметры надо бы хранить у профиля, а словарь хранить отдельно.
            var updater = new DatabaseUpdater(patchProvider);
            if (String.IsNullOrEmpty(profile.DatabaseName)) throw User(new ArgumentException(StrNonEmptyRequirement, nameof(profile.DatabaseName)));
            if (String.IsNullOrEmpty(profile.PatchGroupName)) throw User(new ArgumentException(StrNonEmptyRequirement, nameof(profile.PatchGroupName)));
            return updater.Update(profile.DatabaseServer, profile.DatabaseName, profile.PatchGroupName, progress, testMode);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveOrLoad(true);
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            SaveOrLoad(false);
        }

        private void btnPatchCodes_Click(object sender, EventArgs e)
        {
            var profile = GetSelectedProfile();
            if (profile == null) return;
            using (var codeForm = new PatchCodeTableForm())
            {
                codeForm.SetObject(profile.PatchCodes);
                codeForm.ShowDialog();
            }
        }

        private void btnDownloads_Click(object sender, EventArgs e)
        {
            string path = PatchProvider.GetRootPath();
            System.IO.Directory.CreateDirectory(path);
            System.Diagnostics.Process.Start(path);
        }

        private void chkTimeout_CheckedChanged(object sender, EventArgs e)
        {
            edTimeout.Enabled = chkTimeout.Checked;
            UpdateDBTimeoutSetting();
        }

        private void chkFtpTimeout_CheckedChanged(object sender, EventArgs e)
        {
            edFtpTimeout.Enabled = chkFtpTimeout.Checked;
            UpdateFtpTimeoutSetting();
        }

        private void UpdateDBTimeoutSetting() => UpdateTimeoutSetting(databaseServer, chkTimeout.Checked, edTimeout);
        private void UpdateFtpTimeoutSetting() => UpdateTimeoutSetting(patchServer, chkFtpTimeout.Checked, edFtpTimeout);

        private void UpdateTimeoutSetting(BaseServerInfo serverInfo, bool isChecked, TextBox edTimeout)
        {
            serverInfo.UseTimeout = isChecked;
            try
            {
                serverInfo.Timeout = int.Parse(edTimeout.Text);
            }
            catch
            {
                edTimeout.Text = serverInfo.Timeout.ToString();
                throw;
            }
        }

        private void edTimeout_Validating(object sender, CancelEventArgs e)
        {
            var edTimeout = (TextBox)sender;
            if (int.TryParse(edTimeout.Text, out int _))
                return;

            if (String.IsNullOrEmpty(edTimeout.Text)) edTimeout.Text = DatabaseServerInfo.DefaultTimeout.ToString();

            edTimeout.SelectAll();
            e.Cancel = true;
        }

        private void edTimeout_Validated(object sender, EventArgs e)
        {
            UpdateDBTimeoutSetting();
        }

        private void edFtpTimeout_Validated(object sender, EventArgs e)
        {
            UpdateFtpTimeoutSetting();
        }

        private async void btnTestFtp_Click(object sender, EventArgs e)
        {
            // TODO repeated clicks should cancel previous task.
            var progress = CreateProgress();
            try
            {
                var ftp = new FTP.FTPHelper(patchServer);
                await Task.Run(() =>
                {
                    ftp.Test(progress);
                });
            }
            catch (Exception ex)
            {
                progress.Report(CECommon.ExceptionFormater.GetFullExceptionMessage(ex));
            }
        }

        private void ShowAboutForm()
        {
            using (var f = new AboutForm()) f.ShowDialog(this);
        }

        private void BtnHelp_Click(object sender, EventArgs e)
        {
            ShowAboutForm();
        }

        private void MainForm_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            ShowAboutForm();
        }

        private void chkUsePassiveMode_CheckedChanged(object sender, EventArgs e)
        {
            patchServer.UsePassive = chkUsePassiveMode.Checked;
        }
    }
}
