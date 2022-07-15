using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using CECommon;
using ExternalHelper = DBUpdater.FTP.External.FTPResponseHelper;

namespace DBUpdater.FTP
{
    internal sealed class FTPHelper
    {
        private readonly BaseServerInfo server;

        public FTPHelper(BaseServerInfo server) => this.server = server;

        private FtpWebRequest CreateRequest(string method, string path = null)
        {
            var uri = GetRequestUri(path);

            var request = (FtpWebRequest)WebRequest.Create(uri);
            request.Credentials = new NetworkCredential(server.UserName, server.Password);

            request.UsePassive = false; // Su Passive gaunu timeout.
            //request.EnableSsl = true;
            request.Method = method;
            request.Timeout = 1000 * server.Timeout;

            return request;
        }

        private FtpWebResponse GetResponse(FtpWebRequest request)
        {
            WebResponse webResponse;
            try
            {
                webResponse = request.GetResponse();
            }
            catch (Exception ex)
            {
                throw new Exception($"FTP exception (method {request.Method}): {ex.Message}", ex);
            }
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

        private FtpWebResponse GetResponse(string method, string path = null) => GetResponse(CreateRequest(method, path));

        private static ListEntry ConvertDetails(ExternalHelper.DirectoryDetails details)
        {
            long size = -1;
            if (details == null || !long.TryParse(details.SizeBytes, out size)) return null;
            DateTime? date = DateTime.TryParse(
                details.TimeYear.Contains(":") ?
                $"{details.Month} {details.Day} {DateTime.Today.Year} {details.TimeYear}"
                : $"{details.Month} {details.Day} {details.TimeYear}",
                out var result
                ) ?
                result : (DateTime?)null;
            if (date.HasValue && date.Value.Date > DateTime.Today) date = date.Value.AddYears(-1);
            return new ListEntry
            {
                Name = details.FileName,
                Size = size,
                Timestamp = date,
                IsDirectory = details.IsDirectory,
            };
        }

        internal Uri GetRequestUri(string path = null)
        {
            if (!UriHelper.TryCreate(server.Address, out Uri uri, Uri.UriSchemeFtp))
                throw new ArgumentOutOfRangeException($"Invalid FTP address: {server.Address}");
            if (path != null) uri = UriHelper.Combine(uri, path);
            return uri;
        }

        internal IEnumerable<ListEntry> GetFileList(string dirName, IProgress<string> progress)
        {
            var ftpListRequest = CreateRequest(WebRequestMethods.Ftp.ListDirectoryDetails, dirName);
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

        internal void Test(IProgress<string> progress)
        {
            var request = CreateRequest(WebRequestMethods.Ftp.ListDirectoryDetails);
            progress.Report("Connecting to " + request.RequestUri);
            using(var response = GetResponse(request))
            {
                var msg = new StringBuilder("FTP server responded:").AppendLine();
                if(response.BannerMessage.Length > 0) msg.AppendLine(response.BannerMessage);
                if(response.WelcomeMessage.Length > 0) msg.AppendLine(response.WelcomeMessage);
                progress.Report(msg.ToString());
            }
        }

        internal void Download(string patchGroupCode, string fileName, string localFilePath)
        {
            using (var response = GetResponse(WebRequestMethods.Ftp.DownloadFile, Path.Combine(patchGroupCode, fileName)))
            using (var responseStream = response.GetResponseStream())
            {
                Directory.CreateDirectory(Path.GetDirectoryName(localFilePath));
                using (var targetStream = File.Create(localFilePath))
                {
                    responseStream.CopyTo(targetStream);
                }
            }
        }
    }
}
