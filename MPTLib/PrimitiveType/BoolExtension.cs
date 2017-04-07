using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPT.PrimitiveType
{
    public static class BoolExtension
    {
        public static bool ToRSViewBool(this string value)
        {
            switch (value.ToLower())
            {
                case "1":
                case "t":
                case "true":
                case "on":
                case "y":
                case "yes":
                    return true;
                case "0":
                case "f":
                case "false":
                case "off":
                case "n":
                case "no":
                    return false;
                default:
                    //throw new InvalidCastException("You can't cast a weird value to a bool!");
                    return false;
            }
        }
    }
}
