using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPT.PrimitiveType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPT.PrimitiveType.Tests
{
    [TestClass()]
    public class DictionaryExtensionTests
    {
        [TestMethod()]
        public void AddRangeTest()
        {
            var dict = new Dictionary<int, string>();
            try
            {
                dict[1] = "0";
                var v = "1";
                dict[1] = v;
                Assert.AreEqual(dict[1], v);
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }
    }
}