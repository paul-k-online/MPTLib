using System;

namespace MPT.PrimitiveType
{
    public static class DateTimeExtension
    {
        //using extension method:
        public static DateTime Trim(this DateTime date, long roundTicks)
        {
            return new DateTime(date.Ticks - date.Ticks % roundTicks);
        }
    }
}
