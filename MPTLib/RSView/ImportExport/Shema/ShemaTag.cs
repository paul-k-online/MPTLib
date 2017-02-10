using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MPT.PrimitiveType;

namespace MPT.RSView.ImportExport.XML
{
    public class SchemaTag
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string Folder { get; set; }
        public string Description { get; set; }
        public string DataSource { get; set; }
        public string NodeName { get; set; }
        public string Address { get; set; }
        public string InitialValue { get; set; }
        public string Min { get; set; }
        public string Max { get; set; }
        public string Length { get; set; }
        public string Units { get; set; }
        public SchemaAlarm[] Alarms { get; set; }
        public string[] DataLogs { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
