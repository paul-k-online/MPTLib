using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPT.PrimitiveType;
using System.IO;

namespace MPTLib.Test.PrimitiveType
{
    [TestClass]
    public class NullNumericTest
    {
        [TestMethod]
        public void NullNumeric1Test()
        {
            Assert.AreEqual(Convert.ToDouble("0.0"), 0);
            Assert.AreEqual(Convert.ToDouble(null), 0);

            Assert.AreEqual("".NullDoubleTryParse(),null);

            Assert.AreEqual("0".NullIntTryParse(), 0);
            Assert.AreEqual("".NullIntTryParse(), null);
        }


        [TestMethod]
        public void ConvertStringTest()
        {
            object n = null;
            var e = (object)string.Empty;
            Assert.AreEqual(n.ToNullString(), n);
            Assert.AreEqual(e.ToNullString(), e);

            try
            {
                Assert.AreEqual(n.ToString(), null);
            }
            catch (Exception)
            {
                // ignored
            }

            Assert.AreEqual(Convert.ToString(e), "");
            Assert.AreEqual(Convert.ToString(n), "");

            Assert.AreEqual(e.ToString(), "");
            
            
            
        }


        [TestMethod]
        public void ConvertDoubleTest()
        {
            Assert.AreEqual(Convert.ToDouble(null), 0.0);

            try
            {
                var d = Convert.ToDouble("");
            }
            catch (FormatException)
            {}
            
            
            Assert.AreEqual(Convert.ToDouble("0"), 0.0);
            Assert.AreEqual(Convert.ToDouble("0.1"), 0.1);
            Assert.AreEqual(Convert.ToDouble("1"), 1.0);
            Assert.AreEqual(Convert.ToDouble("1.5"), 1.5);

        }

        [TestMethod]
        public void ConvertIntTest()
        {
            //Assert.AreEqual(Convert.ToInt32(""), 0);
        }


        [TestMethod]
        public void ConvertTest()
        {
            //Assert.AreEqual(Convert.ToInt32("1.0"), 1 );
            Assert.AreEqual(Convert.ToInt32(1.0), 1);

        }


        [TestMethod]
        public void NullParseTest()
        {
            Assert.AreEqual(("").ToNullNumeric<int>(), null);
            Assert.AreEqual((" ").ToNullNumeric<int>(), null);
            Assert.AreEqual(("a").ToNullNumeric<int>(), null);
            Assert.AreEqual(((object)null).ToNullNumeric<int>(), null);

            Assert.AreEqual((1).ToNullNumeric<int>(), 1);
            Assert.AreEqual((1.1).ToNullNumeric<int>(), 1);
            Assert.AreEqual(("1").ToNullNumeric<int>(), 1);
            Assert.AreEqual(("1.0").ToNullNumeric<int>(), 1);
            Assert.AreEqual(("1.1").ToNullNumeric<int>(), 1);

            Assert.AreEqual((1).ToNullNumeric<double>(), 1.0);
            Assert.AreEqual(("1").ToNullNumeric<double>(), 1.0);
            Assert.AreEqual(("1.0").ToNullNumeric<double>(), 1.0);
            Assert.AreEqual(("1.1").ToNullNumeric<double>(), 1.1);

        }


        [TestMethod]
        public void TestNullCast()
        {
            var d = 0.0;
            var od = (object) d;

            var t_d = d.GetType();
            var t_od = od.GetType();

            Assert.AreEqual(t_d, t_od);
        }


        [TestMethod]
        public void ConvertToDoubleTest()
        {
            string v = null;
            var d = Convert.ToDouble(v);
            Assert.AreEqual(d, 0);
        }


        [TestMethod]
        public void PathCombineTest()
        {
            Assert.AreEqual(Path.Combine("asad/asd", ""), "asad/asd");
            try
            {
                Assert.AreEqual(Path.Combine("asad/asd", null), "asad/asd");
            }
            catch (ArgumentNullException)
            {
                //throw;
            }
        }


        [TestMethod]
        public void TestNullInt()
        {
            int? vNull = 10;
            int? r = 10 + vNull;
        }


        [TestMethod]
        public void TestTrim()
        {
            var dt = new DateTime(2000, 05, 05, 14, 38, 59);
            var manualTrimDt = new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, 0);
            var trimDt = dt.Trim(TimeSpan.TicksPerMinute);

            Assert.AreEqual(manualTrimDt, trimDt);
        }
    }
}
