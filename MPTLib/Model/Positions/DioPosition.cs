using System;
using System.Data;

// ReSharper disable once CheckNamespace
namespace MPT.Model
{
    public class DioPosition : Position
    {
        public bool NormValue { get; set; }
        public bool IsAlarm { get; set; }
        public string AlarmText { get; set; }
    }
}