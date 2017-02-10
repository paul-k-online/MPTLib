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

        public static Regex OnlyLetterAndDigitRegex = new Regex("[^a-zA-Z0-9]", RegexOptions.Compiled|RegexOptions.IgnoreCase);
        public static string OnlyLetterAndDigit(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return string.Empty;
            var letterOrDigitArr = Array.FindAll(str.ToCharArray(), char.IsLetterOrDigit);
            return new string(letterOrDigitArr);
        }
    }
}
