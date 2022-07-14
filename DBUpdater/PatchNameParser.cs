using System;
using System.Collections;
using System.Collections.Generic;

namespace DBUpdater
{
    internal sealed class PatchNameParser
    {
        private enum State
        {
            Initial,
            Number,
            Subnumber,
            CodeSeparator,
            Code,
            VersionSeparator,
            Version,
        }

        private static bool IsNumber(char c) => Char.IsNumber(c);
        private static bool IsSubnumberseparator(char c) => c == '_';
        private static bool IsCodeSeparator(char c) => Char.IsWhiteSpace(c);
        private static bool IsVersionSeparator(char c) => Char.IsWhiteSpace(c) || c == '.';

        public static bool TryParse(string name, out float number, out string code, out PatchVersion version)
        {
            var state = State.Initial;
            int codeStartIdx = 0;
            char c;
            decimal dNumber = 0;
            number = 0;
            code = null;
            version = null;
            for (int idx = 0; idx < name.Length; idx++)
            {
                c = name[idx];
                switch (state)
                {
                    case State.Initial:
                        if (IsNumber(c))
                        {
                            state = State.Number;
                            goto case State.Number;
                        }
                        else
                        {
                            if (Char.IsWhiteSpace(c)) break;
                            else return false;
                        }
                    case State.Number:
                    case State.Subnumber:
                        if (IsNumber(c))
                        {
                            int nextDigit = (int)Char.GetNumericValue(c);

                            if (state == State.Number) dNumber = 10 * dNumber + nextDigit;
                            else dNumber += nextDigit * 0.1m;
                            number = (float)dNumber;
                            if ((decimal)number != dNumber) return false; // Слишком много чисел.
                        }
                        else if (IsCodeSeparator(c)) state = State.CodeSeparator;
                        else if (IsSubnumberseparator(c)) state = State.Subnumber;
                        else return false;
                        break;
                    case State.CodeSeparator:
                        if (!IsCodeSeparator(c))
                        {
                            state = State.Code;
                            codeStartIdx = idx;
                        }
                        break;
                    case State.Code:
                        if (IsVersionSeparator(c))
                        {
                            state = State.VersionSeparator;
                            code = name.Substring(codeStartIdx, idx - codeStartIdx);
                        }
                        break;
                    case State.VersionSeparator:
                        if (Char.IsNumber(c))
                        {
                            state = State.Version;
                            var nums = name.Substring(idx).Split('.');
                            if (nums.Length >= 3 && int.TryParse(nums[0], out var appVersion) && int.TryParse(nums[1], out var build) && int.TryParse(nums[2], out var patch))
                            {
                                version = new PatchVersion(appVersion, build, patch);
                                return true;
                            }
                            else if ((nums.Length == 2 || nums.Length == 3) && int.TryParse(nums[0], out var first) && int.TryParse(nums[1], out var second))
                            {
                                version = new PatchVersion(first, 0, second); // Это у EMP есть несколько таких патчов.
                                return true;
                            }
                            return false;
                        }
                        else if (state == State.VersionSeparator && !IsVersionSeparator(c))
                        {
                            state = State.Code;
                        }
                        break;
                    default:
                        return false;
                }
            }
            return false;
        }

        private class TestCaseCollection : IEnumerable<KeyValuePair<string, PatchProvider.PatchInfo>>
        {
            private readonly List<KeyValuePair<string, PatchProvider.PatchInfo>> list =
                new List<KeyValuePair<string, PatchProvider.PatchInfo>>();

            public IEnumerator<KeyValuePair<string, PatchProvider.PatchInfo>> GetEnumerator() => list.GetEnumerator();
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public void Add(string fileName, float number, string code, int version, int build, int patch)
            {
                list.Add(new KeyValuePair<string, PatchProvider.PatchInfo>(fileName, new PatchProvider.PatchInfo
                {
                    Number = number,
                    ArchiveCode = code,
                    Version = new PatchVersion(version, build, patch)
                }));
            }
        }

        private static void Test()
        {
            foreach (var item in new TestCaseCollection
            {
                { "076 CE.07.001.012.rar", 76, "CE", 7, 1, 12 },
                { "085 GY 07.001.000", 85, "GY", 7, 1, 0 },
                { "086 gY 07.001.001.rar", 86, "gY", 7, 1, 1 },
                { "1489 CE L.08.005.085.rar", 1489, "CE L", 8, 5, 85 },
                { "011 EMP.06.001.rar", 11, "EMP", 6, 0, 1 },
                { "000_2 CE.08.005.122.rar", 0.2f, "CE", 8, 5, 122 },
                //{ "16777216 max 1.2.3", 16777216f, "max", 1, 2, 3 }
                { "16777_6 a 1.2.3", 16777.6f, "a", 1, 2, 3 }
            })
            {
                try
                {
                    if (!TryParse(item.Key, out var number, out string code, out PatchVersion version)) throw new Exception("did not parse");
                    if (number != item.Value.Number) throw new Exception(nameof(number));
                    if (code != item.Value.ArchiveCode) throw new Exception(nameof(code));
                    if (!version.Equals(item.Value.Version)) throw new Exception(nameof(version));
                }
                catch (Exception ex)
                {
                    throw new Exception(item.Key, ex);
                }
            }
        }
    }
}
