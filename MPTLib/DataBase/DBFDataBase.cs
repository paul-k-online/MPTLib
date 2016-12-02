using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;

namespace MPT.DataBase
{
    public static class DBFDataBaseExtension
    {
        public static IEnumerable<T> Select<T>(this IDataReader reader, Func<IDataReader, T> projection)
        {
            while (reader.Read())
            {
                yield return projection(reader);
            }
        }
    }


    public class DBFDataBase
    {
        private const string AllTablePattern = "*.dbf";

        private const string ObdcConnectionStringTemplate =
            @"Driver={Microsoft dBase Driver (*.dbf)}; SourceType=DBF; SourceDB={0}; Exclusive=No; NULL=NO; DELETED=NO; BACKGROUNDFETCH=NO; Collate=Machine;";

        private const string OleDbJetConnectionStringTemplate =
            @"Provider=Microsoft.Jet.OLEDB.4.0; Data Source={0}; Extended Properties=dBASE IV; User ID=; Password=;";

        private const string OleDbConnectionStringTemplate =
            @"Provider=VFPOLEDB; Data Source='{0}'; User Id=''; Password=''; Mode=read; Collating Sequence=MACHINE;";

        private const string SqlSelectTemplate = @"SELECT * FROM '{0}'";
        private const string SqlSelectWhereTemplate = @"SELECT * FROM '{0}' WHERE {1}";

        private string ConnectionStringTempltate = OleDbConnectionStringTemplate;
        private string ConnectionString
        {
            get { return DbPath.Exists ? string.Format(ConnectionStringTempltate, DbPath.FullName) : ""; }
        }

        public DirectoryInfo DbPath { get; private set; }
        
        public string DbName { get; private set; }

        private OleDbConnection Connection { get; set; }
        

        public DBFDataBase(string dataBasePath, string dbName = "")
            : this(new DirectoryInfo(dataBasePath), dbName)
        {}

        public DBFDataBase(DirectoryInfo dataBasePath, string dbName = "")
        {
            if (!dataBasePath.Exists)
            {
                throw new DirectoryNotFoundException(dataBasePath.FullName);
            }
            DbPath = dataBasePath;

            DbName = dbName;
            if (string.IsNullOrEmpty(DbName))
            {
                DbName = DbPath.Name;
            }
            
            OpenConnection();
        }


        private bool OpenConnection()
        {
            Connection = new OleDbConnection(ConnectionString);
            try
            {
                if (Connection.State == ConnectionState.Closed)
                {
                    Connection.Open();
                }
                return Connection.State == ConnectionState.Open;
            }
            catch (Exception e)
            {
                return false;
            }           
        }

        public IEnumerable<string> AllTableList
        {
            get
            {
                if (!DbPath.Exists)
                    return null;
                var a = DbPath.GetFiles(AllTablePattern, SearchOption.TopDirectoryOnly);
                var b = a.Select(filename => Path.GetFileNameWithoutExtension(filename.Name));
                return b;
            }
        }

        public string GetSqlSelect(string table, string condition = "")
        {
            if (string.IsNullOrEmpty(condition))
                return string.Format(SqlSelectTemplate, table);
            else
                return string.Format(SqlSelectWhereTemplate, table, condition);
        }

        public IDataReader GetReader(string table, string condition = "")
        {
            var cmdText = GetSqlSelect(table, condition);
            var cmd = new OleDbCommand(cmdText, Connection);
            try
            {
                return cmd.ExecuteReader();
            }
            catch (Exception e)
            {
                return null;
            }
        }


        public IEnumerable<T> ReadTable<T>(Func<IDataRecord, T> projection, string table, string condition = "")
        {
            try
            {
                return GetReader(table, condition).Select(projection);
                
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
