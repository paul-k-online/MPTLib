using System.Xml.Linq;
using MPT.Model;
using MPT.Positions;
using MPT.RSView.ImportExport.XML;

namespace MPTLib.Test
{
    public static class TestData
    {
        public class TestDataProject
        {
            public int PlcId { get; set; }
            public string ExcelFilePath { get; set; }
            public string NodeName { get; set; }
            public ExcelPositionList GetNewExcelPositionList()
            {
                return new ExcelPositionList(ExcelFilePath, PlcId);
            }
        }

        public static readonly XElement Xml;
        public static readonly XElement Xml_TEST;
        public static readonly XElement Xml_K32;

        public static readonly SchemaConverter XmlSchema;
        public static readonly SchemaConverter XmlSchema_TEST;
        public static readonly SchemaConverter XmlSchema_K32 ;

        static TestData()
        {
            Xml =           XElement.Load(@"RSView\ImportExport\POSITIONLIST.xml");
            Xml_TEST =      XElement.Load(@"RSView\ImportExport\POSITIONLIST_TEST.xml");
            Xml_K32 =       XElement.Load(@"RSView\ImportExport\POSITIONLIST_K32.xml");

            XmlSchema = new SchemaConverter(Xml);
            XmlSchema_TEST = new SchemaConverter(Xml_TEST);
            XmlSchema_K32 = new SchemaConverter(Xml_K32);
        }


        public static readonly AiPosition TestAiPos = new AiPosition
            {
                Name = "FRCSA-1011/3",
                Description = "Расход бензина поток 3 dP=10 кПа",
                Units = "т/ч",
                Number = 2,
                Scale = new RangePair { Low = 0, High = 10 },
                Reglament = new RangePair { Low = 3, High = 6 },
                Alarming = new RangePair { Low = 2, High = 8 },
                Blocking = new RangePair { Low = null, High = 9 },
            };


        public static readonly TestDataProject _101_PP18 = new TestDataProject()
        {
            ExcelFilePath = @"_TestData\Excel\101_PP23.xls",
            NodeName = "101_PP23",
            PlcId = 10118,
        };

        public static readonly TestDataProject _101_PP23 = new TestDataProject()
        {
            ExcelFilePath = @"_TestData\Excel\101_PP23.xls",
            NodeName = "101_PP23",
            PlcId = 10123,
        };

        public static readonly TestDataProject _105_Inform2 = new TestDataProject()
        {
            ExcelFilePath = @"_TestData\Excel\Inform2.xlsx",
            NodeName = "105_Inform2",
            PlcId = 10516,
        };

        public static readonly TestDataProject _105_K32 = new TestDataProject()
        {
            ExcelFilePath = @"_TestData\Excel\K3_2.xlsx",
            NodeName = "105_K3_2",
            PlcId = 10516,
        };
    }
}