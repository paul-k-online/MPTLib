using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace MPT.PrimitiveType
{
    public static class EnumExtension
    {
        public static T ToEnum<T>(this bool val) where T : struct, IConvertible /* Enum */
        {
            var t = typeof(T);
            if (!t.IsEnum)
                throw new ArgumentException("Type {0} not Enum", t.ToString());
            return (T)(object)Convert.ToInt32(val);
        }

        public static T ToEnum<T>(this string value, bool ignoreCase = true)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(value))
                    return default(T);
                return (T)Enum.Parse(typeof(T), value, ignoreCase);
            }
            catch
            {
                return default(T);
            }
        }

        public static bool TryToEnum<T>(this string value, out T result, bool ignoreCase = true)
        {
            try
            {
                result = (T)Enum.Parse(typeof(T), value, ignoreCase);
                return true;
            }
            catch (Exception)
            {
                result = default(T);
                return false;
            }
        }
 
        public static string GetDescriptionFromEnumValue(this Enum value)
        {
            DescriptionAttribute attribute = value.GetType()
                .GetField(value.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false)
                .SingleOrDefault() as DescriptionAttribute;
            return attribute == null ? value.ToString() : attribute.Description;
        }

        public static int? GetOrderValueFromEnumValue(this Enum value)
        {
            var attribute = value.GetType()
                .GetField(value.ToString())
                .GetCustomAttributes(typeof(OrderAttribute), false)
                .SingleOrDefault() as OrderAttribute;

            return attribute != null ? attribute.Order : (int?) null;
        }

        public static T GetEnumValueFromDescription<T>(string description)
        {
            var type = typeof(T);
            if (!type.IsEnum)
                throw new ArgumentException();
            var fields = type.GetFields();
            var field = fields
                            .SelectMany(f => f.GetCustomAttributes(
                                typeof(DescriptionAttribute), false), (
                                    f, a) => new { Field = f, Att = a })
                            .Where(a => ((DescriptionAttribute)a.Att)
                                .Description == description).SingleOrDefault();
            return field == null ? default(T) : (T)field.Field.GetRawConstantValue();
        }

        /// <summary>
        /// Gets the DescriptionAttribute valud of an enum value, if none are found uses the string version of the specified value
        /// </summary>
        public static string GetDescription(this Enum value)
        {
            var type = value.GetType();
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
        /// Gets the DescriptionAttribute valud of an enum value, 
        /// if none are found uses the string version of the specified value
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

            return info != null ? ((DisplayNameAttribute)info).DisplayName : value;
        }

    }
}
