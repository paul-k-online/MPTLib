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
        const string ConnectionStringTemplateXlsx = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source='{0}'; Extended Properties=""EXCEL 12.0 XML; HDR=NO; IMEX=1"";";
        const string ConnectionStringTemplateXls = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source='{0}';  Extended Properties=""EXCEL 8.0;      HDR=NO; IMEX=1"";";
        
        const string TableQueryTemplate = "SELECT * FROM [{0}$]";

        public static string ToSheetName(string tableName)
        {
            return string.Format("{0}", tableName);
        }


        private string ConnectionString
        {
            get
            {
                var ext = Path.GetExtension(FilePath);
                return string.Format(
                    ext != null && ext.Equals("xls", StringComparison.InvariantCultureIgnoreCase)? 
                        ConnectionStringTemplateXls : 
                        ConnectionStringTemplateXlsx, 
                    FilePath);
            }
        }

        private string FilePath
        {
            get { return File.FullName; }
        }
        
        public FileInfo File { get; private set; }
                
        public string Name
        {
            get
            {
                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(FilePath);
                return fileNameWithoutExtension != null ? fileNameWithoutExtension.ToUpper() : null;
            }
        }
                
        public ExcelDataBase(FileInfo file) 
        {
            if (file == null) 
                throw new ArgumentNullException("file");
            if (!file.Exists)
                throw new FileNotFoundException(file.FullName);

            File = file;
        }

        public ExcelDataBase(string filePath) : this(new FileInfo(filePath))
        {}



        public DataTable GetSheetDataTable(string tableName)
        {
            if (!SheetList.Contains(tableName, StringComparer.InvariantCultureIgnoreCase))
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
                throw new Exception(string.Format("GetSheetDataTable \"{0}\"",tableName), e);
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
        

        public IEnumerable<string> SheetList
        {
            get
            {
                try
                {
                    using (var connection = new OleDbConnection(ConnectionString))
                    {
                        connection.Open();
                        var excelTables = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] {null, null, null, "TABLE"});
                        connection.Close();

                        if (excelTables == null)
                            return null;

                        return
                            excelTables.AsEnumerable()
                                .Select(row => row["TABLE_NAME"].ToString())
                                .Where(x => x.EndsWith("$"))
                                .Select(x=>x.Remove(x.Length-1));
                    }
                }
                catch (Exception e)
                {
                    throw new Exception("Sheet list error", e);
                }
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}