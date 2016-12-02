using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MPT.Model;

namespace MPT.RSLogix
{
    public static class PositionExtension
    {
        public static string GetAiItemAddress(this AiPosition position, string item)
        {
            const string format = "AI[{0}].{1}";
            return string.Format(format, position.Number, item);
        }

        public static string GetAoItemAddress(this AoPosition position, string item)
        {
            const string format = "AO[{0}].{1}";
            return string.Format(format, position.Number, item);
        }

        public static string GetDioItemAddress(this DioPosition position, string item)
        {
            const string format = "DIO[{0}].{1}";
            return string.Format(format, position.Number, item);
        }
    }
}
