using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using MPT.StringWork;

namespace MPT.RSView.Xml
{
    public static class XmlExtension
    {
        public static string AttrStr(this XElement xElement, string name)
        {
            var attr = xElement.Attributes().FirstOrDefault(x => string.Equals(x.Name.ToString(), name, StringComparison.InvariantCultureIgnoreCase));
            return attr == null ? "" : attr.Value;
        }

        public static string AttrStrParam(this XElement xElement, string name, IDictionary<string, object> paramDict)
        {
            var attrValue = xElement.AttrStr(name);
            return attrValue.FormatDict(paramDict);
        }
    }
}