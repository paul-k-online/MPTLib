using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPT.RSView;

namespace MPTLib.Test.RSView
{
    [TestClass]
    public class TestRSType
    {
        [TestMethod]
        public void TestRSTagType()
        {
            var a = RsViewTagType.F.ToString().ToLower();
            var b = a.ToEnum<RsViewTagType>();
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
