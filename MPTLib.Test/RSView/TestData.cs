using System.Xml.Linq;
using MPT.Model;
using MPT.Positions;

namespace MPTLib.Test.RSView
{
    public static class TestData
    {
        public static AiPosition AiPos = new AiPosition()
        {
            Name = "FRCSA1011_3",
            Description = "Расход бензина поток 3 dP=10 кПа",
            Units = "т/ч",
            Number = 2,
            Scale = new RangePair { Low = 0, High = 10 },
            Reglament = new RangePair { Low = 3, High = 6 },
            Alarming = new RangePair { Low = 2, High = 8 },
            Blocking = new RangePair { Low = null, High = 9 },            
        };

        public const string NodeName = "101_PP18";
        public static XElement RootElement = XElement.Load("RSView\\ImportExport\\POSITIONLIST.xml");
        public static ExcelPositionList ExcelPositionList = new ExcelPositionList(@"TestData\101_PP23.xls");
    }
}
