using System;
using System.Collections.Generic;


namespace MPT.Model
{
    public abstract class Position
    {
        public int? PlcId { get; set; }
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
            get
            {
                const string fullNameTemplate = "{0} - {1}";
                return string.Format(fullNameTemplate, Name, Description);
            }
        }

        public override string ToString()
        {
            //return string.Format("{0}: {1}", Number, Name);
            return Name;
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

    }

    public struct RangePair
    {
        public double? Low { get; set; }
        public double? High { get; set; }

        public override string ToString()
        {
            return string.Format("{0}; {1}", Low, High);
        }
    }
    

    public class AiPosition : Position
    {
        public string Units { get; set; }
        public RangePair Scale { get; set; }
        public RangePair Reglament { get; set; }
        public RangePair Alarming { get; set; }
        public RangePair Blocking { get; set; }
    }

    public class AoPosition : Position
    {
        public enum AoTypeEnum
        {
            // ReSharper disable once InconsistentNaming
            SPPV,
            // ReSharper disable once InconsistentNaming
            PVSP
        }

        public RangePair Scale { get; set; }

        public int? AiNum { get; set; }
        public AiPosition AiPosition { get; set; }

        public AoTypeEnum AoType { get; set; }
        public bool IsCascade { get; set; }
        public bool IsCascadeSlave { get; set; }
        public uint? CascadeMasterNumber { get; set; }

    }

    public class DioPosition : Position
    {
        public bool NormValue { get; set; }
        public bool IsAlarm { get; set; }
        public string AlarmText { get; set; }
    }
}