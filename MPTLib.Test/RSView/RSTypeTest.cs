using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPT.RSView;

namespace MPTLib.Test.RSView
{
    [TestClass]
    public class RSTypeTest
    {
        [TestMethod]
        public void TestRSTagType()
        {
            var a = RSTagType.F.ToString().ToLower();
            var b = a.ToEnum<RSTagType>();
            Assert.AreEqual(a, b.ToString(), true);
        }

        [TestMethod]
        public void TestRSTresholdDirection()
        {
            var a = RSTresholdDirection.I.ToString().ToLower();
            var b = a.ToEnum<RSTresholdDirection>();
            Assert.AreEqual(a, b.ToString(), true);

            Assert.AreEqual("C".ToEnum<RSTresholdDirection>(), default(RSTresholdDirection));
            Assert.AreEqual("XXX".ToEnum<RSTresholdDirection>(), default(RSTresholdDirection));
            Assert.AreEqual("".ToEnum<RSTresholdDirection>(), default(RSTresholdDirection));


        }
    }
}
