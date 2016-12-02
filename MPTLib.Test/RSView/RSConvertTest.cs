using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPT.Model;

using MPT.RSView;

namespace MPTLib.Test.RSView
{
    [TestClass]
    public class RSConvertTest
    {
        [TestMethod]
        public void Test1()
        {
            var ai = new AiPosition()
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

            var tags = ai.ToRSviewTags(false, "101_PP18");


        }
    }
}
