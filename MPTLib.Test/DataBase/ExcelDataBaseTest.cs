using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPT.DataBase;
using MPTLib.Test.RSView;

namespace MPTLib.Test.Excel
{
    [TestClass]
    public class ExcelDataBaseTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var e = new ExcelDataBase(TestData._101_PP23.ExcelFilePath);
            Assert.AreEqual(e.SheetList.Contains("AI"), true);


        }
    }
}
