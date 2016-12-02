namespace MPT.RSView
{
    public enum RsViewDigitalAlarmType
    {
        // ReSharper disable InconsistentNaming
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
        // ReSharper restore InconsistentNaming
    }


    public class RsViewDigitalTag
        : RsViewTag
    {
        public class DigitalAlarm
        {
            public RsViewDigitalAlarmType Type = RsViewDigitalAlarmType.ON;
            public string Label;
            public ushort Severity;
        }
        
        public bool InitialValue;

        public bool IsAlarm
        {
            get { return Alarm == null; }
        }

        public DigitalAlarm Alarm;

        public RsViewDigitalTag(string name, string folder = "")
            : base(name, folder)
        {
        }

    }
}