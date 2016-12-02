using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Configuration;
using System.Security.Cryptography.Pkcs;
using System.Text;
using Ionic.Zip;
using MPT.Collections;
using MPT.RSView.ImportExport.Csv.Tag;

namespace MPT.RSView.ImportExport.Csv
{
    public class CsvGenerator
    {
        private const string VersionHeader = ";###001 - THIS LINE CONTAINS VERSION INFORMATION. DO NOT REMOVE!!!";
        private const string TagFileHeader = ";Tag Type, Tag Name, Tag Description, Read Only, Data Source, Security Code, Alarmed, Data Logged, Native Type, Value Type, Min Analog, Max Analog, Initial Analog, Scale, Offset, DeadBand, Units, Off Label Digital, On Label Digital, Initial Digital, Length String, Initial String, Node Name, Address, Scan Class,  System Source Name, System Source Index, RIO Address, Element Size Block, Number Elements Block, Initial Block";

        private const string TagFileFolderSection = ";Folders Section (Must define folders before tags)";
        private const string TagFileTagSection = ";Tag Section";

        private const string AlarmFileHeader = ";ALARMS";
        private const string AlarmFileAnalogAlarmsSection = ";Analog Alarms";
        private const string AlarmFileAnalogAlarmsHeader = ";Analog A,Tagname,HandshakeTagName,HandshakeAutoReset,Acktagname,AckAutoreset,Deadbandvalue,DeadbandType,Outofalarmlabel,AlarmIdentification,Ack File Msg,Ack Printer Message, Ack Message Source,Out of Alm file msg,Out of Alm printer msg,Out of Alm Message Source ,Thresh1 type,Threshold,Label,MessageSource,File msg,Printer msg,Direction,Severity,Thresh2 type,Threshold,Label,MessageSource,File msg,Printer msg,Direction,Severity,Thresh3 type,Threshold,Label,MessageSource,File msg,Printer msg,Direction,Severity,Thresh4 type,Threshold,Label,MessageSource,File msg,Printer msg,Direction,Severity,Thresh5 type,Threshold,Label,MessageSource,File msg,Printer msg,Direction,Severity,Thresh6 type,Threshold,Label,MessageSource,File msg,Printer msg,Direction,Severity,Thresh7 type,Threshold,Label,MessageSource,File msg,Printer msg,Direction,Severity,Thresh8 type,Threshold,Label,MessageSource,File msg,Printer msg,Direction,Severity";
        private const string AlarmFileDigitalAlarmsSection = ";Digital Alarms";
        private const string AlarmFileDigitalAlarmsHeader =  ";Digital D,Tagname,type,Label,Severity,Message Source,File msg,Printer msg,Out of alm msg source,Out Of alm File msg,Out of alm Printer msg,Ack msg source,Ack File msg,Ack Printer msg,Alarm Identify,Out of alm label,Ack Tag Name,Ack Auto Reset,Handshake Tag Name,Handshake Auto Reset";
        
        private const string DatalogFileHeader = ";Tag Name, Model Name";
        private const string DerivedTagsHeader = ";Tag Name, Expression, Description, DerivedTag File Name";

        private readonly IEnumerable<RsViewTag> _rsViewTags;
        private readonly string _projectName;

        public CsvGenerator(IEnumerable<RsViewTag> rsViewTags, string projectName)
        {
            _rsViewTags = rsViewTags;
            _projectName = projectName;
        }

        private static IEnumerable<string> GetFolderTagStringList(IEnumerable<RsViewTag> rsViewTags)
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
        
        public IEnumerable<CsvTag> GetFolderTagStringList()
        {
            return GetFolderTagStringList(_rsViewTags).Select(CsvTag.CreateFolder);
        }


        #region GetCsv

        public IEnumerable<CsvTag> GetTags()
        {
            return _rsViewTags
                //.OrderBy(x => x.Folder)
                .Select(x => x.ToCsvTag());
        }
        
