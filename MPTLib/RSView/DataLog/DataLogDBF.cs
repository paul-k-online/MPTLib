using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

using MPT.DataBase;

namespace MPT.RSView.DataLog
{
    public class DataLogDBF
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
            public string TagTable { get; set; }
            public string FloatTable { get; set; }
            public string StringTable { get; set; }
        }



        public DBFDataBase DataBase { get; private set; }

        private readonly string _projectName;
        public string ProjectName
        {
            get
            {
                if (!string.IsNullOrEmpty(_projectName))
                    return _projectName;
                if (DataBase.DbPath.Parent != null)
                    return DataBase.DbPath.Parent.Name;
                return "";
            }
        }

        private readonly string _datalogName;
        public string DataLogName
        {
            get
            {
                if (!string.IsNullOrEmpty(_datalogName))
                    return _datalogName;
                if (!string.IsNullOrEmpty(DataBase.DbName))
                    return DataBase.DbName;
                return "";
            }
        }


        public DataLogDBF(string directoryPath, string projectName = null, string datalogName = null) :
            this(new DirectoryInfo(directoryPath), projectName, datalogName)
        {}

        public DataLogDBF(DirectoryInfo directory, string projectName = null, string datalogName = null) :
            this(new DBFDataBase(directory), projectName, datalogName)
        {}

        public DataLogDBF(DBFDataBase dataBase, string projectName = null, string datalogName = null)
        {
            DataBase = dataBase;
            _projectName = projectName;
            _datalogName = datalogName;
        }



        protected const string LongDataLogFileNamePattern = @"^(?<Key>(?<Date>\d{4} \d{2} \d{2}) (?<Count>\d{4})) \((?<Type>(Float|Tagname|String))\)$";
        protected static readonly Regex LongFileNameRegex = new Regex(LongDataLogFileNamePattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);

        protected const string ShortDataLogFileNamePattern = @"^(?<Key>(?<Date>\d{2}\d{2}\d{2})(?<Count>[A-Z]{1}))(?<Type>([FTS]))$";
        protected static readonly Regex ShortFileNameRegex = new Regex(ShortDataLogFileNamePattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);


        public static Tuple<string, RSViewDataLogTableType> GetKeyTypeFileName(string fileName)
        {
            if (LongFileNameRegex.IsMatch(fileName))
            {
                var longMach = LongFileNameRegex.Matches(fileName);
                var fileKey = longMach[0].Groups["Key"].Value;
                var fileType = longMach[0].Groups["Type"].Value;

                var dlgType = (RSViewDataLogTableType) Enum.Parse(typeof(RSViewDataLogTableType), fileType, true);
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

            foreach (var table in DataBase.AllTableList)
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
                        dlgPair.TagTable = table;
                        break;
                    case RSViewDataLogTableType.Float:
                        dlgPair.FloatTable = table;
                        break;
                    case RSViewDataLogTableType.String:
                        dlgPair.StringTable = table;
                        break;
                }
            }
            
            return dict;
        }
    }
}
