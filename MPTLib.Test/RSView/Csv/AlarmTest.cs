using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPT.RSView;
using MPT.RSView.Csv;

namespace MPTLib.Test.RSView.Csv
{
    [TestClass]
    public class AlarmTest
    {
        readonly Regex regexSpace = new Regex("[\x20|\r\n|\r|\n]*");

        [TestMethod]
        public void TestRSViewAlarmTreshold()
        {
            var alarmTreshold = new CsvAlarmTreshold(1.5, "FRCSA1011_1<0.0 (LL)", RSTresholdDirection.D, 1);

            var expected = @" ""C""         , ""1.5""    , ""FRCSA1011_1<0.0 (LL)"" , ""S""          , """"      , """"         , ""D""      , ""1""     ";
            expected = regexSpace.Replace(expected, "");
            var actual = alarmTreshold.ToString();
            actual = regexSpace.Replace(actual, "");
            
            Assert.AreEqual(expected, actual, true);  
        }

        [TestMethod]
        public void TestRSViewAlarmTresholdDefault()
        {

            

            var alarmTreshold = new CsvAlarmTreshold();

            var threshType = alarmTreshold.Type.ToString().ToRS();
            Assert.AreEqual("\"\"", threshType);

            
            var expected = @" """","""","""",""S"","""","""","""",""""     ";
            expected = regexSpace.Replace(expected, "");
            var actual = alarmTreshold.ToString();
            actual = regexSpace.Replace(actual, "");

            Assert.AreEqual(expected, actual, true);
        }




        [TestMethod]
        public void TestAlarmMessageDefault()
        {
            var actual = @" ""S"",            """",       """" ";
            actual = regexSpace.Replace(actual, "");

            var msg = new AlarmMessage()
                                {
                                    Source = AlarmMessage.MessageSource.S,
                                };
            var expected = regexSpace.Replace(msg.ToString(), "");
            Assert.AreEqual(expected, actual, true);
        }

        [TestMethod]
        public void TestAnalogAlarmMessage()
        {
            var actual = @" """"          , """"                 , ""S""                ";
            actual = regexSpace.Replace(actual, "");

            var msg = new AnalogAlarmMessage()
                                {
                                    Source = AlarmMessage.MessageSource.S,
                                };
            var expected = regexSpace.Replace(msg.ToString(), "");
            Assert.AreEqual(expected, actual, true);
        }
        

        [TestMethod]
        public void TestAnalogAlarm1()
        {
          
            var a = @" ""A""      , ""AI\F1011_1\s"" , """"              , ""N""               , """"        , ""N""         , 0            , ""A""         , """"             , """"                 , """"          , """"                 , ""S""               , """"                 , """"                    , ""S""                      , 
""C"", ""1.5""    , ""FRCSA1011_1<0.0 (LL)"" , ""S""          , """"      , """"         , ""D""      , ""1""     , 
""C"", ""2.5""    , ""FRCSA1011_1<1.0 (L)""  , ""S""          ,"""","""",""D"",""1"",                                  
""C"",""3.5"",""FRCSA1011_1<1.1"",""S"","""","""",""D"",""1"",                                           
""C"",""4.5"",""FRCSA1011_1>2.1"",""S"","""","""",""I"",""1"",                                       
""C"",""5.5"",""H"",""S"","""","""",""I"",""1"",                                                      
""C"",""6.5"",""HH"",  ""S"","""","""",""I"",""1"",                                                   
"""","""","""",""S"","""","""","""","""",                                                             
"""","""","""",""S"","""","""","""","""" ";
            
            var alarm = new AnalogAlarm()
                        {
                            TagName = @"AI\F1011_1\s",
                        };
            alarm.Tresholds[1] = new CsvAlarmTreshold(1.5, "FRCSA1011_1<0.0 (LL)", RSTresholdDirection.D, 1);
            alarm.Tresholds[2] = new CsvAlarmTreshold(2.5, "FRCSA1011_1<1.0 (L)", RSTresholdDirection.D, 1);
            alarm.Tresholds[3] = new CsvAlarmTreshold(3.5, "FRCSA1011_1<1.1", RSTresholdDirection.D, 1);
            alarm.Tresholds[4] = new CsvAlarmTreshold(4.5, "FRCSA1011_1>2.1", RSTresholdDirection.I, 1);
            alarm.Tresholds[5] = new CsvAlarmTreshold(5.5, "H", RSTresholdDirection.I, 1);
            alarm.Tresholds[6] = new CsvAlarmTreshold(6.5, "HH", RSTresholdDirection.I, 1);


            var expected = a;
            expected = regexSpace.Replace(expected, "");

            var actual = alarm.ToString();
            actual = regexSpace.Replace(actual, "");

            Assert.AreEqual(expected, actual, true);
        }

        [TestMethod]
        public void TestAnalogAlarm2()
        {
            var b = 
@" ""A""      ,""Logic\P1\KEY""  , """"              , ""N""               , """"        , ""N""         , 0            , ""A""         , """"             , """"                 , """"          , """"                 , ""S""               , """"                 , """"                    , ""S""                      , 

""C"", ""1.5""    , ""ABC""   , ""S""          , """"      , """"         , ""D""      , ""1""     , 
""C"", ""1.7""    , ""?-1: ???? - ???????""  , ""S""          ,"""","""",""I"",""1"",                                  
""C"",""2.5"",""?-1: ???? - ??????"",""S"","""","""",""I"",""1"",                                        
"""","""","""",                   ""S"","""","""","""","""",                                         
"""","""","""",     ""S"","""","""","""","""",                                                        
"""","""","""",        ""S"","""","""","""","""",                                                     
"""","""","""",""S"","""","""","""","""",                                                             
"""","""","""",""S"","""","""","""",""""";
           

            var alarm = new AnalogAlarm()
            {
                TagName = @"Logic\P1\KEY",
            };
            alarm.Tresholds[1] = new CsvAlarmTreshold(1.5, "ABC", RSTresholdDirection.D, 1);
            alarm.Tresholds[2] = new CsvAlarmTreshold(1.7, "?-1: ???? - ???????", RSTresholdDirection.I, 1);
            alarm.Tresholds[3] = new CsvAlarmTreshold(2.5, "?-1: ???? - ??????", RSTresholdDirection.I, 1);



            var expected = b;
            expected = regexSpace.Replace(expected, "");

            var actual = alarm.ToString();
            actual = regexSpace.Replace(actual, "");

            Assert.AreEqual(expected, actual, true);
        }

