using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MPT.RSView
{
    public enum RsViewTresholdDirection
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
    
    public class RsViewAnalogTag : RSViewTag
    {
        public class RsViewAnalogAlarm
        {
            public double Threshold;
            [MaxLength(21)]
            public string Label;
            public ushort Severity = 1;
            public RsViewTresholdDirection Direction;
        }

        public double Min;
        public double Max;
        public double InitialValue;
        public string Units = "";

        public Dictionary<int, RsViewAnalogAlarm> Alarm = new Dictionary<int, RsViewAnalogAlarm>(8)
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

        public bool IsAlarm
        {
            get { return Alarm.Any(x => x.Value != null); }
        }
        
        public RsViewAnalogAlarm Alarm1
        {
            get { return Alarm[1]; }
            set { Alarm[1] = value; }
        }
        public RsViewAnalogAlarm Alarm2
        {
            get { return Alarm[2]; }
            set { Alarm[2] = value; }
        }
        public RsViewAnalogAlarm Alarm3
        {
            get { return Alarm[3]; }
            set { Alarm[3] = value; }
        }
        public RsViewAnalogAlarm Alarm4
        {
            get { return Alarm[4]; }
            set { Alarm[4] = value; }
        }
        public RsViewAnalogAlarm Alarm5
        {
            get { return Alarm[5]; }
            set { Alarm[5] = value; }
        }
        public RsViewAnalogAlarm Alarm6
        {
            get { return Alarm[6]; }
            set { Alarm[6] = value; }
        }
        public RsViewAnalogAlarm Alarm7
        {
            get { return Alarm[7]; }
            set { Alarm[7] = value; }
        }
        public RsViewAnalogAlarm Alarm8
        {
            get { return Alarm[8]; }
            set { Alarm[8] = value; }
        }
        
        public RsViewAnalogTag(string name ="", string folder="") : base(name, folder)
        {}
    }
}