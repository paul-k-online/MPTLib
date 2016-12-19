using System;
using System.Linq;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPT.Positions;
using MPT.RSView.ImportExport;
using MPT.RSView.ImportExport.Csv;
using System.IO;


namespace MPTLib.Test.RSView.ImportExport
{
    [TestClass]
    public class PositionListConverterTest
    {
        [TestMethod]
        public void TestConvert()
        {
            var excelPosList = TestData._101_PP23.GetNewExcelPositionList();
            excelPosList.LoadAllData();

            var Converter = new PositionListConverter(excelPosList, TestData.XmlShema_TEST, TestData._101_PP23.NodeName);

            var aiTags = Converter.ConvertAiPositionsToRsViewTags();
            Assert.IsTrue(aiTags.Any());

            var dioTags = Converter.ConvertDioPositionsToRsViewTags();
            Assert.IsTrue(dioTags.Any());

            var aoTags = Converter.ConvertAoPositionsToRsViewTags();
            Assert.IsTrue(aoTags.Any());

            var allTags = Converter.ConvertAllPositionsToRsViewTags();
            Assert.IsTrue(allTags.Any());
        }

        [TestMethod]
        public void TestCsvTagStringList_101_PP23()
        {
            try
            {
                var excelPosList = TestData._101_PP23.GetNewExcelPositionList();
                var load = excelPosList.LoadAiSheet();
                Assert.AreEqual(load, true);

                var converter = new PositionListConverter(
                                        excelPosList, 
                                        TestData.XmlShema_TEST, 
                                        TestData._101_PP23.NodeName);

                var tags = converter.ConvertAllPositionsToRsViewTags().ToList();
                var csvGen = new CsvGenerator(tags, TestData._101_PP23.NodeName);
                var csvTagStringList = csvGen.GetTagCsvContent();
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
                var rsTags = PositionConvertXmlExtension.ConvertPositionToRsviewTags(
                    TestData.TestAiPos, TestData.XmlShema_TEST, "TestNode");
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
                var excelPositionList = TestData._105_Inform2.GetNewExcelPositionList();
                var load = excelPositionList.LoadAiSheet();
                Assert.AreEqual(load, true);

                var converter = new PositionListConverter(excelPositionList, 
                                        TestData.XmlShema_TEST,
                                        TestData._105_Inform2.NodeName);

                var rsviewTags = converter.ConvertAllPositionsToRsViewTags();
                var csvGenerator = new CsvGenerator(rsviewTags, converter.NodeName);
                var csvTagStringList = csvGenerator.GetTagCsvContent();

                Assert.IsTrue(csvTagStringList.Any());
            }
            catch(Exception e)
            {
                throw e;
            }
        }


        [TestMethod]
        public void TestMethod1()
        {
            var posList = TestData._105_K32.GetNewExcelPositionList();
            try
            {
                posList.LoadAiSheet();
            }
            catch (Exception e)
            {
                throw e;
            }

            
            var converter = new PositionListConverter(posList, TestData.XmlShema_K32, TestData._105_K32.NodeName);
            var csvGenerator = new CsvGenerator(converter.ConvertAiPositionsToRsViewTags(), TestData._105_K32.NodeName);

            var content = csvGenerator.GetZipStream();

            var filePath = Path.Combine(Path.GetTempPath(), csvGenerator.ZipFileName);
            if (File.Exists(filePath))
                File.Delete(filePath);
            var bSave = CsvGenerator.SaveFile(filePath, content);
            Assert.IsTrue(bSave);

            if (bSave)
                File.Delete(filePath);
        }
    }
}
