using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using MPT.RSView.AlarmLog;

namespace MPTLib.Test.RSView.AlarmLog
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var alarmLog = new AlarmLogDataBase(@"_TestData\DLGLOG\ALMLOG");
            foreach(var tableName in alarmLog.AlarmLogTableName)
            {
                var list = alarmLog.ReadTable(tableName);
                var l2 = list.Select(AlarmLogRecord.ConvertFromAlarmLogRecordDBF);
            }
        }
    }
}
