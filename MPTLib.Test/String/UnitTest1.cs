using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPT.StringWork;

namespace MPTLib.Test.String
{
    [TestClass]
    public class NamedFormatTest
    {
        [TestMethod]
        public void TestParsePattern()
        {
            var a = "AI[{NumBer}].{Field}.sgn = {break.sgn}";
            var dict = new Dictionary<string, object>()
                       {
                           {"Field", "break"},
                           {"number", 1},
                           {"break.sgn", false}
                       };

            //var a1 = "";
            //var s = NamedFormat.ParsePattern(a, out a1);

            var test = NamedFormat.FormatDict(a, dict);

            Assert.AreEqual(test, "AI[1].break.sgn = false", true);
        }
    }
}
