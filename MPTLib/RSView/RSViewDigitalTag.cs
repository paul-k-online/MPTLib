namespace MPT.RSView
{
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