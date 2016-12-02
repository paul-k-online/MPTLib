using System.Diagnostics.Contracts;
using System.Linq;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPT.Model;
using MPT.RSView;
using MPT.RSView.Xml;

namespace MPTLib.Test.RSView.Xml
{
    [TestClass]
    public class PositionConvertTest
    {
        public readonly AiPosition AiPos = new AiPosition()
                                                {
                                                    Name = "FRCSA1011_3",
                                                    Description = "Расход бензина поток 3 dP=10 кПа",
                                                    Units = "т/ч",
                                                    Number = 2,
                                                    Scale = new AiPosition.AlarmPair(0, 10),
                                                    Reglament = new AiPosition.AlarmPair(3, 6),
                                                    Alarming = new AiPosition.AlarmPair(2, 8),
                                                    Blocking = new AiPosition.AlarmPair(null, 9),            
                                                };

        public const string NodeName = "101_PP18";

        public XElement RootElement = XElement.Load("RSView\\Xml\\POSITIONLIST.xml");


        [TestMethod]
        public void TestXmlToAnalogTagCurrent()
        {
            var xElement = RootElement.Elem("ANALOG_POSITION")
                .Elems("TAG").FirstOrDefault(x => x.AttrStr("Name")=="Current");

            var paramDict = AiPos.GetParameterValueDictionary();

            var a = xElement.GetAnalogTag(paramDict, NodeName);

            Assert.AreEqual("AI\\F1011_3\\Current", a.FullName, true);
            Assert.AreEqual("Ток", a.Desctiption, true);
            Assert.AreEqual("AI[2].i", a.Address, true);
            Assert.AreEqual(20, a.Max);
        }

        [TestMethod]
        public void TestXmlToAnalogTagState()
        {
            var xElement = RootElement.Elem("ANALOG_POSITION")
                .Elems("TAG").FirstOrDefault(x => x.AttrStr("Name") == "State");

            var analogTag = xElement.GetAnalogTag(AiPos.GetParameterValueDictionary(), "101_PP18");

            Assert.AreEqual("AI\\F1011_3\\State", analogTag.FullName , true);

            var alarm1 = analogTag.Alarm1;

            Assert.AreEqual(alarm1.Direction, RSTresholdDirection.D);
            Assert.AreEqual(alarm1.Label, "<1 т/ч (LL)");
        }

        [TestMethod]
        public void TestXmlToAnalogAlarmStateAlarm1()
        {
            var xElement = RootElement.Elem("ANALOG_POSITION")
                .Elems("TAG").FirstOrDefault(x => x.AttrStr("Name") == "State")
                .Elems("Alarm").FirstOrDefault(x => x.AttrStr("number") == "1");

            var paramDict = AiPos.GetParameterValueDictionary();

            var number = -1;
            var analogAlarm = xElement.GetAnalogAlarm(paramDict, NodeName, out number);

            Assert.AreEqual(analogAlarm.Direction, RSTresholdDirection.D);
            Assert.AreEqual(analogAlarm.Label, "<0 т/ч (LL)");
        }

        [TestMethod]
        public void TestXmlToDigitalAlarmBreak()
        {
            var xElement = RootElement.Elem("ANALOG_POSITION")
                .Elems("TAG").FirstOrDefault(x => x.AttrStr("Name") == "Break")
                .Elems("ALARM").FirstOrDefault();

            var paramDict = AiPos.GetParameterValueDictionary();
            
            var digitalAlarm = xElement.GetDigitalAlarm(paramDict, NodeName);

            Assert.AreEqual(digitalAlarm.Label, "Обрыв", true);
            Assert.AreEqual(digitalAlarm.Severity, 4);
            Assert.AreEqual(digitalAlarm.Type, RSDigitalAlarmType.ON);
        }

        [TestMethod]
        public void TestXmlToDigitalTagBreak()
        {
            var xElement = RootElement.Elem("ANALOG_POSITION")
                .Elems("TAG").FirstOrDefault(x => x.AttrStr("Name") == "Break");

            var paramDict = AiPos.GetParameterValueDictionary();

            var digitalTag = xElement.GetDigitalTag(paramDict, NodeName);

            Assert.AreEqual(digitalTag.Name, "Break", true);
            Assert.AreEqual(digitalTag.InitialValue , false);

        }


    }
}
