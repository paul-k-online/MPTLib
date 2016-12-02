using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPT.Model;

namespace MPTLib.Test.Model
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            using (var db  = new MPTEntities())
            {
                var holidays = db.GetHolidays(2015);
            }
        }
    }
}
