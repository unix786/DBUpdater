using System.Collections;
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
        /// Код в названии патча : код в БД, .
        /// </summary>
        private static readonly Dictionary<string, string> internalList =
            new Dictionary<string, string>
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
