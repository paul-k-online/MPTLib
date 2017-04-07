namespace MPT.Model
{
    public class AoPosition : Position
    {
        public enum AoTypeEnum
        {
            SPPV,
            PVSP
        }

        public RangePair Scale { get; set; }

        public int? AiNum { get; set; }
        public AiPosition AiPosition { get; set; }

        public AoTypeEnum AoType { get; set; }
        public bool IsCascade { get; set; }
        public bool IsCascadeSlave { get; set; }
        public uint? CascadeMasterNumber { get; set; }
        public string Units { get; set; }
    }
}