using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPT.Model;

namespace MPTLib.Test.Model
{
    [TestClass]
    public class AipositionTest
    {
        readonly AiPosition aipos = new AiPosition()
                    {
                        Name = "10%FRCSA.1100/0",
                    };



        [TestMethod]
        public void TestName()
        {
            var l = aipos.Letters;
            Assert.AreEqual(l, "FRCSA");
        }

        [TestMethod]
        public void TestShortName()
        {
            var l = aipos.ShortName;
            Assert.AreEqual(l, "10%F.1100/0");
        }


    }
}
