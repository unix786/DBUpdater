using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace ContourAutoUpdate
{
    internal class PatchProvider
    {
        public PatchServerInfo Server { get; }
        public PatchProvider(PatchServerInfo server) => Server = server;

        private FtpWebRequest CreateRequest(string method, string path = null)
        {
            string ReplaceSeparators(string x) => x.Replace("\\", "//");
            string address = ReplaceSeparators(Server.Address);

            string ftpScheme = Uri.UriSchemeFtp + Uri.SchemeDelimiter;
            if (!address.StartsWith(ftpScheme)) address = ftpScheme + address;

            Uri uri = new Uri(address);
            if (path != null) uri = new Uri(uri, ReplaceSeparators(path));

            var request = (FtpWebRequest)WebRequest.Create(uri);
            request.Credentials = new NetworkCredential(Server.UserName, Server.Password);

            request.UsePassive = false;
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

        /// <summary>
        /// Префиксы файлов (коды патчов) в FTP.
        /// </summary>
        private static readonly Dictionary<string, PatchCode> patchCodes =
            new Dictionary<string, PatchCode>
            {
                { "AWP"      , PatchCode.CEAWP                },
                { "CB"       , PatchCode.CEBudget             },
                { "CF"       , PatchCode.CECashFlow           },
                { "CC"       , PatchCode.CEContract           },
                { "DocFlow"  , PatchCode.CEDocFlow            },
                { "ElDF"     , PatchCode.CEElDocFlow          },
                { "CE"       , PatchCode.CEEnterprise         },
                { "LC"       , PatchCode.CELITContract        },
                { "CE L"     , PatchCode.CELitEnterprise      },
                { "CL"       , PatchCode.CELogistic           },
                { "MF"       , PatchCode.CEManufacture        },
                { "Svyd"     , PatchCode.CESvyd               },
                { "SvydisLT" , PatchCode.CESvydisLT           },
                { "SvydisLit", PatchCode.CESvydLit            },
                { "EUR"      , PatchCode.ChangeCurrencyToEUR  },
                { "CT"       , PatchCode.CTools               },
                { "PGTran"   , PatchCode.PGTran               }, // ...\Database\4.x\Core\SQL Patches\Application\PostgreSQLTransfer\
            };

        /// <summary>
        /// Должен определить номер последнего установленного патча.
        /// </summary>
        internal int GetPatchNumber(string patchGroupCode, Dictionary<PatchCode, PatchVersion> versions, IProgress<string> progress)
        {
            var ftp = CreateRequest("LIST", patchGroupCode);
            using (var response = GetResponse(ftp))
            {
                if (response.StatusCode != FtpStatusCode.OpeningData)
                    throw new Exception($"Cannot connect. Bad FTP server response: {response.StatusDescription}.");

                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    progress.Report("Patches in " + patchGroupCode + ":");
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        progress.Report(line);
                    }
                    //throw new NotImplementedException();
                    return 0;
                }
            }
        }
    }
}
