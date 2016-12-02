using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Xml.Linq;
using MPT.Model;
using MPT.Positions;


namespace MPT.RSView.ImportExport
{
    public class PositionListConverter
    {
        PositionList PositionList { get; set; }
        XElement Shema { get; set; }
        
        public string NodeName { get; private set; }

        public PositionListConverter(PositionList positionList, XElement shema, string nodeName)
        {
            PositionList = positionList;
            Shema = shema;
            NodeName = nodeName;
        }


        private static IEnumerable<RsViewTag> ConvertTags(IEnumerable<Position> positions, XElement shema, string nodeName)
        {
            if (positions == null) return null;
            if (shema == null) return null;
            
            var test = positions.SelectMany(x => x.GetTags(shema, nodeName)).ToList();
            return test;
        }


        public IEnumerable<RsViewTag> GetAiTags()
        {
            var shema = Shema.GetElement("ANALOG_POSITION");
            return ConvertTags(PositionList.AiPositions.Values, shema, NodeName);
        }


        public IEnumerable<RsViewTag> GetDioTags()
        {
            var shema = Shema.GetElement("DIGITAL_POSITION");
            return ConvertTags(PositionList.DioPositions.Values, shema, NodeName);
        }


        public IEnumerable<RsViewTag> GetAoTags()
        {
            var shema = Shema.GetElement("REGULATOR_POSITION");
            return ConvertTags(PositionList.AoPositions.Values, shema, NodeName);
        }


        public IEnumerable<RsViewTag> GetAllTags()
        {
            var tagList = new List<RsViewTag>();
            tagList.AddRange(GetAiTags());
            tagList.AddRange(GetDioTags());
            tagList.AddRange(GetAoTags());
            return tagList;
        }
    }
}