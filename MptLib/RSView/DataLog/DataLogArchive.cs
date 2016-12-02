using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace MPT.RSView.DataLog
{
    public class DatalogArchive
    {
        public enum DatalogFileType
        {
            Tagname,
            Float,
            String
        }

        public class DatalogFilePair
        {
            public FileInfo TagTableFile { get; set; }
            public FileInfo FloatTableFile { get; set; }
            public FileInfo StringTableFile { get; set; }

            public string TagTableName
            {
                get { return Path.GetFileNameWithoutExtension(TagTableFile.Name); }
            }
            public string FloatTableName
            {
                get { return Path.GetFileNameWithoutExtension(FloatTableFile.Name); }
            }
            public string StringTableName
            {
                get { return Path.GetFileNameWithoutExtension(StringTableFile.Name); }
            }
        }

        internal enum DataLogIntervalType
        {
            Seconds,
            Minutes,
            Hours,
            Days
        }

        internal enum DataLogType
        {
            Narrow,
            Wide,
            Sql
        };

        protected const string FilePattern = "*.dbf";
        
        protected const string LongFileNamePattern = @"^(?<Key>(?<Date>\d{4} \d{2} \d{2}) (?<Count>\d{4})) \((?<Type>(Float|Tagname|String))\)"; //\( \w
        protected static readonly Regex LongFileNameRegex = new Regex(LongFileNamePattern, RegexOption);

        protected const string ShortFileNamePattern = @"^(?<Key>(?<Date>\d{2}\d{2}\d{2})(?<Count>\w{1}))(?<Type>(F|T|S))"; //\( \w
        protected static readonly Regex ShortFileNameRegex = new Regex(LongFileNamePattern, RegexOption);

        protected const RegexOptions RegexOption = RegexOptions.IgnoreCase | RegexOptions.Compiled;


        public DirectoryInfo DirectoryPath { get; private set; }

        private readonly string _projectName;
        public string ProjectName
        {
            get
            {
                if (!string.IsNullOrEmpty(_projectName))
                    return _projectName;
                return DirectoryPath.Parent == null ? "" : DirectoryPath.Parent.Name;
            }
        }
        
        private readonly string _datalogName;
        public string DatalogName
        {
            get { return string.IsNullOrEmpty(_datalogName) ? DirectoryPath.Name : _datalogName; }
        }


        public DatalogArchive(string directoryPath, string projectName = null, string datalogName = null) :
            this(new DirectoryInfo(directoryPath), projectName, datalogName)
        {
        }

        public DatalogArchive(DirectoryInfo directoryPath, string projectName = null, string datalogName = null)
        {
            DirectoryPath = directoryPath;
            _projectName = projectName;
            _datalogName = datalogName;
        }




        public IEnumerable<FileInfo> DbfFileList
        {
            get
            {
                return DirectoryPath.GetFiles(FilePattern, SearchOption.TopDirectoryOnly);
            }
        }


        public static KeyValuePair<string, DatalogFileType> GetKeyFileName(string fileName)
        {
            if (LongFileNameRegex.IsMatch(fileName))
            {
                var longMach = LongFileNameRegex.Matches(fileName);
                var fileKey = longMach[0].Groups["Key"].Value;
                var fileType = longMach[0].Groups["Type"].Value;

                var dlgType = (DatalogFileType) Enum.Parse(typeof(DatalogFileType), fileType, true);
                return new KeyValuePair<string, DatalogFileType>(fileKey, dlgType);
            }

            if (ShortFileNameRegex.IsMatch(fileName))
            {
                var shortMath = ShortFileNameRegex.Matches(fileName);
                var key = shortMath[0].Groups["Key"].Value;
                var type = shortMath[0].Groups["Type"].Value;

                var dlgType = default(DatalogFileType);

                switch (type.ToUpper())
                {
                    case "T":
                        dlgType = DatalogFileType.Tagname;
                        break;
                    case "F":
                        dlgType = DatalogFileType.Float;
                        break;
                    case "S":
                        dlgType = DatalogFileType.String;
                        break;
                }
                return new KeyValuePair<string, DatalogFileType>(key, dlgType);
            }
            return new KeyValuePair<string, DatalogFileType>(null, DatalogFileType.Tagname);
        }


        public Dictionary<string, DatalogFilePair> GetDatalogPairList()
        {
            var files = DbfFileList;
            
            var dict = new Dictionary<string, DatalogFilePair>();

            foreach (var file in files)
            {
                var fileName = Path.GetFileNameWithoutExtension(file.Name);
                var kv = GetKeyFileName(fileName);

                if (!dict.ContainsKey(kv.Key))
                {
                    dict.Add(kv.Key, new DatalogFilePair());
                }

                var dlgPair = dict[kv.Key];
                switch (kv.Value)
                {
                    case DatalogFileType.Tagname:
                        dlgPair.TagTableFile = file;
                        break;
                    case DatalogFileType.Float:
                        dlgPair.FloatTableFile = file;
                        break;
                    case DatalogFileType.String:
                        dlgPair.StringTableFile = file;
                        break;
                }
            }
            
            return dict;
        }
    }
}
