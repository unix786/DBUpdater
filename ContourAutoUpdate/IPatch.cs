using System;

namespace ContourAutoUpdate
{
    public interface IPatch : IDisposable
    {
        /// <summary>
        /// Должно вернуть путь к папке с файлами. Это может быть временная папка (она должна существовать пока существует этот объект).
        /// </summary>
        string GetFolderPath();
    }
}