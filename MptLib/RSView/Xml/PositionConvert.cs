using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace MPT.RSView.Xml
{
    public static class PositionConvert
    {
        public static AnalogTag GetAnalogTag(this XElement xElement, IDictionary<string, object> paramDict, string nodeName)
        {
            if (!xElement.EqualsName("TAG") 
                || xElement.AttrStr("Type").ToEnum<RSTagType>() != RSTagType.A)
                return null;

            var tag = new AnalogTag();

            tag.Folder = xElement.AttrStrParam("folder", paramDict);
            tag.Name = xElement.AttrStrParam("name", paramDict);
            tag.Desctiption = xElement.AttrStrParam("description", paramDict);
            tag.Units = xElement.AttrStrParam("max", paramDict);
            tag.Min = Convert.ToDouble(xElement.AttrStrParam("min", paramDict), CultureInfo.InvariantCulture);
            tag.Max = Convert.ToDouble(xElement.AttrStrParam("max", paramDict), CultureInfo.InvariantCulture);
            tag.InitialValue = Convert.ToDouble(xElement.AttrStrParam("initialValue", paramDict), CultureInfo.InvariantCulture);

            tag.SetDataSource(xElement, paramDict, nodeName);

            var alarms = xElement.Elems("ALARM");
            foreach (var alarmElement in alarms)
            {
                var alarmNumber = -1;
                var alarm = alarmElement.GetAnalogAlarm(paramDict, nodeName, out alarmNumber);
                if (alarmNumber >= 0 && alarmNumber <= 8)
                    tag.Alarm[alarmNumber] = alarm;
            }

            tag.SetDatalog(xElement);

            return tag;
        }

        public static AnalogTag.AnalogAlarm GetAnalogAlarm(this XElement xElement, IDictionary<string, object> paramDict, string nodeName, out int number)
        {
            if (!xElement.EqualsName("ALARM") 
                || xElement.Parent == null 
                || !xElement.Parent.EqualsName("TAG")
                || xElement.Parent.AttrStr("Type").ToEnum<RSTagType>() != RSTagType.A
                )
            {
                number = -1;
                return null;
            }

            var analogAlarm = new AnalogTag.AnalogAlarm
                                {
                                    Label = 
                                            xElement.AttrStrParam("Label", paramDict),
                                    Direction = 
                                            xElement.AttrStrParam("Direction", paramDict).ToEnum<RSTresholdDirection>(),
                                    Threshold = 
                                            Convert.ToDouble(xElement.AttrStrParam("Threshold", paramDict), CultureInfo.InvariantCulture),
                                    Severity = 
                                            Convert.ToUInt16(xElement.AttrStrParam("Severity", paramDict))
                                };

            number = Convert.ToInt32(xElement.AttrStrParam("Number", paramDict));
            return analogAlarm;
        }


        public static DigitalTag GetDigitalTag(this XElement xElement, IDictionary<string, object> paramDict, string nodeName)
        {
            if (!xElement.EqualsName("TAG")
                || xElement.AttrStr("Type").ToEnum<RSTagType>() != RSTagType.D)
                return null;

            var tag = new DigitalTag()
                             {
                                 Folder = xElement.AttrStrParam("Folder", paramDict),
                                 Name = xElement.AttrStrParam("Name", paramDict),
                                 Desctiption = xElement.AttrStrParam("description", paramDict),
                                 InitialValue = Convert.ToBoolean(Convert.ToInt32(xElement.AttrStrParam("initialValue", paramDict))),
                             };

            tag.SetDataSource(xElement, paramDict, nodeName);

            var alarmElement = xElement.Elems("ALARM").FirstOrDefault();
            if (alarmElement != null)
                tag.Alarm = alarmElement.GetDigitalAlarm(paramDict, nodeName);
            
            tag.SetDatalog(xElement);

            return tag;
        }

        public static DigitalTag.DigitalAlarm GetDigitalAlarm(this XElement xElement, IDictionary<string, object> paramDict, string nodeName)
        {
            if (!xElement.EqualsName("ALARM") || 
                xElement.Parent == null || 
                !xElement.Parent.EqualsName("TAG") ||
                xElement.Parent.AttrStr("Type").ToEnum<RSTagType>() != RSTagType.D)
                return null;
            
            var analogAlarm = new DigitalTag.DigitalAlarm
                                {
                                    Label = xElement.AttrStrParam("Label", paramDict),
                                    Type = xElement.AttrStrParam("Type", paramDict).ToEnum<RSDigitalAlarmType>(),
                                    Severity = Convert.ToUInt16(xElement.AttrStrParam("Severity", paramDict)),
                                };
            return analogAlarm;
        }


        public static StringTag GetStringTag(this XElement xElement, IDictionary<string, object> paramDict, string nodeName)
        {
            if (xElement.EqualsName("TAG") || xElement.AttrStr("Type").ToEnum<RSTagType>() != RSTagType.S)
                return null;

            var tag = new StringTag()
                        {
                            Folder = xElement.AttrStrParam("Folder", paramDict),
                            Name = xElement.AttrStrParam("Name", paramDict),
                            Desctiption = xElement.AttrStrParam("description", paramDict),
                            Length = Convert.ToUInt16(xElement.AttrStrParam("description", paramDict)),
                            InitialValue = xElement.AttrStrParam("initialValue", paramDict),
                        };

            tag.SetDataSource(xElement, paramDict, nodeName);
            tag.SetDatalog(xElement);

            return tag;
        }



        public static void SetDataSource(this Tag tag, XElement xElement,IDictionary<string, object> paramDict, string nodeName)
        {
            var dataSource = xElement.AttrStrParam("dataSource", paramDict).ToEnum<RSDataSource>();
            if (dataSource != RSDataSource.D) 
                return;
            tag.NodeName = nodeName;
            tag.Address = xElement.AttrStrParam("Address", paramDict);
        }

        public static void SetDatalog(this Tag tag, XElement xElement)
        {
            var datalogElements = xElement.Elems("DataLog");
            foreach (var datalogElement in datalogElements)
            {
                var datalogName = datalogElement.GetDatalogName();
                tag.Datalog.Add(datalogName);
            }
        }



        public static string GetDatalogName(this XElement xElement)
        {
            if (!xElement.EqualsName("DataLog") || 
                xElement.Parent == null || !xElement.Parent.EqualsName("TAG") || xElement.Parent.AttrStr("Type").ToEnum<RSTagType>() == RSTagType.F)
                return null;

            return xElement.AttrStr("Name");
        }


        public static Tag GetTag(this XElement xElement, IDictionary<string, object> paramDict, string nodeName)
        {
            var type = xElement.AttrStr("Type").ToEnum<RSTagType>();
            switch (type)
            {
                case RSTagType.A:
                    return xElement.GetAnalogTag(paramDict, nodeName);
                case RSTagType.D:
                    return xElement.GetDigitalTag(paramDict, nodeName);
                case RSTagType.S:
                    return xElement.GetStringTag(paramDict, nodeName);
            }
            return null;
        }

        public static IEnumerable<Tag> GetTags(XElement xElement, IDictionary<string, object> paramDict, string nodeName)
        {
            return xElement.Elements("TAG").Select(x => x.GetTag(paramDict,nodeName));
        }
    }
}
