using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using MPT.RSView.ImportExport.Csv;

using static MPT.RSView.RSViewAnalogAlarm;

namespace MPTLib.Test.RSView.ImportExport.Csv
{
    [TestClass]
    public class CsvAlarmTest
    {
        [TestMethod]
        public void TestRsViewAlarmTreshold()
        {

            var expected = @"""C"",""1.5"",""FRCSA1011_1<0.0(LL)"",""S"","""","""",""D"",""1""";
            expected = CsvTagTest.RegexSpace.Replace(expected, "");

            var alarmTreshold = new CsvAnalogAlarmTreshold("1.5", "FRCSA1011_1<0.0 (LL)", RSViewTresholdDirection.D);
            var actual = alarmTreshold.ToCsvString();
            actual = CsvTagTest.RegexSpace.Replace(actual, "");

            Assert.AreEqual(expected, actual, true);
        }

        [TestMethod]
        public void Test_CsvAlarmTreshold()
        {
            var alarmTreshold = new CsvAnalogAlarmTreshold();
            var threshType = alarmTreshold.Type.ToString().ToCsvString();
            Assert.AreEqual("\"\"", threshType);

            var expected = @" """","""","""",""S"","""","""","""",""""  ";
            var actual = alarmTreshold.ToCsvString();

            actual = CsvTagTest.RegexSpace.Replace(actual, "");
            expected = CsvTagTest.RegexSpace.Replace(expected, "");

            Assert.AreEqual(CsvTagTest.RegexSpace.Replace(expected, ""), CsvTagTest.RegexSpace.Replace(actual, ""), true);
        }
        
        [TestMethod]
        public void Test_CsvAlarmMessage()
        {
            var actual = @" ""S"",            """",       """" ";
            actual = CsvTagTest.RegexSpace.Replace(actual, "");

            var msg = new CsvAlarmMessage();
            var expected = CsvTagTest.RegexSpace.Replace(msg.ToCsvString(), "");
            Assert.AreEqual(expected, actual, true);
        }

        [TestMethod]
        public void Test_CsvAnalogAlarmMessage()
        {
            var actual = @" """"          , """"                 , ""S""                ";
            actual = CsvTagTest.RegexSpace.Replace(actual, "");

            var analogAlarmMessage = new CsvAnalogAlarmMessage();

            var expected = analogAlarmMessage.ToCsvString();
            expected = CsvTagTest.RegexSpace.Replace(expected, "");

            Assert.AreEqual(expected, actual, true);
        }
        
        [TestMethod]
        public void Test_CsvAnalogAlarm1()
        {
            var expected = @"
""A"",""AI\F1011_1\s"","""",""N"","""",""N"",0,""A"","""","""","""","""",""S"","""","""",""S"",
""C"",""1.5"",""FRCSA1011_1<0.0(LL)"",  ""S"","""","""",""D"",""1"",
""C"",""2.5"",""FRCSA1011_1<1.0(L)"",   ""S"","""","""",""D"",""1"",
""C"",""3.5"",""FRCSA1011_1<1.1"",      ""S"","""","""",""D"",""1"",
""C"",""4.5"",""FRCSA1011_1>2.1"",      ""S"","""","""",""I"",""1"",
""C"",""5.5"",""H"",                    ""S"","""","""",""I"",""1"",
""C"",""6.5"",""HH"",                   ""S"","""","""",""I"",""1"",
"""","""","""",                         ""S"","""","""","""","""",
"""","""","""",                         ""S"","""","""","""",""""";

            var alarm = new CsvAnalogAlarm(@"AI\F1011_1\s");
            alarm.Tresholds[1] = new CsvAnalogAlarmTreshold("1.5", "FRCSA1011_1<0.0 (LL)", RSViewTresholdDirection.D, 1);
            alarm.Tresholds[2] = new CsvAnalogAlarmTreshold("2.5", "FRCSA1011_1<1.0 (L)", RSViewTresholdDirection.D, 1);
            alarm.Tresholds[3] = new CsvAnalogAlarmTreshold("3.5", "FRCSA1011_1<1.1", RSViewTresholdDirection.D, 1);
            alarm.Tresholds[4] = new CsvAnalogAlarmTreshold("4.5", "FRCSA1011_1>2.1", RSViewTresholdDirection.I, 1);
            alarm.Tresholds[5] = new CsvAnalogAlarmTreshold("5.5", "H", RSViewTresholdDirection.I, 1);
            alarm.Tresholds[6] = new CsvAnalogAlarmTreshold("6.5", "HH", RSViewTresholdDirection.I, 1);

            var actual = alarm.ToCsvString();
            actual = CsvTagTest.RegexSpace.Replace(actual, "");

            expected = CsvTagTest.RegexSpace.Replace(expected, "");
            Assert.AreEqual(expected, actual, true);
        }

        [TestMethod]
        public void Test_CsvAnalogAlarm2()
        {
            var b = @"
""A"",""Logic\P1\KEY"","""",""N"","""",""N"",0,""A"","""","""","""","""",""S"","""","""",""S"",
""C"",""1.5"",""ABC"",""S"","""","""",""D"",""1"",
""C"",""1.7"",""?-1:????-???????"",""S"","""","""",""I"",""1"",
""C"",""2.5"",""?-1:????-??????"",""S"","""","""",""I"",""1"",
"""","""","""",""S"","""","""","""","""",
"""","""","""",""S"","""","""","""","""",
"""","""","""",""S"","""","""","""","""",
"""","""","""",""S"","""","""","""","""",
"""","""","""",""S"","""","""","""",""""";
           
            var alarm = new CsvAnalogAlarm(@"Logic\P1\KEY");
            alarm.Tresholds[1] = new CsvAnalogAlarmTreshold("1.5", "ABC", RSViewTresholdDirection.D, 1);
            alarm.Tresholds[2] = new CsvAnalogAlarmTreshold("1.7", "?-1: ???? - ???????", RSViewTresholdDirection.I, 1);
            alarm.Tresholds[3] = new CsvAnalogAlarmTreshold("2.5", "?-1: ???? - ??????", RSViewTresholdDirection.I, 1);
            
            var expected = b;
            expected = CsvTagTest.RegexSpace.Replace(expected, "");

            var actual = alarm.ToCsvString();
            actual = CsvTagTest.RegexSpace.Replace(actual, "");
            Assert.AreEqual(expected, actual, true);
        }

