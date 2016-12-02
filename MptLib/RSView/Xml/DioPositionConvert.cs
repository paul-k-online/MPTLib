using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MPT.Model;

namespace MPT.RSView.Xml
{
    public static class DioPositionConvert
    {
        public static IDictionary<string, object> GetParameterValueDictionary(this DioPosition position, string nodeName)
        {
            var dict = new Dictionary<string, object>()
                        {
                            {"NodeName", nodeName},
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
