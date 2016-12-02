using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using MPT.Model;
using MPT.Strings;

namespace MPT.RSView.ImportExport
{
    public static class PositionConvertXmlExtension
    {
        public static double ToDouble(this string value, double defaultValue=default(double))
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return 0;
            }

            double res;
            if (!Double.TryParse(value, out res))
            {
                throw new Exception("Error ToDouble " + value);
                //return 0;
            }

            return res;
        }
        

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
            var dict = ((Position) position).GetParamValueDictionary();

            dict.Add("SHORTNAME", position.RsViewShortName());
            dict.Add("UNITS", position.Units);
            dict.Add("Scale.Low", position.Scale.Low);
            dict.Add("Scale.High", position.Scale.High);
            dict.Add("Reglament.Low", position.Reglament.Low == null ? "0" : position.Reglament.Low.Value.ToString(CultureInfo.InvariantCulture));
            dict.Add("Reglament.High", position.Reglament.High == null ? "0" : position.Reglament.High.Value.ToString(CultureInfo.InvariantCulture));
            dict.Add("Alarming.Low", position.Alarming.Low == null ? "0" : position.Alarming.Low.Value.ToString(CultureInfo.InvariantCulture));
            dict.Add("Alarming.High", position.Alarming.High == null ? "0" : position.Alarming.High.Value.ToString(CultureInfo.InvariantCulture));
            dict.Add("Blocking.Low", position.Blocking.Low == null ? "0" : position.Blocking.Low.Value.ToString(CultureInfo.InvariantCulture));
            dict.Add("Blocking.High", position.Blocking.High == null ? "0" : position.Blocking.High.Value.ToString(CultureInfo.InvariantCulture));
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

        
        public static RsViewTag ToRsViewTag(this XElement xElement, IDictionary<string, object> paramDict, string nodeName)
        {
            var type = xElement.GetAttributeValue("Type").ToEnum<RsViewTagType>();
            switch (type)
            {
                case RsViewTagType.A:
                    return xElement.ToRsViewAnalogTag(paramDict, nodeName);
                case RsViewTagType.D:
                    return xElement.ToRsViewDigitalTag(paramDict, nodeName);
                case RsViewTagType.S:
                    return xElement.ToRsViewStringTag(paramDict, nodeName);
            }
            return null;
        }
        
        public static RsViewAnalogTag ToRsViewAnalogTag(this XElement xElement, IDictionary<string, object> paramDict, string nodeName)
        {
            try
            {
                if (!xElement.EqualsByName("TAG") || xElement.GetAttributeValue("Type").ToEnum<RsViewTagType>() != RsViewTagType.A)
                    return null;

                var name = xElement.GetAttributeValue("name").FormatDict(paramDict);
                var folder = xElement.GetAttributeValue("folder").FormatDict(paramDict);

                // ReSharper disable once UseObjectOrCollectionInitializer
                var tag = new RsViewAnalogTag(name, folder);

                tag.Description = xElement.GetAttributeValue("description").FormatDict(paramDict);
                tag.Units = xElement.GetAttributeValue("Units").FormatDict(paramDict);
                tag.Min = xElement.GetAttributeValue("Min").FormatDict(paramDict).ToDouble();
                tag.Max = xElement.GetAttributeValue("Max").FormatDict(paramDict).ToDouble();
                tag.InitialValue = xElement.GetAttributeValue("InitialValue").FormatDict(paramDict).ToDouble();

                tag.SetDataSource(xElement, paramDict, nodeName);
                var alarms = xElement.GetElements("ALARM");
                foreach (var alarmElement in alarms)
                {
                    int alarmNumber;
                    var alarm = alarmElement.ToAnalogAlarm(paramDict, nodeName, out alarmNumber);
                    if (alarmNumber >= 0 && alarmNumber <= 8)
                        tag.Alarm[alarmNumber] = alarm;
                }

                tag.SetDatalog(xElement);
                return tag;
            }
            catch (Exception e)
            {
                throw new Exception("ToAnalogTag", e);
            }
        }
        
        public static RsViewAnalogTag.RsViewAnalogAlarm ToAnalogAlarm(this XElement xElement, IDictionary<string, object> paramDict, string nodeName, out int number)
        {
            try
            {
                if (!xElement.EqualsByName("ALARM") || xElement.Parent == null || !xElement.Parent.EqualsByName("TAG") ||
                    xElement.Parent.GetAttributeValue("Type").ToEnum<RsViewTagType>() != RsViewTagType.A)
                {
                    number = -1;
                    return null;
                }

                var analogAlarm = new RsViewAnalogTag.RsViewAnalogAlarm
                {
                    Label = xElement.GetAttributeValue("Label").FormatDict(paramDict),
                    Direction = xElement.GetAttributeValue("Direction").FormatDict(paramDict).ToEnum<RsViewTresholdDirection>(),
                    Threshold = xElement.GetAttributeValue("Threshold").FormatDict(paramDict).ToDouble(),
                    Severity = Convert.ToUInt16(xElement.GetAttributeValue("Severity").FormatDict(paramDict)),
                };

                number = Convert.ToInt32(xElement.GetAttributeValue("Number").FormatDict(paramDict));
                return analogAlarm;
            }
            catch (Exception e)
            {
                throw new Exception("ToAnalogAlarm", e);
            }
        }


        public static RsViewDigitalTag ToRsViewDigitalTag(this XElement xElement, IDictionary<string, object> paramDict, string nodeName)
        {
            if (xElement == null)
                return null;

            try
            {
                if (!xElement.EqualsByName("TAG") || xElement.GetAttributeValue("Type").ToEnum<RsViewTagType>() != RsViewTagType.D)
                    return null;
                
                // ReSharper disable once UseObjectOrCollectionInitializer
                var tag = new RsViewDigitalTag(
                                xElement.GetAttributeValue("Name").FormatDict(paramDict), 
                                xElement.GetAttributeValue("Folder").FormatDict(paramDict));
                tag.Description = xElement.GetAttributeValue("description").FormatDict(paramDict);

                try
                {
                    var val = xElement.GetAttributeValue("initialValue").FormatDict(paramDict);
                    tag.InitialValue = Convert.ToBoolean(Convert.ToInt32(val));
                }
                catch
                {
                    tag.InitialValue = false;
                }

                tag.SetDataSource(xElement, paramDict, nodeName);

                var alarmElement = xElement.GetElements("ALARM").FirstOrDefault();
                if (alarmElement != null)
                    tag.Alarm = alarmElement.ToDigitalAlarm(paramDict, nodeName);

                tag.SetDatalog(xElement);
                return tag;
            }
            catch (Exception e)
            {
                throw new Exception("ToDigitalTag", e);
            }
        }
        
        public static RsViewDigitalTag.DigitalAlarm ToDigitalAlarm(this XElement xElement, IDictionary<string, object> paramDict, string nodeName)
        {
            if (xElement == null)
                return null;

            if (!xElement.EqualsByName("ALARM") || 
                xElement.Parent == null || 
                !xElement.Parent.EqualsByName("TAG") ||
                xElement.Parent.GetAttributeValue("Type").ToEnum<RsViewTagType>() != RsViewTagType.D)
                return null;
            
            var analogAlarm = new RsViewDigitalTag.DigitalAlarm
                                {
                                    Label = xElement.GetAttributeValue("Label").FormatDict(paramDict),
                                    Type = xElement.GetAttributeValue("Type").FormatDict(paramDict).ToEnum<RsViewDigitalAlarmType>(),
                                    Severity = Convert.ToUInt16(xElement.GetAttributeValue("Severity").FormatDict(paramDict)),
                                };
            return analogAlarm;
        }


        public static RsViewStringTag ToRsViewStringTag(this XElement xElement, IDictionary<string, object> paramDict, string nodeName)
        {
            try
            {
                if (!xElement.EqualsByName("TAG") || xElement.GetAttributeValue("Type").ToEnum<RsViewTagType>() != RsViewTagType.S)
                    return null;

                var tag = new RsViewStringTag(
                    xElement.GetAttributeValue("Name").FormatDict(paramDict),
                    xElement.GetAttributeValue("Folder").FormatDict(paramDict))
                {
                    Description = xElement.GetAttributeValue("description").FormatDict(paramDict),
                    Length = Convert.ToUInt16(xElement.GetAttributeValue("length").FormatDict(paramDict)),
                    InitialValue = xElement.GetAttributeValue("initialValue").FormatDict(paramDict),
                };
                tag.SetDataSource(xElement, paramDict, nodeName);
                tag.SetDatalog(xElement);
                return tag;
            }
            catch (Exception e)
            {
                throw new Exception("ToStringTag", e);
            }
        }


        public static void SetDataSource(this RsViewTag tag, XElement xElement,IDictionary<string, object> paramDict, string nodeName)
        {
            var dataSource = xElement.GetAttributeValue("dataSource").FormatDict(paramDict).ToEnum<RsViewDataSource>();
            if (dataSource != RsViewDataSource.D) 
                return;
            tag.NodeName = nodeName;
            tag.Address = xElement.GetAttributeValue("Address").FormatDict(paramDict);
        }

        public static void SetDatalog(this RsViewTag tag, XElement xElement)
        {
            var dlgs = xElement.GetElements("DataLog")
                .Select(x => x.GetDatalogName())
                .Where(x => !string.IsNullOrEmpty(x));
            
            dlgs.ToList().ForEach(dlg => tag.Datalogs.Add(dlg));
        }

        public static string GetDatalogName(this XElement xElement)
        {
            if (!xElement.EqualsByName("DataLog") 
                || xElement.Parent == null || !xElement.Parent.EqualsByName("TAG") 
                || xElement.Parent.GetAttributeValue("Type").ToEnum<RsViewTagType>() == RsViewTagType.F)
                return null;

            return xElement.GetAttributeValue("Name");
        }



        public static IEnumerable<RsViewTag> GetTags(XElement xElement, IDictionary<string, object> paramDict, string nodeName)
        {
            var tagElements = xElement.Elements("TAG");
            return tagElements.Select(element => element.ToRsViewTag(paramDict, nodeName));
        }

        public static IEnumerable<RsViewTag> GetTags(this Position position, XElement shema, string nodeName)
        {
            var paramDict = position.GetParamValueDictionary();

            var aiPosition = position as AiPosition;
            if (aiPosition != null)
                paramDict = aiPosition.GetParamValueDictionary();

            var aoPosition = position as AoPosition;
            if (aoPosition != null)
                paramDict = aoPosition.GetParamValueDictionary();

            var dioPosition = position as DioPosition;
            if (dioPosition != null)
                paramDict = dioPosition.GetParamValueDictionary();

            return GetTags(shema, paramDict, nodeName);
        }
    }
}
