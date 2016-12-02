using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using MPT.Model;

namespace MPT.RSView.Xml
{
    public static class AiPositionExtension
    {
        public static IDictionary<string, object> GetParameterValueDictionary(this AiPosition position)
        {
            var dict = new Dictionary<string, object>()
                       {
                           {"NUMBER", position.Number.ToString()},
                           {"NAME", position.Name},
                           {"SHORTNAME", position.RSViewName()},
                           {"FULLNAME", position.FullName},
                           {"Description", position.Description},
                           {"Units", position.Units},
                           {"Scale.Low", position.Scale.Low},
                           {"Scale.High", position.Scale.High},
                           {"Reglament.Low", position.Reglament.Low == null ? "0" : position.Reglament.Low.Value.ToString(CultureInfo.InvariantCulture)},
                           {"Reglament.High", position.Reglament.High == null ? "0" : position.Reglament.High.Value.ToString(CultureInfo.InvariantCulture)},
                           {"Alarming.Low", position.Alarming.Low == null ? "0" : position.Alarming.Low.Value.ToString(CultureInfo.InvariantCulture)},
                           {"Alarming.High", position.Alarming.High == null ? "0" : position.Alarming.High.Value.ToString(CultureInfo.InvariantCulture)},
                           {"Blocking.Low", position.Blocking.Low == null ? "0" : position.Blocking.Low.Value.ToString(CultureInfo.InvariantCulture)},
                           {"Blocking.High", position.Blocking.High == null ? "0" : position.Blocking.High.Value.ToString(CultureInfo.InvariantCulture)},
                       };
            return dict;
        }


        public static IDictionary<string, object> GetParameterValueDictionary(this DioPosition position)
        {
            var dict = new Dictionary<string, object>()
                        {
                            {"NUMBER", position.Number},
                            {"NAME", position.Name},
                            {"FULLNAME", position.FullName},
                            {"Description", position.Description},
                            {"NormValue", position.NormValue},
                        };
            return dict;
        }
    }
}
