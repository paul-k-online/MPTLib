 // ReSharper disable once CheckNamespace
using System;

namespace MPT.Model
{
    public struct RangePair
    {
        public double? Low { get; set; }
        public double? High { get; set; }

        public RangePair(double? low, double? high) : this()
        {
            Low = low;
            High = high;
        }

        public RangePair SetLow(double? value)
        {
            Low = value;
            return this;
        }

        public RangePair SetHigh(double? value)
        {
            High = value;
            return this;
        }
        

        public override string ToString()
        {
            return string.Format("{0}, {1}", Low, High);
        }
    }
}