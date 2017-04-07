using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

using MPT.PrimitiveType;
using MPT.Model;

namespace MPT.RSView.ImportExport.XML
{
    public static class SchemaConverterExtension
    {
        public static double? ToRSViewDouble(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            double res;
            if (!Double.TryParse(value, out res))
            {
                return null;
                //throw new Exception("ToRSViewDouble " + value);
            }
            return res;
        }

        private static readonly Regex RegexFormatArgs = new Regex(@"{ (?<KEY> [_a-z\d\-\.]+) }",
            RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase);

        public static string FormatDict(this string pattern, IDictionary<string, string> dict)
        {
            if (string.IsNullOrWhiteSpace(pattern))
                return "";

            return RegexFormatArgs.Replace(pattern, (Match match) => {
                var argKey = match.Groups["KEY"].Value;
                var dictKey = dict.Keys.FirstOrDefault(x => x.Equals(argKey, StringComparison.InvariantCultureIgnoreCase));
                if (dictKey == null)
                    return "";
                var value = dict[dictKey];
                if (value == null)
                    value = "";
                return value.ToString();
            });
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
        const string NUMBER = "number";
        const string THRESHOLD = "threshold";

        const string LABEL = "label";
        const string DIRECTION = "direction";
        const string SEVERITY = "severity";

        const string DATALOG = "DATALOG";
        #endregion

        public XElement XElement { get; private set; }
        public SchemaTag[] AiTags { get; private set; }
        public SchemaTag[] DioTags { get; private set; }
        public SchemaTag[] AoTags { get; private set; }


        public SchemaConverter(XElement xElement)
        {
            XElement = xElement;
            AiTags = ParseXMLTags(XElement, AiPositionElement);
            DioTags = ParseXMLTags(XElement, DioPositionElement);
            AoTags = ParseXMLTags(XElement, AoPositionElement);
        }

        #region Parce XML to Shema
        public static SchemaTag[] ParseXMLTags(XElement xElement, string tagName)
        {
            if (xElement == null || string.IsNullOrWhiteSpace(tagName))
                return null;
            try
            {
                var positionElement = xElement.GetTags(tagName).FirstOrDefault();
                if (positionElement == null)
                    return null;
                var xmlTags = positionElement.GetTags(TagElement);
                if (xmlTags == null)
                    return null;
                return xmlTags.Select(ParseXMLTag).ToArray();
            }
            catch (Exception)
            {
                return null;
            }
        }

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
                    Address = xElement.GetAttributeValue(ADDRESS),
                };
                tag.Alarms = xElement.GetTags(ALARM).Select(ParseXMLAlarm).ToList();
                tag.Datalogs = xElement.GetTags(DATALOG).Select(ParseXMLDataLog).ToList();
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

        public static SchemaDatalog ParseXMLDataLog(XElement xElement)
        {
            if (!xElement.EqualsByName(DATALOG))
                return null;
            return new SchemaDatalog() { Name = xElement.GetAttributeValue(NAME) };
        }
        #endregion


        #region Convert Position To RSViewTag
        public IEnumerable<RSViewTag> ConvertPositionToRSViewTags(Position position, string nodeName)
        {
            if (position == null)
                return null;

            IDictionary<string, string> paramDict = null;
            IEnumerable<SchemaTag> schemaTags = null;

            if (position is AiPosition)
            {
                paramDict = GetParamValueDictionary(((AiPosition)position));
                schemaTags = AiTags.ToList();
            }
            else if (position is DioPosition)
            {
                paramDict = GetParamValueDictionary(((DioPosition)position));
                schemaTags = DioTags.ToList();
            }
            else if (position is AoPosition)
            {
                paramDict = GetParamValueDictionary(((AoPosition)position));
                schemaTags = AoTags.ToList();
            }

            return schemaTags.Select(shemaTag => ToRSViewTag(shemaTag, paramDict, nodeName));
        }

        public static RSViewTag ToRSViewTag(SchemaTag shemaTag, IDictionary<string, string> paramDict, string nodeName)
        {
            RSViewTag.TypeEnum tagType;
            if (shemaTag.Type.TryToEnum(out tagType) == false)
                return null;

            RSViewTag tag = null;

            try
            {
                tag = new RSViewTag(shemaTag.Name.FormatDict(paramDict), shemaTag.Folder.FormatDict(paramDict))
                {
                    Description = shemaTag.Description.FormatDict(paramDict),
                    DataSourceType = shemaTag.DataSource.ToEnum<RSViewTag.DataSourceTypeEnum>(),
                    NodeName = nodeName,
                    Address = shemaTag.Address.FormatDict(paramDict),
                };
                tag.SetDatalogs(shemaTag.Datalogs.Select(x => x.Name).ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("ToRSViewTag #:{0} Name:{1}", paramDict["Number"].ToString(), paramDict["Name"].ToString()), ex);
            }


            if (tagType == RSViewTag.TypeEnum.A)
            {
                try
                {
                    var analogTag = new RSViewAnalogTag(tag)
                    {
                        Units = shemaTag.Units.FormatDict(paramDict),
                    };

                    var min = shemaTag.Min.FormatDict(paramDict).ToRSViewDouble();
                    if (min != null)
                        analogTag.Min = min.Value;

                    var max = shemaTag.Max.FormatDict(paramDict).ToRSViewDouble();
                    if (max != null)
                        analogTag.Max = max.Value;

                    var initialValue = shemaTag.InitialValue.FormatDict(paramDict).ToRSViewDouble();
                    if (initialValue != null)
                        analogTag.InitialValue = initialValue.Value;

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
                    throw new Exception(string.Format("ToRSViewTag Analog #:{0} Name:{1}", paramDict["Number"].ToString(), paramDict["Name"].ToString()), ex);
                }
            }

            if (tagType == RSViewTag.TypeEnum.D)
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
                    throw new Exception(string.Format("ToRSViewTag Digital #:{0} Name:{1}", paramDict["Number"].ToString(), paramDict["Name"].ToString()), ex);
                }
            }

