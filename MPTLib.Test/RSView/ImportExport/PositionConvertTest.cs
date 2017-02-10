using System;
using System.Linq;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPT.PrimitiveType;
using MPT.RSView;
using MPT.RSView.ImportExport;
using MPT.RSView.ImportExport.XML;

namespace MPTLib.Test.RSView.ImportExport
{
    [TestClass]
    public class PositionConvertTest
    {
        [TestMethod]
        public void TestTagNameFolder()
        {
            var tag = new RSViewTag("qwe", "");
            Assert.AreEqual(tag.Name, "qwe");
            Assert.AreEqual(tag.TagName, "qwe");
            Assert.AreEqual(tag.Folder, "");

            var tag2 = new RSViewTag("name", @"root\folder");
            Assert.AreEqual(tag2.Name, @"root\folder\name", true);
            Assert.AreEqual(tag2.TagName, "name", true);
            Assert.AreEqual(tag2.Folder, @"root\folder", true);

            var aTag = new RSViewAnalogTag(@"name1\name2", @"root\folder");
            Assert.AreEqual(aTag.Name, @"root\folder\name1\name2", true);
            Assert.AreEqual(aTag.TagName, @"name2", true);
            Assert.AreEqual(aTag.Folder, @"root\folder\name1", true);
        }
        
        [TestMethod]
        public void Test_ToRSViewAnalogTag_V()
        {
            var schemaTag = TestData.XmlSchema_TEST.AiTags.FirstOrDefault(x => x.Name.EndsWith(@"\v", StringComparison.InvariantCultureIgnoreCase));
            Assert.IsNotNull(schemaTag);

            var a = (RSViewAnalogTag) SchemaConverter.ToRSViewTag(
                                        schemaTag, 
                                        TestData.TestAiPos.GetParamValueDictionary(), 
                                        TestData._101_PP18.NodeName);

            Assert.AreEqual(@"AI\F1011_3\v", a.Name, true);
            Assert.AreEqual("AI[2].v", a.Address, true);
            Assert.AreEqual(TestData.TestAiPos.FullName, a.Description, true);
            Assert.AreEqual(TestData.TestAiPos.Scale.High, a.Max);
        }


        [TestMethod]
        public void Test_ToRSViewAnalogTag_State()
        {
            var schemaTag = TestData.XmlSchema_TEST.AiTags.FirstOrDefault(x => x.Name.EndsWith(@"\state", StringComparison.InvariantCultureIgnoreCase));
            Assert.IsNotNull(schemaTag);

            var tag = (RSViewAnalogTag)SchemaConverter.ToRSViewTag(
                                        schemaTag,
                                        TestData.TestAiPos.GetParamValueDictionary(),
                                        TestData._101_PP18.NodeName);

            Assert.AreEqual(@"AI\F1011_3\State", tag.Name , true);
            Assert.AreEqual(TestData.TestAiPos.FullName, tag.Description, true);
            Assert.AreEqual(@"AI[2].State", tag.Address, true);
        }


        [TestMethod]
        public void Test_ToRSViewAnalogAlarm()
        {
            try
            {
                var xmlTEst = TestData.XmlSchema_TEST;

                var schemaTag = xmlTEst.AiTags.FirstOrDefault(x => x.Name.EndsWith(@"\state", StringComparison.InvariantCultureIgnoreCase));
                Assert.IsNotNull(schemaTag);

                var pos = TestData.TestAiPos;
                var tag = (RSViewAnalogTag)SchemaConverter.ToRSViewTag(
                                            schemaTag,
                                            TestData.TestAiPos.GetParamValueDictionary(),
                                            TestData._101_PP18.NodeName);
                var alarm = tag.Alarm1;
                Assert.IsNotNull(alarm);

                Assert.AreEqual(alarm.Direction, RSViewAnalogAlarm.RSViewTresholdDirection.D);
                Assert.AreEqual(alarm.Label, "F1011_3 << 0 т/ч", true);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [TestMethod]
        public void TestToDigitalTagWithAlarm()
        {
            var schemaTag = TestData.XmlSchema_TEST.AiTags.FirstOrDefault(x => x.Name.EndsWith(@"\break", StringComparison.InvariantCultureIgnoreCase));
            Assert.IsNotNull(schemaTag);

            var tag = (RSViewDigitalTag)SchemaConverter.ToRSViewTag(
                            schemaTag,
                            TestData.TestAiPos.GetParamValueDictionary(),
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
        public void TestToStringTag()
        {
            var xmlSchema = TestData.XmlSchema_TEST;

            var schemaTag = xmlSchema.AiTags.FirstOrDefault(x => x.Name.EndsWith(@"\name", StringComparison.InvariantCultureIgnoreCase));
            Assert.IsNotNull(schemaTag);

            var tag = (RSViewStringTag)SchemaConverter.ToRSViewTag(
                            schemaTag,
                            TestData.TestAiPos.GetParamValueDictionary(),
                            TestData._101_PP18.NodeName);

            Assert.AreEqual(tag.TagName, "Name", true);
            Assert.AreEqual(tag.InitialValue, TestData.TestAiPos.Name, true);
            Assert.AreEqual(tag.Description, TestData.TestAiPos.Description, true);
        }
    }
}
