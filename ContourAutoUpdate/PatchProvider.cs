using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using SevenZip;

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
            //string IPatchMetadata.LocalFile { get; set; }

            public DateTime? Timestamp => listEntry == null || listEntry.Timestamp == DateTime.MinValue ? (DateTime?)null : listEntry.Timestamp;

            public long? FileSize => listEntry?.Size;

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
            private readonly string archivePath;
            private readonly string unpackRoot;

            public LocalPatch(string arhivePath, string unpackRoot)
            {
                this.archivePath = arhivePath;
                this.unpackRoot = unpackRoot;
            }

            void IDisposable.Dispose()
            {
                try
                {
                    // Это не всё удалит, если в архиве было несколько папок.
                    if (patchFolder != null) File.Delete(patchFolder);
                }
                catch (Exception ex)
                {
                    Console.Write($"{ex.GetType().Name}: {ex.Message}");
                }
            }

            private string patchFolder;
            string IPatch.GetFolderPath()
            {
                using (var extractor = new SevenZipExtractor(archivePath))
                {
                    string unpackRoot = this.unpackRoot;
                    patchFolder = null;
                    foreach (var archiveFile in extractor.ArchiveFileData)
                    {
                        if (archiveFile.IsDirectory)
                        {
                            bool isRoot = Path.GetDirectoryName(archiveFile.FileName) == String.Empty;
                            if (isRoot)
                            {
                                patchFolder = Path.Combine(unpackRoot, archiveFile.FileName);
                                break;
                            }
                        }
                        else
                        {
                            var fileName = Path.GetFileName(archiveFile.FileName);
                            if (fileName.Equals("0.sql", StringComparison.OrdinalIgnoreCase) || fileName.Equals("000.sql", StringComparison.OrdinalIgnoreCase))
                            {
                                if (archiveFile.FileName == fileName) unpackRoot = patchFolder = Path.Combine(unpackRoot, Path.GetFileNameWithoutExtension(archivePath));
                                else patchFolder = Path.Combine(unpackRoot, Path.GetDirectoryName(archiveFile.FileName));
                                break;
                            }
                        }
                    }
                    if (patchFolder == null) throw new FileLoadException($"Unsupported file structure in \"{archivePath}\".");
                    if (!Directory.Exists(patchFolder)) extractor.ExtractArchive(unpackRoot);
                    return patchFolder;
                }
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
            var unpackRoot = Path.Combine(rootPath, "Unpack", patchGroupName);
            rootPath = Path.Combine(rootPath, "Downloads", patchGroupName);

            var ftp = new FTP.FTPHelper(Server);

            foreach (var patch in patches)
            {
                var meta = (IPatchMetadata)patch;
                string fileName = meta.Remote.Name;
                string localFilePath = Path.Combine(rootPath, fileName);
                var fi = new FileInfo(localFilePath);
                if (!fi.Exists || fi.CreationTime < patch.Timestamp || fi.Length != patch.FileSize)
                {
                    progress.Report($"Downloading {fileName}");
                    ftp.Download(patchGroupName, fileName, localFilePath);
                }
                yield return new LocalPatch(localFilePath, unpackRoot);
            }
        }
    }
}