        [TestMethod]
        public void Test_CsvAnalogAlarm3()
        {
            var expected = @"
""A"",""Logic\P1\MODE"","""",""N"","""",""N"",0,""A"","""","""","""","""",""S"","""","""",""S"",
""C"",""1.1"",""ABC:ABC"",""S"","""","""",""D"",""1"",
""C"",""1.9"",""?-1:?????-???????"",""S"","""","""",""I"",""1"",
"""","""","""",""S"","""","""","""","""",
"""","""","""",""S"","""","""","""","""",
"""","""","""",""S"","""","""","""","""",
"""","""","""",""S"","""","""","""","""",
"""","""","""",""S"","""","""","""","""",
"""","""","""",""S"","""","""","""","""" 
";
            
            var alarm = new CsvAnalogAlarm(@"Logic\P1\MODE");
            alarm.Tresholds[1] = new CsvAnalogAlarmTreshold("1.1", "ABC:ABC", RSViewTresholdDirection.D);
            alarm.Tresholds[2] = new CsvAnalogAlarmTreshold("1.9", "?-1: ????? - ? ??????", RSViewTresholdDirection.I);

            var actual = alarm.ToCsvString();

            expected = CsvTagTest.RegexSpace.Replace(expected, "");
            actual = CsvTagTest.RegexSpace.Replace(actual, "");

            Assert.AreEqual(expected, actual, true);
        }


        [TestMethod]
        public void Test_CsvAnalogAlarmTreshold()
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

            var rs = new List<CsvAnalogAlarmTreshold>() {
                new CsvAnalogAlarmTreshold("1.5", "FRCSA1011_1<0.0(LL)", RSViewTresholdDirection.D),
                new CsvAnalogAlarmTreshold("2.5", "FRCSA1011_1<1.0(L)", RSViewTresholdDirection.D),
                new CsvAnalogAlarmTreshold("3.5", "FRCSA1011_1<1.1", RSViewTresholdDirection.D),
                new CsvAnalogAlarmTreshold("4.5", "FRCSA1011_1>2.1", RSViewTresholdDirection.I),
                new CsvAnalogAlarmTreshold("5.5", "H", RSViewTresholdDirection.I),
                new CsvAnalogAlarmTreshold("6.5", "HH", RSViewTresholdDirection.I),
            };


            var act = rs.Select(x=>x.ToCsvString())
                            .Select(x => CsvTagTest.RegexSpace.Replace(x, "")).ToArray();
            exp = exp
                            .Select(x => CsvTagTest.RegexSpace.Replace(x, "")).ToArray();

            for (int i = 0; i < 6; i++)
            {
                var a1 = exp[i];
                var a2 = act[i];
                Assert.AreEqual(act[i], exp[i], true);
            }
        }
    }
}
