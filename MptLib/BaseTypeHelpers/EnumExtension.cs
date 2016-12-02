using System;
using System.ComponentModel;
using System.Linq;

namespace MPT.BaseTypeHelpers
{
    public static class EnumExtension
    {
        /// <summary>
        /// Gets the DescriptionAttribute valud of an enum value, if none are found uses the string version of the specified value
        /// </summary>
        public static string GetDescription(this Enum value)
        {
            Type type = value.GetType();

            return GetEnumDescription(value.ToString(), type);
        }

        public static string GetDescriptionForEnum(this Type type, object value)
        {
            return GetEnumDescription(value.ToString(), type);
        }

        private static string GetEnumDescription(string value, Type type)
        {
            var memberInfo = type.GetMember(value);

            if (memberInfo.Length <= 0) 
                return value;
            
            // default to the first member info, it's for the specific enum value
            var info = memberInfo.First().GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault();

            return info != null ? ((DescriptionAttribute)info).Description : value;
        }



        /// <summary>
        /// Gets the DescriptionAttribute valud of an enum value, if none are found uses the string version of the specified value
        /// </summary>
        public static string GetDisplayName(this Enum value)
        {
            var type = value.GetType();
            return GetEnumDisplayName(value.ToString(), type);
        }

        public static string GetDisplayNameForEnum(this Type type, object value)
        {
            return GetEnumDisplayName(value.ToString(), type);
        }

        private static string GetEnumDisplayName(string value, Type type)
        {
            var memberInfo = type.GetMember(value);

            if (memberInfo.Length <= 0)
                return value;

            // default to the first member info, it's for the specific enum value
            var info = memberInfo.First().GetCustomAttributes(typeof(DisplayNameAttribute), false).FirstOrDefault();

            return info != null ? ((DescriptionAttribute)info).Description : value;
        }

    }
}