        [TestMethod]
        public void TestAnalogAlarm3()
        {

            var c = 
@" ""A""      ,""Logic\P1\MODE"" , """"              , ""N""               , """"        , ""N""         , 0            , ""A""         , """"             , """"                 , """"          , """"                 , ""S""               , """"                 , """"                    , ""S""                      , 
""C""         , ""1.1""    , ""ABC:ABC"" , ""S""          , """"      , """"         , ""D""      , ""1""     , 
""C""         , ""1.9""    , ""?-1: ????? - ? ??????"", ""S""          ,"""","""",""I"",""1"",                                  
"""","""","""",""S"","""","""","""","""",                                                                
"""","""","""",                   ""S"","""","""","""","""",                                         
"""","""","""",     ""S"","""","""","""","""",                                                        
"""","""","""",        ""S"","""","""","""","""",                                                     
"""","""","""",""S"","""","""","""","""",                                                             
"""","""","""",""S"","""","""","""",""""";
            
            var alarm = new AnalogAlarm()
            {
                TagName = @"Logic\P1\MODE",
            };
            alarm.Tresholds[1] = new CsvAlarmTreshold(1.1, "ABC:ABC", RSTresholdDirection.D, 1);
            alarm.Tresholds[2] = new CsvAlarmTreshold(1.9, "?-1: ????? - ? ??????", RSTresholdDirection.I, 1);

            var expected = c;
            expected = regexSpace.Replace(expected, "");

            var actual = alarm.ToString();
            actual = regexSpace.Replace(actual, "");

            Assert.AreEqual(expected, actual, true);
        }
    }
}
