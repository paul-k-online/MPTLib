using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace MPT.PrimitiveType
{
    public static class StringExtension
    {
        public static string ToNullString(this object obj)
        {
            if (obj == null || Convert.IsDBNull(obj))
                return null;
            return Convert.ToString(obj);
        }

        public static string Format(this string formatString, params object[] args)
        {
            return string.Format(formatString, args);
        }


        private static readonly Regex RegexFormatArgs = new Regex(@"({(?<KEY> [a-zA-Z0-9_\.]+)})",
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
                var dictKey =
                    dict.Keys.FirstOrDefault(x => x.Equals(argKey, StringComparison.InvariantCultureIgnoreCase));
                if (dictKey == null)
                    continue;
                var value = dict[dictKey].ToString();

                replacedPattern = replacedPattern.Replace(string.Format("{{{0}}}", argKey), value);
            }
            return replacedPattern;
        }


        public static string OnlyLetterAndDigit(this string str)
        {
            var OnlyLettersRegex = new Regex("[^a-zA-Z0-9]", RegexOptions.Compiled);

            if (string.IsNullOrWhiteSpace(str))
                return string.Empty;

            var letterOrDigitArr = Array.FindAll(str.ToCharArray(), char.IsLetterOrDigit);
            return new string(letterOrDigitArr);
        }
    }
}
