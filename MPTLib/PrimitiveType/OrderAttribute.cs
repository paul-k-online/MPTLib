using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MPT.PrimitiveType
{
    [AttributeUsage(AttributeTargets.Field)]
    public class OrderAttribute : Attribute
    {
        public readonly int Order;

        public OrderAttribute(int order)
        {
            Order = order;
        }

        public static int? GetOrderValueAttribute<T>(T t) where T : struct
        {
            var type = t.GetType();
            var attrType = typeof (OrderAttribute);
            if (!Attribute.IsDefined(type, attrType)) 
                return null;

            var attributeValue = Attribute.GetCustomAttribute(type, typeof(OrderAttribute)) as OrderAttribute; // получаем значение атрибута
            if (attributeValue != null) 
                return attributeValue.Order;
            return null;
        }

        public class ByOrderAttribyteComparer<T> : IComparer<T> where T : struct, IComparable
        {
            public int Compare(T x, T y)
            {
                var xOrderValue = OrderAttribute.GetOrderValueAttribute(x);
                var yOrderValue = OrderAttribute.GetOrderValueAttribute(y);

                if (xOrderValue == null && yOrderValue == null)
                    return x.CompareTo(y);

                if (xOrderValue == null || yOrderValue != null)
                    return 1;
                if (xOrderValue != null || yOrderValue == null)
                    return -1;

                return xOrderValue.Value.CompareTo(yOrderValue.Value);
            }

            

            public bool Equals(T x, T y)
            {
                throw new NotImplementedException();
            }

            public int GetHashCode(T obj)
            {
                throw new NotImplementedException();
            }
        }

    }


    public static class OrderHelper
    {
        public static int GetOrder<TEnum>(TEnum value) where TEnum : struct
        {
            int order;

            if (!OrderHelperImpl<TEnum>.Values.TryGetValue(value, out order))
            {
                order = int.MaxValue;
            }

            return order;
        }

        private static class OrderHelperImpl<TEnum>
        {
            public static readonly Dictionary<TEnum, int> Values;

            static OrderHelperImpl()
            {
                var values = new Dictionary<TEnum, int>();

                var fields = typeof(TEnum).GetFields(BindingFlags.Static | BindingFlags.Public);

                var unordered = int.MaxValue - 1;

                for (var i = fields.Length - 1; i >= 0; i--)
                {
                    var field = fields[i];

                    var order = (OrderAttribute)field.GetCustomAttributes(typeof(OrderAttribute), false).FirstOrDefault();

                    int order2;

                    if (order != null)
                    {
                        order2 = order.Order;
                    }
                    else
                    {
                        order2 = unordered;
                        unordered--;
                    }

                    values[(TEnum)field.GetValue(null)] = order2;
                }

                Values = values;
            }
        }
    }


}