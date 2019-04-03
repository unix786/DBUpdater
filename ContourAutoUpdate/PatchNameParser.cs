using System;

namespace ContourAutoUpdate
{
    internal sealed class PatchNameParser
    {
        private enum State
        {
            Initial,
            Number,
            CodeSeparator,
            Code,
            VersionSeparator
        }

        public static bool TryParse(string name, out int number, out string code, out PatchVersion version)
        {
            var state = State.Initial;
            int startIdx = 0;
            char c;
            number = 0;
            code = null;
            version = null;
            for (int idx = 0; idx < name.Length; idx++)
            {
                c = name[idx];
                switch (state)
                {
                    case State.Initial:
                        if (Char.IsNumber(c))
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
                        if (Char.IsNumber(c)) number = 10 * number + (int)Char.GetNumericValue(c);
                        else if (Char.IsWhiteSpace(c)) state = State.CodeSeparator;
                        else return false;
                        break;
                    case State.CodeSeparator:
                        if (!Char.IsWhiteSpace(c))
                        {
                            state = State.Code;
                            startIdx = idx;
                        }
                        break;
                    case State.Code:
                        if (Char.IsWhiteSpace(c) || c == '.')
                        {
                            state = State.VersionSeparator;
                            code = name.Substring(startIdx, idx - startIdx);
                        }
                        break;
                    case State.VersionSeparator:
                        if (Char.IsNumber(c))
                        {
                            var nums = name.Substring(idx).Split('.');
                            if (nums.Length > 3 && int.TryParse(nums[0], out var appVersion) && int.TryParse(nums[1], out var build) && int.TryParse(nums[2], out var patch))
                            {
                                version = new PatchVersion(appVersion, build, patch);
                                return true;
                            }
                            return false;
                        }
                        break;
                    default:
                        return false;
                }
            }
            return false;
        }
    }
}
