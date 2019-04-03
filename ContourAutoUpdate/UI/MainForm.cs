using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ContourAutoUpdate.State;

namespace ContourAutoUpdate.UI
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
                const string rootSectionName = "ContourAutoUpdate";
                try
                {
                    if (isSave) regRoot.RenameSection(rootSectionName, rootSectionName + "_bak");
                    using (var root = regRoot.Section(rootSectionName))
                    {
                        var f = isSave ? root.GetSaveAction() : root.GetLoadAction();
                        f("PatchServer", patchServer);
                        f("DatabaseServer", databaseServer);
                        f("Profiles", profileManager);

                        const string keySelectedProfile = "selectedProfileRef";
                        if (isSave)
                        {
                            root.WriteRef(keySelectedProfile, GetSelectedProfile());
                        }
                        else
                        {
                            var profile = root.ReadRef(keySelectedProfile) as Profile;
                            if (profile != null && profileManager.Profiles.Contains(profile)) selectProfileIdx = profileManager.Profiles.IndexOf(profile);
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

            try
            {
                chkUseDBLogin.CheckedChanged -= ChkUseDBLogin_CheckedChanged;
                chkUseDBLogin.Checked = databaseServer.UseDBLogin;
            }
            finally
            {
                chkUseDBLogin.CheckedChanged += ChkUseDBLogin_CheckedChanged;
            }
            UpdateDBLoginControls();

            RefreshProfileListData();
        }

        protected override void OnClosed(EventArgs e)
        {
            SaveOrLoad(true);
            base.OnClosed(e);
        }

        private const int NoSelection = -1;

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var prof = new Profile
            {
                PatchServer = patchServer,
                DatabaseServer = databaseServer,
            };

            var selected = profileList.SelectedIndex;
            if (selected == NoSelection || selected + 1 >= profileManager.Profiles.Count)
            {
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
        private bool stopPrompt = false;

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
            if (runningTaskToken != null)
            {
                try
                {
                    stopPrompt = true;
                    if (MessageBox.Show("Stop update?", "Update paused", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        runningTaskToken.Cancel();
                        //try { await task; } finally { }
                    }
                }
                finally
                {
                    stopPrompt = false;
                }
                return;
            }
            string strStart = btnStartStop.Text;
            try
            {
                var idx = profileList.SelectedIndex;
                if (idx == NoSelection) throw UserEx("No profile selected");
                IProgress<string> progress = new Progress<string>(
                    (message) =>
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
                        //if (stopRequest == true) throw User(new TaskCanceledException());
                    });
                runningTaskToken = new CancellationTokenSource();
                var cancellationToken = runningTaskToken.Token;
                progress = new ProgressProxy(progress,
                    (message) =>
                    {
                        if (stopPrompt)
                        {
                            if (InvokeRequired)
                            {
                                while (stopPrompt) Thread.Sleep(200);

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
                var runningTask = RunUpdate(profileManager[idx], progress);
                btnStartStop.Text = "Stop update";
                try
                {
                    await runningTask;
                }
                catch (OperationCanceledException ex)
                {
                    throw User(ex);
                }
                catch (Exception ex)
                {
                    progress.Report(ex.GetType().Name + ": " + ex.Message);
                }
            }
            catch (UserActionException ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
            finally
            {
                btnStartStop.Text = strStart;
                runningTaskToken = null;
            }
        }

        /// <summary>
        /// Исключение из-за деиствия пользователя.
        /// </summary>
        private class UserActionException : InvalidOperationException
        {
            public UserActionException(string message, Exception innerException = null) : base(message, innerException) { }
        }

        private const string StrNonEmptyRequirement = "Must be nonempty string";

        private Task RunUpdate(Profile profile, IProgress<string> progress)
        {
            var patchProvider = new PatchProvider(profile.PatchServer);
            var updater = new DatabaseUpdater(patchProvider);
            if (String.IsNullOrEmpty(profile.DatabaseName)) throw User(new ArgumentException(StrNonEmptyRequirement, nameof(profile.DatabaseName)));
            if (String.IsNullOrEmpty(profile.PatchGroupName)) throw User(new ArgumentException(StrNonEmptyRequirement, nameof(profile.PatchGroupName)));
            return updater.Update(profile.DatabaseServer, profile.DatabaseName, profile.PatchGroupName, progress);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveOrLoad(true);
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            SaveOrLoad(false);
        }
    }
}
