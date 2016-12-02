using System;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPT.Positions;
using MPT.RSView.ImportExport;
using MPT.RSView.ImportExport.Csv;

namespace MPTLib.Test.RSView.ImportExport
{
    [TestClass]
    public class PositionListConverterTest
    {
        //public static ExcelPositionList ExcelPositionList = new ExcelPositionList(@"TestData\101_PP23.xls");
        public static PositionListConverter Converter = new PositionListConverter(TestData.ExcelPositionList, TestData.RootElement, TestData.NodeName);

        [TestMethod]
        public void TestConvert()
        {
            TestData.ExcelPositionList.LoadAllData();

            var aiTags = Converter.GetAiTags();
            var dioTags = Converter.GetDioTags();
            var aoTags = Converter.GetAoTags();
            var allTags = Converter.GetAllTags();
        }

        [TestMethod]
        public void TestCvsTagFile()
        {
            try
            {
                TestData.ExcelPositionList.LoadAllData();
                var tags = Converter.GetAiTags();
                var file = new CsvGenerator(tags, TestData.NodeName);
                //file.CollectFolders();
            }
            catch (Exception e)
            {
                throw new Exception("convert", e);
            }
        }


    }
}
