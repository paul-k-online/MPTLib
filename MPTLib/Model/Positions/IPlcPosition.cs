using System;
using System.Collections.Generic;

namespace MPT.Model
{
    public interface IPlcIdPosition : IEquatable<IPlcIdPosition>
    {
        int PlcId { get; set; }
        int Number { get; set; }
    }


    public class ByPlcIdNumberPositionComparer : IEqualityComparer<IPlcIdPosition>
    {
        private static readonly ByPlcIdNumberPositionComparer _comparer = new ByPlcIdNumberPositionComparer();
        public static ByPlcIdNumberPositionComparer Comparer { get { return _comparer; } }

        public bool Equals(IPlcIdPosition x, IPlcIdPosition y)
        {
            if (x == null && y == null)
                return true;
            if (x == null | y == null)
                return false;
            return x.PlcId == y.PlcId && x.Number == y.Number;
        }

        public int GetHashCode(IPlcIdPosition x)
        {
            return (x.PlcId ^ x.Number).GetHashCode();
        }
    }

}