            if (tagType == RSViewTag.TypeEnum.S)
            {
                try
                {
                    var stringTag = new RSViewStringTag(tag)
                    {
                        InitialValue = shemaTag.InitialValue.FormatDict(paramDict),
                    };
                    return stringTag;
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("ToRSViewTag String #:{0} Name:{1}", paramDict["Number"].ToString(), paramDict["Name"].ToString()), ex);
                }
            }
            return tag;
        }

        public static RSViewAnalogAlarm ToRSViewAnalogAlarm(SchemaAlarm shemaAlarm, IDictionary<string, string> paramDict, out short number)
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
                    Direction = shemaAlarm.Direction.FormatDict(paramDict).ToEnum<RSViewAnalogAlarm.TresholdDirection>(),
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

        public static RSViewDigitalAlarm ToRSViewDigitalAlarm(SchemaAlarm shemaAlarm, IDictionary<string, string> paramDict)
        {
            if (shemaAlarm == null)
                return null;

            try
            {
                var alarm = new RSViewDigitalAlarm
                {
                    Label = shemaAlarm.Label.FormatDict(paramDict),
                    Type = shemaAlarm.Type.FormatDict(paramDict).ToEnum<RSViewDigitalAlarm.RSViewDigitalAlarmType>(),
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


        #region GetParamValueDictionary
        public static IDictionary<string, string> GetParamValueDictionary(Position position)
        {
            var dict = new Dictionary<string, string>();
            dict["Number"] = position.Number.ToString();
            dict["Name"] = position.Name;
            dict["Description"] = position.Description;
            dict["RSViewName"] = RSViewTag.GetValidTagName(position.Name);
            return dict;
        }

        public static IDictionary<string, string> GetParamValueDictionary(AiPosition position)
        {
            var dict = GetParamValueDictionary(((Position)position));

            dict["FirstLetter"] = RSViewTag.GetValidTagName(position.Name);
            dict["ShortName"] = RSViewTag.GetValidTagName(position.Name);

            AiPosition.AiPositionName aiPosName = position.GetAiPositionName();
            if (aiPosName != null)
            {
                dict["FirstLetter"] = aiPosName.AiTypeLetter;
                dict["ShortName"] = RSViewTag.GetValidTagName(aiPosName.GetShortName());
            }

            dict["Units"] = position.Units;

            dict["Scale.Low"] = position.Scale.Low?.ToString(CultureInfo.InvariantCulture) ?? "0";
            dict["Scale.High"] = position.Scale.High?.ToString(CultureInfo.InvariantCulture) ?? "0";

            dict["Reglament.Low"] = position.Reglament.Low?.ToString(CultureInfo.InvariantCulture) ?? "0";
            dict["Reglament.Low.En"] = (position.Reglament.Low != null).ToEnum<RSViewDigitEnum>().ToString();

            dict["Reglament.High"] = position.Reglament.High?.ToString(CultureInfo.InvariantCulture) ?? "0";
            dict["Reglament.High.En"] = (position.Reglament.High != null).ToEnum<RSViewDigitEnum>().ToString();

            dict["Alarming.Low"] = position.Alarming.Low?.ToString(CultureInfo.InvariantCulture) ?? "0";
            dict["Alarming.Low.En"] = (position.Alarming.Low != null).ToEnum<RSViewDigitEnum>().ToString();

            dict["Alarming.High"] = position.Alarming.High?.ToString(CultureInfo.InvariantCulture) ?? "0";
            dict["Alarming.High.En"] = (position.Alarming.High != null).ToEnum<RSViewDigitEnum>().ToString();

            dict["Blocking.Low"] = position.Blocking.Low?.ToString(CultureInfo.InvariantCulture) ?? "0";
            dict["Blocking.Low.En"] = (position.Blocking.Low != null).ToEnum<RSViewDigitEnum>().ToString();

            dict["Blocking.High"] = position.Blocking.High?.ToString(CultureInfo.InvariantCulture) ?? "0";
            dict["Blocking.High.En"] = (position.Blocking.High != null).ToEnum<RSViewDigitEnum>().ToString();

            return dict;
        }

        public static IDictionary<string, string> GetParamValueDictionary(DioPosition position)
        {
            var dict = GetParamValueDictionary(((Position)position));
            dict["NormValue"] = position.NormValue.ToEnum<RSViewDigitEnum>().ToString();
            return dict;
        }

        public static IDictionary<string, string> GetParamValueDictionary(AoPosition position)
        {
            var dict = GetParamValueDictionary(((Position)position));
            dict["AiNUM"] = position.AiNum?.ToString(CultureInfo.InvariantCulture) ?? "0";
            dict["Units"] = position.Units;

            dict["AoTYPE"] = position.AoType.ToString();
            dict["Scale.Low"] = position.Scale.Low?.ToString(CultureInfo.InvariantCulture) ?? "0";
            dict["Scale.High"] = position.Scale.High?.ToString(CultureInfo.InvariantCulture) ?? "0";
            return dict;
        }
        #endregion
    }
}