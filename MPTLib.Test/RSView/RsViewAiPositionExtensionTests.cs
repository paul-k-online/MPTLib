using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPT.Model;
using MPT.RSView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPT.RSView.Tests
{
    [TestClass()]
    public class RSViewTagTests
    {
        [TestMethod()]
        public void RSViewCleanTagNameTest()
        {
            var list = new List<Tuple<string, string,string>>()
            {
                new Tuple<string, string,string>("FRС1397", "FR_1397", "F"),
                new Tuple<string, string,string>("FBiborcsai1356", "FBiborcsai1356", "F"),

                new Tuple<string, string,string>("44 TR 101/40","44_TR_101_40", "T"),
                new Tuple<string, string,string>("44 TR 101/40_lp1","44_TR_101_40_lp1", "T"),

                new Tuple<string, string,string>("10%FRCSA.1100/0","10_FRCSA_1100_0", "F"),
                new Tuple<string, string,string>("10%FR_1356_В_Д_", "10_FR_1356_", "F"),

                new Tuple<string, string,string>("FrcboBisai1356", "FrcboBisai1356", "F"),
                new Tuple<string, string,string>("FSCRo1", "FSCRo1", "F"),

                new Tuple<string, string,string>("FrcbB1356", "FrcbB1356", "F"),
                new Tuple<string, string,string>("TR1356ВД", "TR1356_", "T"),
                new Tuple<string, string,string>("dpR1356_ВД", "dpR1356_", "DP"),
                new Tuple<string, string,string>("FR1356_В_Д_", "FR1356_", "F"),
                new Tuple<string, string,string>("FR_1356_В_Д_", "FR_1356_", "F"),
                new Tuple<string, string,string>("P1515 (БИТЫЙ КАНАЛ )","P1515_", "P"),
                new Tuple<string, string,string>("42TRS-121/1","42TRS_121_1", "T"),
            };


            foreach (var item in list)
            {
                var validTagName = RSViewTag.GetValidTagName(item.Item1);
                Assert.AreEqual(validTagName, item.Item2, ignoreCase: true);

                var aiPosName = AiPosition.AiPositionName.Parse(item.Item1);
                if (aiPosName != null)
                {
                    Assert.AreEqual(aiPosName.AiTypeLetter, item.Item3, ignoreCase: true);
                    var shortName = aiPosName.GetShortName();
                }
            }
        }
    }
}