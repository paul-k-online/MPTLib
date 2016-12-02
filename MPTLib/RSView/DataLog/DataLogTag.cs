using System.Collections.Generic;

namespace MPT.RSView.DataLog
{
    public partial class DataLogTag
    {
        public DataLogTag()
        {
            DatalogFloats = new HashSet<DataLogFloat>();
        }

        public short TagIndex { get; set; }
        public string TagName { get; set; }
        public short TagType { get; set; }
        public short TagDataType { get; set; }
        
        public ICollection<DataLogFloat> DatalogFloats { get; set; }


        public static DataLogTag FromDataLogTagDBF(DataLogTagDBF dbfItem)
        {
            return new DataLogTag()
            {
                TagDataType = dbfItem.TagDataTyp,
                TagIndex = dbfItem.TTagIndex,
                TagName = dbfItem.TagName,
                TagType = dbfItem.TagType,
            };
        }
    }
}
