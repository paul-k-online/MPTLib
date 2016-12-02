namespace MPT.RSView
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


    public class RSViewDigitalTag
        : RSViewTag
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
        {
        }

    }
}