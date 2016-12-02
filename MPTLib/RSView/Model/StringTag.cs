
namespace MPT.RSView
{
    public class RsViewStringTag
        : RSViewTag
    {
        public string InitialValue;
        public ushort Length = 200;

        public RsViewStringTag(string name, string folder="") 
            : base(name, folder)
        {}
    }
}