using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPT.Model;
using MPT.Positions;

namespace MPTLib.Test.Positions
{
    [TestClass]
    public class ExcelPositionListTest
    {

            
        [TestMethod]
        public void TestGetPairPlcMessgesDictionary()
        {        
            var sqlMessages = new List<PlcMessage>
            {
                new PlcMessage() {Number = 1, Text = "1"},
                new PlcMessage() {Number = 2, Text = "2"},
                new PlcMessage() {Number = 3, Text = "3"},
            };

            var excelMessages = new List<PlcMessage>
            {
                new PlcMessage() {Number = 1, Text = "1"},
                new PlcMessage() {Number = 2, Text = "2a"},
                new PlcMessage() {Number = 4, Text = "4"},
            };

            var sqlDict = sqlMessages.ToDictionary(x => x.Number, y => y);
            var excelDict = excelMessages.ToDictionary(x => x.Number, y => y);

            var merge =  new PlcMessagesMerge(sqlDict.Values, excelDict.Values, 99999);
            var pairDict = merge.PlcMessagePairDictionary;

            Assert.AreEqual(pairDict[1].Item1.Text, pairDict[1].Item2.Text, true);
            Assert.AreEqual(pairDict[2], new Tuple<PlcMessage, PlcMessage>(sqlDict[2], excelDict[2]));
            Assert.AreEqual(pairDict[3].Item2, null);
            Assert.AreEqual(pairDict[4].Item1, null); 
        }
    }
}
