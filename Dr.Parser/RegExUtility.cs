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
            foreach (string value in list.Split('|'))
            {
                // parse regex options
                object option = Enum.Parse(typeof(RegexOptions), value, true);
                if (option != null)
                {
                    options |= (RegexOptions)option;
                }
                else
                {
                    // other options
                    switch (value.ToUpper().Trim())
                    {
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
}