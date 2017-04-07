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
        
        public static string GetAttributeValue(this XElement xElement, string attribyteName, bool ignoreCase = true)
        {
            var attribute = xElement.GetAttributes(attribyteName, ignoreCase).FirstOrDefault();
            return (attribute == null || string.IsNullOrWhiteSpace(attribute.Value)) ? "" : attribute.Value;
        }

        public static IEnumerable<XElement> GetTags(this XElement xElement, string name, bool ignoreCase = true)
        {
            if (xElement == null)
                return null;
            var stringComparison = ignoreCase ? StringComparison.InvariantCultureIgnoreCase : StringComparison.InvariantCulture;
            return xElement.Elements().Where(x => string.Equals(x.Name.ToString(), name, stringComparison));
        }
    }
}