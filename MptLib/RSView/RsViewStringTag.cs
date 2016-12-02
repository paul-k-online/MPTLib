
namespace MPT.RSView
{
    public class RsViewStringTag
        : RsViewTag
    {
        public string InitialValue;
        public ushort Length = 200;

        public RsViewStringTag(string name, string folder="") 
            : base(name, folder)
        {}
    }
}