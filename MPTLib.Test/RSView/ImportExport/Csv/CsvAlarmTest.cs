using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPT.RSView;
using MPT.RSView.ImportExport.Csv;
using MPT.RSView.ImportExport.Csv.Tag;

namespace MPTLib.Test.RSView.ImportExport.Csv
{
    [TestClass]
    public class CsvAlarmTest
    {
        static readonly Regex RegexSpace = new Regex("[\x20|\r\n|\r|\n]*");

        [TestMethod]
        public void TestRsViewAlarmTreshold()
        {
            var alarmTreshold = new CsvAnalogAlarmTreshold(1.5, "FRCSA1011_1<0.0 (LL)", RsViewTresholdDirection.D);
            var expected = @"""C"",""1.5"",""FRCSA1011_1<0.0(LL)"",""S"","""","""",""D"",""1""";
            expected = RegexSpace.Replace(expected, "");
            var actual = alarmTreshold.ToString();
            actual = RegexSpace.Replace(actual, "");
            Assert.AreEqual(expected, actual, true);  
        }

        [TestMethod]
        public void TestRsViewAlarmTresholdDefault()
        {
            var alarmTreshold = new CsvAnalogAlarmTreshold();
            var threshType = alarmTreshold.Type.ToString().ToRsViewFormat();
            Assert.AreEqual("\"\"", threshType);
            var expected = @" """","""","""",""S"","""","""","""",""""     ";
            expected = RegexSpace.Replace(expected, "");
            var actual = alarmTreshold.ToString();
            actual = RegexSpace.Replace(actual, "");
            Assert.AreEqual(expected, actual, true);
        }
        
        [TestMethod]
        public void TestAlarmMessageDefault()
        {
            var actual = @" ""S"",            """",       """" ";
            actual = RegexSpace.Replace(actual, "");

            var msg = new CsvAlarmMessage()
                                {
                                    Source = CsvAlarmMessage.MessageSource.S,
                                };
            var expected = RegexSpace.Replace(msg.ToString(), "");
            Assert.AreEqual(expected, actual, true);
        }

        [TestMethod]
        public void TestAnalogAlarmMessage()
        {
            var actual = @" """"          , """"                 , ""S""                ";
            actual = RegexSpace.Replace(actual, "");

            var msg = new CsvAnalogAlarmMessage()
                                {
                                    Source = CsvAlarmMessage.MessageSource.S,
                                };
            var expected = RegexSpace.Replace(msg.ToString(), "");
            Assert.AreEqual(expected, actual, true);
        }
        

        [TestMethod]
        public void TestAnalogAlarm1()
        {
            var a = @"
""A"",""AI\F1011_1\s"","""",""N"","""",""N"",0,""A"","""","""","""","""",""S"","""","""",""S"",
""C"",""1.5"",""FRCSA1011_1<0.0(LL)"",""S"","""","""",""D"",""1"",
""C"",""2.5"",""FRCSA1011_1<1.0(L)"",""S"","""","""",""D"",""1"",
""C"",""3.5"",""FRCSA1011_1<1.1"",""S"","""","""",""D"",""1"",
""C"",""4.5"",""FRCSA1011_1>2.1"",""S"","""","""",""I"",""1"",
""C"",""5.5"",""H"",""S"","""","""",""I"",""1"",
""C"",""6.5"",""HH"",""S"","""","""",""I"",""1"",
"""","""","""",""S"","""","""","""","""",
"""","""","""",""S"","""","""","""",""""";

            var alarm = new CsvAnalogAlarm(@"AI\F1011_1\s");
            alarm.Tresholds[1] = new CsvAnalogAlarmTreshold(1.5, "FRCSA1011_1<0.0 (LL)", RsViewTresholdDirection.D, 1);
            alarm.Tresholds[2] = new CsvAnalogAlarmTreshold(2.5, "FRCSA1011_1<1.0 (L)", RsViewTresholdDirection.D, 1);
            alarm.Tresholds[3] = new CsvAnalogAlarmTreshold(3.5, "FRCSA1011_1<1.1", RsViewTresholdDirection.D, 1);
            alarm.Tresholds[4] = new CsvAnalogAlarmTreshold(4.5, "FRCSA1011_1>2.1", RsViewTresholdDirection.I, 1);
            alarm.Tresholds[5] = new CsvAnalogAlarmTreshold(5.5, "H", RsViewTresholdDirection.I, 1);
            alarm.Tresholds[6] = new CsvAnalogAlarmTreshold(6.5, "HH", RsViewTresholdDirection.I, 1);


            var expected = a;
            expected = RegexSpace.Replace(expected, "");

            var actual = alarm.ToString();
            actual = RegexSpace.Replace(actual, "");

            Assert.AreEqual(expected, actual, true);
        }

        [TestMethod]
        public void TestAnalogAlarm2()
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
            alarm.Tresholds[1] = new CsvAnalogAlarmTreshold(1.5, "ABC", RsViewTresholdDirection.D, 1);
            alarm.Tresholds[2] = new CsvAnalogAlarmTreshold(1.7, "?-1: ???? - ???????", RsViewTresholdDirection.I, 1);
            alarm.Tresholds[3] = new CsvAnalogAlarmTreshold(2.5, "?-1: ???? - ??????", RsViewTresholdDirection.I, 1);
            
            var expected = b;
            expected = RegexSpace.Replace(expected, "");

            var actual = alarm.ToString();
            actual = RegexSpace.Replace(actual, "");
            Assert.AreEqual(expected, actual, true);
        }

        [TestMethod]
        public void TestAnalogAlarm3()
        {
            var c = @"
""A"",""Logic\P1\MODE"","""",""N"","""",""N"",0,""A"","""","""","""","""",""S"","""","""",""S"",
""C"",""1.1"",""ABC:ABC"",""S"","""","""",""D"",""1"",
""C"",""1.9"",""?-1:?????-???????"",""S"","""","""",""I"",""1"",
"""","""","""",""S"","""","""","""","""",
"""","""","""",""S"","""","""","""","""",
"""","""","""",""S"","""","""","""","""",
"""","""","""",""S"","""","""","""","""",
"""","""","""",""S"","""","""","""","""",
"""","""","""",""S"","""","""","""",""""";
            
            var alarm = new CsvAnalogAlarm(@"Logic\P1\MODE");
            alarm.Tresholds[1] = new CsvAnalogAlarmTreshold(1.1, "ABC:ABC", RsViewTresholdDirection.D);
            alarm.Tresholds[2] = new CsvAnalogAlarmTreshold(1.9, "?-1: ????? - ? ??????", RsViewTresholdDirection.I);

            var expected = c;
            expected = RegexSpace.Replace(expected, "");

            var actual = alarm.ToString();
            actual = RegexSpace.Replace(actual, "");

            Assert.AreEqual(expected, actual, true);
        }


        [TestMethod]
        public void TeasAnalogThreashorl()
        {
            string[] a =
            {
                @"""C"",""1.5"",""FRCSA1011_1<0.0(LL)"",""S"","""","""",""D"",""1""",
                @"""C"",""2.5"",""FRCSA1011_1<1.0(L)"",""S"","""","""",""D"",""1""",
                @"""C"",""3.5"",""FRCSA1011_1<1.1"",""S"","""","""",""D"",""1""",
                @"""C"",""4.5"",""FRCSA1011_1>2.1"",""S"","""","""",""I"",""1""",
                @"""C"",""5.5"",""H"",""S"","""","""",""I"",""1""",
                @"""C"",""6.5"",""HH"",""S"","""","""",""I"",""1""",

                @"""C"",""1.5"",""П-1:Ключ-Закрыт"",""S"","""","""",""D"",""1""",
                @"""C"",""1.7"",""П-1:Ключ-Автомат"",""S"","""","""",""I"",""1""",
                @"""C"",""2.5"",""П-1:Ключ-Открыт"",""S"","""","""",""I"",""1""",
                
                @"""C"",""1.1"",""П-1:Режим-Простой"",""S"","""","""",""D"",""1""",
                @"""C"",""1.9"",""П-1:Режим-ВРаботе"",""S"","""","""",""I"",""1""",

                @""""","""","""",""S"","""","""","""",""""",
            };


            var a0 = new CsvAnalogAlarmTreshold(1.5, "FRCSA1011_1<0.0(LL)", RsViewTresholdDirection.D);
            var a1 = new CsvAnalogAlarmTreshold(2.5, "FRCSA1011_1<1.0(L)", RsViewTresholdDirection.D);
            var a2 = new CsvAnalogAlarmTreshold(3.5, "FRCSA1011_1<1.1", RsViewTresholdDirection.D);
            var a3 = new CsvAnalogAlarmTreshold(4.5, "FRCSA1011_1>2.1", RsViewTresholdDirection.I);
            var a4 = new CsvAnalogAlarmTreshold(5.5, "H", RsViewTresholdDirection.I);
            var a5 = new CsvAnalogAlarmTreshold(6.5, "HH", RsViewTresholdDirection.I);



            var act0 = RegexSpace.Replace(a0.ToString(), "");
            var act1 = RegexSpace.Replace(a1.ToString(), "");
            var act2 = RegexSpace.Replace(a2.ToString(), "");
            var act3 = RegexSpace.Replace(a3.ToString(), "");
            var act4 = RegexSpace.Replace(a4.ToString(), "");
            var act5 = RegexSpace.Replace(a5.ToString(), "");



            Assert.AreEqual(act0, a[0], true);
            Assert.AreEqual(act1, a[1], true);
            Assert.AreEqual(act2, a[2], true);
            Assert.AreEqual(act3, a[3], true);
            Assert.AreEqual(act4, a[4], true);
            Assert.AreEqual(act5, a[5], true);





        }
    }
}
