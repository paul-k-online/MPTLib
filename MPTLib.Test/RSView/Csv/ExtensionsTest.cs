using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPT.RSView.Csv;

namespace MPTLib.Test.RSView.Csv
{
    [TestClass()]
    public class ExtensionsTest
    {
        [TestMethod]
        public void TestToRSBool()
        {
            Assert.AreEqual( false.ToString().ToRS(), "\"False\"", true);            
        }

        [TestMethod]
        public void TestToRSInt()
        {
            //Assert.AreEqual( ((int?)null).ToRS(), "");
            Assert.AreEqual( ((int?)1).ToRS(), "1");
        }

        [TestMethod]
        public void TestToRSDouble()
        {
            Assert.AreEqual( ((double?)null).ToRS(), "");
            Assert.AreEqual( (0.0).ToRS(), "0");
            Assert.AreEqual( (1.1).ToRS(), "1.1");
        }

        [TestMethod]
        public void TestToRSString()
        {

            Assert.AreEqual( ((string)null).ToRS(), "");
            
            Assert.AreEqual( string.Empty.ToRS(), "\"\"");
            Assert.AreEqual( "".ToRS(), "\"\"");
            
            Assert.AreEqual( "1".ToRS(), "\"1\"");
        }
    }
}
