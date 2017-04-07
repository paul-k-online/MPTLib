using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

using MPT.DataBase;
using MPT.PrimitiveType;

namespace MPT.RSView.DataLog
{
    public class DBFDataLog : DBFDataBase
    {
        public enum RSViewDataLogTableType
        {
            Tagname,
            Float,
            String
        }

        public enum RSViewDataLogIntervalType
        {
            Seconds,
            Minutes,
            Hours,
            Days,
            Manual
        }

        public enum RSViewDataLogType
        {
            Narrow,
            Wide,
            Sql
        };


        public class DataLogFilePairDBF
        {
            public string TagFile { get; set; }
            public string FloatFile { get; set; }
            public string StringFile { get; set; }
        }


        private readonly string _projectName;
        public string ProjectName
        {
            get
            {
                if (!string.IsNullOrEmpty(_projectName))
                    return _projectName;
                return DbName;
            }
        }

        public DBFDataLog(string directoryPath, string projectName = null) :
            this(new DirectoryInfo(directoryPath), projectName)
        {}

        public DBFDataLog(DirectoryInfo directory, string projectName = null) : 
            base(directory)
        {
            _projectName = projectName;
        }



        protected static readonly Regex LongFileNameRegex = 
            new Regex(@"^(?<Key>(?<Date>\d{4} \d{2} \d{2}) (?<Count>\d{4})) \((?<Type>(Float|Tagname|String))\)$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        protected static readonly Regex ShortFileNameRegex = 
            new Regex(@"^(?<Key>(?<Date>\d{2}\d{2}\d{2})(?<Count>[A-Z]{1}))(?<Type>([FTS]))$", RegexOptions.IgnoreCase | RegexOptions.Compiled);


        public static Tuple<string, RSViewDataLogTableType> GetKeyTypeFileName(string fileName)
        {
            if (LongFileNameRegex.IsMatch(fileName))
            {
                var longMach = LongFileNameRegex.Matches(fileName);
                var fileKey = longMach[0].Groups["Key"].Value;
                var fileType = longMach[0].Groups["Type"].Value;
                
                var dlgType = fileType.ToEnum<RSViewDataLogTableType>();
                return new Tuple<string, RSViewDataLogTableType>(fileKey, dlgType);
            }

            if (ShortFileNameRegex.IsMatch(fileName))
            {
                var shortMath = ShortFileNameRegex.Matches(fileName);
                var key = shortMath[0].Groups["Key"].Value;
                var type = shortMath[0].Groups["Type"].Value;

                var dlgType = default(RSViewDataLogTableType);

                switch (type.ToUpper())
                {
                    case "T":
                        dlgType = RSViewDataLogTableType.Tagname;
                        break;
                    case "F":
                        dlgType = RSViewDataLogTableType.Float;
                        break;
                    case "S":
                        dlgType = RSViewDataLogTableType.String;
                        break;
                }
                return new Tuple<string, RSViewDataLogTableType>(key, dlgType);
            }
            return null;
        }


        public Dictionary<string, DataLogFilePairDBF> GetDatalogPairList()
        {
            var dict = new Dictionary<string, DataLogFilePairDBF>();

            foreach (var table in GetTableList())
            {
                var typeTuple = GetKeyTypeFileName(table);

                if (typeTuple == null)
                    continue;

                var kv = new KeyValuePair<string, RSViewDataLogTableType>(typeTuple.Item1, typeTuple.Item2);

                if (!dict.ContainsKey(kv.Key))
                {
                    dict.Add(kv.Key, new DataLogFilePairDBF());
                }

                var dlgPair = dict[kv.Key];
                switch (kv.Value)
                {
                    case RSViewDataLogTableType.Tagname:
                        dlgPair.TagFile = table;
                        break;
                    case RSViewDataLogTableType.Float:
                        dlgPair.FloatFile = table;
                        break;
                    case RSViewDataLogTableType.String:
                        dlgPair.StringFile = table;
                        break;
                }
            }            
            return dict;
        }
    }
}
