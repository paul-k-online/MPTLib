using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPT.RSView.Csv
{
    public class DigitalAlarm
    {
        /*
;Digital Alarms
         * 
         * 
;Digital D, Tagname,             type, Label,              Severity, Message Source, File msg, Printer msg, Out of alm msg source, Out Of alm File msg, Out of alm Printer msg, Ack msg source, Ack File msg, Ack Printer msg, Alarm Identify, Out of alm label, Ack Tag Name, Ack Auto Reset, Handshake Tag Name, Handshake Auto Reset
"D",        "AI\FRCA2013_1\NAN", "ON", "FRCA2013_1 обрыв", "5",      "S",            "",       "",          "S",                   "",                  "",                     "S",            "",           "",              "",             "",               "",           "N",            "",                 "N"

"D","AI\FRCA3013_2\NAN","ON","FRCA3013_2 обрыв","5","S","","","S","","","S","","","","","","N","","N"
         * */

        public RSTagType TagType = RSTagType.A;
        public string TagName;

        // TAB Alarm States
        public RSDigitalAlarmType Type;
        public string Label;
        public ushort? SeverityValue = null;
        public string Severity
        {
            get { return SeverityValue.ToString(); }
        }

        // TAB Alarm Message
        public AlarmMessage InAlarmMessage = new AlarmMessage();
        public AlarmMessage OutOfAlarmMessage = new AlarmMessage();
        public AlarmMessage AckAlarmMessage = new AlarmMessage();

        // TAB Advenced
        public string AlarmIdentify;
        public string Outofalmlabel;
        // // redion Alarm Acknovlegde
        public string AckTagName;
        public string AckAutoReset = "N";
        // // redion Alarm Handshake
        public string HandshakeTagName;
        public string HandshakeAutoReset ="N";


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
            return string.Join(",", fields.Select(x => x.ToRS()));
        }
    }
}
