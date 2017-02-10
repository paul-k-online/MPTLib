using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPT.RSView
{
    public class RSViewDigitalAlarm
    {
        public enum RSViewDigitalAlarmType
        {
            /// <summary>
            /// On
            /// </summary>
            ON,
            /// <summary>
            /// Off
            /// </summary>
            OFF,
            /// <summary>
            /// Any Change 
            /// </summary>
            COS,
            /// <summary>
            /// Changes to On
            /// </summary>
            COSON,
            /// <summary>
            /// Changes to Off
            /// </summary>
            COSOFF
        }

        public RSViewDigitalAlarmType Type = RSViewDigitalAlarmType.ON;
        public string Label;
        public ushort Severity;
    }
}