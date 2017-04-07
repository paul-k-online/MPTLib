using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Text.RegularExpressions;

namespace MPT.RSView.DataLog
{
    public class ActLog
    {
        public byte Type { get; set; }
        
        public uint Id { get; set; }

        public DateTime DateTime { get; set; }

        public char DstFlag { get; set; }

        [StringLength(20)]
        public string Category { get; set; }

        [StringLength(31)]
        public string Source { get; set; }

        [StringLength(20)]
        public string User { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        [StringLength(15)]
        public string UserStation { get; set; }

        [StringLength(15)]
        public string LoggingStation { get; set; }


        public static readonly Regex FileNameRegex =
                    new Regex(@"^(?<Key>(?<Date> (?<Year> ([12]\d{3})|(\d{2})) (?<Month> [01]\d) (?<Day> [0123]\d) ) (?<Count>[A-Z]))  (?<Type>[I])$",
                        RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);


        public static ActLog FromDBF(IDataRecord record)
        {
            //n 1
            var type = Convert.ToByte(record["type"]);
            //n 10
            var id = Convert.ToUInt32(record["id"]);
            
            //Date 8
            var date = Convert.ToDateTime(record["date"]);
            //c 8
            var time = Convert.ToString(record["time"]).Trim();
            //n 5
            var militime = Convert.ToUInt16(record["militime"]);
            
            //c 1
            var dstFlag = Convert.ToChar(record["dstflag"]);
            //c 20
            var category = Convert.ToString(record["category"]).Trim();
            //c 31
            var source = Convert.ToString(record["source"]).Trim();
            //c 20
            var user = Convert.ToString(record["user"]).Trim();
            //c 255
            var dscrptn = Convert.ToString(record["dscrptn"]).Trim();
            //c 15
            var userstn = Convert.ToString(record["userstn"]).Trim();
            //c 15
            var loggingstn = Convert.ToString(record["loggingstn"]).Trim();

            var item = new ActLog()
            {
                Type = type,
                Id = id,
                DateTime = date.Add(TimeSpan.Parse(time).Add(TimeSpan.FromMilliseconds(militime))),
                DstFlag = dstFlag,
                Category = category,
                Source = source,
                User = user,
                Description = dscrptn,
                UserStation = userstn,
                LoggingStation = loggingstn,
            };
            return item;
        }
    }
}
