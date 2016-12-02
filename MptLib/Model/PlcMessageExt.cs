using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPT.Model
{
    partial class PlcMessage
    {
        public override string ToString()
        {
            return string.Format("{0}: {1}", Number, Text);
        }

        public class PlcMessageByIdComparer : IEqualityComparer<PlcMessage>
        {
            public bool Equals(PlcMessage x, PlcMessage y)
            {
                return x.PlcId == y.PlcId && x.Number == y.Number;
            }

            public int GetHashCode(PlcMessage obj)
            {
                return obj.PlcId ^ obj.Number;
            }
        }

        public class PlcMessageComparer : IEqualityComparer<PlcMessage>
        {
            public bool Equals(PlcMessage x, PlcMessage y)
            {
                return x.PlcId == y.PlcId && x.Number == y.Number && x.Text == y.Text;
            }

            public int GetHashCode(PlcMessage obj)
            {
                return obj.PlcId ^ obj.Number ^ obj.Text.GetHashCode();
            }
        }

        public class PlcMessageByTextComparer : IEqualityComparer<PlcMessage>
        {
            public bool Equals(PlcMessage x, PlcMessage y)
            {
                return x.Text == y.Text;
            }

            public int GetHashCode(PlcMessage obj)
            {
                return obj.Text.GetHashCode();
            }
        }
    }
}
