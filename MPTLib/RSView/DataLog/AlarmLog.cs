using System;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;

using MPT.PrimitiveType;

namespace MPT.RSView.DataLog
{
    public class AlarmLog
    {
        public enum RSViewAlarmLogTransType
        {
            InAlm,
            OutAl,
            Acked,
        }

        public DateTime DateTime { get; set; }

        public RSViewAlarmLogTransType TransType { get; set; }

        [StringLength(255)]
        public string TagName { get; set; }

        public RSViewTag.TypeEnum TagType { get; set; }

        public float TagValue { get; set; }

        public float ThreshValue { get; set; }

        public ushort ThreshNumber { get; set; }

        [StringLength(20)]
        public string ThreshLabel { get; set; }

        public ushort Severity { get; set; }

        public char DestinationFlag { get; set; }

        public ushort AlarmType { get; set; }

        [StringLength(20)]
        public string UserId { get; set; }

        [StringLength(15)]
        public string UserStation { get; set; }

        [StringLength(15)]
        public string LoggingStation { get; set; }


        public static readonly Regex FileNameRegex =
                new Regex(@"^(?<Key>(?<Date> (?<Year> ([12]\d{3})|(\d{2})) (?<Month> [01]\d) (?<Day> [0123]\d) ) (?<Count> [A-Z]))  (?<Type>[L])$",
                    RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);


        public static AlarmLog FromDBF(IDataRecord record)
        {
            //Date 8
            var date = Convert.ToDateTime(record["date"]);
            //c 8
            var time = Convert.ToString(record["time"]).Trim();
            //n 5
            var militime = Convert.ToUInt16(record["militime"]);
        
            // c 5
            var transType = Convert.ToString(record["transtype"]).Trim();
            // c 255
            var tagName = Convert.ToString(record["tagname"]).Trim();
            //float 20,10
            var tagValue = Convert.ToSingle(record["tagvalue"]);
            // c 1
            var tagType = Convert.ToString(record["tagtype"]).Trim();

            //float 20,10
            var threshVal = Convert.ToSingle(record["threshval"]);
            // n 1,0
            var threshNum = Convert.ToUInt16(record["threshnum"]);
            // c 20
            var threshLabl = Convert.ToString(record["threshlabl"]).Trim();

            // n 1,0
            var severity = Convert.ToUInt16(record["severity"]);
            // c 1
            var dstFlag = Convert.ToString(record["dstflag"]).Trim();
            // c 20
            var userId = Convert.ToString(record["userid"]).Trim();
            // n 1,0
            var alarmType = Convert.ToUInt16(record["alarmtype"]);

            // c254
            //var Dscription = Convert.ToString(record["dscription"]).Trim();
            // c 15
            var userStn = System.Convert.ToString(record["userstn"]).Trim();
            // c 15
            var loggingStn = System.Convert.ToString(record["loggingstn"]).Trim();
            

            var item = new AlarmLog()
            {
                DateTime = date.Add(TimeSpan.Parse(time).Add(TimeSpan.FromMilliseconds(militime))),

                TransType = transType.ToEnum<RSViewAlarmLogTransType>(),
                TagName = tagName,
                TagValue = tagValue,
                TagType = tagType.ToEnum<RSViewTag.TypeEnum>(),

                ThreshValue = threshVal,
                ThreshNumber = threshNum,
                ThreshLabel = threshLabl,

                Severity = severity,
                DestinationFlag = dstFlag.FirstOrDefault(),
                UserId = userId,
                AlarmType = alarmType,

                UserStation = userStn,
                LoggingStation = loggingStn,
            };
            return item;
        }
    }
}
