using System.IO;
using System.Xml.Linq;
using MPT.DataBase;
using MPT.Model;
using MPT.Positions;

namespace MPTLib.Test.RSView
{
    public static class TestData
    {
        public class TestDataExcelPlc
        {
            public string FilePath { get; set; }
            public string NodeName { get; set; }
            public int PlcId { get; set; }

            public ExcelDataBase ExcelDataBase
            {
                get { return new ExcelDataBase(FilePath); }
            }

            public ExcelPositionList ExcelPositionList
            {
                get { return new ExcelPositionList(FilePath, PlcId); }
            }
        }

        public const string PositionListXmlFile = @"RSView\ImportExport\POSITIONLIST_TEST.xml";
        public static readonly XElement RootElement = XElement.Load(PositionListXmlFile);

        public static readonly AiPosition AiPos = new AiPosition
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



        public static TestDataExcelPlc _101_PP18 = new TestDataExcelPlc()
        {
            FilePath = @"_TestData\101_PP18.xls",
            NodeName = "101_PP18",
            PlcId = 10118,
        };

        public static TestDataExcelPlc _101_PP23 = new TestDataExcelPlc()
        {
            FilePath = @"_TestData\101_PP23.xls",
            NodeName = "101_PP23",
            PlcId = 10123,
        };

        public static TestDataExcelPlc _105_Inform2 = new TestDataExcelPlc()
        {
            FilePath = @"_TestData\Inform2.xlsx",
            NodeName = "105_Inform2",
            PlcId = 10516,
        };


    }
}