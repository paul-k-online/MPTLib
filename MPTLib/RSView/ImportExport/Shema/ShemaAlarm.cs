using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPT.RSView.ImportExport.XML
{
    public class SchemaAlarm
    {
        public string AlarmType;
        public string Type;
        public string Number;
        public string Threshold;
        public string Label;
        public string Direction;
        public string Severity;

        public override string ToString()
        {
            return string.Format("[{0}]: {1}", Number, Label);
        }
    }
}
