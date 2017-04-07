using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using MPT.RSView.ImportExport.Csv;
using static MPT.RSView.RSViewAnalogAlarm;

namespace MPTLib.Test.RSView.ImportExport.Csv
{
    [TestClass()]
    public class CsvAlarmTest
    {
        [TestMethod]
        public void CsvAnalogAlarmTreshold_Test()
        {
            var expected = @"""C"",""1.5"",""FRCSA1011_1<0.0(LL)"",""S"","""","""",""D"",""1""";

            var alarmTreshold = new CsvAnalogAlarmTreshold("1.5", "FRCSA1011_1<0.0 (LL)", TresholdDirection.D);
            var actual = alarmTreshold.ToCsvString();

            Assert.AreEqual(CsvTagTest.CsvRegexClearSpace.Replace(expected, ""),
                            CsvTagTest.CsvRegexClearSpace.Replace(actual, ""), true);
        }

        [TestMethod]
        public void CsvAnalogAlarmTreshold1_Test()
        {
            var alarmTreshold = new CsvAnalogAlarmTreshold();
            var threshType = alarmTreshold.Type.ToString().ToCsvString();
            Assert.AreEqual("\"\"", threshType);

            var expected = @" """","""","""",""S"","""","""","""",""""  ";
            var actual = alarmTreshold.ToCsvString();

            Assert.AreEqual(CsvTagTest.CsvRegexClearSpace.Replace(expected, ""), 
                            CsvTagTest.CsvRegexClearSpace.Replace(actual, ""), true);
        }
        
        [TestMethod]
        public void Test_CsvAlarmMessage()
        {
            var actual = @" ""S"",            """",       """" ";

            var msg = new CsvAlarmMessage();
            var expected = msg.ToCsvString();

            Assert.AreEqual(CsvTagTest.CsvRegexClearSpace.Replace(expected, ""),
                            CsvTagTest.CsvRegexClearSpace.Replace(actual, ""), true);
        }

        [TestMethod]
        public void Test_CsvAnalogAlarmMessage()
        {
            var actual = @" """"          , """"                 , ""S""                ";

            var analogAlarmMessage = new CsvAnalogAlarmMessage();
            var expected = analogAlarmMessage.ToCsvString();

            Assert.AreEqual(CsvTagTest.CsvRegexClearSpace.Replace(expected, ""),
                            CsvTagTest.CsvRegexClearSpace.Replace(actual, ""), true);
        }
        
        [TestMethod]
        public void CsvAnalogAlarm1_Test()
        {
            var expected = @"
                    ""A"",""AI\F1011_1\s"","""",""N"","""",""N"",0,""A"","""","""","""","""",""S"","""","""",""S"",
                    ""C"",""1.5"",""FRCSA1011_1<0.0(LL)"",  ""S"","""","""",""D"",""1"",
                    ""C"",""2.5"",""FRCSA1011_1<1.0(L)"",   ""S"","""","""",""D"",""1"",
                    ""C"",""3.5"",""FRCSA1011_1<1.1"",      ""S"","""","""",""D"",""1"",
                    ""C"",""4.5"",""FRCSA1011_1>2.1"",      ""S"","""","""",""I"",""1"",
                    ""C"",""5.5"",""H"",                    ""S"","""","""",""I"",""1"",
                    ""C"",""6.5"",""HH"",                   ""S"","""","""",""I"",""1"",
                    """","""","""",                         ""S"","""","""","""", """",
                    """","""","""",                         ""S"","""","""","""", """"";

            var alarm = new CsvAnalogAlarm(@"AI\F1011_1\s");
            alarm.Tresholds[1] = new CsvAnalogAlarmTreshold("1.5", "FRCSA1011_1<0.0 (LL)", TresholdDirection.D, 1);
            alarm.Tresholds[2] = new CsvAnalogAlarmTreshold("2.5", "FRCSA1011_1<1.0 (L)", TresholdDirection.D, 1);
            alarm.Tresholds[3] = new CsvAnalogAlarmTreshold("3.5", "FRCSA1011_1<1.1", TresholdDirection.D, 1);
            alarm.Tresholds[4] = new CsvAnalogAlarmTreshold("4.5", "FRCSA1011_1>2.1", TresholdDirection.I, 1);
            alarm.Tresholds[5] = new CsvAnalogAlarmTreshold("5.5", "H", TresholdDirection.I, 1);
            alarm.Tresholds[6] = new CsvAnalogAlarmTreshold("6.5", "HH", TresholdDirection.I, 1);

            var actual = alarm.ToCsvString();

            Assert.AreEqual(CsvTagTest.CsvRegexClearSpace.Replace(expected, ""),
                            CsvTagTest.CsvRegexClearSpace.Replace(actual, ""), true);
        }

        [TestMethod]
        public void CsvAnalogAlarm2_Test()
        {
            var expected = @"
                    ""A"",""Logic\P1\KEY"","""",""N"","""",""N"",0,""A"","""","""","""","""",""S"","""","""",""S"",
                    ""C"",""1.5"",""ABC"",""S"","""","""",""D"",""1"",
                    ""C"",""1.7"",""1:"",""S"","""","""",""I"",""1"",
                    ""C"",""2.5"",""2:"",""S"","""","""",""I"",""1"",
                    """","""","""",""S"","""","""","""","""",
                    """","""","""",""S"","""","""","""","""",
                    """","""","""",""S"","""","""","""","""",
                    """","""","""",""S"","""","""","""","""",
                    """","""","""",""S"","""","""","""",""""";
           
            var alarm = new CsvAnalogAlarm(@"Logic\P1\KEY");
            alarm.Tresholds[1] = new CsvAnalogAlarmTreshold("1.5", "ABC", TresholdDirection.D, 1);
            alarm.Tresholds[2] = new CsvAnalogAlarmTreshold("1.7", "1:", TresholdDirection.I, 1);
            alarm.Tresholds[3] = new CsvAnalogAlarmTreshold("2.5", "2:", TresholdDirection.I, 1);
            var actual = alarm.ToCsvString();

            Assert.AreEqual(CsvTagTest.CsvRegexClearSpace.Replace(expected, ""),
                            CsvTagTest.CsvRegexClearSpace.Replace(actual, ""), true);
        }

