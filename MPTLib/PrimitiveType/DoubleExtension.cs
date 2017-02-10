using System;

namespace MPT.PrimitiveType
{
    public static class DoubleExtension
    {
        public static double SoftRound(this double v, int softDigits = 3)
        {
            try
            {
                var log10 = Math.Log10(Math.Abs(v));
                var fl = Math.Floor(log10);
                var exp = Convert.ToInt32(log10);

                var digits = 0;
                if (exp <= -(softDigits))
                    digits = Math.Abs(exp) + 1;
                else if (exp < 0)
                    digits = Math.Abs(exp) + softDigits - 1;
                else if (exp <= softDigits)
                    digits = softDigits - exp;
                else if (exp > softDigits)
                    digits = 0;
                return Math.Round(v, digits);
            }
            catch (Exception)
            {
                return v;
            }
        }
    }
}
