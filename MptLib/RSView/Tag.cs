using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.IO;
using System.Linq;
using System.Text;

namespace MPT.RSView
{
    public class Tag
    {
        public string Folder;
        public string Name;
        public string FullName
        {
            get { return Path.Combine(Folder, Name); }
        }
        public string Desctiption;

        public bool IsMemorySourceData
        {
            get { return string.IsNullOrWhiteSpace(NodeName); }
        }

        public string NodeName;
        public string Address;
    }

    public class AnalogTag : Tag
    {
        public class AnalogAlarm
        {
            public double Threshold;
            [MaxLength(21)]
            public string Label;
            public ushort Severity = 1;
            public RSTresholdDirection Direction;
        }

        public double Min;
        public double Max;
        public double InitialValue;
        public string Units = "";

        public bool IsAlarm
        {
            get { return Alarm.Any(x => x.Value != null); }
        }

        public Dictionary<int, AnalogAlarm> Alarm = new Dictionary<int, AnalogAlarm>
                                                    {
                                                         {1,null},
                                                         {2,null},
                                                         {3,null},
                                                         {4,null},
                                                         {5,null},
                                                         {6,null},
                                                         {7,null},
                                                         {8,null},
                                                     };
        public AnalogAlarm Alarm1
        {
            get { return Alarm[1]; }
            set { Alarm[1] = value; }
        }
        public AnalogAlarm Alarm2
        {
            get { return Alarm[2]; }
            set { Alarm[2] = value; }
        }
        public AnalogAlarm Alarm3
        {
            get { return Alarm[3]; }
            set { Alarm[3] = value; }
        }
        public AnalogAlarm Alarm4
        {
            get { return Alarm[4]; }
            set { Alarm[4] = value; }
        }
        public AnalogAlarm Alarm5
        {
            get { return Alarm[5]; }
            set { Alarm[5] = value; }
        }
        public AnalogAlarm Alarm6
        {
            get { return Alarm[6]; }
            set { Alarm[6] = value; }
        }
        public AnalogAlarm Alarm7
        {
            get { return Alarm[7]; }
            set { Alarm[7] = value; }
        }
        public AnalogAlarm Alarm8
        {
            get { return Alarm[8]; }
            set { Alarm[8] = value; }
        }


        public static Dictionary<int, AnalogAlarm> CreateDefaultAlarms()
        {
            return new Dictionary<int, AnalogAlarm>
                                                    {
                                                         {1,new AnalogAlarm()
                                                            {
                                                                Threshold = 0.9,
                                                                Label = "(LL)",
                                                                Severity = 1,
                                                                Direction = RSTresholdDirection.D
                                                            }},
                                                         {2,new AnalogAlarm()
                                                            {
                                                                Threshold = 1.9,
                                                                Label = "(L)",
                                                                Severity = 2,
                                                                Direction = RSTresholdDirection.D
                                                            }},                                  
                                                         {3,new AnalogAlarm()
                                                            {
                                                                Threshold = 2.9,
                                                                Label = "(RL)",
                                                                Severity = 3,
                                                                Direction = RSTresholdDirection.D
                                                            }},
                                                        {4,new AnalogAlarm()
                                                            {
                                                                Threshold = 4.9,
                                                                Label = "(RH)",
                                                                Severity = 3,
                                                                Direction = RSTresholdDirection.I
                                                            }},


                                                        {5,new AnalogAlarm()
                                                            {
                                                                Threshold = 5.9,
                                                                Label = "(H)",
                                                                Severity = 2,
                                                                Direction = RSTresholdDirection.I
                                                            }},

                                                        {6,new AnalogAlarm()
                                                            {
                                                                Threshold = 6.9,
                                                                Label = "(HH)",
                                                                Severity = 1,
                                                                Direction = RSTresholdDirection.I
                                                            }},
                                                    };
        }
    }

    public class DigitalTag : Tag
    {
        public class DigitalAlarm
        {
            public enum AlarmType
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
            
            public AlarmType Type = AlarmType.ON;
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



    public class StringTag : Tag
    {
        public string InitialValue = "";
        public ushort Length = 200;
    }
}
