using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Text;
using MPT.Model;
using MPT.RSLogix;
using NLog;

namespace MPT.RSView
{
    public static class RSConvert
    {

        public const string RSAiFolderTemplate = "AI/{0}";


        public static Dictionary<string, Tuple<string, string>> AiPositionMap = new Dictionary<string, Tuple<string, string>>()
                                                                 {
                                                                     {"HH", new Tuple<string, string>(".HH.V","bmax")}
                                                                 };


        public static IEnumerable<Tag> ToRSviewTags(this AiPosition position, bool logged, string nodeName)
        {
            var tags = new List<Tag>();
            
            var folderTag = new Tag()
                            {
                                Address = string.Format(RSAiFolderTemplate, position.ShortName),
                            };  
            tags.Add(folderTag);
            

            var enTag = new DigitalTag()
                        {
                            Folder = folderTag.FullName,
                            Name = "EN",
                            Desctiption = "Позиция в обработке",
                            NodeName = nodeName,
                            Address = position.GetItemAddress("EN"),
                        };
            tags.Add(enTag);


            var currentTag = new AnalogTag()
                             {
                                 Folder = folderTag.FullName,
                                 Name = "i",
                                 Desctiption = "Ток",
                                 Min = -1,
                                 Max = 21,
                                 Units = "мА",
                                 NodeName = nodeName,
                                 Address = position.GetItemAddress("i"),
                             };
            tags.Add(currentTag);


            var vTag = new AnalogTag()
                       {
                           Folder = folderTag.FullName,
                           Name = "v",
                           Desctiption = position.FullName,
                           Min = position.RSMin(),
                           Max = position.RSMax(),
                           Units = position.Units,
                           NodeName = nodeName,
                           Address = position.GetItemAddress("v"),
                       };
            tags.Add(vTag);


            var vMinTag = new AnalogTag()
                          {
                              Folder = folderTag.FullName,
                              Name = "vMin",
                              Desctiption = position.FullName,
                              Min = position.RSMin(),
                              Max = position.RSMax(),
                              Units = position.Units,
                              NodeName = nodeName,
                              Address = position.GetItemAddress("vMin"),
                          };
            tags.Add(vMinTag);


            var nameTag = new StringTag()
                          {
                              Folder = folderTag.FullName,
                              Name = "Name",
                              Desctiption = position.Description,
                              InitialValue = position.Name,
                          };
            tags.Add(nameTag);


            tags.Add( position.CreateAnalogTag(folderTag.FullName, "rMin", "Регламент Min", position.Reglament.Low ?? 0));
            tags.Add( position.CreateAnalogTag(folderTag.FullName, "rMax", "Регламент Max", position.Reglament.High ?? 0));
            tags.Add( position.CreateAnalogTag(folderTag.FullName, "sMin", "Сигнализация Min", position.Alarming.Low ?? 0));
            tags.Add( position.CreateAnalogTag(folderTag.FullName, "sMax", "Сигнализация Max", position.Alarming.High ?? 0));
            tags.Add( position.CreateAnalogTag(folderTag.FullName, "bMin", "Блокировка Min", position.Blocking.Low ?? 0));
            tags.Add( position.CreateAnalogTag(folderTag.FullName, "bMax", "Блокировка Max", position.Blocking.High ?? 0));
            

            var breakTag = new DigitalTag()
                       {
                           Folder = folderTag.FullName,
                           Name = "break",
                           Desctiption = position.FullName,
                           NodeName = nodeName,
                           Address = position.GetItemAddress("break", "sgn"),
                           Alarm = new DigitalTag.DigitalAlarm()
                                   {
                                       Label = "Обрыв",
                                       Severity = 4,
                                   },
                       };
            tags.Add(breakTag);

            var stateTag = position.CreateAnalogTag(folderTag.FullName, "State", position.Description, 
                0, nodeName, position.GetItemAddress("state"));

            const string lowTemplate = "< {0} ({1})";
            const string highTemplate = "> {0} ({1})";


            stateTag.Alarm = AnalogTag.CreateDefaultAlarms();
            stateTag.Alarm1.Label = position.Blocking.Low.ToRSAlarmLabel(lowTemplate, "LL");
            stateTag.Alarm2.Label = position.Alarming.Low.ToRSAlarmLabel(lowTemplate, "L");
            stateTag.Alarm3.Label = position.Reglament.Low.ToRSAlarmLabel(lowTemplate, "RL");
            stateTag.Alarm4.Label = position.Reglament.High.ToRSAlarmLabel(highTemplate, "RH");
            stateTag.Alarm5.Label = position.Alarming.High.ToRSAlarmLabel(highTemplate, "H");
            stateTag.Alarm6.Label = position.Blocking.High.ToRSAlarmLabel(highTemplate, "HH");
            
            return tags;
        }



    }
}
