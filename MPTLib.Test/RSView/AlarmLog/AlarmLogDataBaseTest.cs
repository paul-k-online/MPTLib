using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using MPT.RSView.DataLog;
using MPT.DataBase;

namespace MPTLib.Test.RSView.AlarmLog
{
    [TestClass]
    public class LogRecordTest
    {
        [TestMethod]
        public void AlarmLogRecordTest()
        {
            var logDB = new DBFDataBase(@"_TestData\DLGLOG\ALMLOG");
            var logTables = logDB.GetTableList().Where(tableName => MPT.RSView.DataLog.AlarmLog.FileNameRegex.IsMatch(tableName));
            foreach (var logTable in logTables)
            {
                var list = logDB.ReadTable(MPT.RSView.DataLog.AlarmLog.FromDBF, logTable).ToList();
                Assert.IsTrue(list.Any());
            }
        }

        [TestMethod]
        public void ActLogRecordTest()
        {
            var logDB = new DBFDataBase(@"_TestData\DLGLOG\ACTLOG");
            var logTables = logDB.GetTableList().Where(tableName => MPT.RSView.DataLog.ActLog.FileNameRegex.IsMatch(tableName));
            foreach (var logTable in logTables)
            {
                var list = logDB.ReadTable(MPT.RSView.DataLog.ActLog.FromDBF, logTable).ToList();
                Assert.IsTrue(list.Any());
            }
        }
    }
}
