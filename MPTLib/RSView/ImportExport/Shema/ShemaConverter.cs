using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

using MPT.PrimitiveType;
using static MPT.RSView.RSViewAnalogAlarm;
using static MPT.RSView.RSViewDigitalAlarm;
using System.Text.RegularExpressions;
using MPT.Model;
using System.Globalization;

namespace MPT.RSView.ImportExport.XML
{
    public static class SchemaConverterExtension
    {
        private static readonly Regex RegexFormatArgs = new Regex(@"({(?<KEY>[a-zA-Z0-9_\-\.]+)})",
            RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase);
        public static string FormatDict(this string pattern, IDictionary<string, object> dict)
        {
            if (string.IsNullOrWhiteSpace(pattern))
                return string.Empty;

            var replacedPattern = pattern;
            var matches = RegexFormatArgs.Matches(pattern);
            foreach (Match match in matches)
            {
                var argKey = match.Groups["KEY"].Value;
                var dictKey = dict.Keys.FirstOrDefault(x => x.Equals(argKey, StringComparison.InvariantCultureIgnoreCase));
                if (dictKey == null)
                    continue;
                var value = dict[dictKey].ToString();
                replacedPattern = replacedPattern.Replace(string.Format("{{{0}}}", argKey), value);
            }
            return replacedPattern;
        }
        public static double ToRSViewDouble(this string value, double defaultValue = default(double))
        {
            if (string.IsNullOrWhiteSpace(value))
                return 0;

            double res;
            if (!Double.TryParse(value, out res))
            {
                throw new Exception("ToDouble " + value);
                //return 0;
            }
            return res;
        }
    }

    public class SchemaConverter
    {
        #region xml const
        const string AiPositionElement = "AiPosition";
        const string DioPositionElement = "DioPosition";
        const string AoPositionElement = "AoPosition";

        const string TagElement = "TAG";

        const string TYPE = "type";
        const string NAME = "name";
        const string FOLDER = "folder";
        const string DESCRIPTION = "description";
        const string UNITS = "units";
        const string MIN = "min";
        const string MAX = "max";

        const string INITIALVALUE = "initialValue";
        const string LENGTH = "length";

        const string DATASOURCE = "dataSource";
        const string NODENAME = "nodeName";
        const string ADDRESS = "address";

        const string ALARM = "ALARM";
        const string ANALOGALARM = "ANALOGALARM";
        const string ALARMTYPE = "alarmType";
        const string NUMBER = "number";
        const string THRESHOLD = "threshold";

        const string DIGITALALARM = "DIGITALALARM";
        const string LABEL = "label";
        const string DIRECTION = "direction";
        const string SEVERITY = "severity";

        const string DATALOG = "DATALOG";
        #endregion

        public XElement XElement { get; private set; }
        public SchemaTag[] AiTags { get; private set; }
        public SchemaTag[] DioTags { get; private set; }
        public SchemaTag[] AoTags { get; private set; }

        public static SchemaTag[] ConvertXmlTags(XElement xElement, string tagName)
        {
            if (xElement == null || string.IsNullOrWhiteSpace(tagName))
                return null;

            try
            {
                var positionElement = xElement.GetTag(tagName);
                if (positionElement == null)
                    return null;
                var xmlTags = positionElement.GetTags(TagElement);
                if (xmlTags == null)
                    return null;
                return xmlTags.Select(ParseXMLTag).ToArray();
            }
            catch(Exception)
            {
                return null;
            }
        }

        public SchemaConverter(XElement xElement)
        {
            XElement = xElement;
            AiTags = ConvertXmlTags(xElement, AiPositionElement);
            DioTags = ConvertXmlTags(xElement, DioPositionElement);
            AoTags = ConvertXmlTags(xElement, AoPositionElement);
        }

