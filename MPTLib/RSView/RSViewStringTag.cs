namespace MPT.RSView
{
    public class RSViewStringTag : RSViewTag
    {
        public string InitialValue;
        public ushort Length = 200;

        public RSViewStringTag(string name, string folder="") 
            : base(name, folder)
        {}
    }
}