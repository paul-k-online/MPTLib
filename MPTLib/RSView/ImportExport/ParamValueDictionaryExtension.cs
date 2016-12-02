using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using MPT.PrimitiveType;
using MPT.Model;


namespace MPT.RSView.ImportExport
{
    public static class ParamValueDictionaryExtension
    {
        public static IDictionary<string, object> GetParamValueDictionary(this Position position)
        {
            return new Dictionary<string, object>
            {
                {"Number", position.Number},
                {"NodeName", position.NodeName},
                {"Name", position.Name},
                {"FullName", position.FullName},
                {"Description", position.Description},
            };
        }

        public static IDictionary<string, object> GetParamValueDictionary(this AiPosition position)
        {
            var dict = ((Position)position).GetParamValueDictionary();

            dict.Add("FirstLetter", position.RsViewFirstLetter());

            dict.Add("ShortName", position.RsViewShortName());
            dict.Add("Units", position.Units);

            dict.Add("Scale.Low", position.Scale.Low);
            dict.Add("Scale.High", position.Scale.High);

            dict.Add("Reglament.Low", position.Reglament.Low?.ToString(CultureInfo.InvariantCulture) ?? "0");
            dict.Add("Reglament.Low.En", position.Reglament.Low != null);

            dict.Add("Reglament.High", position.Reglament.High?.ToString(CultureInfo.InvariantCulture) ?? "0");
            dict.Add("Reglament.High.En", position.Reglament.High != null);

            dict.Add("Alarming.Low", position.Alarming.Low?.ToString(CultureInfo.InvariantCulture) ?? "0");
            dict.Add("Alarming.Low.En", position.Alarming.Low != null);

            dict.Add("Alarming.High", position.Alarming.High?.ToString(CultureInfo.InvariantCulture) ?? "0");
            dict.Add("Alarming.High.En", position.Alarming.High != null);

            dict.Add("Blocking.Low", position.Blocking.Low?.ToString(CultureInfo.InvariantCulture) ?? "0");
            dict.Add("Blocking.Low.En", position.Blocking.Low != null);

            dict.Add("Blocking.High", position.Blocking.High?.ToString(CultureInfo.InvariantCulture) ?? "0");
            dict.Add("Blocking.High.En", position.Blocking.High != null);

            return dict;
        }

        public static IDictionary<string, object> GetParamValueDictionary(this DioPosition position)
        {
            var dict = ((Position)position).GetParamValueDictionary();

            dict.Add("NormValue", position.NormValue);
            return dict;
        }

        public static IDictionary<string, object> GetParamValueDictionary(this AoPosition position)
        {
            var dict = ((Position)position).GetParamValueDictionary();

            dict.Add("AiNUM", position.AiNum);
            dict.Add("AoTYPE", position.AoType);
            dict.Add("Scale.Low", position.Scale.Low);
            dict.Add("Scale.High", position.Scale.High);

            return dict;
        }
    }
}
