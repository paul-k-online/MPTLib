using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using MPT.RSView;
using MPT.RSView.ImportExport.Csv;

namespace MPT.Test.RSView.ImportExport.CSV
{
    [TestClass]
    public class CsvDataLogTest
    {
        [TestMethod]
        public void CsvDataLogTest1()
        {
            var d = new CsvDataLog() { TagName = "t1", ModelName = "d" };
            var a = d.ToCsvString();
            var e = @"""t1"",""d""";
            Assert.AreEqual(a,e);
        }
    }
}
