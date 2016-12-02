using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Configuration;
using MPT.Collections;

namespace MPT.RSView.ImportExport.Csv
{
    public class CsvConverter
    {
        private const string VersionHeader = ";###001 - THIS LINE CONTAINS VERSION INFORMATION. DO NOT REMOVE!!!";


        private const string TagFileHeader =
            ";Tag Type, Tag Name, Tag Description, Read Only, Data Source, Security Code, Alarmed, Data Logged, Native Type, Value Type, Min Analog, Max Analog, Initial Analog, Scale, Offset, DeadBand, Units, Off Label Digital, On Label Digital, Initial Digital, Length String, Initial String, Node Name, Address, Scan Class,  System Source Name, System Source Index, RIO Address, Element Size Block, Number Elements Block, Initial Block";

        private const string TagFileFolderSection = ";Folders Section (Must define folders before tags)";
        private const string TagFileTagSection = ";Tag Section";


        private const string AlarmFileHeader = ";ALARMS";
        private const string AlarmFileAnalogAlarmsSection = ";Analog Alarms";

        private const string AlarmFileAnalogAlarmsHeader =
            ";Analog A,Tagname,HandshakeTagName,HandshakeAutoReset,Acktagname,AckAutoreset,Deadbandvalue,DeadbandType,Outofalarmlabel,AlarmIdentification,Ack File Msg,Ack Printer Message, Ack Message Source,Out of Alm file msg,Out of Alm printer msg,Out of Alm Message Source ,Thresh1 type,Threshold,Label,MessageSource,File msg,Printer msg,Direction,Severity,Thresh2 type,Threshold,Label,MessageSource,File msg,Printer msg,Direction,Severity,Thresh3 type,Threshold,Label,MessageSource,File msg,Printer msg,Direction,Severity,Thresh4 type,Threshold,Label,MessageSource,File msg,Printer msg,Direction,Severity,Thresh5 type,Threshold,Label,MessageSource,File msg,Printer msg,Direction,Severity,Thresh6 type,Threshold,Label,MessageSource,File msg,Printer msg,Direction,Severity,Thresh7 type,Threshold,Label,MessageSource,File msg,Printer msg,Direction,Severity,Thresh8 type,Threshold,Label,MessageSource,File msg,Printer msg,Direction,Severity";

        private const string AlarmFileDigitalAlarmsSection = ";Digital Alarms";

        private const string AlarmFileDigitalAlarmsHeader =
            ";Digital D,Tagname,type,Label,Severity,Message Source,File msg,Printer msg,Out of alm msg source,Out Of alm File msg,Out of alm Printer msg,Ack msg source,Ack File msg,Ack Printer msg,Alarm Identify,Out of alm label,Ack Tag Name,Ack Auto Reset,Handshake Tag Name,Handshake Auto Reset";


        private const string DatalogFileHeader = ";Tag Name, Model Name";

        private const string DerivedTagsHeader = ";Tag Name, Expression, Description, DerivedTag File Name";

        private const string FileNameTemplate = "{0}-{1}";



        private readonly IEnumerable<RsViewTag> _rsViewTags;
        private readonly string _projectName;

        public CsvConverter(IEnumerable<RsViewTag> rsViewTags, string projectName)
        {
            _rsViewTags = rsViewTags;
            _projectName = projectName;
        }

        public static IEnumerable<string> GetAllFolders(IEnumerable<RsViewTag> rsViewTags)
        {
            var folders = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

            foreach (var rsViewTag in rsViewTags)
            {
                var tag = rsViewTag;
                while ((tag = tag.Parent) != null)
                {
                    folders.Add(tag.FullName);
                }
            }
            return folders.OrderBy(x => x);
        }

        public IEnumerable<CsvTag> GetFolderTags()
        {
            return GetAllFolders(_rsViewTags).Select(CsvTag.CreateFolder);
        }

