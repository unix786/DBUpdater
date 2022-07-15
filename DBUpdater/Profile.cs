using System;
using DBUpdater.State;

namespace DBUpdater
{
    internal class Profile : ISaveable
    {
        public PatchServerInfo PatchServer { get; private set; }
        public DatabaseServerInfo DatabaseServer { get; private set; }
        public string DatabaseName { get; set; }
        public string PatchGroupName { get; set; } = "CPRO";
        public PatchCodeTable PatchCodes { get; private set; } = new PatchCodeTable();

        public Profile(PatchServerInfo patchServer, DatabaseServerInfo databaseServer)
        {
            PatchServer = patchServer;
            DatabaseServer = databaseServer;
        }

        /// <summary>Для <see cref="ISaveable.Load"/>.</summary>
        public Profile() : this(null, null) { }

        public Profile Clone()
        {
            return new Profile(PatchServer, DatabaseServer)
            {
                DatabaseName = DatabaseName,
                PatchGroupName = PatchGroupName,
                PatchCodes = PatchCodes.Clone(),
            };
        }

        private const string refKey = "#ref";
        void ISaveable.Save(IWriter root, string name)
        {
            using (var writer = root.Section(name))
            {
                writer.WriteRef(refKey, this);
                writer.WriteRef(nameof(PatchServer), PatchServer);
                writer.WriteRef(nameof(DatabaseServer), DatabaseServer);
                writer.Write(nameof(DatabaseName), DatabaseName);
                writer.Write(nameof(PatchGroupName), PatchGroupName);
                writer.Save(nameof(PatchCodes), PatchCodes);
            }
        }

        void ISaveable.Load(IWriter root, string name)
        {
            using (var writer = root.Section(name))
            {
                writer.LinkRef(refKey, this);
                PatchServer = (PatchServerInfo)writer.ReadRef(nameof(PatchServer));
                DatabaseServer = (DatabaseServerInfo)writer.ReadRef(nameof(DatabaseServer));
                DatabaseName = writer.Read(nameof(DatabaseName));
                PatchGroupName = writer.Read(nameof(PatchGroupName));
                writer.Load(nameof(PatchCodes), PatchCodes);
            }
        }
    }

    internal class BaseServerInfo : ISaveable
    {
        public string Address { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        /// <summary>Timeout in seconds.</summary>
        public int Timeout { get; internal set; }
        public bool UseTimeout { get; internal set; }
        private const string refKey = "#ref";
        internal const int DefaultTimeout = 10;

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
            writer.WriteBoolean(nameof(UseTimeout), UseTimeout);
            writer.Write(nameof(Timeout), Timeout.ToString());
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
            UseTimeout = writer.ReadBoolean(nameof(UseTimeout));
            Timeout = int.TryParse(writer.Read(nameof(Timeout)), out var val) ? val : DefaultTimeout;
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

        protected override void OnLoad(IWriter writer)
        {
            base.OnLoad(writer);
            if (String.IsNullOrEmpty(Address)) Address = @"localhost:21\Contour\C4\Patches\";
            if (String.IsNullOrEmpty(UserName)) UserName = "anonymous";
        }
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
            UseDBLogin = writer.ReadBoolean(nameof(UseDBLogin));
        }
    }
}
