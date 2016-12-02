using System;
using System.Collections.Generic;


namespace MPT.Model
{
    public class Position
    {
        private const string FullNameTemplate = "{0} - {1}";
        
        public int PlcId { get; set; }

        public int Number { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Note { get; set; }

        public int? GroupId { get; set; }

        
        /// <summary>
        /// полное название
        /// </summary>
        public string FullName
        {
            get { return string.Format(FullNameTemplate, Name, Description); }
        }

        /// <summary>
        /// сравнивать только по индексу
        /// </summary>
        public class ByIndexEqualityComparer : IEqualityComparer<Position>
        {
            public bool Equals(Position x, Position y)
            {
                return x.PlcId == y.PlcId && x.Number == y.Number ;
            }
            public int GetHashCode(Position x)
            {
                return (x.PlcId ^ x.Number).GetHashCode();
            }
        }

        public virtual void CopyFrom(Position pos)
        {
            PlcId = pos.PlcId;
            Number = pos.Number;
            Name = pos.Name;
            Description = pos.Description;
            GroupId = pos.GroupId;
        }
        
        public override string ToString()
        {
            return Name;
        }
    }

    public class AiPosition : Position
    {
        public class AlarmPair
        {
            public double? Low { get; set; }
            public double? High { get; set; }

            public AlarmPair(double? low = null, double? high = null)
            {
                Low = low;
                High = high;
            }

            public override string ToString()
            {
                return String.Format("{0}; {1}", Low, High);
            }
        }

        public string Units { get; set; }

        public AlarmPair Scale { get; set; }

        public AlarmPair Reglament { get; set; }
        
        public AlarmPair Alarming { get; set; }

        public AlarmPair Blocking { get; set; }

        public AiPosition()
        {
            Scale = new AlarmPair();
            Reglament = new AlarmPair();
            Alarming = new AlarmPair();
            Blocking = new AlarmPair();
        }



        /*
        public void CopyFrom(AiPosition pos)
        {
            base.CopyFrom(pos);
            Scale = new AlarmPair(pos.Scale.Low, pos.Scale.High);
            Units = pos.Units;
            Reglament = new AlarmPair(pos.Reglament.Low, pos.Reglament.High);
            Alarming = new AlarmPair(pos.Alarming.Low, pos.Alarming.High);
            Blocking = new AlarmPair(pos.Blocking.Low, pos.Blocking.High);
        }
        */
    }


    public class AoPosition : Position
    {        
    }

    public class DioPosition : Position
    {
        public bool NormValue { get; set; }

        public bool IsAlarm { get; set; }

        public string AlarmText { get; set; }
    }
}
