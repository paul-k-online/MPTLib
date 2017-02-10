using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MPT.PrimitiveType;

namespace MPT.Test.PrimitiveType
{
    [TestClass()]
    public class StringToIntListTests
    {
        [TestMethod()]
        public void SplitToIntListTest()
        {
            var a1 = "10 9 8 7";
            var b1 = StringToIntList.SplitToIntList(a1).ToArray();
            Assert.AreEqual(b1[0], 10);

            var a2 = "-10-9-8 7";
            var b2 = StringToIntList.SplitToIntList(a2).ToArray();
            Assert.AreEqual(b2[0], 10);
        }

        [TestMethod]
        public void StringSplitTest()
        {
            CollectionAssert.AreEqual(StringToIntList.SplitToIntList("1, 2, 5 77").ToArray(),   new[] { 1, 2, 5, 77 });
            Assert.IsNull(StringToIntList.SplitToIntList(""));
            CollectionAssert.AreEqual(StringToIntList.SplitToIntList("as asd asd asd asd asd ").ToArray(), new int[] { });
        }
    }
}