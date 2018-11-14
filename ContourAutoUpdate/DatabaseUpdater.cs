using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CECommon.DataAccess;

namespace ContourAutoUpdate
{
    internal class DatabaseUpdater
    {
        private readonly PatchProvider patchProvider;
        public DatabaseUpdater(PatchProvider patchProvider)
        {
            this.patchProvider = patchProvider;
        }

        private static readonly Dictionary<string, PatchCode> patchCodes =
            new Dictionary<string, PatchCode>
            {
                { "CEAWP"                , PatchCode.CEAWP                },
                { "CEBudget"             , PatchCode.CEBudget             },
                { "CECashFlow"           , PatchCode.CECashFlow           },
                { "CEContract"           , PatchCode.CEContract           },
                { "CEDocFlow"            , PatchCode.CEDocFlow            },
                { "CEElDocFlow"          , PatchCode.CEElDocFlow          },
                { "CEEnterprise"         , PatchCode.CEEnterprise         },
                { "CELITContract"        , PatchCode.CELITContract        },
                { "CELitEnterprise"      , PatchCode.CELitEnterprise      },
                { "CELogistic"           , PatchCode.CELogistic           },
                { "CEManufacture"        , PatchCode.CEManufacture        },
                { "CESvyd"               , PatchCode.CESvyd               },
                { "CESvydisLT"           , PatchCode.CESvydisLT           },
                { "CESvydLit"            , PatchCode.CESvydLit            },
                { "ChangeCurrencyToEUR"  , PatchCode.ChangeCurrencyToEUR  },
                { "CTools"               , PatchCode.CTools               },
                { "PGTran"               , PatchCode.PGTran               },
            };

        public Task Update(DatabaseServerInfo serverInfo, string databaseName, string patchGroupName, IProgress<string> progress)
        {
            return Task.Run(() =>
            {
                var dbProvider = CEProvider.CreateProvider(DBSettings.DBProvider, serverInfo.Address, databaseName, 2);
                CEContext ctx;
                if (serverInfo.UseDBLogin) ctx = dbProvider.CreateContext(serverInfo.UserName, serverInfo.Password, null);
                else ctx = dbProvider.CreateContext();
                var versions = new Dictionary<PatchCode, PatchVersion>();

                var selInstalledVersions = new SCmd(ctx, "SELECT * FROM ABIServApplication");
                using (var table = selInstalledVersions.RunRetDataTable())
                using (var reader = table.CreateDataReader())
                {
                    while (reader.Read())
                    {
                        string code = (string)reader["Code"];
                        var version = new PatchVersion(
                            (int)reader["Version"],
                            (int)reader["Build"],
                            (int)reader["Patch"]
                            );
                        if (patchCodes.TryGetValue(code, out PatchCode patchCode))
                        {
                            versions[patchCode] = version;
                        }
                        else
                        {
                            throw new Exception($"Unknown patch code \"{code}\" (version {version}).");
                        }
                    }
                }

                progress.Report("Versions in " + databaseName);
                foreach (var item in versions) progress.Report($"{item.Key}: {item.Value}");

                int number = patchProvider.GetPatchNumber(patchGroupName, versions, progress);
                progress.Report($"Patch number {number}.");

                progress.Report("Process complete.");
            });
        }
    }
}
