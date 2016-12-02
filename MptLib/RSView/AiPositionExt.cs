using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using MPT.Model;

namespace MPT.RSView
{
    public static class AiPositionExt
    {
        static readonly Regex NameRegex = new Regex(@"^  ((?<Prefix>\d+)[-_\.%])?   (?<Letters>[A-Za-z]+) [-_\.]? (?<Digits>\d+)  ([-_\.\\\/%](?<Postfix>\d+))?$", RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase);

        public static string RSViewName(this AiPosition position)
        {
            var m = NameRegex.Match(position.Name);
            if (!m.Success) 
                return position.Name;

            var prefix = m.Groups["Prefix"];
            var letters = m.Groups["Letters"];
            var digits = m.Groups["Digits"];
            var postfix = m.Groups["Postfix"];

            var sb = new StringBuilder();
            if (prefix.Success)
                sb.Append(prefix.Value).Append("-");
            if (letters.Success)
                sb.Append(letters.Value[0]);
            if (digits.Success)
                sb.Append(digits.Value);
            if (postfix.Success)
                sb.Append("_").Append(postfix.Value);

            return sb.ToString();
        }
    }
}
