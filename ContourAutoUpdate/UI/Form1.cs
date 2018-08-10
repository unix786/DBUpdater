using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ContourAutoUpdate.State;

namespace ContourAutoUpdate.UI
{
    public partial class Form1 : Form
    {
        private readonly ProfileManager profileManager;
        private readonly PatchServerInfo patchServer;
        private readonly DatabaseServerInfo databaseServer;

        public Form1()
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
            if (!isSave) RebindControls();
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

        private void RebindControls()
        {
            BindBaseServerInfo(edDatabaseAddress, edDatabaseLogin, edDatabasePassword, databaseServer);
            BindBaseServerInfo(edPatchAddress, edPatchLogin, edPatchPass, patchServer);
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
            string textPrev = button1.Text;
            IProgress<int> p = new Progress<int>((x) => button1.Text = $"Counter: {x}");
            await Task.Run(() =>
            {
                while (true)
                {
                    Thread.Sleep(1000);
                    counter++;
                    p.Report(counter);
                }
            });
            button1.Text = textPrev;
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

        private static UserErrorException UserError(string message) => new UserErrorException(message);

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                var idx = profileList.SelectedIndex;
                if (idx == NoSelection) throw UserError("No profile selected");
            }
            catch (UserErrorException ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private class UserErrorException : InvalidOperationException
        {
            public UserErrorException(string message) : base(message) { }
        }

        private Task RunUpdate(Profile profile)
        {
            var patchProvider = new PatchProvider(profile.PatchServer);
            var updater = new DatabaseUpdater(patchProvider);
            return updater.Update(profile.DatabaseServer, profile.DatabaseName, profile.PatchGroupName);
        }
    }
}
