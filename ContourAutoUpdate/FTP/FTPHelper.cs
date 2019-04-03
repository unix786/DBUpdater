using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using ExternalHelper = ContourAutoUpdate.FTP.External.FTPResponseHelper;

namespace ContourAutoUpdate.FTP
{
    internal sealed class FTPHelper
    {
        private readonly BaseServerInfo server;

        public FTPHelper(BaseServerInfo server) => this.server = server;

        private FtpWebRequest CreateRequest(string method, string path = null)
        {
            string ReplaceSeparators(string x) => x.Replace("\\", "//");
            string address = ReplaceSeparators(server.Address);

            string ftpScheme = Uri.UriSchemeFtp + Uri.SchemeDelimiter;
            if (!address.StartsWith(ftpScheme)) address = ftpScheme + address;

            Uri uri = new Uri(address);
            if (path != null) uri = new Uri(uri, ReplaceSeparators(path));

            var request = (FtpWebRequest)WebRequest.Create(uri);
            request.Credentials = new NetworkCredential(server.UserName, server.Password);

            request.UsePassive = false; // Su Passive gaunu timeout.
            //request.EnableSsl = true;
            request.Method = method;
            request.Timeout = 500;

            return request;
        }

        private FtpWebResponse GetResponse(FtpWebRequest request)
        {
            var webResponse = request.GetResponse();
            try
            {
                return (FtpWebResponse)webResponse;
            }
            catch
            {
                if (webResponse != null) webResponse.Dispose();
                throw;
            }
        }

        private static ListEntry ConvertDetails(ExternalHelper.DirectoryDetails details)
        {
            long size = -1;
            if (details == null || !long.TryParse(details.SizeBytes, out size)) return null;
            DateTime? date = DateTime.TryParse($"{details.Month} {details.Day} {details.TimeYear}", out var result) ? result : (DateTime?)null;
            return new ListEntry
            {
                Name = details.FileName,
                Size = size,
                Timestamp = date ?? DateTime.MinValue,
                IsDirectory = details.IsDirectory,
            };
        }

        internal IEnumerable<ListEntry> GetFileList(string dirName, IProgress<string> progress)
        {
            var ftpListRequest = CreateRequest("LIST", dirName);
            using (var response = GetResponse(ftpListRequest))
            {
                if (response.StatusCode != FtpStatusCode.OpeningData)
                    throw new Exception($"Cannot connect. Bad FTP server response: {response.StatusDescription}.");

                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        var details = ExternalHelper.GetDirectoryDetails(line);
                        var listEntry = ConvertDetails(details);
                        if (listEntry == null) progress.Report($"Warning: failed to interpret FTP response \"{line}\"!");
                        else if (listEntry.Size > 0 && !listEntry.IsDirectory) yield return listEntry;
                    }
                }
            }
        }
    }
}
