using System.Collections.Generic;
using MPT.Model;

namespace MPT.Positions
{
    public abstract class PositionList
    {
        public Dictionary<int, AiPosition> AiPositions { get; protected set; }

        public Dictionary<int, AoPosition> AoPositions { get; protected set; }

        public Dictionary<int, DioPosition> DioPositions { get; protected set; }

        public Dictionary<int, PlcMessage> PlcMessages { get; protected set; }
    }
}