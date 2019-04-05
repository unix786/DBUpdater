﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ContourAutoUpdate.State;

namespace ContourAutoUpdate
{
    internal sealed class PatchCodeInfo : ISaveable
    {
        public string ArchiveCode { get; private set; }
        /// <summary>Может быть null.</summary>
        public string DBCode { get; private set; }
        public bool ShouldSkip { get; private set; }

        public PatchCodeInfo(string archiveCode, string dbCode, bool shouldSkip)
        {
            ArchiveCode = archiveCode;
            DBCode = dbCode;
            ShouldSkip = shouldSkip;
        }

        public PatchCodeInfo() { }

        public override string ToString() => $"{ArchiveCode} -> {DBCode ?? "<unknown>"}{(ShouldSkip ? " (skip)" : null)}";

        void ISaveable.Save(IWriter writer, string name)
        {
            using (var section = writer.Section(name))
            {
                section.Write(nameof(ArchiveCode), ArchiveCode);
                section.Write(nameof(DBCode), DBCode);
                section.WriteBoolean(nameof(ShouldSkip), ShouldSkip);
            }
        }

        void ISaveable.Load(IWriter writer, string name)
        {
            using (var section = writer.Section(name))
            {
                ArchiveCode = section.Read(nameof(ArchiveCode));
                DBCode = section.Read(nameof(DBCode));
                ShouldSkip = section.ReadBoolean(nameof(ShouldSkip));
            }
        }
    }

    internal class PatchCodeTable : IEnumerable<PatchCodeInfo>, ISaveable
    {
        /// <summary>
        /// Код в названии патча : код в БД.
        /// </summary>
        private static readonly Dictionary<string, string> internalList =
            new Dictionary<string, string>
            {
                { "CT"          , "CTools"              },
                { "CE"          , "CEEnterprise"        },
                { "CE L"        , "CELitEnterprise"     },
                { "LC"          , "CELITContract"       },
                { "CE R"        , "CERusEnterprise"     },

                // Application
                { "AS"          , "CEAutoService"       },
                { "AWP"         , "CEAWP"               },
                { "BT"          , "CEBalans"            },
                { "CB"          , "CEBudget"            },
                { "CF"          , "CECashFlow"          },
                { "EUR"         , "ChangeCurrencyToEUR" },
                { "CC"          , "CEContract"          },
                { "DocFlow"     , "CEDocFlow"           },
                { "ElDF"        , "CEElDocFlow"         },
                { "FL"          , "CEFixByLots"         },
                { "GoodYear"    , "CEEmpYearReport"     },
                { "Hot"         , "CEHot"               },
                { "CL"          , "CELogistic"          },
                { "LVA"         , "CELVA"               },
                { "MF"          , "CEManufacture"       },
                { "PGTran"      , "PGTran"              },
                { "TR"          ,  "CETracker"          },

                // Solutions
                { "ACM"         , "CEViginta"           },
                { "AF"          , "CEAF"                },
                { "Alkesta"     , "CEAlkesta"           },
                { "Alovex"      , "CEAlovex"            },
                { "AML"         , "CEAML"               },
                { "Ard"         , "CEArdena"            },
                { "ATH"         , "CEAth"               },
                { "AuP"         , "CEAuP"               },
                { "AP"          , "CEAP"                },
                { "AV"          , "CEAV"                },
                { "BR"          , "CEBR"                },
                { "Camargo"     , "CECamargo"           },
                { "Castrade"    , "CECastrade"          },
                //{ "CDM"         , null                  },
                { "DIEM"        , "CEDiem"              },
                { "EMP"         , "CEEmp"               },
                { "FG"          , "CEFG"                },
                { "Hosp"        , "CEHospital"          },
                { "IPV"         , "CEIPV"               },
                { "Lavango"     , "CELavango"           },
                { "Lindstrem"   , "CELindstrem"         },
                { "Mediena"     , "CEMediena"           },
                { "Merlana"     , "CEMerlana"           },
                { "MT"          , "CEMT"                },
                { "PLAZA"       , "CEPlaza"             },
                { "Rud"         , "CERud"               },
                { "Salprone"    , "CESalprone"          },
                { "SForm"       , "CESForm"             },
                { "Stebule"     , "CEStebule"           },
                { "STP"         , "CESTP"               },
                { "Svyd"        , "CESvyd"              },
                { "SvydisLit"   , "CESvydLit"           },
                { "SvydisLT"    , "CESvydisLT"          },
                { "SvydisLV"    , "CESvydisLV"          },
                { "T-Way"       , "CET-Way"             },
                { "Tasty"       , "CETasty"             },
                { "UA"          , "UA"                  },
                { "Vestchema"   , "CEVestchema"         },
                { "Vidukle"     , "CEVidukle"           },
                { "VG"          , "CEViginta"           },
                //{ "VTsistema"   , null                  },
            };

        private Dictionary<string, PatchCodeInfo> dict = internalList.ToDictionary((x) => x.Key, (x) => new PatchCodeInfo(x.Key, x.Value, false));

        internal void ReplaceTable(Dictionary<string, PatchCodeInfo> newList)
        {
            dict = newList;
        }

        public PatchCodeInfo this[string archiveCode] => dict.TryGetValue(archiveCode, out var info) ? info : new PatchCodeInfo(archiveCode, null, false);

        public IEnumerator<PatchCodeInfo> GetEnumerator() => dict.Values.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        void ISaveable.Save(IWriter writer, string name)
        {
            writer.Save(name, dict.Values);
        }

        void ISaveable.Load(IWriter writer, string name)
        {
            var codes = new List<PatchCodeInfo>();
            writer.Load(name, codes);

            dict.Clear();
            foreach (var item in codes) dict[item.ArchiveCode] = item;
        }

        internal PatchCodeTable Clone() => new PatchCodeTable { dict = new Dictionary<string, PatchCodeInfo>(dict) };
    }
}
