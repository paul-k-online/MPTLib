using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace MPT.PrimitiveType
{
    public static class XElementExtension
    {
        public static bool EqualsByName(this XElement xElement, string name)
        {
            return string.Equals(xElement.Name.ToString(), name, StringComparison.InvariantCultureIgnoreCase);
        }

        public static IEnumerable<XAttribute> GetAttributes(this XElement xElement, string name, bool ignoreCase = true)
        {
            var stringComparison = ignoreCase ? StringComparison.InvariantCultureIgnoreCase : StringComparison.InvariantCulture;
            return xElement.Attributes().Where(x => string.Equals(x.Name.ToString(), name, stringComparison));
        }
        
        public static string GetAttributeValue(this XElement xElement, string name, bool ignoreCase = true)
        {
            var attribute = xElement.GetAttributes(name, ignoreCase).FirstOrDefault();
            return (attribute == null || string.IsNullOrWhiteSpace(attribute.Value)) ? "" : attribute.Value;
        }

        public static bool WhereAttribureEquals(this XElement xElement, string name, string value, bool ignoreCase = true)
        {
            var v = xElement.GetAttributeValue(name, ignoreCase);
            return v.Equals(value, ignoreCase ? StringComparison.InvariantCultureIgnoreCase : StringComparison.InvariantCulture);
        }

        public static bool WhereAttribureContain(this XElement xElement, string name, string value, bool ignoreCase = true)
        {
            var v = xElement.GetAttributeValue(name, ignoreCase);
            return v.ToUpper().Contains(value.ToUpper());
        }

        public static IEnumerable<XElement> GetElements(this XElement xElement, string name, bool ignoreCase = true)
        {
            if (xElement == null)
                return null;
            var stringComparison = ignoreCase ? StringComparison.InvariantCultureIgnoreCase : StringComparison.InvariantCulture;
            return xElement.Elements().Where(x => string.Equals(x.Name.ToString(), name, stringComparison));
        }

        public static XElement GetElement(this XElement xElement, string name, bool ignoreCase = true)
        {
            return xElement.GetElements(name, ignoreCase).FirstOrDefault();
        }
    }
}