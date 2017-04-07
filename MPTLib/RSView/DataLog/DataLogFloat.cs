using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;

namespace MPT.RSView.DataLog
{
    public partial class DataLogFloat
    {
        public DateTime? DateTime { get; set; }
        public short? Msec { get; set; }
        public short? TagIndex { get; set; }
        public double? Value { get; set; }
        public char? Status { get; set; }
        public char? Marker { get; set; }


        public virtual DataLogTag DatalogTag { get; set; }
        

        public static DataLogFloat FromDBF(IDataRecord record)
        {
            // Date 8
            var Date = Convert.ToDateTime(record["date"]);
            // c 8
            var Time = Convert.ToString(record["time"]);
            // n 3
            var Millitm = Convert.ToInt16(record["millitm"]);
            // n 5
            var TagIndex = Convert.ToInt16(record["tagindex"]);
            // f 17,3
            var Value = Convert.ToSingle(record["value"]);
            // c 1
            var Status = Convert.ToChar(record["status"]);
            // c 1
            var Marker = Convert.ToChar(record["marker"]);
            // c 4
            var Internal = Convert.ToString(record["internal"]);

            return new DataLogFloat()
            {
                DateTime = Date.Add(TimeSpan.Parse(Time)),
                Msec = Millitm,
                TagIndex = TagIndex,
                Value = Value,
                Status = Status,
                Marker = Marker,
            };
        }


        public class ByIdComparer : IEqualityComparer<DataLogFloat>
        {
            public bool Equals(DataLogFloat x, DataLogFloat y)
            {
                return
                    x.TagIndex == y.TagIndex && x.DateTime == y.DateTime;
            }

            public int GetHashCode(DataLogFloat obj)
            {
                return 
                    obj.TagIndex.GetHashCode() ^
                    obj.DateTime.GetHashCode();
            }
        }
    }
}
