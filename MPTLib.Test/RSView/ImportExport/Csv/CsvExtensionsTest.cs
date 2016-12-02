using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPT.RSView.ImportExport.Csv;

namespace MPTLib.Test.RSView.ImportExport.Csv
{
    [TestClass()]
    public class CsvExtensionsTest
    {
        [TestMethod]
        public void TestToRSBool()
        {
            Assert.AreEqual(false.ToString().ToRsViewFormat(), "\"False\"", true);            
        }

        [TestMethod]
        public void TestToRSInt()
        {
            //Assert.AreEqual( ((int?)null).ToRS(), "");
            Assert.AreEqual( ((int?)1).ToRsViewFormat(), "1");
        }

        [TestMethod]
        public void TestToRSDouble()
        {
            Assert.AreEqual( ((double?)null).ToRsViewFormat(), "");
            Assert.AreEqual( (0.0).ToRsViewFormat(), "0");
            Assert.AreEqual( (1.1).ToRsViewFormat(), "1.1");
        }

        [TestMethod]
        public void TestToRSString()
        {

            Assert.AreEqual( ((string)null).ToRsViewFormat(), "");
            
            Assert.AreEqual( string.Empty.ToRsViewFormat(), "\"\"");
            Assert.AreEqual( "".ToRsViewFormat(), "\"\"");
            
            Assert.AreEqual( "1".ToRsViewFormat(), "\"1\"");
        }



    }
}
