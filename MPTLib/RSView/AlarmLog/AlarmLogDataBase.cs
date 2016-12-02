using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;
using AutoMapper;

using MPT.DataBase;
using MPT.RSView.DataLog;

namespace MPT.RSView.AlarmLog
{
    public class AlarmLogDataBase
    {
        private DBFDataBase DataBase { get; set; }

        //public bool IsLongFileName { get; private set; }


        public AlarmLogDataBase(string dataBasePath) : 
            this(new DBFDataBase(dataBasePath))
        {}

        public AlarmLogDataBase(DBFDataBase dataBase)
        {
            DataBase = dataBase;
        }



        protected static readonly Regex LongFileNameRegex = new Regex(@"^(?<Key>(?<Date>\d{4}\d{2}\d{2})(?<Count>[A-Z]{1}))(?<Type>([L]))$", 
                                                                        RegexOptions.IgnoreCase | RegexOptions.Compiled);

        protected static readonly Regex ShortFileNameRegex = new Regex(@"^(?<Key>(?<Date>\d{2}\d{2}\d{2})(?<Count>[A-Z]{1}))(?<Type>([L]))$", 
                                                                        RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public static bool IsAlarmLogTableName(string tableName, out bool UseLongFileName)
        {
            UseLongFileName = false;
            if (LongFileNameRegex.IsMatch(tableName))
            {
                UseLongFileName = true;
                return true;
            }

            return ShortFileNameRegex.IsMatch(tableName);
        }

        public IEnumerable<string> AlarmLogTableName
        {
            get
            {
                bool zero = false;
                return DataBase.AllTableList.Where(x => IsAlarmLogTableName(x, out zero));
            }
        }



        public static AlarmLogRecordDBF MapToAlarmLogRecord(IDataRecord record)
        {
            return Mapper.Map<AlarmLogRecordDBF>(record);
        }

        public IEnumerable<AlarmLogRecordDBF> ReadTable(string tableName, string condition = null)
        {
            var reader = DataBase.GetReader(tableName, condition);

            try
            {
                var list = reader.Select(AlarmLogRecordDBF.ConvertFromreader).ToList();
                return list;
            }
            catch(Exception e)
            {
                throw e;
            }
            
            
        }
    }
}