namespace MPT.RSView
{
    public class RSViewDigitalTag : RSViewTag
    {
        public class DigitalAlarm
        {
            public RSViewDigitalAlarmType Type = RSViewDigitalAlarmType.ON;
            public string Label;
            public ushort Severity;
        }
        
        public bool InitialValue;

        public bool IsAlarm
        {
            get { return Alarm != null; }
        }

        public DigitalAlarm Alarm;

        public RSViewDigitalTag(string name, string folder = "")
            : base(name, folder)
        {}
    }
}