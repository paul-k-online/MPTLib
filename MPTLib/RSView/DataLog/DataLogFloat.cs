using System;
using System.Linq;
using System.Collections.Generic;


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
        


        public static DataLogFloat FromDBF(DataLogFloatDBF dbfItem)
        {
            return new DataLogFloat()
            {
                DateTime = dbfItem.Date.Add(TimeSpan.Parse(dbfItem.Time)).AddMilliseconds(dbfItem.Millitm),
                Msec = dbfItem.Millitm,
                TagIndex = dbfItem.TagIndex,
                Value = dbfItem.Value,
                Marker = dbfItem.Marker.FirstOrDefault(),
                Status = dbfItem.Status.FirstOrDefault(),
                
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
