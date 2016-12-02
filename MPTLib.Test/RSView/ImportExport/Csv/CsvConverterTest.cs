using System;
using System.IO;
using System.Text;
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
        public void TestInit()
        {
            try
            {
                PositionListConverter PositionListConverter = new PositionListConverter(TestData.ExcelPositionList, TestData.RootElement, TestData.NodeName);
            }
            catch (Exception)
            {
                
                throw;
            }
            
            
        }

        [TestMethod]
        public void TestMethod1()
        {
            try
            {
                var tags = PositionListConverter.GetAllTags();

                var csvGenerator = new CsvGenerator(tags, TestData.NodeName);
                var tagfile = csvGenerator.GetTagStringList();
                var datalogFile = csvGenerator.GetDatalogStringList();
                var alarmFile = csvGenerator.GetAlarmStringList();


                File.WriteAllLines(@"d:\tag.csv", tagfile);
                File.WriteAllLines(@"d:\dlg.csv", datalogFile);
                File.WriteAllLines(@"d:\tag.csv", alarmFile);
            }
            catch (Exception e)
            {
                throw;
            }
            
        }


        [TestMethod]
        public void TestToCsvAnalogAlarm()
        {
            try
            {
                var rsTags = TestData.AiPos.GetTagsByDefault("test");

                var csvGenerator = new CsvGenerator(rsTags, TestData.NodeName);

                var tags = csvGenerator.GetTagStringList();
                var aAlarms = csvGenerator.GetAnalogAlarms();
                var dAlarms = csvGenerator.GetDigitalAlarms();

                var alarmFile = csvGenerator.GetAlarmStringList();
                var datalogs = csvGenerator.GetDatalogs();

                /*
                var fileStream = new StreamWriter(@"d:\file.txt", false, Encoding.GetEncoding(1251));
                var test = CsvGenerator.GetStream(tags, ref fileStream);
                fileStream.Close();
                */

                var stream = CsvGenerator.GetStream(tags);
                var f = File.OpenWrite(@"d:\file.txt");
                stream.Position = 0;
                stream.CopyTo(f);
                f.Close();



            }
            catch (Exception)
            {
                
                throw;
            }

        }


        [TestMethod]
        public void TestZip()
        {
            try
            {
                var rsTags = TestData.AiPos.GetTagsByDefault("test");
                var csvGenerator = new CsvGenerator(rsTags, TestData.NodeName);

                var zip = csvGenerator.GetZipPackageStream();
                zip.Position = 0;

                var f = File.OpenWrite(string.Format("d:\\test_{0}.zip", DateTime.Now.ToString("yy-MM-dd_hhmm")));
                zip.CopyTo(f);
                f.Close();


            }
            catch (Exception)
            {
                
                throw;
            }
        }


    }
}