        public IEnumerable<CsvTag> GetTags()
        {
            return _rsViewTags
                .OrderBy(x => x.FullName)
                .Select(x => x.ToCsvTag());
        }

        public IEnumerable<CsvDataLog> GetDatalogs()
        {
            return _rsViewTags.Where(x => x.Datalogs != null)
                .SelectMany(tag => tag.Datalogs,
                    (tag, datalog) => new CsvDataLog {ModelName = datalog.ToUpper(), TagName = tag.FullName})
                .OrderBy(x => x.ModelName).ToList()
                ;
        }

        public IEnumerable<CsvAnalogAlarm> GetAnalogAlarms()
        {
            var analogTags = _rsViewTags
                .Where(x => x is RsViewAnalogTag).Cast<RsViewAnalogTag>()
                .Where(x => x.IsAlarm);

            /*
            var alarmList = new List<CsvAnalogAlarm>();
            foreach (var rsViewAnalogTag in at)
            {
                var alarm = rsViewAnalogTag.ToCsvAnalogAlarm();
                if(alarm!=null)
                    alarmList.Add(alarm);
            }
            */
            var csvAnalogAlarms = analogTags.Select(x => x.ToCsvAnalogAlarm());
            return csvAnalogAlarms;
        }

        public IEnumerable<CsvDigitalAlarm> GetDigitalAlarms()
        {
            var digitalTags = _rsViewTags
                .Where(x => x is RsViewDigitalTag).Cast<RsViewDigitalTag>()
                .Where(x => x.IsAlarm);

            var csvDigitalAlarms = digitalTags.Select(x => x.ToCsvDigitalAlarm());
            return csvDigitalAlarms;
        }

        public IEnumerable<string> GetTagFile()
        {
            // ReSharper disable once UseObjectOrCollectionInitializer
            var tagList = new List<string>
            {
                VersionHeader,
                "",
                TagFileHeader,
                "",

            };

            tagList.Add(TagFileFolderSection);
            var folders = GetFolderTags();
            tagList.AddRange(folders.Select(x => x.ToString()));
            tagList.Add("");

            tagList.Add(TagFileTagSection);
            tagList.AddRange(GetTags().Select(x => x.ToString()));

            return tagList;
        }

        public IEnumerable<CsvDerivedTag> GetDerivedTags()
        {
            return null;
        }

        public IEnumerable<string> GetAlarmFile()
        {
            var alarmList = new List<string>
            {
                VersionHeader,
                "",
                AlarmFileHeader,
                "",
                AlarmFileAnalogAlarmsSection,
                AlarmFileAnalogAlarmsHeader,
            };

            alarmList.AddRange(GetAnalogAlarms().Select(x => x.ToString()));

            alarmList.Add("");
            alarmList.Add(AlarmFileDigitalAlarmsSection);
            alarmList.Add(AlarmFileDigitalAlarmsHeader);
            alarmList.AddRange(GetDigitalAlarms().Select(x => x.ToString()));

            return alarmList;
        }

        public IEnumerable<string> GetDatalogFile()
        {
            var list = new List<string>
            {
                VersionHeader,
                "",
                DatalogFileHeader,
                ""
            };
            list.AddRange(GetDatalogs().Select(x => x.ToString()));
            return list;
        }

        public IEnumerable<string> GetDerivedTagsFile()
        {
            var list = new List<string>
            {
                VersionHeader,
                "",
                DerivedTagsHeader,
                "",
            };
            list.AddRange(GetDerivedTags().Select(x => x.ToString()));
            return list;
        }


        public string GetFileName(string fileType)
        {
            return string.Format(FileNameTemplate, _projectName, fileType);
        }
        public string TagsFileName
        {
            get { return GetFileName("Tags"); }
        }
        public string AlarmsFileName
        {
            get { return GetFileName("Alarms"); }
        }
        public string DatalogFileName
        {
            get { return GetFileName("DataLog"); }
        }
        public string TagFileName
        {
            get { return GetFileName("DerivedTags"); }
        }



    }
}