        public IEnumerable<CsvDataLog> GetDatalogs()
        {
            var datalogs = _rsViewTags
                .Where(x => x.Datalogs != null)
                .SelectMany(tag => tag.Datalogs,
                    (tag, datalog) => new CsvDataLog {ModelName = datalog.ToUpper(), TagName = tag.FullName});
            
            return datalogs.OrderBy(x => x.ModelName);
        }

        public IEnumerable<CsvAnalogAlarm> GetAnalogAlarms()
        {
            var analogTags = _rsViewTags
                .Where(x => x is RsViewAnalogTag).Cast<RsViewAnalogTag>()
                .Where(x => x.IsAlarm);

            var csvAnalogAlarms = analogTags.Select(x => x.ToCsvAnalogAlarm());
            return csvAnalogAlarms.Where(x => x != null);
        }

        public IEnumerable<CsvDigitalAlarm> GetDigitalAlarms()
        {
            var digitalTags = _rsViewTags
                .Where(x => x is RsViewDigitalTag).Cast<RsViewDigitalTag>()
                .Where(x => x.IsAlarm);

            var csvDigitalAlarms = digitalTags.Select(x => x.ToCsvDigitalAlarm());
            return csvDigitalAlarms.Where(x => x != null);
        }

        public IEnumerable<CsvDerivedTag> GetDerivedTags()
        {
            return null;
        }

        #endregion

        
        #region GetStringlist

        public IEnumerable<string> GetTagStringList()
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
            var folders = GetFolderTagStringList();
            tagList.AddRange(folders.Select(x => x.ToString()));
            tagList.Add("");

            tagList.Add(TagFileTagSection);
            var tags = GetTags();
            tagList.AddRange(tags.Select(x => x.ToString()));

            return tagList;
        }

        public IEnumerable<string> GetAlarmStringList()
        {
            // ReSharper disable once UseObjectOrCollectionInitializer
            var alarmList = new List<string>
            {
                VersionHeader,
                "",
                AlarmFileHeader,
            };


            alarmList.Add("");
            alarmList.Add(AlarmFileAnalogAlarmsSection);
            alarmList.Add(AlarmFileAnalogAlarmsHeader);
            
            var analogAlarms = GetAnalogAlarms().ToList();
            if (analogAlarms != null && analogAlarms.Any())
                alarmList.AddRange(analogAlarms.Select(alarm => alarm.ToString()));


            alarmList.Add("");
            alarmList.Add(AlarmFileDigitalAlarmsSection);
            alarmList.Add(AlarmFileDigitalAlarmsHeader);
            
            var digitalAlarms = GetDigitalAlarms().ToList();
            if(digitalAlarms!=null && digitalAlarms.Any())
                alarmList.AddRange(digitalAlarms.Select(alarm => alarm.ToString()));

            return alarmList;
        }

        public IEnumerable<string> GetDatalogStringList()
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

        public IEnumerable<string> GetDerivedTagsStringList()
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

        #endregion

        
        #region GetFileName

        public string GetFileName(string fileType)
        {
            const string fileNameTemplate = "{0}-{1}.csv";
            return string.Format(fileNameTemplate, _projectName, fileType);
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

        public string DerivedTagsFileName
        {
            get { return GetFileName("DerivedTags"); }
        }

        #endregion


        public static Stream GetStream(IEnumerable<string> strings)
        {
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms, Encoding.GetEncoding(1251));
            try
            {
                foreach (var s in strings)
                {
                    writer.WriteLine(s);
                }
                writer.Flush();
                ms.Position = 0;
                return ms;
            }
            catch
            {
                return null;
            }
        }



        public Stream GetZipPackageStream()
        {
            var ms = new MemoryStream();
            using (var zip = new ZipFile())
            {
                var te = zip.AddEntry(TagsFileName, GetStream(GetTagStringList()));
                var ae = zip.AddEntry(AlarmsFileName, GetStream(GetAlarmStringList()));
                var de = zip.AddEntry(DatalogFileName, GetStream(GetDatalogStringList()));
                //szip.AddEntry(DerivedTagsFileName, GetStream(GetDerivedTagsStringList()));
                zip.Save(ms);
            }
            return ms;
        }
    }
}
