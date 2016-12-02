using System;
using System.Collections.Generic;

namespace MPT.Model
{
    public interface IPlcIdPosition : IEquatable<IPlcIdPosition>
    {
        int PlcId { get; set; }
        int Number { get; set; }
    }


    public class PlcPositionComparer : IEqualityComparer<IPlcIdPosition>
    {
        // ReSharper disable once InconsistentNaming
        private static readonly PlcPositionComparer _comparer = new PlcPositionComparer();
        public static PlcPositionComparer Comparer { get { return _comparer; } }

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