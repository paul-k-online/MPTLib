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

    public class RSViewDigitalTag : RSViewTag
    {



        public bool InitialValue;
        public bool HasAlarm
        {
            get { return Alarm != null; }
        }

        public RSViewDigitalAlarm Alarm;

        public RSViewDigitalTag(string name, string folder = "") : base(name, folder)
        { }

        public RSViewDigitalTag(RSViewTag other) : base(other)
        { }
    }
}