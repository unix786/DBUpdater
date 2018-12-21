using System;
using ContourAutoUpdate.State;

namespace ContourAutoUpdate
{
    internal class Profile : ISaveable
    {
        public PatchServerInfo PatchServer { get; set; }
        public DatabaseServerInfo DatabaseServer { get; set; }
        public string DatabaseName { get; set; }
        public string PatchGroupName { get; set; }

        public Profile Clone()
        {
            return new Profile
            {
                PatchServer = PatchServer.Clone(),
                DatabaseServer = DatabaseServer.Clone(),
                DatabaseName = DatabaseName,
                PatchGroupName = PatchGroupName
            };
        }

        void ISaveable.Save(IWriter root, string name)
        {
            using (var writer = root.Section(name))
            {
                writer.WriteRef(nameof(PatchServer), PatchServer);
                writer.WriteRef(nameof(DatabaseServer), DatabaseServer);
                writer.Write(nameof(DatabaseName), DatabaseName);
                writer.Write(nameof(PatchGroupName), PatchGroupName);
            }
        }

        void ISaveable.Load(IWriter root, string name)
        {
            using (var writer = root.Section(name))
            {
                PatchServer = (PatchServerInfo)writer.ReadRef(nameof(PatchServer));
                DatabaseServer = (DatabaseServerInfo)writer.ReadRef(nameof(DatabaseServer));
                DatabaseName = writer.Read(nameof(DatabaseName));
                PatchGroupName = writer.Read(nameof(PatchGroupName));
            }
        }
    }

    internal class BaseServerInfo : ISaveable
    {
        public string Address { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        private const string refKey = "#ref";

        public T Clone<T>() where T : BaseServerInfo, new()
        {
            return new T
            {
                Address = Address,
                UserName = UserName,
                Password = Password,
            };
        }

        protected virtual void OnSave(IWriter writer)
        {
            writer.Write(nameof(Address), Address);
            writer.Write(nameof(UserName), UserName);
            PasswordSaver.Save(nameof(Password), writer, Password);
        }

        void ISaveable.Save(IWriter root, string name)
        {
            using (var writer = root.Section(name))
            {
                writer.WriteRef(refKey, this);
                OnSave(writer);
            }
        }

        protected virtual void OnLoad(IWriter writer)
        {
            Address = writer.Read(nameof(Address));
            UserName = writer.Read(nameof(UserName));
            Password = PasswordSaver.Load(nameof(Password), writer);
        }

        void ISaveable.Load(IWriter root, string name)
        {
            using (var writer = root.Section(name))
            {
                writer.LinkRef(refKey, this);
                OnLoad(writer);
            }
        }
    }

    internal class PatchServerInfo : BaseServerInfo
    {
        public PatchServerInfo Clone() => Clone<PatchServerInfo>();
    }

    internal class DatabaseServerInfo : BaseServerInfo
    {
        public bool UseDBLogin { get; set; }

        public DatabaseServerInfo Clone()
        {
            var clone = Clone<DatabaseServerInfo>();
            clone.UseDBLogin = UseDBLogin;
            return clone;
        }

        protected override void OnSave(IWriter writer)
        {
            base.OnSave(writer);
            writer.Write(nameof(UseDBLogin), UseDBLogin.ToString());
        }

        protected override void OnLoad(IWriter writer)
        {
            base.OnLoad(writer);
            string strUseDBLogin = writer.Read(nameof(UseDBLogin));
            UseDBLogin = strUseDBLogin == null ? false : Boolean.Parse(strUseDBLogin);
        }
    }
}
