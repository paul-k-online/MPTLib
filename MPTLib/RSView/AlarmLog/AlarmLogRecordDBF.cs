using System;
using System.Data;
using System.ComponentModel.DataAnnotations;

namespace MPT.RSView.AlarmLog
{
    public class AlarmLogRecordDBF
    {
        // Date (8)
        public DateTime Date { get; set; }
        
        // c 8
        [StringLength(8)]
        public string Time { get; set; }

        // n 5,0
        public ushort Militime { get; set; }

        // c 5
        [StringLength(5)]
        public string TransType { get; set; }

        // c 255
        [StringLength(255)]
        public string TagName { get; set; }
        
        // float 20,10
        public float TagValue { get; set; }

        // c 1
        [StringLength(1)]
        public string TagType { get; set; }
        
        //float 20,10
        public float ThreshVal { get; set; }
        
        // n 1,0
        public ushort ThreshNum { get; set; }

        // c 20
        [StringLength(20)]
        public string ThreshLabl { get; set; }
        
        // n 1,0
        public ushort Severity { get; set; }

        // c 1
        [StringLength(1)]
        public string DstFlag { get; set; }

        // c 20
        [StringLength(20)]
        public string UserId { get; set; }

        // n 1,0
        public ushort AlarmType { get; set; }

        // c254
        [StringLength(254)]
        public string Dscription { get; set; }

        // c 15
        [StringLength(15)]
        public string UserStn { get; set; }

        // c 15
        [StringLength(15)]
        public string LoggingStn { get; set; }


        public static AlarmLogRecordDBF ConvertFromreader(IDataRecord record)
        {
            var item = new AlarmLogRecordDBF()
            {
                Date = Convert.ToDateTime(record["date"]),
                Time = Convert.ToString(record["time"]).Trim(),
                Militime = Convert.ToUInt16(record["militime"]),
                TransType = Convert.ToString(record["transtype"]).Trim(),
                TagName = Convert.ToString(record["tagname"]).Trim(),
                TagValue = Convert.ToSingle(record["tagvalue"]),
                TagType = Convert.ToString(record["tagtype"]).Trim(),
                ThreshVal = Convert.ToSingle(record["threshval"]),
                ThreshNum = Convert.ToUInt16(record["threshnum"]),
                ThreshLabl = Convert.ToString(record["threshlabl"]).Trim(),
                Severity = Convert.ToUInt16(record["severity"]),
                DstFlag = Convert.ToString(record["dstflag"]).Trim(),
                UserId = Convert.ToString(record["userid"]).Trim(),
                AlarmType = Convert.ToUInt16(record["alarmtype"]),
                Dscription = Convert.ToString(record["dscription"]).Trim(),
                UserStn  = Convert.ToString(record["userstn"]).Trim(),
                LoggingStn = Convert.ToString(record["loggingstn"]).Trim(),
            };
            return item;
        }
    }
}
