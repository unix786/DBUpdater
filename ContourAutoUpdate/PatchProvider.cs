using System;
using System.Collections.Generic;

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
            string LocalFile { get; set; }
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

        /// <summary>
        /// Должно скачать патч, раскрыть в папку и вернуть путь к ему.
        /// </summary>
        internal IEnumerable<IPatch> Prepare(IEnumerable<PatchInfo> patches, IProgress<string> progress)
        {
            throw new NotImplementedException();
        }
    }
}
