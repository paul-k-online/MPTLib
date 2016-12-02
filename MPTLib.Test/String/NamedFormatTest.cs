using System;
using System.Collections.Generic;
using System.Globalization;
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
            var dict = new Dictionary<string, object>()
                       {
                           {"Field", "break"},
                           {"number", 1},
                           {"break.sgn.Value", false},
                           {"UnitS", "Т/ч"},
                           {"BlOckinG.LoW", 1.1.ToString(CultureInfo.InvariantCulture)},
                       };

            var a = "AI[{NumBer}].{Field}.sgn = {break.sgn.Value}".FormatDict(dict);
            Assert.AreEqual(a, "AI[1].break.sgn = false", true);

            var b = "<{Blocking.LOW} {UNITs} (LL)".FormatDict(dict);
            Assert.AreEqual(b, "<1.1 Т/ч (Ll)", true);
            
        }
    }
}
