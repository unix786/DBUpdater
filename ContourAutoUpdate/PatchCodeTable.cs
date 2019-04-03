using System.Collections.Generic;
using System.Linq;

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
                { "CEAWP"              , "AWP"       },
                { "CEBudget"           , "CB"        },
                { "CECashFlow"         , "CF"        },
                { "CEContract"         , "CC"        },
                { "CEDocFlow"          , "DocFlow"   },
                { "CEElDocFlow"        , "ElDF"      },
                { "CEEnterprise"       , "CE"        },
                { "CELITContract"      , "LC"        },
                { "CELitEnterprise"    , "CE L"      },
                { "CELogistic"         , "CL"        },
                { "CEManufacture"      , "MF"        },
                { "CESvyd"             , "Svyd"      },
                { "CESvydisLT"         , "SvydisLT"  },
                { "CESvydLit"          , "SvydisLit" },
                { "ChangeCurrencyToEUR", "EUR"       },
                { "CTools"             , "CT"        },
                { "PGTran"             , "PGTran"    }, // ...\Database\4.x\Core\SQL Patches\Application\PostgreSQLTransfer\
            };

        /// <summary>
        /// По коду патча в базе данных получает код в архиве.
        /// </summary>
        public Dictionary<string, string> DBCodeDictionary { get; } = internalList.ToDictionary((e) => e.Key, (e) => e.Value);

        /// <summary>
        /// По коду в архиве получает код в базе данных.
        /// </summary>
        public Dictionary<string, string> ArchiveCodeDictionary { get; } = internalList.ToDictionary((e) => e.Value, (e) => e.Key);
    }
}
