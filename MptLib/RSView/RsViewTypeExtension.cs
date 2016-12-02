using System;

namespace MPT.RSView
{
    public static class RsViewTypeExtension
    {
        public static T ToEnum<T>(this string value, bool ignoreCase = true)
        {
            try
            {
                return (T)Enum.Parse(typeof(T), value, ignoreCase);
            }
            catch
            {
                return default(T);
            }
        }
    }
}