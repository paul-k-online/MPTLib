using System.Collections.Generic;
using System.Linq;

namespace MPT.RSView.Csv
{
    public class AlarmMessage
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
            return string.Join(",", fields.Select(x => x.ToRS()));
        }
    }

    public class AnalogAlarmMessage : AlarmMessage
    {
        public override string ToString()
        {
            var fields = new List<object>()
                         {
                             FileMessage,
                             PrinterMessage,
                             Source,
                         };
            return string.Join(",", fields.Select(x => x.ToRS()));
        }
    }
}