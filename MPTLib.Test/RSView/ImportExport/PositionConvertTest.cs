using System;
using System.Linq;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPT.PrimitiveType;
using MPT.RSView;
using MPT.RSView.ImportExport;


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
        public void TestToAnalogTag()
        {
            var xElement = TestData.XmlShema_TEST
                .GetElement("AiPosition")
                .GetElements("TAG")
                .FirstOrDefault(x => x.WhereAttribureContain("Name", "\\v"));

            var position = TestData.TestAiPos;
            var paramDict = position.GetParamValueDictionary();
            var a = (RSViewAnalogTag)xElement.ToRsViewTag(paramDict, TestData._101_PP18.NodeName);
                
            Assert.AreEqual(@"AI\F1011_3\v", a.Name, true);
            Assert.AreEqual("AI[2].v", a.Address, true);
            Assert.AreEqual(TestData.TestAiPos.FullName, a.Description, true);
            Assert.AreEqual(TestData.TestAiPos.Scale.High, a.Max);
        }


        [TestMethod]
        public void Test_ToRsViewAnalogTag()
        {
            var tagStateShema = TestData.XmlShema_TEST.GetElement("AiPosition")
                .GetElements("TAG").FirstOrDefault(x => x.WhereAttribureContain("Name", "state"));
            Assert.AreNotEqual(null, tagStateShema);

            var paramDict = TestData.TestAiPos.GetParamValueDictionary();
            var analogTag = tagStateShema.ToRsViewAnalogTag(paramDict, "101_PP18");

            Assert.AreEqual(@"AI\F1011_3\State", analogTag.Name , true);
            Assert.AreEqual(TestData.TestAiPos.FullName, analogTag.Description, true);
            Assert.AreEqual(@"AI[2].State", analogTag.Address, true);
        }


        [TestMethod]
        public void Test_ToRsViewAnalogAlarm()
        {
            var tagShema = TestData.XmlShema_TEST.GetElement("AiPosition")
                .GetElements("TAG").FirstOrDefault(x => x.WhereAttribureContain("Name", "state"));
            Assert.AreNotEqual(null, tagShema);

            var alarmShema = tagShema.GetElements("ALARM").FirstOrDefault();
            Assert.AreNotEqual(null, alarmShema);

            var paramDict = TestData.TestAiPos.GetParamValueDictionary();

            var number = 0;
            var analogAlarm = alarmShema.ToRsViewAnalogAlarm(paramDict, "123", out number);

            Assert.AreEqual(analogAlarm.Direction, RSViewTresholdDirection.D);
            Assert.AreEqual(analogAlarm.Label, "F1011_3 << 0 т/ч", true);
        }


        [TestMethod]
        public void Test_ToDigitalAlarm()
        {
            var tagShema = TestData.XmlShema_TEST.GetElement("AiPosition")
                .GetElements("TAG").FirstOrDefault(x => x.WhereAttribureContain("Name", "break"));
            Assert.AreNotEqual(null, tagShema);

            var alarmShema = tagShema.GetElements("ALARM").FirstOrDefault();
            Assert.AreNotEqual(null, alarmShema);

            var paramDict = TestData.TestAiPos.GetParamValueDictionary();

            var digitalAlarm = alarmShema.ToRsViewDigitalAlarm(paramDict, "NodeName");

            Assert.AreEqual(digitalAlarm.Label, "Обрыв", true);
            Assert.AreEqual(digitalAlarm.Severity, 4);
            Assert.AreEqual(digitalAlarm.Type, RSViewDigitalAlarmType.ON);
        }

        [TestMethod]
        public void TestToDigitalTagWithAlarm()
        {
            var tagShema = TestData.XmlShema_TEST.GetElement("AiPosition")
                .GetElements("TAG").FirstOrDefault(x => x.WhereAttribureContain("Name","Break"));
            Assert.AreNotEqual(null, tagShema);

            var paramDict = TestData.TestAiPos.GetParamValueDictionary();

            var digitalTag = tagShema.ToRsViewDigitalTag(paramDict, TestData._101_PP18.NodeName);

            Assert.AreEqual(digitalTag.TagName, "Break", true);
            Assert.AreEqual(digitalTag.Address, "AI[2].Break.sgn", true);

            Assert.AreEqual(digitalTag.InitialValue, false);
        }

        [TestMethod]
        public void TestToStringTag()
        {
            var tagShema = TestData.XmlShema_TEST
                .GetElement("AiPosition")
                .GetElements("TAG")
                .FirstOrDefault(x => x.WhereAttribureContain("Name","Name"));
            Assert.AreNotEqual(null, tagShema);

            var paramDict = TestData.TestAiPos.GetParamValueDictionary();

            var stringTag = (RSViewStringTag)tagShema.ToRsViewTag(paramDict, TestData._101_PP18.NodeName);

            Assert.AreEqual(stringTag.TagName, "Name", true);
            Assert.AreEqual(stringTag.InitialValue, TestData.TestAiPos.Name, true);
            Assert.AreEqual(stringTag.Description, TestData.TestAiPos.Description, true);
        }
    }
}
