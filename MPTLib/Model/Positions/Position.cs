using System.Collections.Generic;

namespace MPT.Model
{
    public abstract class Position : IPlcIdPosition
    {
        public int PlcId { get; set; }

        public int Number { get; set; }
        
        public string Name { get; set; }

        public string Description { get; set; }

        public string Note { get; set; }

        public int? GroupId { get; set; }
        
        
        /// <summary>
        /// полное название
        /// </summary>
        public string FullName
        {
            get { return string.Format("{0} - {1}", Name, Description); }
        }

        public bool Equals(IPlcIdPosition other)
        {
            return ByPlcIdNumberPositionComparer.Comparer.Equals(this, other);
        }

        public override string ToString()
        {
            return Name;
        }

        /// <summary>
        /// сравнивать только по индексу
        /// </summary>
        public class ByIdComparer : IEqualityComparer<Position>
        {
            public bool Equals(Position x, Position y)
            {
                return x.PlcId == y.PlcId && x.Number == y.Number ;
            }

            public int GetHashCode(Position x)
            {
                return (x.PlcId ^ x.Number).GetHashCode();
            }
        }

    }
}