        [TestMethod]
        public void CsvAnalogAlarm3_Test()
        {
            var expected = @"
                    ""A"",""Logic\P1\MODE"","""",""N"","""",""N"",0,""A"","""","""","""","""",""S"","""","""",""S"",
                    ""C"",""1.1"",""ABC:ABC"",""S"","""","""",""D"",""1"",
                    ""C"",""1.9"",""щдщдщ"",""S"","""","""",""I"",""1"",
                    """","""","""",""S"","""","""","""","""",
                    """","""","""",""S"","""","""","""","""",
                    """","""","""",""S"","""","""","""","""",
                    """","""","""",""S"","""","""","""","""",
                    """","""","""",""S"","""","""","""","""",
                    """","""","""",""S"","""","""","""","""" 
                    ";
            
            var alarm = new CsvAnalogAlarm(@"Logic\P1\MODE");
            alarm.Tresholds[1] = new CsvAnalogAlarmTreshold("1.1", "ABC:ABC", TresholdDirection.D);
            alarm.Tresholds[2] = new CsvAnalogAlarmTreshold("1.9", "щдщдщ", TresholdDirection.I);
            var actual = alarm.ToCsvString();

            Assert.AreEqual(CsvTagTest.CsvRegexClearSpace.Replace(expected, ""),
                            CsvTagTest.CsvRegexClearSpace.Replace(actual, ""), true);
        }


        [TestMethod]
        public void CsvAnalogAlarmTreshold3_Test()
        {
            string[] exp =
            {
                @"  ""C"",""1.5"",""FRCSA1011_1<0.0(LL)"",""S"","""","""",""D"",""1""    ",
                @"  ""C"",""2.5"",""FRCSA1011_1<1.0(L)"",""S"","""","""",""D"",""1"" ",
                @"  ""C"",""3.5"",""FRCSA1011_1<1.1"",""S"","""","""",""D"",""1""    ",
                @"  ""C"",""4.5"",""FRCSA1011_1>2.1"",""S"","""","""",""I"",""1""    ",
                @"  ""C"",""5.5"",""H"",""S"","""","""",""I"",""1""                  ",
                @"  ""C"",""6.5"",""HH"",""S"","""","""",""I"",""1""                 ",

                @"  ""C"",""1.5"",""П-1:Ключ-Закрыт"",""S"","""","""",""D"",""1""    ",
                @"  ""C"",""1.7"",""П-1:Ключ-Автомат"",""S"","""","""",""I"",""1""   ",
                @"  ""C"",""2.5"",""П-1:Ключ-Открыт"",""S"","""","""",""I"",""1""    ",

                @"  ""C"",""1.1"",""П-1:Режим-Простой"",""S"","""","""",""D"",""1""  ",
                @"  ""C"",""1.9"",""П-1:Режим-ВРаботе"",""S"","""","""",""I"",""1""  ",

                @"  """","""","""",""S"","""","""","""",""""                        ",
            };

            CsvAnalogAlarmTreshold[] act = 
            {
                new CsvAnalogAlarmTreshold("1.5", "FRCSA1011_1<0.0(LL)", TresholdDirection.D),
                new CsvAnalogAlarmTreshold("2.5", "FRCSA1011_1<1.0(L)", TresholdDirection.D),
                new CsvAnalogAlarmTreshold("3.5", "FRCSA1011_1<1.1", TresholdDirection.D),
                new CsvAnalogAlarmTreshold("4.5", "FRCSA1011_1>2.1", TresholdDirection.I),
                new CsvAnalogAlarmTreshold("5.5", "H", TresholdDirection.I),
                new CsvAnalogAlarmTreshold("6.5", "HH", TresholdDirection.I),
            };

            for (int i = 0; i < 6; i++)
            {
                var a = CsvTagTest.CsvRegexClearSpace.Replace(act[i].ToCsvString(), "");
                var e = CsvTagTest.CsvRegexClearSpace.Replace(exp[i], "");
                Assert.AreEqual(a, e, true);
            }
        }
    }
}
