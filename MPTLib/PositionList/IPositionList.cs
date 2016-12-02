using System.Collections.Generic;
using MPT.Model;

namespace MPT.Positions
{
    public interface IPositionList
    {
        IList<Position> AllPositions { get; }

        IDictionary<int, AiPosition> AiPositions { get; }

        IDictionary<int, AoPosition> AoPositions { get; }

        IDictionary<int, DioPosition> DioPositions { get; }

        IDictionary<int, PlcMessage> PlcMessages { get; }
    }
}