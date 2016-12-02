using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using MPT.Model;

namespace MPT.RSView.Xml
{
    public static class PositionXmlConvert
    {
        public static IDictionary<string, object> GetParameterValueDictionary(this AiPosition position, string nodeName)
        {
            /*
            var a = position.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .ToDictionary(prop => prop.Name.ToUpper(), prop => prop.GetValue(position, null));
            */
            var dict = new Dictionary<string, object>()
                        {
                            {"NodeName", nodeName},
                            {"NUMBER", position.Number},
                            {"NAME", position.Name},
                            {"SHORTNAME", position.ShortName},
                            {"FULLNAME", position.FullName},
                            {"Description", position.Description},
                            {"Units", position.Units},
                            {"Scale.Low", position.Scale.Low},
                            {"Scale.High", position.Scale.High},
                            {"Reglament.Low", position.Reglament.Low},
                            {"Reglament.High", position.Reglament.High},
                            {"Alarming.Low", position.Alarming.Low},
                            {"Alarming.High", position.Alarming.High},
                            {"Blocking.Low", position.Blocking.Low},
                            {"Blocking.High", position.Blocking.High},
                        };
            return dict;
        }
        
        public static IEnumerable<Tag> CreateTagsByXml(this AiPosition position, XElement analogPositionElement,
            string nodeName)
        {
            throw new NotImplementedException();
        }

        
        public static AnalogTag GetAnalogTag(this AiPosition position, XElement xElement, string nodeName)
        {
            if (!string.Equals(xElement.Name.ToString(), "TAG", StringComparison.InvariantCultureIgnoreCase) ||
                xElement.AttrStr("Type").ToEnum<RSTagType>() != RSTagType.A)
                return null;

            var paramDict = position.GetParameterValueDictionary(nodeName);

            var tag = new AnalogTag
                      {
                          Folder = xElement.AttrStrParam("folder", paramDict),
                          Name = xElement.AttrStrParam("name", paramDict),
                          Desctiption = xElement.AttrStrParam("description", paramDict),
                          Units = xElement.AttrStrParam("max", paramDict),
                          Min = Convert.ToDouble(xElement.AttrStrParam("min", paramDict)),
                          Max = Convert.ToDouble(xElement.AttrStrParam("max", paramDict))
                      };

            var dataSource = xElement.AttrStrParam("dataSource", paramDict).ToEnum<RSDataSource>();

            switch (dataSource)
            {
                case RSDataSource.D:
                    tag.NodeName = nodeName;
                    tag.Address = xElement.AttrStrParam("Address", paramDict); 
                    break;
                case RSDataSource.M:
                    tag.InitialValue = Convert.ToDouble(xElement.AttrStrParam("initialValue", paramDict));
                    break;
            }

            var alarms = xElement.Elements("ALARM");
            foreach (var alarmElement in alarms)
            {
                var alarmNumber = -1;
                var alarm = position.GetAnalogAlarm(alarmElement, nodeName, out alarmNumber);
                if (alarmNumber >= 0 && alarmNumber <= 8)
                    tag.Alarm[alarmNumber] = alarm;
            }
            return tag;
        }


        public static AnalogTag.AnalogAlarm GetAnalogAlarm(this AiPosition position,  XElement xElement, string nodeName, out int number)
        {
            var paramDict = position.GetParameterValueDictionary(nodeName);
            var analogAlarm = new AnalogTag.AnalogAlarm
                                {
                                    Label = xElement.AttrStrParam("Label", paramDict),
                                    Direction = xElement.AttrStrParam("Direction", paramDict).ToEnum<RSTresholdDirection>(),
                                    Threshold = Convert.ToDouble(xElement.AttrStrParam("Threshold", paramDict)),
                                };
            number = Convert.ToInt32(xElement.AttrStrParam("Number", paramDict));
            return analogAlarm;
        }

        public static DigitalTag GetDigitalTag(this AiPosition position, XElement xElement, string nodeName)
        {
            if (!string.Equals(xElement.Name.ToString(), "TAG", StringComparison.InvariantCultureIgnoreCase) ||
                xElement.AttrStr("Type").ToEnum<RSTagType>() != RSTagType.D)
                return null;

            var paramDict = position.GetParameterValueDictionary(nodeName);
            var digitalTag = new DigitalTag()
                             {
                                 Folder = xElement.AttrStrParam("Folder", paramDict),
                                 Name = xElement.AttrStrParam("Name", paramDict),
                             };
            return digitalTag;
        }





        public static Tag GetTag(this AiPosition position, XElement xElement,  string nodeName)
        {
            switch (xElement.AttrStr("Type").ToEnum<RSTagType>())
            {
                case RSTagType.A:
                    return position.GetAnalogTag(xElement, nodeName);
                case RSTagType.D:
                    return position.GetDigitalTag(xElement, nodeName);
            }
            return null;
        }

        public static IEnumerable<Tag> GetTags(this AiPosition position, XElement xElement, string nodeName)
        {
            if (!string.Equals(xElement.Name.ToString(), "ANALOG_POSITION", StringComparison.InvariantCultureIgnoreCase))
                return null;
            
            return xElement.Elements("TAG").Select(x=>position.GetTag(x,nodeName));
        }




    }
}
