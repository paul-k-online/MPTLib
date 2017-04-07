using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPT.RSView;
using static MPT.RSView.RSViewAnalogAlarm;

namespace MPT.PrimitiveType.Tests
{
    [TestClass()]
    public class EnumTest1
    {
        [TestMethod()]
        public void ToEnumTest()
        {
            var t = true;
            var e = t.ToEnum<RSViewDigitEnum>();
            Assert.AreEqual(e, RSViewDigitEnum.ON);
        }
    }

    [TestClass]
    public class EnumTest
    {
        [TestMethod]
        public void TestRSTagType()
        {
            var a = RSViewTag.TypeEnum.F.ToString().ToLower();
            var b = a.ToEnum<RSViewTag.TypeEnum>();
            Assert.AreEqual(a, b.ToString(), true);
        }

        [TestMethod]
        public void TestRSTresholdDirection()
        {
            var a = TresholdDirection.I.ToString().ToLower();
            var b = a.ToEnum<TresholdDirection>();
            Assert.AreEqual(a, b.ToString(), true);
            Assert.AreEqual("C".ToEnum<TresholdDirection>(), default(TresholdDirection));
            Assert.AreEqual("XXX".ToEnum<TresholdDirection>(), default(TresholdDirection));
            Assert.AreEqual("".ToEnum<TresholdDirection>(), default(TresholdDirection));
        }
    }
}
