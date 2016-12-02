using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPT.RSView;
using MPT.RSView.Csv;
using Tag = MPT.RSView.Csv.Tag;

namespace MPTLib.Test.RSView.Csv
{
    /// <summary>
    /// Summary description for RsViewTagTEst1
    /// </summary>
    [TestClass]
    public class TagTest
    {
        readonly Regex regexSpace = new Regex("[\x20]*");


        [TestMethod]
        public void TestEnumToString()
        {
            Assert.AreEqual(RSTagType.A.ToRS(), "\"A\"");
        }

        
        [TestMethod]
        public void TestFolderTag()
        {
            var expected = @"""F"", ""AI\FRCA2013_1"", """", ""F""";
            expected = regexSpace.Replace(expected, "");
            
            var tag = Tag.CreateFolder(@"AI\FRCA2013_1");
            var actual = regexSpace.Replace(tag.ToString(), "");

            Assert.AreEqual(expected, actual, true);            
        }
        
        
        [TestMethod]
        public void TestAnalogTagMemory()
        {
            var expected = @"""A""      , ""AI\FRCA2013_2\bmax"", ""Блокировка по MAX""                                   , ""F""      , ""M""        , ""*""          , ""F""    , ""F""        , ""D""        , ""L""       , 0         , 1600      , 0             , 1    , 0     , 0       , ""м3/ч"",                  ,                 ,                ,              ,               ,           ,           ,           ,                   ,                    ,            ,                   ,                      ,";
            expected = regexSpace.Replace(expected, "");
            var tag = Tag.CreateAnalog(@"AI\FRCA2013_2\bmax", "Блокировка по MAX", 0, 1600, "м3/ч", 0);
            

            var actual = tag.ToString();
            actual = regexSpace.Replace(actual, "");

            Assert.AreEqual(expected, actual, true);
        }

        [TestMethod]
        public void TestAnalogTagData()
        {
            var expected = @" ""A""      , ""AI\FRCA2013_1\v""   , ""FRCA-2013_1 Расход топливного газа поток 1 П-2""      , ""F""      , ""D""        , ""*""          , ""F""    , ""F""        , ""D""        , ""L""       , 0         , 1600      , 0             , 1    , 0     , 0       , ""м3/ч"",                  ,                 ,                ,              ,               , ""101_pp23"", ""AI[8].v"" , ""A""       ,                   ,                    ,            ,                   ,                      ,";
            expected = regexSpace.Replace(expected, "");

            var tag = Tag.CreateAnalog(@"AI\FRCA2013_1\v", "FRCA-2013_1 Расход топливного газа поток 1 П-2", 0, 1600, "м3/ч", 0)
                .SetDataSource("101_pp23", "AI[8].v");

            var actual = tag.ToString();
            actual = regexSpace.Replace(actual, "");

            Assert.AreEqual(expected, actual, true);
        }

        [TestMethod]
        public void TestDigitalTag()
        {
            var expected = @" ""D""      , ""AI\FRCA2013_1\fo""  , ""FRCA-2013_1 Расход топливного газа поток 1 dP=16 кПа"", ""F""      , ""D""        , ""*""          , ""F""    , ""F""        ,            ,           ,           ,           ,               ,      ,       ,         ,       , ""Off""            , ""On""            , ""Off""          ,              ,               , ""101_pp23"", ""AI[8].EN"", ""A""       ,                   ,                    ,            ,                   ,                      , ";
            expected = regexSpace.Replace(expected, "");

            var tag = Tag.CreateDigit(@"AI\FRCA2013_1\fo", "FRCA-2013_1 Расход топливного газа поток 1 dP=16 кПа", false)
                .SetDataSource("101_pp23", "AI[8].EN");

            var actual = tag.ToString();
            actual = regexSpace.Replace(actual, "");

            Assert.AreEqual(expected, actual, true);
        }



        [TestMethod]
        public void TestStringTag()
        {
            var expected = @"  ""S"",       ""AI\FRCSA2011_3\n"",   ""?????? ??????? ????? 3 ?-2"",                           ""F"",       ""M"",         ""*"",           ""F"",     ""F""        ,            ,           ,           ,           ,               ,      ,       ,         ,       ,                  ,                 ,                , 200          , ""FRCSA-2011_3"",           ,           ,           ,,,,,, ";
            expected = regexSpace.Replace(expected, "");

            var tag = Tag.CreateString(@"AI\FRCSA2011_3\n", "?????? ??????? ????? 3 ?-2", "FRCSA-2011_3", 200);

            var actual = tag.ToString();
            actual = regexSpace.Replace(actual, "");

            Assert.AreEqual(expected, actual, true);
        }
    }
}
