using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;

namespace MPT.RSView
{
    public class DigitalTag : Tag
    {
        public class DigitalAlarm
        {
            public RSDigitalAlarmType Type = RSDigitalAlarmType.ON;
            public string Label;
            public ushort Severity;
        }
        
        public bool InitialValue;

        public bool IsAlarm
        {
            get { return Alarm == null; }
        }
        public DigitalAlarm Alarm;
    }
}