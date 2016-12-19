using System.Xml.Linq;
using MPT.Model;
using MPT.Positions;

namespace MPTLib.Test
{
    public static class TestData
    {
        public class TestDataProject
        {
            public string ExcelFilePath { get; set; }
            public string NodeName { get; set; }
            public int PlcId { get; set; }

            public ExcelPositionList GetNewExcelPositionList()
            {
                return new ExcelPositionList(ExcelFilePath, PlcId);
            }
        }

        public static readonly XElement XmlShema 
            = XElement.Load(@"RSView\ImportExport\POSITIONLIST.xml");
        public static readonly XElement XmlShema_TEST 
            = XElement.Load(@"RSView\ImportExport\POSITIONLIST_TEST.xml");
        public static readonly XElement XmlShema_K32 
            = XElement.Load(@"RSView\ImportExport\POSITIONLIST_K32.xml");


        public static readonly AiPosition TestAiPos = new AiPosition
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


        public static TestDataProject _101_PP18 = new TestDataProject()
        {
            ExcelFilePath = @"_TestData\101_PP23.xls",
            NodeName = "101_PP23",
            PlcId = 10118,
        };

        public static TestDataProject _101_PP23 = new TestDataProject()
        {
            ExcelFilePath = @"_TestData\101_PP23.xls",
            NodeName = "101_PP23",
            PlcId = 10123,
        };

        public static TestDataProject _105_Inform2 = new TestDataProject()
        {
            ExcelFilePath = @"_TestData\Inform2.xlsx",
            NodeName = "105_Inform2",
            PlcId = 10516,
        };

        public static TestDataProject _105_K32 = new TestDataProject()
        {
            ExcelFilePath = @"_TestData\K3_2.xlsx",
            NodeName = "105_K3_2",
            PlcId = 10516,
        };
    }
}