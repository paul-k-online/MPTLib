using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using MPT.PrimitiveType;
using MPT.Model;

namespace MPT.RSView.ImportExport
{
    public static class PositionConvertXmlExtension
    {
        public static double ToDouble(this string value, double defaultValue = default(double))
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


        #region ToRsView
        public static IEnumerable<RSViewTag> ToRsViewTags(this XElement positionShema, IDictionary<string, object> paramDict, string nodeName)
        {
            var tagElements = positionShema.Elements("TAG");
            return tagElements.Select(x => ToRsViewTag(x, paramDict, nodeName));
        }

        public static RSViewTag ToRsViewTag(this XElement xElement, IDictionary<string, object> paramDict, string nodeName)
        {
            var type = xElement.GetAttributeValue("Type").ToEnum<RSViewTagType>();
            switch (type)
            {
                case RSViewTagType.A:
                    return xElement.ToRsViewAnalogTag(paramDict, nodeName);
                case RSViewTagType.D:
                    return xElement.ToRsViewDigitalTag(paramDict, nodeName);
                case RSViewTagType.S:
                    return xElement.ToRsViewStringTag(paramDict, nodeName);
            }
            return null;
        }

        public static RsViewAnalogTag ToRsViewAnalogTag(this XElement xElement, IDictionary<string, object> paramDict, string nodeName)
        {
            try
            {
                if (!xElement.EqualsByName("TAG") || 
                    xElement.GetAttributeValue("Type").ToEnum<RSViewTagType>() != RSViewTagType.A)
                    return null;

                var name = xElement.GetAttributeValue("Name").FormatDict(paramDict);
                var folder = xElement.GetAttributeValue("Folder").FormatDict(paramDict);

                // ReSharper disable once UseObjectOrCollectionInitializer
                var tag = new RsViewAnalogTag(name, folder);

                tag.Description = xElement.GetAttributeValue("Description").FormatDict(paramDict);
                tag.Units = xElement.GetAttributeValue("Units").FormatDict(paramDict);
                tag.Min = xElement.GetAttributeValue("Min").FormatDict(paramDict).ToDouble();
                tag.Max = xElement.GetAttributeValue("Max").FormatDict(paramDict).ToDouble();
                tag.InitialValue = xElement.GetAttributeValue("InitialValue").FormatDict(paramDict).ToDouble();

                tag.SetDataSource(xElement, paramDict, nodeName);

                var alarms = xElement.GetElements("ALARM");
                foreach (var alarmElement in alarms)
                {
                    int alarmNumber;
                    var alarm = alarmElement.ToRsViewAnalogAlarm(paramDict, nodeName, out alarmNumber);
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

        public static RsViewAnalogTag.RsViewAnalogAlarm ToRsViewAnalogAlarm(this XElement xElement, IDictionary<string, object> paramDict, string nodeName, out int number)
        {
            try
            {
                if (!xElement.EqualsByName("ALARM") || 
                    xElement.Parent == null || 
                    !xElement.Parent.EqualsByName("TAG") ||
                    xElement.Parent.GetAttributeValue("Type").ToEnum<RSViewTagType>() != RSViewTagType.A)
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

        public static RSViewDigitalTag ToRsViewDigitalTag(this XElement xElement, IDictionary<string, object> paramDict, string nodeName)
        {
            if (xElement == null)
                return null;

            try
            {
                if (!xElement.EqualsByName("TAG") || xElement.GetAttributeValue("Type").ToEnum<RSViewTagType>() != RSViewTagType.D)
                    return null;
                
                // ReSharper disable once UseObjectOrCollectionInitializer
                var tag = new RSViewDigitalTag(
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
                    tag.Alarm = alarmElement.ToRsViewDigitalAlarm(paramDict, nodeName);

                tag.SetDatalog(xElement);
                return tag;
            }
            catch (Exception e)
            {
                throw new Exception("ToDigitalTag", e);
            }
        }

        public static RSViewDigitalTag.DigitalAlarm ToRsViewDigitalAlarm(this XElement xElement, IDictionary<string, object> paramDict, string nodeName)
        {
            if (xElement == null)
                return null;

            if (!xElement.EqualsByName("ALARM") || 
                xElement.Parent == null || 
                !xElement.Parent.EqualsByName("TAG") ||
                xElement.Parent.GetAttributeValue("Type").ToEnum<RSViewTagType>() != RSViewTagType.D)
                return null;
            
            var analogAlarm = new RSViewDigitalTag.DigitalAlarm
                                {
                                    Label = xElement.GetAttributeValue("Label").FormatDict(paramDict),
                                    Type = xElement.GetAttributeValue("Type").FormatDict(paramDict).ToEnum<RSViewDigitalAlarmType>(),
                                    Severity = Convert.ToUInt16(xElement.GetAttributeValue("Severity").FormatDict(paramDict)),
                                };
            return analogAlarm;
        }

        public static RsViewStringTag ToRsViewStringTag(this XElement xElement, IDictionary<string, object> paramDict, string nodeName)
        {
            try
            {
                if (!xElement.EqualsByName("TAG") || xElement.GetAttributeValue("Type").ToEnum<RSViewTagType>() != RSViewTagType.S)
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

        public static void SetDataSource(this RSViewTag tag, XElement xElement,IDictionary<string, object> paramDict, string nodeName)
        {
            var dataSource = xElement.GetAttributeValue("dataSource").FormatDict(paramDict).ToEnum<RSViewTagDataSourceType>();
            if (dataSource != RSViewTagDataSourceType.D) 
                return;
            tag.NodeName = nodeName;
            tag.Address = xElement.GetAttributeValue("Address").FormatDict(paramDict);
        }

        public static void SetDatalog(this RSViewTag tag, XElement xElement)
        {
            var dlgs = xElement.GetElements("DataLog")
                .Select(x => x.GetDatalogName())
                .Where(x => !string.IsNullOrEmpty(x));
            
            dlgs.ToList().ForEach(dlg => tag.Datalogs.Add(dlg));
        }

        private static string GetDatalogName(this XElement xElement)
        {
            if (!xElement.EqualsByName("DataLog") 
                || xElement.Parent == null || !xElement.Parent.EqualsByName("TAG") 
                || xElement.Parent.GetAttributeValue("Type").ToEnum<RSViewTagType>() == RSViewTagType.F)
                return null;

            return xElement.GetAttributeValue("Name");
        }

        #endregion

        #region PositionConvert

        public static IEnumerable<RSViewTag> ConvertPositionToRsviewTags(Position position, XElement positionListShema, string nodeName)
        {
            if (position == null)
                return null;

            IDictionary<string, object> paramDict = null;
            XElement positionShema = null;

            if (position is AiPosition)
            {
                paramDict = ((AiPosition)position).GetParamValueDictionary();
                positionShema = positionListShema.GetElement("AiPosition");
            }
            else if (position is DioPosition)
            {
                paramDict = ((DioPosition)position).GetParamValueDictionary();
                positionShema = positionListShema.GetElement("DioPosition");
            }
            else if (position is AoPosition)
            {
                paramDict = ((AoPosition)position).GetParamValueDictionary();
                positionShema = positionListShema.GetElement("AoPosition");
            }
            else
            {
                return null;
            }

            return ToRsViewTags(positionShema, paramDict, nodeName);
        }

        public static IEnumerable<RSViewTag> ConvertPositionsToRsviewTags(IEnumerable<Position> positions, XElement positionShema, string nodeName)
        {
            if (positions == null) return null;
            if (positionShema == null) return null;
            if (string.IsNullOrWhiteSpace(nodeName)) return null;

            return positions
#if !DEBUG
                .AsParallel()
#endif
                .SelectMany(position => ConvertPositionToRsviewTags(position, positionShema, nodeName));
        }

#endregion
    }
}
