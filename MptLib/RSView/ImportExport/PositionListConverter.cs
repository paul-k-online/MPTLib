using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using MPT.Model;
using MPT.Positions;
using MPT.RSView.ImportExport.XML;

namespace MPT.RSView.ImportExport
{
    public class RSViewPositionListConverter
    {
        public IPositionList PositionList { get; private set; }
        public SchemaConverter ShemaConverter { get; private set; }
        public string NodeName { get; private set; }

        public RSViewPositionListConverter(IPositionList positionList, XElement shema, string nodeName) : 
            this(positionList, new SchemaConverter(shema), nodeName)
        {}

        public RSViewPositionListConverter(IPositionList positionList, SchemaConverter shemaConverter, string nodeName)
        {
            PositionList = positionList;
            ShemaConverter = shemaConverter;
            NodeName = nodeName;
        }


        #region ConvertToRsViewTags
        public IEnumerable<RSViewTag> ConvertPositionsToRsViewTags(IEnumerable<Position> positions)
        {
            if (positions == null) return null;
            return ConvertPositionsToRsviewTags(positions, ShemaConverter, NodeName);
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


        #region PositionConvert
        public static IEnumerable<RSViewTag> ConvertPositionsToRsviewTags(IEnumerable<Position> positions, SchemaConverter positionShema, string nodeName)
        {
            if (positions == null) return null;
            if (positionShema == null) return null;
            if (string.IsNullOrWhiteSpace(nodeName)) return null;

            return positions
#if !DEBUG
                .AsParallel()
#endif
                .SelectMany(pos => positionShema.ConvertPositionToRSViewTags(pos, nodeName));
        }

        #endregion

    }
}