using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPT.RSView.Csv;

namespace MPTLib.Test.RSView.Csv
{
    [TestClass]
    public class DataLogTest
    {
        [TestMethod]
        public void TestRSViewDataLogTest()
        {
            var datalog = new DataLog()
                                    {
                                        TagName = @"DIO\LAH503",
                                        ModelName = "Dlg_dio"
                                    };
            var t = @"""DIO\LAH503"",""Dlg_dio""";
            Assert.AreEqual(datalog.ToString(), t);
        }
    }
}
