using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Win32;

namespace ContourAutoUpdate.State
{
    internal class RegistryWriter : IWriter
    {
        private readonly RegistryWriter parent;
        private readonly string sectionName;
        private RegistryKey _regKey;
        private RegistryKey Reg(bool isWrite)
        {
            if (_regKey == null && parent != null)
            {
                if (isWrite) _regKey = parent.Reg(isWrite).CreateSubKey(sectionName);
                else _regKey = parent.Reg(isWrite)?.OpenSubKey(sectionName);
            }
            return _regKey;
        }
        private readonly SortedList<int, object> references;

        public RegistryWriter(RegistryKey regKey)
        {
            parent = null;
            sectionName = null;
            _regKey = regKey;
            references = new SortedList<int, object>();
        }

        private RegistryWriter(RegistryWriter parent, string sectionName)
        {
            this.parent = parent;
            this.sectionName = sectionName;
            _regKey = null;
            references = parent.references;
        }

        public void Dispose()
        {
            if (!(sectionName == null || _regKey == null)) _regKey.Dispose();
        }

        IEnumerable<string> IWriter.GetNames()
        {
            var reg = Reg(false);
            if (reg == null) return Array.Empty<string>();
            return reg.GetSubKeyNames().Union(reg.GetValueNames());
        }

        string IWriter.Read(string name) => Reg(false)?.GetValue(name)?.ToString();

        private static void CopyItems(RegistryKey src, RegistryKey dest)
        {
            foreach (var name in src.GetSubKeyNames())
            {
                using (var subSrc = src.OpenSubKey(name))
                using (var subDest = dest.CreateSubKey(name))
                    CopyItems(subSrc, subDest);
            }
            foreach (var name in src.GetValueNames()) dest.SetValue(name, src.GetValue(name));
        }

        void IWriter.RenameSection(string prevName, string newName)
        {
            var reg = Reg(false);
            if (reg == null) return;
            using (var src = reg.OpenSubKey(prevName))
            {
                if (src == null) return;

                reg.DeleteSubKeyTree(newName, false);
                using (var dest = reg.CreateSubKey(newName))
                {
                    CopyItems(src, dest);
                }

                reg.DeleteSubKeyTree(prevName, false);
            }
        }

        IWriter IWriter.Section(string sectionName) => new RegistryWriter(this, sectionName);

        void IWriter.Write(string name, string value)
        {
            if (value == null) Reg(false)?.DeleteValue(name, false);
            else Reg(true).SetValue(name, value);
        }

        void IWriter.WriteRef<T>(string name, T obj)
        {
            if (obj == null)
            {
                Reg(false)?.DeleteValue(name, false);
            }
            else
            {
                int key = references.IndexOfValue(obj);
                if (key == -1)
                {
                    key = references.Count;
                    references.Add(key, obj);
                }
                Reg(true).SetValue(name, key);
            }
        }

        object IWriter.ReadRef(string name)
        {
            var keyObj = Reg(false)?.GetValue(name);
            if (keyObj == null) return null;
            return references[(int)keyObj];
        }

        void IWriter.LinkRef<T>(string name, T obj)
        {
            var reg = Reg(false);
            if (reg == null) return;
            var keyObj = reg.GetValue(name);
            if (keyObj == null) return;
            var key = (int)keyObj;
            if (references.TryGetValue(key, out object obj2) && obj2 != null)
                throw new InvalidOperationException("Key already linked with " + obj2.ToString());
            references[key] = obj;
        }
    }
}
