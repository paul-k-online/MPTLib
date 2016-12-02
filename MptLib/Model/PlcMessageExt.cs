using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPT.Model
{
    partial class PlcMessage : IEquatable<PlcMessage>
    {
        public bool Equals(PlcMessage other)
        {
            return PlcMessageComparer.Comparer.Equals(this, other);
        }

        public override string ToString()
        {
            return string.Format(!string.IsNullOrWhiteSpace(Text) ? "{0}: {1}" : "{0}", Number, Text);
        }

        public class PlcMessageComparer : IEqualityComparer<PlcMessage>
        {
            // ReSharper disable once InconsistentNaming
            private static readonly PlcMessageComparer _comparer = new PlcMessageComparer();

            public static PlcMessageComparer Comparer
            {
                get { return _comparer; }
            }

            public bool Equals(PlcMessage x, PlcMessage y)
            {
                if (x == null && y == null)
                    return true;

                if (x == null ^ y == null)
                    return false;

                return x.PlcId == y.PlcId && x.Number == y.Number && string.Equals(x.Text, y.Text);
            }

            public int GetHashCode(PlcMessage x)
            {
                return x.PlcId ^ x.Number ^ x.Text.GetHashCode();
            }
        }

        public class PlcMessageByIdComparer : IEqualityComparer<PlcMessage>
        {
            private static readonly PlcMessageByIdComparer _comparer = new PlcMessageByIdComparer();

            public static PlcMessageByIdComparer Comparer
            {
                get { return _comparer; }
            }

            public bool Equals(PlcMessage x, PlcMessage y)
            {
                if (x == null && y == null)
                    return true;

                if (x == null ^ y == null)
                    return false;
                
                return x.PlcId == y.PlcId && x.Number == y.Number;
            }

            public int GetHashCode(PlcMessage obj)
            {
                return obj.PlcId ^ obj.Number;
            }
        }

        public class PlcMessageByTextComparer : IEqualityComparer<PlcMessage>
        {
            private static readonly PlcMessageByTextComparer _comparer = new PlcMessageByTextComparer();

            public static PlcMessageByTextComparer Comparer
            {
                get { return _comparer; }
            }

            public bool Equals(PlcMessage x, PlcMessage y)
            {
                if (x == null && y == null)
                    return true;

                if (x == null ^ y == null)
                    return false;
                
                return x.Text == y.Text;
            }

            public int GetHashCode(PlcMessage obj)
            {
                return obj.Text.GetHashCode();
            }
        }
    }
}
