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
            Assert.AreEqual(false.ToString().ToCsvString(), "\"False\"", true);            
        }

        [TestMethod]
        public void TestToRSInt()
        {
            //Assert.AreEqual( ((int?)null).ToRS(), "");
            Assert.AreEqual( ((int?)1).ToCsvString(), "1");
        }

        [TestMethod]
        public void TestToRSDouble()
        {
            Assert.AreEqual( ((double?)null).ToCsvString(), "");
            Assert.AreEqual( (0.0).ToCsvString(), "0");
            Assert.AreEqual( (1.1).ToCsvString(), "1.1");
        }

        [TestMethod]
        public void TestToRSString()
        {

            Assert.AreEqual( ((string)null).ToCsvString(), "");
            
            Assert.AreEqual( string.Empty.ToCsvString(), "\"\"");
            Assert.AreEqual( "".ToCsvString(), "\"\"");
            
            Assert.AreEqual( "1".ToCsvString(), "\"1\"");
        }



    }
}
