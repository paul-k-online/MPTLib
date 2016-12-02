using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MPT.Model;

namespace MPT.RSLogix
{
    public static class PositionExtension
    {
        public const string AiPositionItemTemplate = "AI[{0}].{1}";

        public const string AiPositionSubItemTemplate = "AI[{0}].{1}.{2}";


        public static string GetItemAddress(this AiPosition position, string item)
        {
            return string.Format(AiPositionItemTemplate, position.Number, item);
        }

        public static string GetItemAddress(this AiPosition position, string subItem, string item)
        {
            return string.Format(AiPositionSubItemTemplate, position.Number,subItem, item);
        }
    }
}
