using System;
using System.Collections.Generic;

namespace ContourAutoUpdate.State
{
    internal static class Extensions
    {
        public static void Save(this IWriter writer, string name, ISaveable obj) => obj.Save(writer, name);

        public static void Save<T>(this IWriter writer, string sectionName, ICollection<T> values) where T : ISaveable
        {
            if (values.Count == 0) return;
            using (var section = writer.Section(sectionName))
            {
                int count = 0;
                foreach (var item in values) item.Save(section, (++count).ToString());
            }
        }

        public static void Load(this IWriter writer, string name, ISaveable obj) => obj.Load(writer, name);

        public static void Load<T>(this IWriter writer, string sectionName, ICollection<T> values) where T : ISaveable, new()
        {
            using (var section = writer.Section(sectionName))
            {
                foreach (var name in section.GetNames())
                {
                    var val = new T();
                    val.Load(section, name);
                    values.Add(val);
                }
            }
        }

        public static void SaveOrLoad(this IWriter writer, string sectionName, ISaveable saveable, bool isSave)
        {
            if (isSave) saveable.Save(writer, sectionName);
            else saveable.Load(writer, sectionName);
        }

        public static void SaveOrLoad<T>(this IWriter writer, string sectionName, ICollection<T> values, bool isSave) where T : ISaveable, new()
        {
            if (isSave) writer.Save(sectionName, values);
            else writer.Load(sectionName, values);
        }

        public static Action<string, ISaveable> GetSaveAction(this IWriter writer) => (name, value) => writer.Save(name, value);
        public static Action<string, ISaveable> GetLoadAction(this IWriter writer) => (name, value) => writer.Load(name, value);

        public static T Read<T>(this IWriter writer, string name) where T : ISaveable, new() => writer.Read(name, () => new T());
    }
}
