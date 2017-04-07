using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;

namespace MPT.DataBase
{
    public class ExcelDataBase
    {
        //const string ConnectionStringTemplate = @"provider=Microsoft.Jet.OLEDB.4.0; Data Source='{0}'; Extended Properties=EXCEL 8.0;";//HDR=Yes;"; //IMEX=1
        //const string ConnectionStringTemplate = @"Provider=Microsoft.Jet.OLEDB.4.0; Data Source='{0}';  Extended Properties=""EXCEL 8.0; HDR=NO; IMEX=1"";";
        
        //const string ConnectionStringTemplateXlsx = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source='{0}'; Extended Properties=""EXCEL 12.0 XML; HDR=NO; IMEX=1"";";
        //const string ConnectionStringTemplateXls = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source='{0}';  Extended Properties=""EXCEL 8.0;      HDR=NO; IMEX=1"";";
        private const string ConnectionStringTemplate = @"provider=Microsoft.ACE.OLEDB.12.0; Data Source='{0}'; Extended Properties=""EXCEL 8.0"" "; //Extended Properties = EXCEL 8.0;//HDR=Yes;"; //IMEX=1

        private const string TableQueryTemplate = "SELECT * FROM [{0}$]";

        private string ConnectionString
        {
            get
            {
                const string conTemplate = ConnectionStringTemplate;
                /*
                var ext = Path.GetExtension(FilePath);
                conTemplate = (ext != null && ext.Equals("xls", StringComparison.InvariantCultureIgnoreCase)) ?
                        ConnectionStringTemplateXls :
                        ConnectionStringTemplateXlsx;
                        */
                return string.Format(conTemplate,  FilePath);
            }
        }

        private string FilePath => File.FullName;

        public FileInfo File { get; private set; }


        public string Name
        {
            get
            {
                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(FilePath);
                return fileNameWithoutExtension?.ToUpper();
            }
        }
        

        public ExcelDataBase(FileInfo file) 
        {
            if (file == null) 
                throw new ArgumentNullException(nameof(file));
            if (!file.Exists)
                throw new FileNotFoundException(file.FullName);
            File = file;
        }


        public ExcelDataBase(string filePath) : this(new FileInfo(filePath))
        {}


        public DataTable GetSheetDataTable(string tableName)
        {
            if (!GetSheetList().Contains(tableName, StringComparer.InvariantCultureIgnoreCase))
                return null;

            try
            {
                var tableQuery = string.Format(TableQueryTemplate, tableName);
                var adapter = new OleDbDataAdapter(tableQuery, ConnectionString);
                
                var dataTable = new DataTable(tableName);
                adapter.Fill(dataTable);
                return dataTable;
            }
            catch (Exception e)
            {
                throw new Exception($"GetSheetDataTable \"{tableName}\"", e);
            }
        }


        public IEnumerable<T> GetDataList<T>(string sheetName, Func<DataRow, T> convertRowFunc)
        {
            var dataTable = GetSheetDataTable(sheetName);
            var rows = dataTable.AsEnumerable();

#if DEBUG
            var rows1 = rows;
#else
            var rows1 = rows.AsParallel();
#endif
            var convertList = rows1.Select(convertRowFunc).ToList();
            var convertedList = convertList.Where(pos => pos != null).ToList();
            return convertedList;
        }


        public IEnumerable<string> GetSheetList()
        {
            try
            {
                using (var connection = new OleDbConnection(ConnectionString))
                {
                    connection.Open();
                    var excelTables = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] {null, null, null, "TABLE"});
                    connection.Close();

                    return
                        excelTables?.AsEnumerable()
                            .Select(row => row["TABLE_NAME"].ToString())
                            .Where(x => x.EndsWith("$"))
                            .Select(x => x.Remove(x.Length - 1));
                }
            }
            catch (Exception e)
            {
                throw new Exception("Sheet list error", e);
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}