using System;
using System.Linq;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using MPT.PrimitiveType;
using MPTLib.Test.RSView;

namespace MPTLib.Test.Xml.Xpath
{
    [TestClass]
    public class XPathTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            try
            {
                var root = XElement.Load(TestData.PositionListXmlFile);
                var aiPos = root.GetElement("AiPosition");

                var valueTag = aiPos
                                    .GetElements("tAg")
                                    .FirstOrDefault(t => t.GetAttributeValue("NamE").Contains("\\v"));
                Assert.AreEqual(valueTag.GetAttributeValue("Description"), "{FULLNAME}", true);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
