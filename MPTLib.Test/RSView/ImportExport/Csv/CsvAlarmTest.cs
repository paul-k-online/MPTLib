using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPT.RSView;
using MPT.RSView.ImportExport.Csv;

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
            var expected = @" ""C""         , ""1.5""    , ""FRCSA1011_1<0.0 (LL)"" , ""S""          , """"      , """"         , ""D""      , ""1""     ";
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
          
            var a = @" ""A""      , ""AI\F1011_1\s"" , """"              , ""N""               , """"        , ""N""         , 0            , ""A""         , """"             , """"                 , """"          , """"                 , ""S""               , """"                 , """"                    , ""S""                      , 
""C"", ""1.5""    , ""FRCSA1011_1<0.0 (LL)"" , ""S""          , """"      , """"         , ""D""      , ""1""     , 
""C"", ""2.5""    , ""FRCSA1011_1<1.0 (L)""  , ""S""          ,"""","""",""D"",""1"",                                  
""C"",""3.5"",""FRCSA1011_1<1.1"",""S"","""","""",""D"",""1"",                                           
""C"",""4.5"",""FRCSA1011_1>2.1"",""S"","""","""",""I"",""1"",                                       
""C"",""5.5"",""H"",""S"","""","""",""I"",""1"",                                                      
""C"",""6.5"",""HH"",  ""S"","""","""",""I"",""1"",                                                   
"""","""","""",""S"","""","""","""","""",                                                             
"""","""","""",""S"","""","""","""","""" ";

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
            
            var alarm = new CsvAnalogAlarm(@"Logic\P1\MODE");
            alarm.Tresholds[1] = new CsvAnalogAlarmTreshold(1.1, "ABC:ABC", RsViewTresholdDirection.D, 1);
            alarm.Tresholds[2] = new CsvAnalogAlarmTreshold(1.9, "?-1: ????? - ? ??????", RsViewTresholdDirection.I, 1);

            var expected = c;
            expected = RegexSpace.Replace(expected, "");

            var actual = alarm.ToString();
            actual = RegexSpace.Replace(actual, "");

            Assert.AreEqual(expected, actual, true);
        }
    }
}
