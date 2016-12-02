using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPT.Model;
using MPT.RSView;

namespace MPTLib.Test.RSView
{
    [TestClass]
    public class AiPositionExtensionTest
    {
        [TestMethod]
        public void TestName1()
        {
            var aiPos = new AiPosition()
                        {
                            Name = "10%FRCSA.1100/0",
                        };
            var rsViewName = aiPos.RSViewName();

            Assert.AreEqual(rsViewName, "10-F1100_0", true);
        }
    }
}
