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
            var a = RSViewTresholdDirection.I.ToString().ToLower();
            var b = a.ToEnum<RSViewTresholdDirection>();
            Assert.AreEqual(a, b.ToString(), true);

            Assert.AreEqual("C".ToEnum<RSViewTresholdDirection>(), default(RSViewTresholdDirection));
            Assert.AreEqual("XXX".ToEnum<RSViewTresholdDirection>(), default(RSViewTresholdDirection));
            Assert.AreEqual("".ToEnum<RSViewTresholdDirection>(), default(RSViewTresholdDirection));


        }
    }
}
