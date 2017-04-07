using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;

namespace MPT.DataBase
{
    public static class DataReaderExtension
    {
        public static IEnumerable<T> Select<T>(this IDataReader reader, Func<IDataRecord, T> projection)
        {
            while (reader.Read())
            {
                yield return projection(reader);
            }
            reader.Close();
        }
    }


    public class DBFDataBase
    {
        private const string DbfTablePattern = "*.dbf";

        //OdbcConnection odbcConnection = new OdbcConnection(
        // @"Driver={Microsoft Visual FoxPro Driver};SourceType=DBF;SourceDB={0};Exclusive=No;Collate=Machine;NULL=NO;DELETED=NO;BACKGROUNDFETCH=NO;"
        
        private const string ObdcConnectionStringTemplate = @"Driver={Microsoft dBase Driver (*.dbf)}; SourceType=DBF; SourceDB={0}; Exclusive=No; NULL=NO; DELETED=NO; BACKGROUNDFETCH=NO; Collate=Machine;";
        private const string JetOleDbConnectionStringTemplate = @"Provider=Microsoft.Jet.OLEDB.4.0; Data Source={0}; Extended Properties=dBASE IV; User ID=; Password=;";
        private const string VFPOleDbConnectionStringTemplate = @"Provider=VFPOLEDB; Data Source='{0}'; User Id=''; Password=''; Mode=read; Collating Sequence=MACHINE; Exclusive=No";

        private const string SqlSelectTemplate = @"SELECT * FROM '{0}'";
        private const string SqlSelectWhereTemplate = @"SELECT * FROM '{0}' WHERE {1}";

        private string ConnectionStringTempltate = VFPOleDbConnectionStringTemplate;


        public DirectoryInfo DbPath { get; private set; }
        private string ConnectionString
        {
            get { return string.Format(ConnectionStringTempltate, DbPath.FullName); }
        }

        private OleDbConnection _connection;

        private string _dbName;
        public string DbName
        {
            get { return string.IsNullOrEmpty(_dbName) ? DbPath.Name : _dbName; }
        }


        public DBFDataBase(string path, string dbName = null)
            : this(new DirectoryInfo(path), dbName)
        {}

        public DBFDataBase(DirectoryInfo path, string dbName = null)
        {
            if (!path.Exists)
                throw new DirectoryNotFoundException(path.FullName);
            DbPath = path;
            _dbName = dbName;
        }


        private bool OpenConnection()
        {
            try
            {
                if (_connection == null)
                    _connection = new OleDbConnection(ConnectionString);
                if (_connection.State != ConnectionState.Open)
                    _connection.Open();
                return _connection.State == ConnectionState.Open;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public IEnumerable<string> GetTableList()
        {
            try
            {
                return DbPath.GetFiles(DbfTablePattern).Select(filename => Path.GetFileNameWithoutExtension(filename.Name));
            }
            catch (Exception)
            {
                throw;
            }
        }

        private string GetSqlSelect(string table, string condition = null)
        {
            if (string.IsNullOrEmpty(condition))
                return string.Format(SqlSelectTemplate, table);
            else
                return string.Format(SqlSelectWhereTemplate, table, condition);
        }


        public IDataReader GetReader(string table, string condition = null)
        {
            try
            {
                OpenConnection();
                var cmd = new OleDbCommand(GetSqlSelect(table, condition), _connection);
                return cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public IEnumerable<T> ReadTable<T>(Func<IDataRecord, T> projection, string table, string condition = null)
        {
            try
            {
                return GetReader(table, condition).Select(projection);
            }
            catch (Exception e)
            {
                throw new Exception("Read table", e);
            }
        }
    }
}
