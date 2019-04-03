using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace ContourAutoUpdate
{
    internal class PatchProvider
    {
        private PatchServerInfo Server { get; }
        public PatchProvider(PatchServerInfo server) => Server = server;

        public PatchCodeTable PatchCodes => Server.PatchCodes;

        public sealed class PatchInfo : IPatchMetadata
        {
            public float Number { get; set; }
            public string ArchiveCode { get; set; }
            public PatchVersion Version { get; set; }

            private FTP.ListEntry listEntry;
            FTP.ListEntry IPatchMetadata.Remote { get => listEntry; set => listEntry = value; }
            string IPatchMetadata.LocalFile { get; set; }

            public DateTime? Timestamp { get { return listEntry == null || listEntry.Timestamp == DateTime.MinValue ? (DateTime?)null : listEntry.Timestamp; } }

            public override string ToString()
            {
                return $"({Number}) {ArchiveCode} v{Version}";
            }
        }

        private interface IPatchMetadata
        {
            FTP.ListEntry Remote { get; set; }
            //string LocalFile { get; set; }
        }

        /// <summary>
        /// Получает список патчов из сервера.
        /// </summary>
        /// <param name="patchGroupCode">Группа патчов.</param>
        /// <param name="progress"></param>
        internal IEnumerable<PatchInfo> GetPatchList(string patchGroupCode, IProgress<string> progress)
        {
            var ftp = new FTP.FTPHelper(Server);
            foreach (var item in ftp.GetFileList(patchGroupCode, progress))
            {
                if (item.Size == 0) continue; // Ещё может быть -1.
                if (item.Size > 0 && PatchNameParser.TryParse(item.Name, out var number, out string code, out var version))
                {
                    var info = new PatchInfo
                    {
                        Number = number,
                        ArchiveCode = code,
                        Version = version,
                    };
                    ((IPatchMetadata)info).Remote = item;
                    yield return info;
                }
                else
                {
                    progress.Report($"Ignoring remote file: {item}.");
                }
            }
        }

        private class LocalPatch : IPatch
        {
            public LocalPatch(string path, string unpackPath)
            {

            }

            void IDisposable.Dispose()
            {
            }

            private string Unpack(string localFilePath, string unpackPath)
            {
                // Reikia turėti omenyje, kad archyvų viduje yra direkorija.
                throw new NotImplementedException();
            }

            string IPatch.GetFolderPath()
            {
            }
        }

        /// <summary>
        /// Должно скачать патч, раскрыть в папку и вернуть путь к ему.
        /// </summary>
        internal IEnumerable<IPatch> Prepare(string patchGroupName, IEnumerable<PatchInfo> patches, IProgress<string> progress)
        {
            var rootPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            rootPath = Path.Combine(rootPath, Application.CompanyName);
            //path = Path.Combine(path, Application.ProductName);
            rootPath = Path.Combine(rootPath, Path.GetFileNameWithoutExtension(Application.ExecutablePath));
            var unpackPath = Path.Combine(rootPath, "Unpack", patchGroupName);
            rootPath = Path.Combine(rootPath, "Downloads", patchGroupName);

            var ftp = new FTP.FTPHelper(Server);

            foreach (var patch in patches)
            {
                var meta = (IPatchMetadata)patch;
                string fileName = meta.Remote.Name;
                string localFilePath = Path.Combine(rootPath, fileName);
                var fi = new FileInfo(localFilePath);
                //if(fi.Exists && fi.time...)
                ftp.Download(patchGroupName, fileName, localFilePath);
                yield return new LocalPatch(localFilePath, unpackPath);
            }
        }
    }
}
