namespace ContourAutoUpdate
{
    internal class PatchCodeTable
    {
        /// <summary>
        /// Словарь: Общий код, Код в БД, Код в названии патча.
        /// </summary>
        private static readonly Map<string, string> internalList =
            new Map<string, string>
            {
                { "AWP"        , "CEAWP"              },
                { "CB"         , "CEBudget"           },
                { "CF"         , "CECashFlow"         },
                { "CC"         , "CEContract"         },
                { "DocFlow"    , "CEDocFlow"          },
                { "ElDF"       , "CEElDocFlow"        },
                { "CE"         , "CEEnterprise"       },
                { "LC"         , "CELITContract"      },
                { "CE L"       , "CELitEnterprise"    },
                { "CL"         , "CELogistic"         },
                { "MF"         , "CEManufacture"      },
                { "Svyd"       , "CESvyd"             },
                { "SvydisLT"   , "CESvydisLT"         },
                { "SvydisLit"  , "CESvydLit"          },
                { "EUR"        , "ChangeCurrencyToEUR"},
                { "CT"         , "CTools"             },
                { "PGTran"     , "PGTran"             }, // ...\Database\4.x\Core\SQL Patches\Application\PostgreSQLTransfer\
            };

        public bool TryGetDBCode(string archiveCode, out string code) => internalList.Forward.TryGetValue(archiveCode, out code);
    }
}
