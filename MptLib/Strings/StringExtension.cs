using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace MPT.Strings
{
    public static class StringExtension
    {
        private static readonly Regex RegexFormatArgs = new Regex(@"({(?<KEY> [A-Za-z0-9_\.]+)})", 
            RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase);

        public static string FormatDict(this string pattern, IDictionary<string, object> dict)
        {
            if (string.IsNullOrWhiteSpace(pattern))
                return "";

            var replacedPattern = pattern;
            var matches = RegexFormatArgs.Matches(pattern);
            foreach (Match match in matches)
            {
                var argKey = match.Groups["KEY"].Value;
                var dictKey = dict.Keys.FirstOrDefault(x => x.Equals(argKey, StringComparison.InvariantCultureIgnoreCase));
                if (dictKey == null)
                    continue;
                var value = dict[dictKey].ToString();

                replacedPattern = replacedPattern.Replace(string.Format("{{{0}}}", argKey), value);
            }
            return replacedPattern;
        }

        public static Regex OnlyLettersRegex = new Regex("[^a-zA-Z0-9]", RegexOptions.Compiled);

        public static string OnlyLetterAndDigit(this string str)
        {
            //OnlyLettersRegex.Replace(str, "");

            var arr = str.ToCharArray();

            arr = Array.FindAll(arr, (c => (char.IsLetterOrDigit(c)
                                             //|| char.IsWhiteSpace(c)
                                              //|| c == '-'
                                              )));
            return new string(arr);
        }


    }
}
