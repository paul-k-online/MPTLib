using System;
using System.Data;

namespace MPT.RSView.DataLog
{
    public partial class DataLogFloatDBF
    {
        // Date8
        public DateTime Date { get; set; }
        // c8
        public string Time { get; set; }
        // n3
        public short Millitm { get; set; }
        // n5
        public short TagIndex { get; set; }
        // f17,3
        public float Value { get; set; }
        // c1
        public string Status { get; set; }
        // c1
        public string Marker { get; set; }
        // c4
        public string Internal { get; set; }


        public static DataLogFloatDBF FromReader(IDataRecord record)
        {
            var item = new DataLogFloatDBF()
            {
                Date = Convert.ToDateTime(record["date"]),
                Time = Convert.ToString(record["time"]),
                Millitm = Convert.ToInt16(record["millitm"]),
                TagIndex = Convert.ToInt16(record["tagindex"]),
                Value = Convert.ToSingle(record["value"]),
                Status = Convert.ToString(record["status"]),
                Marker = Convert.ToString(record["marker"]),
                Internal = Convert.ToString(record["internal"]),
            };
            return item;
        }
    }
}