using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;

namespace MPT.Excel
{
    public class ExcelDataBase
    {
        const string ConnectionStringTemplate = @"provider=Microsoft.Jet.OLEDB.4.0; Data Source='{0}'; Extended Properties=Excel 8.0;";//HDR=Yes;"; //IMEX=1
        const string TableQueryTemplate = "SELECT * FROM [{0}$]";

        public FileInfo File { get; private set; }

        private string FilePath
        {
            get { return File.FullName; }
        }

        public string Name
        {
            get
            {
                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(FilePath);
                return fileNameWithoutExtension != null ? fileNameWithoutExtension.ToUpper() : null;
            }
        }

        private string ConnectionString
        {
            get { return string.Format(ConnectionStringTemplate, FilePath); }
        }

        public ExcelDataBase(FileInfo file) 
        {
            if (file == null) 
                throw new ArgumentNullException("file");
            if (!file.Exists)
                throw new FileNotFoundException(file.FullName);

            File = file;
        }

        public ExcelDataBase(string filePath)
            : this(new FileInfo(filePath))
        {
        }


        public DataTable GetDataTableFromSheet(string tableName)
        {
            /*
            var r = SheetList.SkipWhile(x => !x.EndsWith("$")).Any(x => x.Equals(tableName + "$"));
            if (!r)
            {
                throw new ObjectNotFoundException(tableName);
            }
            */
            try
            {
                var tableQuery = string.Format(TableQueryTemplate, tableName);
                var adapter = new OleDbDataAdapter(tableQuery, ConnectionString);

                var dataTable = new DataTable(tableName);
                adapter.Fill(dataTable);
                return dataTable;
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<T> GetDataList<T>(string sheetName, Func<DataRow, T> convertRowFunc)
        {
            var dataTable = GetDataTableFromSheet(sheetName);

            return dataTable.AsEnumerable()
                .Select(convertRowFunc)
                .Where(pos => pos != null)
                //.ToList()
                ;
        }

        public List<string> SheetList
        {
            get
            {
                try
                {
                    using (var connection = new OleDbConnection(ConnectionString))
                    {
                        connection.Open();
                        var excelTables = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables,
                            new Object[] {null, null, null, "TABLE"});
                        connection.Close();
                        return excelTables.AsEnumerable().Select(x => x["TABLE_NAME"].ToString()).ToList();
                    }
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }


        public override string ToString()
        {
            return Name;
        }

    }
}