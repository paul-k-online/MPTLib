using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPT.PrimitiveType;
using MPT.RSView.ImportExport.XML;
using MPTLib.Test;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace MPT.RSView.ImportExport.XML.Tests
{
    [TestClass()]
    public class SchemaConverterTests
    {
        [TestMethod]
        public void ParsePatternTest()
        {
            var dict = new Dictionary<string, string>()
                       {
                           {"Field", "break"},
                           {"number", "1"},
                           {"break.sgn.Value", (true).ToEnum<RSViewDigitEnum>().ToString()},
                           {"UnitS", "Т/ч"},
                           {"BlOckinG.LoW", (1.1).ToString(CultureInfo.InvariantCulture)},
                       };
            Assert.AreEqual("AI[1].break.sgn = on", "AI[{NumBer}].{Field}.sgn = {break.sgn.Value}".FormatDict(dict), true);
            Assert.AreEqual("A123<1.1 Т/ч (LL)", "A123<{Blocking.LOW} {UNITs} (LL)".FormatDict(dict), true);
        }


        [TestMethod]
        public void TagNameFolderTest()
        {
            var tag = new RSViewTag("qwe", "");
            Assert.AreEqual(tag.TagPath, "qwe");
            Assert.AreEqual(tag.TagName, "qwe");
            Assert.AreEqual(tag.Folder, "");

            var tag2 = new RSViewTag("name", @"root\folder");
            Assert.AreEqual(tag2.TagPath, @"root\folder\name", true);
            Assert.AreEqual(tag2.TagName, "name", true);
            Assert.AreEqual(tag2.Folder, @"root\folder", true);

            var aTag = new RSViewAnalogTag(@"name1\name2", @"root\folder");
            Assert.AreEqual(aTag.TagPath, @"root\folder\name1\name2", true);
            Assert.AreEqual(aTag.TagName, @"name2", true);
            Assert.AreEqual(aTag.Folder, @"root\folder\name1", true);
        }


        [TestMethod]
        public void ToRSViewAnalogTag_V_Test()
        {
            var schemaTag = TestData.XmlSchema_TEST.AiTags.FirstOrDefault(x => x.Name.EndsWith(@"\v", StringComparison.InvariantCultureIgnoreCase));
            Assert.IsNotNull(schemaTag);

            var a = (RSViewAnalogTag)SchemaConverter.ToRSViewTag(
                                        schemaTag,
                                        SchemaConverter.GetParamValueDictionary(TestData.TestAiPos),
                                        TestData._101_PP18.NodeName);

            Assert.AreEqual(@"AI\F1011_3\v", a.TagPath, true);
            Assert.AreEqual("AI[2].v", a.Address, true);
            Assert.AreEqual(TestData.TestAiPos.Scale.High, a.Max);
        }


        [TestMethod]
        public void ToRSViewAnalogTagStateTest()
        {
            var schemaTag = TestData.XmlSchema_TEST.AiTags.FirstOrDefault(x => x.Name.EndsWith(@"\state", StringComparison.InvariantCultureIgnoreCase));
            Assert.IsNotNull(schemaTag);

            var tag = (RSViewAnalogTag)SchemaConverter.ToRSViewTag(
                                        schemaTag,
                                        SchemaConverter.GetParamValueDictionary(TestData.TestAiPos),
                                        TestData._101_PP18.NodeName);

            Assert.AreEqual(@"AI\F1011_3\State", tag.TagPath, true);
            Assert.AreEqual(@"AI[2].State", tag.Address, true);
        }


        [TestMethod]
        public void ToRSViewAnalogAlarmTest()
        {
            try
            {
                var schemaTag = TestData.XmlSchema_TEST.AiTags.FirstOrDefault(x => x.Name.EndsWith(@"\state", StringComparison.InvariantCultureIgnoreCase));
                Assert.IsNotNull(schemaTag);

                var pos = TestData.TestAiPos;
                var dict = SchemaConverter.GetParamValueDictionary(TestData.TestAiPos);
                var tag = (RSViewAnalogTag)SchemaConverter.ToRSViewTag(
                                            schemaTag,
                                            dict,
                                            TestData._101_PP18.NodeName);
                var alarm = tag.Alarm1;
                Assert.IsNotNull(alarm);

                Assert.AreEqual(alarm.Direction, RSViewAnalogAlarm.TresholdDirection.D);
                Assert.AreEqual(alarm.Label, "F1011_3 << 0 т/ч", true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [TestMethod]
        public void ToDigitalTagAlarmTest()
        {
            var schemaTag = TestData.XmlSchema_TEST.AiTags.FirstOrDefault(x => x.Name.EndsWith(@"\break", StringComparison.InvariantCultureIgnoreCase));
            Assert.IsNotNull(schemaTag);

            var dict = SchemaConverter.GetParamValueDictionary(TestData.TestAiPos);

            var tag = (RSViewDigitalTag)SchemaConverter.ToRSViewTag(
                            schemaTag,
                            dict,
                            TestData._101_PP18.NodeName);

            Assert.AreEqual(tag.TagName, "Break", true);
            Assert.AreEqual(tag.Address, "AI[2].Break.sgn", true);
            Assert.AreEqual(tag.InitialValue, false);

            var alarm = tag.Alarm;
            Assert.IsNotNull(alarm);

            Assert.AreEqual(alarm.Label, "Обрыв", true);
            Assert.AreEqual(alarm.Severity, 4);
            Assert.AreEqual(alarm.Type, RSViewDigitalAlarm.RSViewDigitalAlarmType.ON);
        }


        [TestMethod]
        public void ToStringTagTest()
        {
            var schemaTag = TestData.XmlSchema_TEST.AiTags.FirstOrDefault(x => x.Name.EndsWith(@"\name", StringComparison.InvariantCultureIgnoreCase));

            var tag = (RSViewStringTag)SchemaConverter.ToRSViewTag(
                            schemaTag,
                            SchemaConverter.GetParamValueDictionary(TestData.TestAiPos),
                            TestData._101_PP18.NodeName);

            Assert.AreEqual(tag.TagName, "Name", true);
            Assert.AreEqual(tag.InitialValue, TestData.TestAiPos.Name, true);
            Assert.AreEqual(tag.Description, TestData.TestAiPos.Description, true);
        }


        [TestMethod()]
        public void ParseXMLTagTest()
        {
            var schemaTag = TestData.XmlSchema_TEST.AiTags.FirstOrDefault(x => x.Name.EndsWith(@"\v", StringComparison.InvariantCultureIgnoreCase));
            Assert.AreEqual(schemaTag.Datalogs.Count(), 2);

            var rsviewAnalogTag = (RSViewAnalogTag)SchemaConverter.ToRSViewTag(
                            schemaTag,
                            SchemaConverter.GetParamValueDictionary(TestData.TestAiPos),
                            TestData._101_PP18.NodeName);

            Assert.AreEqual(schemaTag.Datalogs.Count(), rsviewAnalogTag.DatalogCount);

        }
    }
}