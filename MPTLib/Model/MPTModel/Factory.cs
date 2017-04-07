using System;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace MPT.Model
{
    public partial class Factory
    {

        public class ByIdComparer : IEqualityComparer<Factory>
        {
            public static ByIdComparer Comparer = new ByIdComparer();

            public bool Equals(Factory x, Factory y)
            {
                return x.Number == y.Number;
            }

            public int GetHashCode(Factory obj)
            {
                return obj.Number ?? 0;
            }
        }

        public string FullName
        {
            get
            {
                var format = "{0} - {1}";
                if (Number == null)
                    format = "{1}";
                return string.Format(format, Number, Description);
            }
        }
    }

}
