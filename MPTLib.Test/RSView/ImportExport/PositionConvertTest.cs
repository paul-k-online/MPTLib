using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            var tag = new RsViewTag("qwe", "");
            Assert.AreEqual(tag.FullName, "qwe");
            Assert.AreEqual(tag.Name, "qwe");
            Assert.AreEqual(tag.Folder, "");

            var tag2 = new RsViewTag("name", @"root\folder");
            Assert.AreEqual(tag2.FullName, @"root\folder\name");
            Assert.AreEqual(tag2.Name, "name");
            Assert.AreEqual(tag2.Folder, @"root\folder");

            var aTag = new RsViewAnalogTag(@"name1\name2", @"root\folder");
            Assert.AreEqual(aTag.FullName, @"root\folder\name1\name2");
            Assert.AreEqual(aTag.Name, @"name2");
            Assert.AreEqual(aTag.Folder, @"root\folder\name1");
        }
        
        [TestMethod]
        public void TestToAnalogTag()
        {
            var xElement = TestData.RootElement
                .GetElement("ANALOG_POSITION")
                .GetElements("TAG")
                .FirstOrDefault(x => x.GetAttributeValue("Name").Contains("Current"));

            var paramDict = TestData.AiPos.GetParamValueDictionary();

            try
            {
                var a = (RsViewAnalogTag)xElement.ToRsViewTag(paramDict, TestData.NodeName);
                
                Assert.AreEqual("AI\\F1011_3\\Current", a.FullName, true);
                Assert.AreEqual("Ток", a.Description, true);
                Assert.AreEqual("AI[2].i", a.Address, true);
                Assert.AreEqual(a.Max, 20);
            }
            catch (Exception)
            {
                throw new Exception();
            }

        }

        [TestMethod]
        public void TestAnalogTagWithAlarm()
        {
            var xElement = TestData.RootElement.GetElement("ANALOG_POSITION")
                .GetElements("TAG").FirstOrDefault(x => x.GetAttributeValue("Name").Contains("State"));

            var analogTag = xElement.ToRsViewAnalogTag(TestData.AiPos.GetParamValueDictionary(), "101_PP18");

            Assert.AreEqual("AI\\F1011_3\\State", analogTag.FullName , true);
            Assert.AreEqual(analogTag.Alarm1.Direction, RsViewTresholdDirection.D);
            Assert.AreEqual(analogTag.Alarm1.Label, "<0 т/ч (LL)", true);
            Assert.AreEqual(analogTag.Alarm6.Label, ">9 т/ч (HH)", true);
        }

        [TestMethod]
        public void TestToAnalogAlarm()
        {
            var xElement = TestData.RootElement.GetElement("ANALOG_POSITION")
                .GetElements("TAG").FirstOrDefault(x => x.GetAttributeValue("Name").Contains("State"))
                .GetElements("Alarm").FirstOrDefault(x => x.GetAttributeValue("number") == "1");

            var paramDict = TestData.AiPos.GetParamValueDictionary();

            var number = -1;
            var analogAlarm = xElement.ToAnalogAlarm(paramDict, TestData.NodeName, out number);

            Assert.AreEqual(analogAlarm.Direction, RsViewTresholdDirection.D);
            Assert.AreEqual(analogAlarm.Label, "<0 т/ч (LL)");
        }

        [TestMethod]
        public void TestToDigitalAlarm()
        {
            var xElement = TestData.RootElement.GetElement("ANALOG_POSITION")
                .GetElements("TAG").FirstOrDefault(x => x.GetAttributeValue("Name").Contains("Break"))
                .GetElements("ALARM").FirstOrDefault();

            var paramDict = TestData.AiPos.GetParamValueDictionary();

            var digitalAlarm = xElement.ToDigitalAlarm(paramDict, TestData.NodeName);

            Assert.AreEqual(digitalAlarm.Label, "Обрыв", true);
            Assert.AreEqual(digitalAlarm.Severity, 4);
            Assert.AreEqual(digitalAlarm.Type, RsViewDigitalAlarmType.ON);
        }

        [TestMethod]
        public void TestToDigitalTagWithAlarm()
        {
            var xElement = TestData.RootElement.GetElement("ANALOG_POSITION")
                .GetElements("TAG").FirstOrDefault(x => x.GetAttributeValue("Name").Contains("Break"));

            var paramDict = TestData.AiPos.GetParamValueDictionary();

            var digitalTag = xElement.ToRsViewDigitalTag(paramDict, TestData.NodeName);

            Assert.AreEqual(digitalTag.Name, "Break", true);
            Assert.AreEqual(digitalTag.InitialValue , false);
        }

        [TestMethod]
        public void TestToStringTag()
        {
            var xElement = TestData.RootElement
                .GetElement("ANALOG_POSITION")
                .GetElements("TAG")
                .FirstOrDefault(x => x.GetAttributeValue("Name").Contains("Name"));

            var paramDict = TestData.AiPos.GetParamValueDictionary();

            var stringTag = (RsViewStringTag)xElement.ToRsViewTag(paramDict, TestData.NodeName);

            Assert.AreEqual(stringTag.Name, "Name", true);
            Assert.AreEqual(stringTag.InitialValue, TestData.AiPos.Name, true);
            Assert.AreEqual(stringTag.Description, TestData.AiPos.Description, true);
        }
    }
}
