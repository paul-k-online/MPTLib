using System;
using System.Data;
using System.Linq;
using System.ComponentModel.DataAnnotations;

using MPT.PrimitiveType;

namespace MPT.RSView.AlarmLog
{
    public class AlarmLogRecord
    {
        public enum RSViewAlarmLogTransType
        {
            InAlm,
            OutAl,
            Acked,
        }

        // Date (8)
        public DateTime DateTime { get; set; }

        // c 5
        public RSViewAlarmLogTransType TransType { get; set; }

        // c 255
        [StringLength(255)]
        public string TagName { get; set; }

        // c 1
        public RSViewTagType TagType { get; set; }

        // float 20,10
        public float TagValue { get; set; }

        //float 20,10
        public float ThreshValue { get; set; }

        // n 1,0
        public ushort ThreshNumber { get; set; }

        // c 20
        [StringLength(20)]
        public string ThreshLabel { get; set; }

        // n 1,0
        public ushort Severity { get; set; }

        // c 1
        //[StringLength(1)]
        public char DestinationFlag { get; set; }

        // n 1,0
        public ushort AlarmType { get; set; }

        // c254
        //[StringLength(254)]
        //public string Description { get; set; }

        // c 20
        [StringLength(20)]
        public string UserId { get; set; }

        // c 15
        [StringLength(15)]
        public string UserStation { get; set; }

        // c 15
        [StringLength(15)]
        public string LoggingStation { get; set; }





        public static AlarmLogRecord ConvertFromAlarmLogRecordDBF(AlarmLogRecordDBF recordDBF)
        {
            var item = new AlarmLogRecord()
            {
                DateTime = recordDBF.Date
                                        .Add(TimeSpan.Parse(recordDBF.Time))
                                        .AddMilliseconds(recordDBF.Militime),

                TransType = recordDBF.TransType.ToEnum<RSViewAlarmLogTransType>(),
                Severity = recordDBF.Severity,
                AlarmType = recordDBF.AlarmType,
                DestinationFlag = recordDBF.DstFlag.FirstOrDefault(),

                TagType = recordDBF.TagType.ToEnum<RSViewTagType>(),
                TagName = recordDBF.TagName,
                TagValue = recordDBF.TagValue,

                ThreshNumber = recordDBF.ThreshNum,
                ThreshLabel = recordDBF.ThreshLabl,
                ThreshValue = recordDBF.ThreshVal,

                UserId = recordDBF.UserId,
                UserStation = recordDBF.UserStn,
                
                LoggingStation = recordDBF.LoggingStn,
            };
            return item;
        }
    }
}