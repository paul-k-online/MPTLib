using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using MPT.Model;

namespace MPT.DataLog
{
// ReSharper disable once InconsistentNaming
    public class DbfDataBase
    {
        private const string AllTablePattern = "*.DBF";

        private const string ConnectionStringTepltate =
          // @"Provider=Microsoft.Jet.OLEDB.4.0; Data Source={0}; Extended Properties=dBASE IV; User ID=; Password=;";
          // @"Driver={Microsoft dBase Driver (*.dbf)}; SourceType=DBF; SourceDB={0}; Exclusive=No; NULL=NO; DELETED=NO; BACKGROUNDFETCH=NO; Collate=Machine;";         // OdbcConnection 
          // @"Provider=vfpoledb;Data Source=" + path + ";Collating Sequence=general;";   
          @"provider=VFPOLEDB;data source='{0}';user id='';password='';mode=read;";

        private const string SqlSelectTemplate = @"SELECT * FROM '{0}'";
        private const string SqlSelectWhereTemplate = @"SELECT * FROM '{0}' WHERE {1}";


        public string ConnectionString
        {
            get
            {
                return !DbPath.Exists ? 
                    String.Empty : 
                    String.Format(ConnectionStringTepltate, DbPath.FullName);
            }
        }

        public DirectoryInfo DbPath { get; private set; }
        
        public string DbName { get; private set; }

        private OleDbConnection Connection { get; set; }

        
        public DbfDataBase(string dataBasePath, string dbName = "")
            : this(new DirectoryInfo(dataBasePath), dbName)
        {
        }

        public DbfDataBase(DirectoryInfo dataBasePath, string dbName = "")
        {
            if (!dataBasePath.Exists)
            {
                throw new DirectoryNotFoundException(dataBasePath.FullName);
            }
            DbPath = dataBasePath;

            DbName = dbName;
            if (String.IsNullOrEmpty(DbName))
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
            catch (Exception)
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
            var sqlText = String.IsNullOrEmpty(condition) ?
                String.Format(SqlSelectTemplate, table) :
                String.Format(SqlSelectWhereTemplate, table, condition);
            return sqlText;
        }

        public IDataReader GetReader(string table, string condition = "")
        {
            var cmd = new OleDbCommand(GetSqlSelect(table,condition), Connection);
            try
            {
                var reader = cmd.ExecuteReader();
                return reader;
            }
            catch (Exception)
            {
                return null;
            }
        }

    }
}
