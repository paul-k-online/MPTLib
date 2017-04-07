using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MPT.RSView
{
    public class RSViewAnalogAlarm
    {
        public enum TresholdDirection
        {
            /// <summary>
            /// Decreasing
            /// </summary>
            D,
            /// <summary>
            /// Increasing
            /// </summary>
            I
        }

        /// <summary>
        /// value or tagname with value
        /// </summary>
        [MaxLength(21)]
        [DefaultValue("")]
        public string Threshold { get; set; }

        [MaxLength(21)]
        [DefaultValue("")]
        public string Label { get; set; }

        [DefaultValue(1)]
        public ushort Severity { get; set; }

        public TresholdDirection Direction { get; set; }
    }

    public class RSViewAnalogTag : RSViewTag
    {
        [DefaultValue(0)]
        public double Min { get; set; }

        [DefaultValue(100)]
        public double Max { get; set; }
        public double InitialValue { get; set; }
        public string Units { get; set; }

        public readonly Dictionary<int, RSViewAnalogAlarm> Alarms =
            new Dictionary<int, RSViewAnalogAlarm>()
            {
                {1, null},
                {2, null},
                {3, null},
                {4, null},
                {5, null},
                {6, null},
                {7, null},
                {8, null},
            };

        public bool HasAlarm
        {
            get { return Alarms.Any(x => x.Value != null); }
        }
        
        public RSViewAnalogAlarm Alarm1
        {
            get { return Alarms[1]; }
            set { Alarms[1] = value; }
        }
        public RSViewAnalogAlarm Alarm2
        {
            get { return Alarms[2]; }
            set { Alarms[2] = value; }
        }
        public RSViewAnalogAlarm Alarm3
        {
            get { return Alarms[3]; }
            set { Alarms[3] = value; }
        }
        public RSViewAnalogAlarm Alarm4
        {
            get { return Alarms[4]; }
            set { Alarms[4] = value; }
        }
        public RSViewAnalogAlarm Alarm5
        {
            get { return Alarms[5]; }
            set { Alarms[5] = value; }
        }
        public RSViewAnalogAlarm Alarm6
        {
            get { return Alarms[6]; }
            set { Alarms[6] = value; }
        }
        public RSViewAnalogAlarm Alarm7
        {
            get { return Alarms[7]; }
            set { Alarms[7] = value; }
        }
        public RSViewAnalogAlarm Alarm8
        {
            get { return Alarms[8]; }
            set { Alarms[8] = value; }
        }
        
        public RSViewAnalogTag(string name ="", string folder="") : base(name, folder)
        { }

        public RSViewAnalogTag(RSViewTag other) : base(other)
        { }
    }
}