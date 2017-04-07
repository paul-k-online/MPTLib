using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPT.PrimitiveType;
using MPT.Model;
using MPT.Positions;

namespace MPTLib.Test.Positions
{
    [TestClass]
    public class ExcelPositionListTest
    {
        [TestMethod]
        public void TestLoadAiPositions()
        {
                var excelPositionList = new ExcelPositionList(TestData._105_Inform2.ExcelFilePath, TestData._105_Inform2.PlcId);
                var AIdataTableList = excelPositionList.ExcelDataBase.GetSheetDataTable("AI").AsEnumerable().ToList();
                var nameIndex = (int) ExcelPositionConvert.AiPosition_ExcelColumnNumber.Name;
                var whereName = AIdataTableList
                    .Where(x => !string.IsNullOrWhiteSpace(x.ItemArray[nameIndex].ToNullString()));
                var excelRowCount = whereName.Count();
                excelPositionList.LoadAiSheet();
                var aiPositionCount = excelPositionList.AiPositions.Count;
                Assert.AreEqual(excelRowCount - 1, aiPositionCount);
        }


        [TestMethod]            
        public void TestGetPairPlcMessgesDictionary()
        {

            var sqlMessages = new List<PlcMessage>
            {
                new PlcMessage() {Number = 1, Text = "a"},
                new PlcMessage() {Number = 2, Text = "a"},
                new PlcMessage() {Number = 3, Text = "a"},
                new PlcMessage() {Number = 10, Text = "a", Severity = 1},
                new PlcMessage() {Number = 11, Text = "a",},
            };

            var excelMessages = new List<PlcMessage>
            {
                new PlcMessage() {Number = 1, Text = "a"},
                new PlcMessage() {Number = 2, Text = "a"},
                new PlcMessage() {Number = 4, Text = "a"},
                new PlcMessage() {Number = 10, Text = "a", Severity = 2},
                new PlcMessage() {Number = 11, Text = "a", Group = 2},
            };

            var sqlDict = sqlMessages.ToDictionary(x => x.Number, y => y);
            var excelDict = excelMessages.ToDictionary(x => x.Number, y => y);

            var merge =  new PlcMessagesMerge(sqlDict.Values, excelDict.Values);
            var pairDict = merge.PlcMessagePairDictionary;

            var pair = pairDict[1];
            Assert.IsTrue(pair.Item1.Equals(pair.Item2));
            Assert.AreEqual(pairDict[2], new Tuple<PlcMessage, PlcMessage>(sqlDict[2], excelDict[2]));
            Assert.IsNull(pairDict[3].Item2);
            Assert.IsNull(pairDict[4].Item1);

            var diff = merge.Diff;
            //Assert.AreEqual(diff.Count, 3);

            var addUpdateList = merge.GetAddOrUpdateMessages();
            var removeList = merge.GetRemoveMessages();
        }


        [TestMethod]
        public void TestDupl()
        {
            var excelMessages = new List<PlcMessage>
            {
                new PlcMessage() {Number = 1, Text = "a"},
                new PlcMessage() {Number = 2, Text = "a"},
                new PlcMessage() {Number = 2, Text = "b"},
                new PlcMessage() {Number = 10, Text = "a", Severity = 2},
                new PlcMessage() {Number = 11, Text = "a", Group = 2},
            };
            var dupl = excelMessages.GroupBy(x => x.Number).Where(x=>x.Count()>1);
        }
    }
}
