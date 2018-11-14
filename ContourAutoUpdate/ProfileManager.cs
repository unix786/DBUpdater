using System.Collections.Generic;
using ContourAutoUpdate.State;

namespace ContourAutoUpdate
{
    internal class ProfileManager : ISaveable
    {
        public List<PatchServerInfo> PatchServers { get; } = new List<PatchServerInfo>();
        public List<DatabaseServerInfo> Databases { get; } = new List<DatabaseServerInfo>();
        public List<Profile> Profiles { get; } = new List<Profile>();

        public Profile this[int index] => Profiles[index];

        void ISaveable.Save(IWriter writer, string name)
        {
            OnSaveOrLoad(writer, name, true);
        }

        void ISaveable.Load(IWriter writer, string name)
        {
            PatchServers.Clear();
            Databases.Clear();
            Profiles.Clear();
            OnSaveOrLoad(writer, name, false);
        }

        private void OnSaveOrLoad(IWriter writer, string name, bool isSave)
        {
            using (var section = writer.Section(name))
            {
                section.SaveOrLoad("Servers", PatchServers, isSave);
                section.SaveOrLoad("Databases", Databases, isSave);
                section.SaveOrLoad("Profiles", Profiles, isSave);
            }
        }

    }
}
