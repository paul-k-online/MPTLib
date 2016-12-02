using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPT.RSView;
using MPT.PrimitiveType;

namespace MPTLib.Test.RSView
{
    [TestClass]
    public class RSTypeTest
    {
        [TestMethod]
        public void TestRSTagType()
        {
            var a = RSViewTagType.F.ToString().ToLower();
            var b = a.ToEnum<RSViewTagType>();
            Assert.AreEqual(a, b.ToString(), true);
        }

        [TestMethod]
        public void TestRSTresholdDirection()
        {
            var a = RsViewTresholdDirection.I.ToString().ToLower();
            var b = a.ToEnum<RsViewTresholdDirection>();
            Assert.AreEqual(a, b.ToString(), true);

            Assert.AreEqual("C".ToEnum<RsViewTresholdDirection>(), default(RsViewTresholdDirection));
            Assert.AreEqual("XXX".ToEnum<RsViewTresholdDirection>(), default(RsViewTresholdDirection));
            Assert.AreEqual("".ToEnum<RsViewTresholdDirection>(), default(RsViewTresholdDirection));


        }
    }
}
