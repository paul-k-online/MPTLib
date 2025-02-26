﻿using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            var excelPosList = TestData._101_PP23.GetNewExcelPositionList();
            excelPosList.LoadAllData();

            var Converter = new RSViewPositionListConverter(excelPosList, TestData.XmlSchema, TestData._101_PP23.NodeName);

            var aiTags = Converter.ConvertAiPositionsToRsViewTags();
            Assert.IsTrue(aiTags.Any());

            var dioTags = Converter.ConvertDioPositionsToRsViewTags();
            Assert.IsTrue(dioTags.Any());

            //var aoTags = Converter.ConvertAoPositionsToRsViewTags();
            //Assert.IsTrue(aoTags.Any());

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

                var converter = new RSViewPositionListConverter(
                                        excelPosList, 
                                        TestData.XmlSchema, 
                                        TestData._101_PP23.NodeName);

                var tags = converter.ConvertAllPositionsToRsViewTags().ToList();
                var csvGen = new CsvGenerator(tags, TestData._101_PP23.NodeName);
                var csvTagContent = csvGen.GetTagCsvContent();
                Assert.IsTrue(csvTagContent.Any());

                var csvAlarmContent = csvGen.GetAlarmCsvContent();
                Assert.IsTrue(csvAlarmContent.Any());
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        [TestMethod]
        public void TestInformToZip()
        {
            var excelPositionList = TestData._105_Inform2.GetNewExcelPositionList();
            var load = excelPositionList.LoadAiSheet();
            Assert.AreEqual(load, true);

            var converter = new RSViewPositionListConverter(excelPositionList,
                                    TestData.XmlSchema,
                                    TestData._105_Inform2.NodeName);

            var rsviewTags = converter.ConvertAllPositionsToRsViewTags();
            var csvGenerator = new CsvGenerator(rsviewTags, converter.NodeName);
            var csvTagStringList = csvGenerator.GetTagCsvContent();

            Assert.IsTrue(csvTagStringList.Any());
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
            
            var converter = new RSViewPositionListConverter(posList, TestData.XmlSchema_K32, TestData._105_K32.NodeName);
            var csvGenerator = new CsvGenerator(converter.ConvertAllPositionsToRsViewTags(), TestData._105_K32.NodeName);

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
