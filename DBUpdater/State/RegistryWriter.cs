using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Win32;

namespace DBUpdater.State
{
    internal class RegistryWriter : IWriter
    {
        private readonly RegistryWriter parent;
        private readonly string sectionName;
        private RegistryKey _regKey;

        /// <summary>
        /// При чтении может вернуть null.
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="regKey">E.g. <see cref="System.Windows.Forms.Application.UserAppDataRegistry"/></param>
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

        void IWriter.DeleteSection(string sectionName) => Reg(false)?.DeleteSubKeyTree(sectionName, false);

        private RegistryWriter SectionInternal(string sectionName) => new RegistryWriter(this, sectionName);
        IWriter IWriter.Section(string sectionName) => SectionInternal(sectionName);

        void IWriter.Write(string name, string value)
        {
            if (value == null) Reg(false)?.DeleteValue(name, false);
            else Reg(true).SetValue(name, value);
        }

        /// <summary>Результат <see cref="GetKey"/> в случае отсутствия записи.</summary>
        const int KeyNotFound = -1;
        private int GetKey(object obj) => references.IndexOfValue(obj);
        private void AssignKey(object obj, ref int key)
        {
            key = references.Count;
            references.Add(key, obj);
        }

        private void TryDeleteValue(string name) => Reg(false)?.DeleteValue(name, false);
        private void TryDeleteKey(string name) => Reg(false)?.DeleteSubKeyTree(name, false);

        private void SetValueInternal(string name, object value) => Reg(true).SetValue(name, value);

        void IWriter.WriteRef<T>(string name, T obj)
        {
            if (obj == null)
            {
                TryDeleteValue(name);
            }
            else
            {
                int key = GetKey(obj);
                if (key == KeyNotFound) AssignKey(obj, ref key);
                SetValueInternal(name, key);
            }
        }

        private object TryReadValueInternal(string name) => Reg(false)?.GetValue(name);
        private object ReadRefInternal(string name)
        {
            var keyObj = TryReadValueInternal(name);
            if (keyObj == null) return null;
            return references[(int)keyObj];
        }

        object IWriter.ReadRef(string name) => ReadRefInternal(name);

        private void LinkRefInternal(string name, object obj)
        {
            var reg = Reg(false);
            if (reg == null) return;
            var keyObj = reg.GetValue(name);
            if (keyObj == null) return;
            var key = (int)keyObj;
            if (references.TryGetValue(key, out object obj2) && obj2 != null)
            {
                if (ReferenceEquals(obj, obj2)) return;
                throw new InvalidOperationException("Key already linked with " + obj2.ToString());
            }
            references[key] = obj;
        }

        void IWriter.LinkRef<T>(string name, T obj) => LinkRefInternal(name, obj);

        private const string RefKey = "#Ref";
        private const string RefValue = "Data";

        void IWriter.Write<T>(string name, T obj)
        {
            int key = GetKey(obj);
            if (key == KeyNotFound)
            {
                TryDeleteValue(name);
                var sec = SectionInternal(name);
                obj.Save(sec, RefValue);

                // Возможно что объект сам сохранит ссылку на себя. Тогда не будем повторно сохранять.
                key = GetKey(obj);
                if (key == KeyNotFound)
                {
                    AssignKey(obj, ref key);
                    sec.SetValueInternal(RefKey, key);
                }
            }
            else
            {
                TryDeleteKey(name);
                SetValueInternal(name, key);
            }
        }

        T IWriter.Read<T>(string name, Func<T> createFunc)
        {
            object obj = ReadRefInternal(name);
            if (obj == null)
            {
                var sec = SectionInternal(name);
                var restoredObj = createFunc();
                restoredObj.Load(sec, RefValue);
                int key = sec.GetKey(restoredObj);
                if (key == KeyNotFound) sec.LinkRefInternal(RefKey, restoredObj);
                return restoredObj;
            }
            else
            {
                return (T)obj;
            }
        }

        internal void WriteBinary(string fn, byte[] resBytes)
        {
            Reg(true).SetValue(fn, resBytes, RegistryValueKind.Binary);
        }

        internal byte[] ReadBinary(string fn)
        {
            return (byte[])Reg(false)?.GetValue(fn);
        }
    }
}
