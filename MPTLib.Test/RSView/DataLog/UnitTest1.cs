using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using MPT.DataBase;
using MPT.RSView.DataLog;

namespace MPT.Test.RSView.DataLog
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var dataLogArchive = new DataLogDBF(@"_TestData\DlgLog\Dlg_min\");
            var dictionaryDbList = dataLogArchive.GetDatalogPairList();
        }
    }
}
