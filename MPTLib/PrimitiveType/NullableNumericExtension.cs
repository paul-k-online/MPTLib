using System;

namespace MPT.PrimitiveType
{
    public static class NullableNumericExtension
    {
        public static double? NullDoubleTryParse(this string input)
        {
            double result;
            var success = double.TryParse(input, out result);
            return success ? (double?)result : null;
        }

        public static int? NullIntTryParse(this string input)
        {
            int result;
            var success = int.TryParse(input, out result);
            return success ? (int?)result : null;
        }

        public static short? NullShortTryParse(this string input)
        {
            short result;
            var success = short.TryParse(input, out result);
            return success ? (short?)result : null;
        }
        
        public static T? ToNullNumeric<T>(this object obj) where T : struct 
        {
            if (obj == null || Convert.IsDBNull(obj)) 
                return null;
            
            if (obj is string)
                if (string.IsNullOrWhiteSpace(obj.ToString()))
                    return null;

            double d;
            if (!double.TryParse(obj.ToString(), out d))
                return null;

            return (T)Convert.ChangeType(d, typeof(T));
        }
    }
}