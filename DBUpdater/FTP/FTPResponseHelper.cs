// Contour Enterprise 
// Copyright (c) 2000-2018 Contour Lab
// 2018-04-04 MR
// Файл из "C:\Projects\CE2006\CESvydisBaseImportStockM\FTPResponseHelper.cs".

using System;
using System.Linq;

namespace DBUpdater.FTP.External
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
    }
}

