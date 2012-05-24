using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Dr.Parser
{
    public static class RegExUtility
    {

        public static void RegexOptionsParse(string list, ref RegexOptions options)
        {
            bool isFirstMath = false;
            RegexOptionsParse(list, ref  options, ref isFirstMath);
        }

        public static void RegexOptionsParse(string list, ref RegexOptions options, ref bool IsFirstMatch)
        {
            foreach (string _opt in list.Split('|'))
            {
                switch (_opt.ToUpper().Trim())
                {
                    case "IGNORECASE":
                        {
                            options |= RegexOptions.IgnoreCase;
                        }
                        break;
                    case "IGNOREWHITESPACE":
                        {
                            options |= RegexOptions.IgnorePatternWhitespace;
                        }
                        break;
                    case "SINGLELINE":
                        {
                            options |= RegexOptions.Singleline;
                        }
                        break;
                    case "MULTILINE":
                        {
                            options |= RegexOptions.Multiline;
                        }
                        break;
                    case "FIRSTMATCH":
                        {
                            IsFirstMatch = true;
                        }
                        break;
                }
            }
        }
    }
}