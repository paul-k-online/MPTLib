using System;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.XPath;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
                var aiPos = root.Element("ANALOG_POSITION");
                var tags = aiPos.Elements("Tag").ToList();
                var firstTag = tags.First();
                var n1 = firstTag.XPathSelectElement("name");
                var n2 = firstTag.XPathSelectElement("Name");
            }
            catch (Exception)
            {
                
                throw;
            }
        }
    }
}
