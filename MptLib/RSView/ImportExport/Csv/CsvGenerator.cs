using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Ionic.Zip;
using MPT.RSView.ImportExport.Csv.Tag;

namespace MPT.RSView.ImportExport.Csv
{
    public class CsvGenerator
    {
        #region Const
        const string fileNameTemplate = "{0}-{1}.csv";


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

        #endregion

        private static string CleanFileName(string fileName)
        {
            return Path.GetInvalidFileNameChars().Aggregate(fileName, (current, c) => current.Replace(c.ToString(), string.Empty));
        }

        public string ZipFileName
        {
            get
            {
                return string.Format("{0}_{1}.zip", CleanFileName(_projectName), DateTime.Now.ToString("yyyyMMdd_hhmmss"));
            }
        }



        private readonly IEnumerable<RSViewTag> _rsViewTags;
        private readonly string _projectName;


        public CsvGenerator(IEnumerable<RSViewTag> rsViewTags, string projectName)
        {
            _rsViewTags = rsViewTags;
            _projectName = projectName;
        }


        #region Private GetCsvTags

        private IEnumerable<CsvTag> GetFolderTagStringList()
        {
            return GetFolderTagStringList(_rsViewTags)
                    .Select(CsvTag.CreateFolder);
        }

        private IEnumerable<CsvTag> GetTags()
        {
            return _rsViewTags.Select(x => x.ToCsvTag());
        }

        private IEnumerable<CsvAnalogAlarm> GetAnalogAlarms()
        {
            var analogTagsWithAlarm = _rsViewTags
                .Where(x => x is RsViewAnalogTag).Cast<RsViewAnalogTag>()
                .Where(x => x.IsAlarm);
            var csvAnalogAlarms = analogTagsWithAlarm.Select(x => x.ToCsvAnalogAlarm());
            return csvAnalogAlarms.Where(x => x != null);
        }

        private IEnumerable<CsvDigitalAlarm> GetDigitalAlarms()
        {
            var digitalTags = _rsViewTags
                .Where(x => x is RSViewDigitalTag).Cast<RSViewDigitalTag>()
                .Where(x => x.IsAlarm);

            var csvDigitalAlarms = digitalTags.Select(x => x.ToCsvDigitalAlarm());
            return csvDigitalAlarms.Where(x => x != null);
        }

        private IEnumerable<CsvDataLog> GetDatalogs()
        {
            var datalogs = _rsViewTags
                .Where(x => x.Datalogs != null)
                .SelectMany(tag =>
                    tag.Datalogs, (tag, datalog) =>
                        new CsvDataLog { ModelName = datalog.ToUpper(), TagName = tag.Name }
                );
            return datalogs.OrderBy(x => x.ModelName);
        }

        private IEnumerable<CsvDerivedTag> GetDerivedTags()
        {
            return null;
        }

        #endregion


        #region Private GetStringlist

        public IEnumerable<string> GetFolderTagStringList(IEnumerable<RSViewTag> rsViewTags)
        {
            var folders = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);
            foreach (var rsViewTag in rsViewTags)
            {
                var tag = rsViewTag;
                while ((tag = tag.ParentFolder) != null)
                {
                    folders.Add(tag.Name);
                }
            }
            return folders.OrderBy(x => x);
        }

        public IEnumerable<string> GetTagStringList()
        {
            var list = new List<string>
            {
                VersionHeader,
                "",
                TagFileHeader,
            };

            list.Add("");
            list.Add(TagFileFolderSection);
            list.AddRange(GetFolderTagStringList().Select(x => x.ToString()));

            list.Add("");
            list.Add(TagFileTagSection);
            list.AddRange(GetTags().Select(x => x.ToString()));

            return list;
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
            if (digitalAlarms != null && digitalAlarms.Any())
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
            var derivedTagList = new List<string>
            {
                VersionHeader,
                "",
                DerivedTagsHeader,
                "",
            };
            derivedTagList.AddRange(GetDerivedTags().Select(x => x.ToString()));
            return derivedTagList;
        }

        #endregion

        
        #region FileName

        public string GetFileName(string fileType)
        {
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


        private static string ListToString(IEnumerable<string> list)
        {
            return string.Join(Environment.NewLine, list);
        }

        public Stream GetZipStream(Encoding enc = null)
        {
            if (GetTags().Count() <= 0)
                return null;

            var memoryStream = new MemoryStream();
            using (var zipFile = (enc == null) ? new ZipFile() : new ZipFile(enc))
            {
                zipFile.AddEntry(TagsFileName, ListToString(GetTagStringList()));

                if (GetAnalogAlarms().Count() > 0 || GetDigitalAlarms().Count() > 0)
                    zipFile.AddEntry(AlarmsFileName, ListToString(GetAlarmStringList()));

                if (GetDatalogs().Count() > 0)
                    zipFile.AddEntry(DatalogFileName, ListToString(GetDatalogStringList()));
                zipFile.Save(memoryStream);
                memoryStream.Position = 0;
            }
            return memoryStream;

        }

        public bool SaveZip(string filePath, Encoding enc = null)
        {
            try
            {
                var zipStream = GetZipStream();
                if (zipStream == null)
                    return false;

                using (var zipFile = File.Create(filePath))
                {
                    zipStream.Position = 0;
                    zipStream.CopyTo(zipFile);
                    zipFile.Close();
                }
                return true;
            }
            catch (Exception e)
            {
                throw e;                
            }
        }

        public bool SaveZipToFolder(string folderPath, Encoding enc = null)
        {
            return SaveZip(Path.Combine(folderPath, ZipFileName), enc);
        }
    }
}
