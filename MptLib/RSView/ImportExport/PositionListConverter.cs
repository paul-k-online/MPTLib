using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using MPT.Model;
using MPT.Positions;


namespace MPT.RSView.ImportExport
{
    public class PositionListConverter
    {
        private IPositionList PositionList { get; }
        private XElement Shema { get; }
        public string NodeName { get; }

        public PositionListConverter(IPositionList positionList, XElement shema, string nodeName)
        {
            PositionList = positionList;
            Shema = shema;
            NodeName = nodeName;
        }


        #region ConvertToRsViewTags
        public IEnumerable<RSViewTag> ConvertPositionsToRsViewTags(IEnumerable<Position> positions)
        {
            if (positions == null) return null;
            return PositionConvertXmlExtension.ConvertPositionsToRsviewTags(positions, Shema, NodeName);
        }

        public IEnumerable<RSViewTag> ConvertAiPositionsToRsViewTags()
        {
            var positions = PositionList.AiPositions;
            if (positions == null) return null;
            return ConvertPositionsToRsViewTags(positions.Values.ToList());
        }

        public IEnumerable<RSViewTag> ConvertDioPositionsToRsViewTags()
        {
            var positions = PositionList.DioPositions;
            if (positions == null) return null;
            return ConvertPositionsToRsViewTags(positions.Values.ToList());
        }

        public IEnumerable<RSViewTag> ConvertAoPositionsToRsViewTags()
        {
            var positions = PositionList.AoPositions;
            if (positions == null) return null;
            return ConvertPositionsToRsViewTags(positions.Values.ToList());
        }

        public IEnumerable<RSViewTag> ConvertAllPositionsToRsViewTags()
        {
            var positions = PositionList.AllPositions;
            if (positions == null) return null;
            return ConvertPositionsToRsViewTags(positions);
        }

        #endregion
    }
}