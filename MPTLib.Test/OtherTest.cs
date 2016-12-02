using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MPTLib.Test
{
    [TestClass]
    public class OtherTest
    {
        [TestMethod]
        public void TestConvertToDouble()
        {
            string v = null;
            var d = Convert.ToDouble(v);
            Assert.AreEqual(d, 0);
        }
        [TestMethod]
        public void TestPathCombine()
        {
            Assert.AreEqual(Path.Combine("asad/asd", ""), "asad/asd");
            Assert.AreEqual(Path.Combine("asad/asd", null), "asad/asd");
        }

    }
}