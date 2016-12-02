using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPT.PrimitiveType;

namespace MPTLib.Test.PrimitiveType
{
    [TestClass]
    public class TestDateTime
    {
        [TestMethod]
        public void TestTrim()
        {
            var dt = new DateTime(2000,05,05, 14,38,59);
            var manualTrimDt = new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, 0);
            var trimDt = dt.Trim(TimeSpan.TicksPerMinute);

            Assert.AreEqual(manualTrimDt, trimDt);
        }
    }
}
