using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPT.WorkSchedule;

namespace MPTLib.Test.WorkSchedule
{
    [TestClass]
    public class WorkScheduleTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var day = new ScheduleDay(2015, 03, 09);

            Assert.AreEqual(day.IsRestday, false);

            var smenDay = day[(int)Smena.Day];
            Assert.AreEqual(smenDay.Hours, 8);
            Assert.AreEqual(smenDay.IsNight, false);

            //Assert.AreEqual(smenDay.Hours, day.hour_D);
            
            var smenA = day[Smena.A];
            Assert.AreEqual(smenA.Hours, 0);
            Assert.AreEqual(smenA.IsNight, false);

            var smenaB = day[Smena.B];
            Assert.AreEqual(smenaB.Hours, 12);
            Assert.AreEqual(smenaB.IsNight, true);

            var smenaC = day[Smena.C];
            Assert.AreEqual(smenaC.Hours, 12);
            Assert.AreEqual(smenaC.IsNight, false);

            var smenaD = day[Smena.D];
            Assert.AreEqual(smenaD.Hours, 0);
            Assert.AreEqual(smenaD.IsNight, false);
            //Assert.Inconclusive("TODO: Implement code to verify target");
        }

        [TestMethod]
        public void TestHoliday()
        {
            var day = new ScheduleDay(2015, 03, 08);

            Assert.AreEqual(day.IsRestday, true);
            Assert.AreEqual(day.IsHoliday, true);
            Assert.AreEqual(day.IsPreHoliday,false);

            var smenDay = day[Smena.Day];
            Assert.AreEqual(smenDay.Hours, 0);
            Assert.AreEqual(smenDay.IsNight, false);
        }

        [TestMethod]
        public void TestPreHoliday()
        {
            var day = new ScheduleDay(2015, 03, 07);
            
            Assert.AreEqual(day.IsRestday, true);
            Assert.AreEqual(day.IsPreHoliday, true);
            Assert.AreEqual(day.IsHoliday, false);

            var smenDay = day[Smena.Day];
            Assert.AreEqual(smenDay.Hours, 0);
            Assert.AreEqual(smenDay.IsNight, false);
        }
    }
}
