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
        
        public bool Equals(IPlcIdPosition other)
        {
            return ByPlcIdNumberPositionComparer.Comparer.Equals(this, other);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}