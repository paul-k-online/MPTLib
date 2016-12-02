using System.Collections.Generic;
using MPT.Model;
using MPT.RSLogix;
using MPT.RSView;

namespace MPT.RSView.ImportExport
{
    public static class PositionConvertDefaultExtension
    {
        public static string ToRsViewAlarmLabel(this double? value, string template="", string defaultValue = "")
        {
            return value == null ? defaultValue : string.Format(template, value, defaultValue);
        }
        
        public static RsViewAnalogTag CreateAnalogTagDefault(this AiPosition position, 
            string name, string folder ="", 
            string descriprion ="", 
            double initialValue = 0, 
            string nodeName = null, string address = null)
        {
            var tag = new RsViewAnalogTag(name, folder)
                      {
                          Description = descriprion,
                          InitialValue = initialValue,
                          NodeName = nodeName,
                          Address = address,
                          Min = position.Scale.Low ?? 0,
                          Max = position.Scale.High ?? 0,
                          Units = position.Units,
                      };
            return tag;
        }

        public static IEnumerable<RsViewTag> GetTagsByDefault(this AiPosition position, string nodeName)
        {
            var folder = string.Format(@"AI\{0}", position.RsViewShortName());
            
            var tags = new List<RsViewTag>();
            var enTag = new RsViewDigitalTag("en", folder)
                        {
                            Description = "Позиция в обработке",
                            NodeName = nodeName,
                            Address = position.GetAiItemAddress("EN"),
                        };
            tags.Add(enTag);


            var currentTag = new RsViewAnalogTag("i", folder)
                                {
                                    Description = "Ток",
                                    Min = -1,
                                    Max = 21,
                                    Units = "мА",
                                    NodeName = nodeName,
                                    Address = position.GetAiItemAddress("i"),
                                };
            tags.Add(currentTag);


            var vTag = new RsViewAnalogTag("v", folder)
                        {
                            Description = position.FullName,
                            Min = position.Scale.Low.Value,
                            Max = position.Scale.High.Value,
                            Units = position.Units,
                            NodeName = nodeName,
                            Address = position.GetAiItemAddress("v"),
                        };
            vTag.Datalogs.Add("dlg_sec");
            tags.Add(vTag);


            var vAvrTag = new RsViewAnalogTag("vAvr", folder)
            {
                Description = position.FullName,
                Min = position.Scale.Low.Value,
                Max = position.Scale.High.Value,
                Units = position.Units,
                NodeName = nodeName,
                Address = position.GetAiItemAddress("vMin"),
            };
            vAvrTag.Datalogs.Add("dlg_min");
            tags.Add(vAvrTag);


            var nameTag = new RsViewStringTag("Name", folder)
                            {
                                Description = position.Description,
                                InitialValue = position.Name,
                            };
            tags.Add(nameTag);


            tags.Add(position.CreateAnalogTagDefault("rMin", folder, "Регламент Min", position.Reglament.Low ?? 0));
            tags.Add(position.CreateAnalogTagDefault("rMax", folder, "Регламент Max", position.Reglament.High ?? 0));
            tags.Add(position.CreateAnalogTagDefault("sMin", folder, "Сигнализация Min", position.Alarming.Low ?? 0));
            tags.Add(position.CreateAnalogTagDefault("sMax", folder, "Сигнализация Max", position.Alarming.High ?? 0));
            tags.Add(position.CreateAnalogTagDefault("bMin", folder, "Блокировка Min", position.Blocking.Low ?? 0));
            tags.Add(position.CreateAnalogTagDefault("bMax", folder, "Блокировка Max", position.Blocking.High ?? 0));


            var breakTag = new RsViewDigitalTag("break", folder)
                            {
                                Description = position.FullName,
                                NodeName = nodeName,
                                Address = position.GetAiItemAddress("break.sgn"),
                                Alarm = new RsViewDigitalTag.DigitalAlarm()
                                {
                                    Label = "Обрыв",
                                    Severity = 4,
                                    Type = RsViewDigitalAlarmType.ON,
                                },
                            };
            tags.Add(breakTag);

            var stateTag = position.CreateAnalogTagDefault("State", folder, position.Description, 0, nodeName, position.GetAiItemAddress("state"));

            const string lowTemplate = "< {0} ({1})";
            const string highTemplate = "> {0} ({1})";

            stateTag.Alarm1 = new RsViewAnalogTag.RsViewAnalogAlarm()
            {
                Threshold = 0.9,
                Label = position.Blocking.Low.ToRsViewAlarmLabel(lowTemplate, "LL"),
                Severity = 1,
                Direction = RsViewTresholdDirection.D
            };

            stateTag.Alarm2 = new RsViewAnalogTag.RsViewAnalogAlarm()
            {
                Threshold = 1.9,
                Label = position.Alarming.Low.ToRsViewAlarmLabel(lowTemplate, "L"),
                Severity = 2,
                Direction = RsViewTresholdDirection.D
            };

            stateTag.Alarm3 = new RsViewAnalogTag.RsViewAnalogAlarm()
            {
                Threshold = 2.9,
                Label = position.Reglament.Low.ToRsViewAlarmLabel(lowTemplate, "RL"),
                Severity = 3,
                Direction = RsViewTresholdDirection.D
            };

            stateTag.Alarm4 = new RsViewAnalogTag.RsViewAnalogAlarm()
            {
                Threshold = 4.9,
                Label = position.Reglament.High.ToRsViewAlarmLabel(highTemplate, "RH"),
                Severity = 3,
                Direction = RsViewTresholdDirection.I
            };

            stateTag.Alarm5 = new RsViewAnalogTag.RsViewAnalogAlarm()
            {
                Threshold = 5.9,
                Label = position.Alarming.High.ToRsViewAlarmLabel(highTemplate, "H"),
                Severity = 2,
                Direction = RsViewTresholdDirection.I
            };

            stateTag.Alarm6 = new RsViewAnalogTag.RsViewAnalogAlarm()
            {
                Threshold = 6.9,
                Label = position.Blocking.High.ToRsViewAlarmLabel(highTemplate, "HH"),
                Severity = 1,
                Direction = RsViewTresholdDirection.I
            };

            tags.Add(stateTag);

            return tags;
        }
    }
}
