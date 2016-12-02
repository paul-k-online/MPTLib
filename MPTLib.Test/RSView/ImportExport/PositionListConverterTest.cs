using System;
using System.Linq;
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
        [TestMethod]
        public void TestConvert()
        {
            TestData._101_PP23.ExcelPositionList.LoadAllData();
            var Converter = new PositionListConverter(TestData._101_PP23.ExcelPositionList, TestData.RootElement, TestData._101_PP23.NodeName);

            var aiTags = Converter.ConvertAiPositionsToRsViewTags();
            var dioTags = Converter.ConvertDioPositionsToRsViewTags();
            var aoTags = Converter.ConvertAoPositionsToRsViewTags();
            var allTags = Converter.ConvertAllPositionsToRsViewTags();
        }

        [TestMethod]
        public void TestCsvTagStringList_101_PP23()
        {
            try
            {
                var load = TestData._101_PP23.ExcelPositionList.LoadAiSheet();
                Assert.AreEqual(load, true);

                var converter = new PositionListConverter(
                                        TestData._101_PP23.ExcelPositionList, 
                                        TestData.RootElement, 
                                        TestData._101_PP23.NodeName);

                var tags = converter.ConvertAllPositionsToRsViewTags().ToList();
                var csvGen = new CsvGenerator(tags, TestData._101_PP23.NodeName);
                var csvTagStringList = csvGen.GetTagStringList();
                Assert.AreNotEqual(0, csvTagStringList.ToList().Count);
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        [TestMethod]
        public void TestDefaultPositionToZip()
        {
            try
            {
                var shema = XElement.Load(@"RSView\ImportExport\POSITIONLIST_TEST.xml");
                var rsTags = PositionConvertXmlExtension.ConvertPositionToRsviewTags(TestData.AiPos, shema, "TestNode");
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        [TestMethod]
        public void TestInformToZip()
        {
            try
            {
                var excelPositionList = new ExcelPositionList(@"_TestData\inform2.xlsx");
                excelPositionList.LoadAiSheet();

                var shema = XElement.Load(@"RSView\ImportExport\POSITIONLIST.xml");
                var converter = new PositionListConverter(excelPositionList, shema, "105_INFORM");

                var rsviewTags = converter.ConvertAllPositionsToRsViewTags();
                var csvGenerator = new CsvGenerator(rsviewTags, converter.NodeName);
                //csvGenerator.SaveZipToFolder("d:\\");
            }
            catch(Exception e)
            {
                throw e;
            }
        }


        [TestMethod]
        public void TestMethod1()
        {
            var posList = new ExcelPositionList(@"_TestData\K3_2.xlsx");
            try
            {
                posList.LoadAiSheet();
            }
            catch (Exception e)
            {
                throw e;
            }

            XElement k32Shema = XElement.Load(@"RSView\ImportExport\K32_Shema.xml");
            PositionListConverter k32Converter = new PositionListConverter(posList, k32Shema, @"K-3/2");
            CsvGenerator csvGen = new CsvGenerator(k32Converter.ConvertAiPositionsToRsViewTags(), k32Converter.NodeName);

            csvGen.SaveZipToFolder(@"d:\");
        }


    }
}
