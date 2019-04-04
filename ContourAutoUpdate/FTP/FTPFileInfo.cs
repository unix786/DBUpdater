using System;

namespace ContourAutoUpdate.FTP
{
    /// <summary>
    /// Информация из одной строки из ответа на команду "LIST".
    /// </summary>
    internal sealed class ListEntry
    {
        /// <summary>
        /// True - директория; False - файл.
        /// </summary>
        public bool IsDirectory { get; internal set; }
        public string Name { get; set; }
        public long Size { get; set; }
        public DateTime? Timestamp { get; set; }

        public override string ToString()
        {
            return $"\"{Name}\" {Size}B {Timestamp}";
        }
    }
}
