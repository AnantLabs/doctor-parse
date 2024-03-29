﻿using System;
using System.Linq;
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

        /// <summary>
        /// Parse string settings
        /// </summary>
        /// <param name="list">string settings</param>
        /// <param name="options">reg ex options</param>
        /// <param name="isFirstMatch"></param>
        public static void RegexOptionsParse(string list, ref RegexOptions options, ref bool isFirstMatch)
        {
            List<string> unknownParameters = null;


            foreach (string value in list.Split('|').Where(i=>!string.IsNullOrEmpty(i)).ToArray<string>())
            {
                // parse regex options
                try
                {
                    options |= (RegexOptions)Enum.Parse(typeof(RegexOptions), value, true); ;
                }
                catch (ArithmeticException)
                {
                    // other options
                    switch (value.ToUpper().Trim())
                    {

                        case "FIRSTMATCH":
                            {
                                isFirstMatch = true;
                            }
                            break;

                        default:
                            {
                               
                                if (unknownParameters == null)
                                    unknownParameters = new List<string>();

                                unknownParameters.Add(value);
                            }
                            break;
                    }
                }

                // unknown agument axception
                if (unknownParameters != null)
                {
                    throw new ArgumentException(string.Format("Unknown parameters ({0})", string.Join(",", unknownParameters.ToArray()) ));
                }
            }


        }
    }
}