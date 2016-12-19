using System.Collections.Generic;
using System.Linq;

namespace MPT.RSView.ImportExport.Csv
{
    public class CsvAlarmMessage
    {
        public enum MessageSource
        {
            /// <summary>
            /// System Message Source
            /// </summary>
            S,
            /// <summary>
            /// User Message Source
            /// </summary>
            U,
            /// <summary>
            /// Custom Message Source
            /// </summary>
            C,
        }

        public MessageSource Source = MessageSource.S;
        public string FileMessage = "";
        public string PrinterMessage = "";

        public override string ToString()
        {
            var fields = new List<object>()
                         {
                             Source,
                             FileMessage,
                             PrinterMessage,
                         };
            return string.Join(",", fields.Select(x => x.ToCsvString()));
        }
    }


    public class CsvDigitalAlarm
    {
        private RSViewTagType TagType = RSViewTagType.D;
        private string TagName;

        // TAB Alarm States
        public RSViewDigitalAlarmType Type = RSViewDigitalAlarmType.ON;
        public string Label;
        public ushort? SeverityValue = null;
        public string Severity
        {
            get { return SeverityValue.ToString(); }
        }

        // TAB Alarm Message
        public CsvAlarmMessage InAlarmMessage = new CsvAlarmMessage();
        public CsvAlarmMessage OutOfAlarmMessage = new CsvAlarmMessage();
        public CsvAlarmMessage AckAlarmMessage = new CsvAlarmMessage();

        // TAB Advanced
        public string AlarmIdentify ="";
        public string Outofalmlabel ="";
        // // redion Alarm Acknovlegde
        public string AckTagName = "";
        public string AckAutoReset = "N";
        // // redion Alarm Handshake
        public string HandshakeTagName ="";
        public string HandshakeAutoReset ="N";

        public CsvDigitalAlarm(string tagname)
        {
            TagName = tagname;
        }

        public override string ToString()
        {
            var fields = new List<object>()
                         {
                             TagType,
                             TagName,
                             Type,
                             Label,
                             Severity,
                             InAlarmMessage,
                             OutOfAlarmMessage,
                             AckAlarmMessage,
                             AlarmIdentify,
                             Outofalmlabel,
                             AckTagName,
                             AckAutoReset,
                             HandshakeTagName,
                             HandshakeAutoReset,
                        };
            return string.Join(",", fields.Select(x => x.ToCsvString()));
        }
    }
}
