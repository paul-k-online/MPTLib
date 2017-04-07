using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPT.RSView;
using MPT.RSView.ImportExport.Csv;

namespace MPTLib.Test.RSView.ImportExport.Csv
{
    [TestClass]
    public class CsvTagTest
    {
        public static readonly Regex CsvRegexClearSpace = new Regex(@"[\x20\r\n\t\s]+", RegexOptions.Compiled);

        [TestMethod]
        public void TestEnumToString()
        {
            Assert.AreEqual(RSViewTag.TypeEnum.A.ToCsvString(), "\"A\"");
        }

        [TestMethod]
        public void TestFolderTag()
        {
            var expected = @"""F"", ""AI\FRCA2013_1"", """", ""F""";
            expected = CsvRegexClearSpace.Replace(expected, "");
            
            var tag = CsvTag.CreateFolder(@"AI\FRCA2013_1");
            var actual = tag.ToCsvString();
            actual = CsvRegexClearSpace.Replace(actual, "");

            Assert.AreEqual(expected, actual, true);            
        }
        
        [TestMethod]
        public void TestAnalogTagMemory()
        {
            var expected = @"""A""      , ""AI\FRCA2013_2\bmax"", ""Блокировка по MAX""      , 
""F""      , ""M""        , ""*""          , ""F""    , ""F""        , ""D""        , ""L""       , 0         , 1600      , 
0             , 1    , 0     , 0       , ""м3/ч"",                  ,                 ,                ,              ,
,           ,           ,           ,                   ,                    ,            ,                   ,                      ,";
            expected = CsvRegexClearSpace.Replace(expected, "");

            var tag = CsvTag.CreateAnalog(@"AI\FRCA2013_2\bmax", "Блокировка по MAX", 0, 1600, "м3/ч");
            var actual = tag.ToCsvString();
            actual = CsvRegexClearSpace.Replace(actual, "");

            Assert.AreEqual(expected, actual, true);
        }

        [TestMethod]
        public void Test_CsvAnalogTag()
        {
            var expected = @" ""A""      , ""AI\FRCA2013_1\v""   , ""FRCA-2013_1 Расход топливного газа поток 1 П-2""      , ""F""      , ""D""        , ""*""          , ""F""    , ""F""        , ""D""        , ""L""       , 0         , 1600      , 0             , 1    , 0     , 0       , ""м3/ч"",                  ,                 ,                ,              ,               , ""101_pp23"", ""AI[8].v"" , ""A""       ,                   ,                    ,            ,                   ,                      ,";
            expected = CsvRegexClearSpace.Replace(expected, "");

            var tag = CsvTag.CreateAnalog(@"AI\FRCA2013_1\v", "FRCA-2013_1 Расход топливного газа поток 1 П-2", 0, 1600, "м3/ч", 0)
                .SetDataSource("101_pp23", "AI[8].v");

            var actual = tag.ToCsvString();
            actual = CsvRegexClearSpace.Replace(actual, "");

            Assert.AreEqual(expected, actual, true);
        }

        [TestMethod]
        public void Test_CsvDigitalTag()
        {
            var expected = @" ""D""      , ""AI\FRCA2013_1\fo""  , ""FRCA-2013_1 Расход топливного газа поток 1 dP=16 кПа"", ""F""      , ""D""        , ""*""          , ""F""    , ""F""        ,            ,           ,           ,           ,               ,      ,       ,         ,       , ""Off""            , ""On""            , ""Off""          ,              ,               , ""101_pp23"", ""AI[8].EN"", ""A""       ,                   ,                    ,            ,                   ,                      , ";
            expected = CsvRegexClearSpace.Replace(expected, "");

            var tag = CsvTag.CreateDigit(@"AI\FRCA2013_1\fo", "FRCA-2013_1 Расход топливного газа поток 1 dP=16 кПа")
                .SetDataSource("101_pp23", "AI[8].EN");

            var actual = tag.ToCsvString();
            actual = CsvRegexClearSpace.Replace(actual, "");

            Assert.AreEqual(expected, actual, true);
        }

        [TestMethod]
        public void TestStringTag()
        {
            var expected = @"  ""S"",       ""AI\FRCSA2011_3\n"",   ""?????? ??????? ????? 3 ?-2"",                           ""F"",       ""M"",         ""*"",           ""F"",     ""F""        ,            ,           ,           ,           ,               ,      ,       ,         ,       ,                  ,                 ,                , 200          , ""FRCSA-2011_3"",           ,           ,           ,,,,,, ";
            expected = CsvRegexClearSpace.Replace(expected, "");

            var tag = CsvTag.CreateString(@"AI\FRCSA2011_3\n", "?????? ??????? ????? 3 ?-2", "FRCSA-2011_3", 200);

            var actual = tag.ToCsvString();
            actual = CsvRegexClearSpace.Replace(actual, "");

            Assert.AreEqual(expected, actual, true);
        }

        [TestMethod]
        public void Test_CsvDigitalAlarm()
        {
            var expected =
                @" ""D"",""AI\FRCA3013_2\NAN"",""ON"",""FRCA3013_2 обрыв"",""5"",""S"","""","""",""S"","""","""",""S"","""","""","""","""","""",""N"","""",""N""";
            expected = CsvRegexClearSpace.Replace(expected, "");

            var tag = new RSViewDigitalTag(@"AI\FRCA3013_2\NAN")
            {
                Alarm = new RSViewDigitalAlarm()
                {
                    Label = "FRCA3013_2 обрыв",
                    Severity = 5,
                    Type = RSViewDigitalAlarm.RSViewDigitalAlarmType.ON
                }
            };

            var actual = tag.GetCsvDigitalAlarm().ToCsvString();
            actual = CsvRegexClearSpace.Replace(actual, "");

            Assert.AreEqual(expected, actual, true);
        }

    }
}
