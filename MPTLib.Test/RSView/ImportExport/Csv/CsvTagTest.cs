using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPT.RSView;
using MPT.RSView.ImportExport.Csv;

namespace MPTLib.Test.RSView.ImportExport.Csv
{
    /// <summary>
    /// Summary description for RsViewTagTEst1
    /// </summary>
    [TestClass]
    public class CsvTagTest
    {
        readonly Regex _regexSpace = new Regex("[\x20]*");
        
        [TestMethod]
        public void TestEnumToString()
        {
            Assert.AreEqual(RsViewTagType.A.ToRsViewFormat(), "\"A\"");
        }

        [TestMethod]
        public void TestFolderTag()
        {
            var expected = @"""F"", ""AI\FRCA2013_1"", """", ""F""";
            expected = _regexSpace.Replace(expected, "");
            
            var tag = CsvTag.CreateFolder(@"AI\FRCA2013_1");
            var actual = _regexSpace.Replace(tag.ToString(), "");

            Assert.AreEqual(expected, actual, true);            
        }
        
        [TestMethod]
        public void TestAnalogTagMemory()
        {
            var expected = @"""A""      , ""AI\FRCA2013_2\bmax"", ""Блокировка по MAX""                                   , ""F""      , ""M""        , ""*""          , ""F""    , ""F""        , ""D""        , ""L""       , 0         , 1600      , 0             , 1    , 0     , 0       , ""м3/ч"",                  ,                 ,                ,              ,               ,           ,           ,           ,                   ,                    ,            ,                   ,                      ,";
            expected = _regexSpace.Replace(expected, "");
            var tag = CsvTag.CreateAnalog(@"AI\FRCA2013_2\bmax", "Блокировка по MAX", 0, 1600, "м3/ч");
            

            var actual = tag.ToString();
            actual = _regexSpace.Replace(actual, "");

            Assert.AreEqual(expected, actual, true);
        }

        [TestMethod]
        public void TestAnalogTagData()
        {
            var expected = @" ""A""      , ""AI\FRCA2013_1\v""   , ""FRCA-2013_1 Расход топливного газа поток 1 П-2""      , ""F""      , ""D""        , ""*""          , ""F""    , ""F""        , ""D""        , ""L""       , 0         , 1600      , 0             , 1    , 0     , 0       , ""м3/ч"",                  ,                 ,                ,              ,               , ""101_pp23"", ""AI[8].v"" , ""A""       ,                   ,                    ,            ,                   ,                      ,";
            expected = _regexSpace.Replace(expected, "");

            var tag = CsvTag.CreateAnalog(@"AI\FRCA2013_1\v", "FRCA-2013_1 Расход топливного газа поток 1 П-2", 0, 1600, "м3/ч", 0)
                .SetDataSource("101_pp23", "AI[8].v");

            var actual = tag.ToString();
            actual = _regexSpace.Replace(actual, "");

            Assert.AreEqual(expected, actual, true);
        }

        [TestMethod]
        public void TestDigitalTag()
        {
            var expected = @" ""D""      , ""AI\FRCA2013_1\fo""  , ""FRCA-2013_1 Расход топливного газа поток 1 dP=16 кПа"", ""F""      , ""D""        , ""*""          , ""F""    , ""F""        ,            ,           ,           ,           ,               ,      ,       ,         ,       , ""Off""            , ""On""            , ""Off""          ,              ,               , ""101_pp23"", ""AI[8].EN"", ""A""       ,                   ,                    ,            ,                   ,                      , ";
            expected = _regexSpace.Replace(expected, "");

            var tag = CsvTag.CreateDigit(@"AI\FRCA2013_1\fo", "FRCA-2013_1 Расход топливного газа поток 1 dP=16 кПа")
                .SetDataSource("101_pp23", "AI[8].EN");

            var actual = tag.ToString();
            actual = _regexSpace.Replace(actual, "");

            Assert.AreEqual(expected, actual, true);
        }

        [TestMethod]
        public void TestStringTag()
        {
            var expected = @"  ""S"",       ""AI\FRCSA2011_3\n"",   ""?????? ??????? ????? 3 ?-2"",                           ""F"",       ""M"",         ""*"",           ""F"",     ""F""        ,            ,           ,           ,           ,               ,      ,       ,         ,       ,                  ,                 ,                , 200          , ""FRCSA-2011_3"",           ,           ,           ,,,,,, ";
            expected = _regexSpace.Replace(expected, "");

            var tag = CsvTag.CreateString(@"AI\FRCSA2011_3\n", "?????? ??????? ????? 3 ?-2", "FRCSA-2011_3", 200);

            var actual = tag.ToString();
            actual = _regexSpace.Replace(actual, "");

            Assert.AreEqual(expected, actual, true);
        }
    }
}