        #region Parce XML Tag
        public static SchemaTag ParseXMLTag(XElement xElement)
        {
            if (xElement == null || !xElement.EqualsByName(TagElement))
                return null;
            try
            {
                var tag = new SchemaTag
                {
                    Type = xElement.GetAttributeValue(TYPE),
                    Name = xElement.GetAttributeValue(NAME),
                    Folder = xElement.GetAttributeValue(FOLDER),
                    Description = xElement.GetAttributeValue(DESCRIPTION),
                    Units = xElement.GetAttributeValue(UNITS),
                    Min = xElement.GetAttributeValue(MIN),
                    Max = xElement.GetAttributeValue(MAX),
                    InitialValue = xElement.GetAttributeValue(INITIALVALUE),
                    Length = xElement.GetAttributeValue(LENGTH),
                    DataSource = xElement.GetAttributeValue(DATASOURCE),
                    NodeName = xElement.GetAttributeValue(NODENAME),
                    Address = xElement.GetAttributeValue(ADDRESS),
                };
                tag.Alarms = xElement.GetTags(ALARM).Select(ParseXMLAlarm).ToArray();
                tag.DataLogs = xElement.GetTags(DATALOG).Select(ParseXMLDataLog).ToArray();
                return tag;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static SchemaAlarm ParseXMLAlarm(XElement xElement)
        {
            if (!xElement.EqualsByName(ALARM))
                return null;

            return new SchemaAlarm()
            {
                Type = xElement.GetAttributeValue(TYPE),
                Label = xElement.GetAttributeValue(LABEL),
                Severity = xElement.GetAttributeValue(SEVERITY),

                Number = xElement.GetAttributeValue(NUMBER),
                Threshold = xElement.GetAttributeValue(THRESHOLD),
                Direction = xElement.GetAttributeValue(DIRECTION),
            };
        }
        public static string ParseXMLDataLog(XElement xElement)
        {
            if (!xElement.EqualsByName(DATALOG))
                return null;
            return xElement.GetAttributeValue(NAME);
        }
        #endregion

        #region Convert Position To RSViewTag
        public IEnumerable<RSViewTag> ConvertPositionToRSViewTags(Position position, string nodeName)
        {
            if (position == null) return null;

            IDictionary<string, object> paramDict = null;
            IEnumerable<SchemaTag> schemaTags = null;

            if (position is AiPosition)
            {
                paramDict = ((AiPosition)position).GetParamValueDictionary();
                schemaTags = AiTags.ToList();
            }
            else if (position is DioPosition)
            {
                paramDict = ((DioPosition)position).GetParamValueDictionary();
                schemaTags = DioTags.ToList();
            }
            else if (position is AoPosition)
            {
                paramDict = ((AoPosition)position).GetParamValueDictionary();
                schemaTags = AoTags.ToList();
            }

            return ToRSViewTag(schemaTags, paramDict, nodeName);
        }
        #endregion

        #region Convert Dict To RSViewTag

        public static IEnumerable<RSViewTag> ToRSViewTag(IEnumerable<SchemaTag> shemaTags, IDictionary<string, object> paramDict, string nodeName)
        {
            return shemaTags.Select(shemaTag => ToRSViewTag(shemaTag, paramDict, nodeName));
        }
        public static RSViewTag ToRSViewTag(SchemaTag shemaTag, IDictionary<string, object> paramDict, string nodeName)
        {
            RSViewTagType tagType;
            if (shemaTag.Type.TryToEnum(out tagType) == false)
                return null;

            RSViewTag tag = null;
            try
            {
                tag = new RSViewTag(shemaTag.Name.FormatDict(paramDict), shemaTag.Folder.FormatDict(paramDict))
                {
                    Description = shemaTag.Description.FormatDict(paramDict),
                    DataSourceType = shemaTag.DataSource.ToEnum<RSViewTagDataSourceType>(),
                    NodeName = nodeName,
                    Address = shemaTag.Address.FormatDict(paramDict),
                };
                tag.SetDatalogs(shemaTag.DataLogs);
            }
            catch (Exception ex)
            {
                throw new Exception("ToRSViewTag", ex);
            }


            if (tagType == RSViewTagType.A)
            {
                try
                {
                    var analogTag = new RSViewAnalogTag(tag)
                    {
                        Units = shemaTag.Units.FormatDict(paramDict),
                        Min = shemaTag.Min.FormatDict(paramDict).ToRSViewDouble(),
                        Max = shemaTag.Max.FormatDict(paramDict).ToRSViewDouble(),
                        InitialValue = shemaTag.InitialValue.FormatDict(paramDict).ToRSViewDouble(),
                    };
                    foreach (var shemaAlarm in shemaTag.Alarms)
                    {
                        short alarmNumber;
                        var alarm = ToRSViewAnalogAlarm(shemaAlarm, paramDict, out alarmNumber);
                        if (alarmNumber >= 0 && alarmNumber <= 8)
                            analogTag.Alarms[alarmNumber] = alarm;
                    }
                    return analogTag;
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("ToAnalogTag #:{0} Name:{1}", paramDict["Number"].ToString(), paramDict["Name"].ToString()), ex);
                }
            }

            if (tagType == RSViewTagType.D)
            {
                try
                {
                    var digitalTag = new RSViewDigitalTag(tag)
                    {
                        InitialValue = shemaTag.InitialValue.FormatDict(paramDict).ToRSViewBool(),
                        Alarm = ToRSViewDigitalAlarm(shemaTag.Alarms.FirstOrDefault(), paramDict),
                    };
                    return digitalTag;
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("ToRsViewDigitalTag #:{0} Name:{1}", paramDict["Number"].ToString(), paramDict["Name"].ToString()), ex);
                }
            }

            if (tagType == RSViewTagType.S)
            {
                try
                {
                    var stringTag = new RSViewStringTag(tag)
                    {
                        InitialValue = shemaTag.InitialValue.FormatDict(paramDict),
                    };
                    return stringTag;
                }
                catch (Exception)
                {

                    throw;
                }
            }

            return tag;
        }
        public static RSViewAnalogAlarm ToRSViewAnalogAlarm(SchemaAlarm shemaAlarm, IDictionary<string, object> paramDict, out short number)
        {
            if (shemaAlarm == null)
            {
                number = -1;
                return null;
            }

            try
            {
                var analogAlarm = new RSViewAnalogAlarm
                {
                    Label = shemaAlarm.Label.FormatDict(paramDict),
                    Direction = shemaAlarm.Direction.FormatDict(paramDict).ToEnum<RSViewTresholdDirection>(),
                    Threshold = shemaAlarm.Threshold.FormatDict(paramDict),
                    Severity = Convert.ToUInt16(shemaAlarm.Severity.FormatDict(paramDict)),
                };
                number = Convert.ToInt16(shemaAlarm.Number);
                return analogAlarm;
            }
            catch (Exception ex)
            {
                throw new Exception("ToAnalogAlarm", ex);
            }
        }
        public static RSViewDigitalAlarm ToRSViewDigitalAlarm(SchemaAlarm shemaAlarm, IDictionary<string, object> paramDict)
        {
            if (shemaAlarm == null)
                return null;

            try
            {
                var alarm = new RSViewDigitalAlarm
                {
                    Label = shemaAlarm.Label.FormatDict(paramDict),
                    Type = shemaAlarm.Type.FormatDict(paramDict).ToEnum<RSViewDigitalAlarmType>(),
                    Severity = Convert.ToUInt16(shemaAlarm.Severity.FormatDict(paramDict)),
                };
                return alarm;
            }
            catch (Exception ex)
            {
                throw new Exception("ToDigitalAlarm", ex);
            }
        }
        #endregion

    }

    public static class ParamValueDictionaryExtension
    {
        public static IDictionary<string, object> GetParamValueDictionary(this Position position)
        {
            return new Dictionary<string, object>
            {
                {"Number", position.Number},
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