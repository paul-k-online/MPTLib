using System.ComponentModel;

namespace MPT.RSView
{
    public class RSViewStringTag : RSViewTag
    {
        public string InitialValue;

        [DefaultValue(200)]
        public ushort Length { get; set; }

        public RSViewStringTag(string name, string folder="") : base(name, folder)
        { }

        public RSViewStringTag(RSViewTag other) : base(other)
        { }
    }
}