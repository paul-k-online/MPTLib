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
        
        public static string GetAiItemAddress(this AiPosition position, string item)
        {
            return string.Format(AiPositionItemTemplate, position.Number, item);
        }
    }
}
