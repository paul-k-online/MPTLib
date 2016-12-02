using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using MPT.StringWork;

namespace MPT.RSView.Xml
{
    public static class XmlExtension
    {
        public static bool EqualsName(this XElement xElement, string name)
        {
            return string.Equals(xElement.Name.ToString(), name, StringComparison.InvariantCultureIgnoreCase);
        }

        public static string AttrStr(this XElement xElement, string name)
        {
            var attr = xElement.Attributes().FirstOrDefault(x => string.Equals(x.Name.ToString(), name, StringComparison.InvariantCultureIgnoreCase));
            return attr == null ? null : attr.Value;
        }

        public static string AttrStrParam(this XElement xElement, string name, IDictionary<string, object> paramDict)
        {
            var attrValue = xElement.AttrStr(name);
            if (string.IsNullOrWhiteSpace(attrValue))
                return null;
            return attrValue.FormatDict(paramDict);
        }

        public static IEnumerable<XElement> Elems(this XElement xElement, string name)
        {
            return xElement.Elements().Where(x => string.Equals(x.Name.ToString(), name, StringComparison.InvariantCultureIgnoreCase));
        }

        public static XElement Elem(this XElement xElement, string name)
        {
            return xElement.Elems(name).FirstOrDefault();
        }
    }
}