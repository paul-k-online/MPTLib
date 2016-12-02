using System;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPT.Model;
using MPT.RSView;
using MPT.RSView.Xml;

namespace MPTLib.Test.RSView
{
    [TestClass]
    public class PositionExtensionTest
    {

        readonly AiPosition aiPosition = new AiPosition()
        {
            Name = "FRCSA1011_3",
            Description = "Расход бензина поток 3 dP=10 кПа",
            Units = "т/ч",
            Number = 2,
            Scale = new AiPosition.AlarmPair(0, 2.5),
            Reglament = new AiPosition.AlarmPair(1.1, 2.1),
            Alarming = new AiPosition.AlarmPair(1.0, null),
            Blocking = new AiPosition.AlarmPair(null, null),
        };


        [TestMethod]
        public void TestAiToXmlAnalog()
        {
            var xml = @"
<TAG type=""A"" folder=""AI\{SHORTNAME}"" name=""Current"" description=""Ток"" min=""0"" max=""20"" units=""мА"" dataSource=""D"" address=""AI[{NUMBER}].i""/> 
";
            var xElement = XElement.Parse(xml);


            var a = aiPosition.GetAnalogTag(xElement, "101_PP18");

            Assert.AreEqual("AI\\F1011_3\\Current", a.FullName, true);
            Assert.AreEqual("Ток", a.Desctiption, true);
            Assert.AreEqual("AI[2].i", a.Address, true);
            Assert.AreEqual("20", a.Max.ToString(), true);
        }
    }
}
