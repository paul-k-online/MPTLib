 // ReSharper disable once CheckNamespace
namespace MPT.Model
{
    public class AiPosition : Position
    {
        public string Units { get; set; }
        public RangePair Scale { get; set; }
        public RangePair Reglament { get; set; }
        public RangePair Alarming { get; set; }
        public RangePair Blocking { get; set; }
    }
}