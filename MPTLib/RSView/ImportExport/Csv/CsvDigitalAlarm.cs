using System.Collections.Generic;
using System.Linq;

namespace MPT.RSView.ImportExport.Csv
{
    public class CsvDigitalAlarm
    {
        RSViewTag.TypeEnum TagType = RSViewTag.TypeEnum.D;
        string TagName;

        // Alarm States
        RSViewDigitalAlarm.RSViewDigitalAlarmType Type = RSViewDigitalAlarm.RSViewDigitalAlarmType.ON;
        string Label;
        ushort? SeverityValue = null;
        string Severity { get { return SeverityValue.ToString(); } }

        // Alarm Message
        CsvAlarmMessage InAlarmMessage = new CsvAlarmMessage();
        CsvAlarmMessage OutOfAlarmMessage = new CsvAlarmMessage();
        CsvAlarmMessage AckAlarmMessage = new CsvAlarmMessage();

        // Advanced
        string AlarmIdentify ="";
        string Outofalmlabel ="";
        // // Alarm Acknovlegde
        string AckTagName = "";
        string AckAutoReset = "N";
        // // Alarm Handshake
        string HandshakeTagName ="";
        string HandshakeAutoReset ="N";

        public CsvDigitalAlarm(string tagname, string label = null, ushort? severityValue = null,
            RSViewDigitalAlarm.RSViewDigitalAlarmType type = RSViewDigitalAlarm.RSViewDigitalAlarmType.ON)
        {
            TagName = tagname;
            Label = label;
            SeverityValue = severityValue;
            Type = type;
        }

        public string ToCsvString()
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
