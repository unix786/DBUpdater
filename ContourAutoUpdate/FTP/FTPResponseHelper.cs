// Contour Enterprise 
// Copyright (c) 2000-2018 Contour Lab
// 2018-04-04 MR
// Файл из "C:\Projects\CE2006\CESvydisBaseImportStockM\FTPResponseHelper.cs".

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace ContourAutoUpdate.FTP.External
{
    /// <summary>
    /// Методы из CESvydisBaseImportStockM.FTPResponseHelper.
    /// </summary>
    internal static class FTPResponseHelper
    {
        public sealed class DirectoryDetails
        {
            public string Permissions { get; set; }
            public string FileCode { get; set; }
            public string Owner { get; set; }
            public string Group { get; set; }
            public string SizeBytes { get; set; }
            public string Month { get; set; }
            public string Day { get; set; }
            public string TimeYear { get; set; }
            public string FileName { get; set; }

            public bool IsDirectory { get { return Permissions[0] == 'd'; } }
        }

        public static DirectoryDetails GetDirectoryDetails(string line)
        {
            string[] tokens = line.Split(new[] { ' ' }, 9, StringSplitOptions.RemoveEmptyEntries);

            if (tokens.Count() >= 9)
            {
                return new DirectoryDetails
                {
                    Permissions = tokens[0],
                    FileCode = tokens[1],
                    Owner = tokens[2],
                    Group = tokens[3],
                    SizeBytes = tokens[4],
                    Month = tokens[5],
                    Day = tokens[6],
                    TimeYear = tokens[7],
                    FileName = tokens[8]
                };
            }
            return null;
        }

        public static FtpWebResponse GetResponse(string url, NetworkCredential credentials, string method, bool usePassive = true)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(url);
            request.Credentials = credentials;
            request.UsePassive = usePassive;
            request.Method = method;

            return (FtpWebResponse)request.GetResponse();
        }

        public static string[] ReadDirectory(string directoryUrl, NetworkCredential credentials, bool usePassive = true)
        {
            List<string> lines = new List<string>();
            string method = WebRequestMethods.Ftp.ListDirectoryDetails;

            using (FtpWebResponse listResponse = GetResponse(directoryUrl, credentials, method, usePassive))
            using (Stream listStream = listResponse.GetResponseStream())
            using (StreamReader listReader = new StreamReader(listStream))
            {
                while (!listReader.EndOfStream)
                {
                    lines.Add(listReader.ReadLine());
                }
            }

            return lines.ToArray();
        }

        public static List<DirectoryDetails> GetDirectoryDetails(string directoryUrl, NetworkCredential credentials, bool usePassive = true)
        {
            List<DirectoryDetails> details = new List<DirectoryDetails>();

            string[] lines = ReadDirectory(directoryUrl, credentials, usePassive);

            foreach (var line in lines)
            {
                var detail = GetDirectoryDetails(line);
                details.Add(detail);
            }

            return details;
        }

        public static void DeleteFile(string fileUrl, NetworkCredential credentilas, bool usePassive = true)
        {
            var deleteRespone = GetResponse(fileUrl, credentilas, WebRequestMethods.Ftp.DeleteFile, usePassive);
            deleteRespone.Close();
        }

        public static string DownloadFile(string fileUrl, string targetPath, NetworkCredential credentials, bool usePassive = true)
        {
            string targetFilePath = targetPath + @"\" + Path.GetFileName(fileUrl);

            using (FtpWebResponse downloadResponse =
              GetResponse(fileUrl, credentials, WebRequestMethods.Ftp.DownloadFile))
            using (Stream sourceStream = downloadResponse.GetResponseStream())
            using (Stream targetStream = File.Create(targetFilePath))
            {
                sourceStream.CopyTo(targetStream);
            }

            return targetFilePath;
        }
    }
}

