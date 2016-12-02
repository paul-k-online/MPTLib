using System;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPT.Positions;
using MPT.RSView.ImportExport;
using MPT.RSView.ImportExport.Csv;

namespace MPTLib.Test.RSView.ImportExport.Csv
{
    [TestClass]
    public class CsvConverterTest
    {
        public static PositionListConverter PositionListConverter = new PositionListConverter(TestData.ExcelPositionList, TestData.RootElement, TestData.NodeName);


        [TestMethod]
        public void TestMethod1()
        {
            try
            {
                TestData.ExcelPositionList.ReadAllData();
                var tags = PositionListConverter.GetAllTags();

                var file = new CsvConverter(tags, TestData.NodeName);
                var datalogs = file.GetDatalogs();
                var folderTags = file.GetFolderTags();

                var aa = file.GetAnalogAlarms();
            }
            catch (Exception e)
            {
                throw;
            }
            
        }


        [TestMethod]
        public void TestToCsvAnalogAlarm()
        {
            var rsTags = TestData.AiPos.GetTagsByDefault("test");

            var file = new CsvConverter(rsTags, TestData.NodeName);

            var f = file.GetTagFile();
            var content = string.Join(Environment.NewLine, f);

            var aAlarms = file.GetAnalogAlarms();
            var dAlarms = file.GetDigitalAlarms();

            var alarmFile = file.GetAlarmFile();
            var datalogs = file.GetDatalogs();



        }
    }
}
