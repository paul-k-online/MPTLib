using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace MPT.PrimitiveType
{
    public static class StringToIntList
    {
        private static Regex SplitNumbersRegex = new Regex(@"\D+", RegexOptions.Compiled);
        public static IEnumerable<int> SplitToIntList(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return null;
            var numberList = SplitNumbersRegex.Split(str);
            return numberList.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => Convert.ToInt32(x)).ToList();
        }
    }
}
