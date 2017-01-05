using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

}
