using System;
using System.Collections.Generic;
using System.Data;

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


        public static DataLogTag FromDBF(IDataRecord record)
        {
            // n 5
            var tagName = Convert.ToString(record["tagname"]).Trim();
            // c 255
            var ttagIndex = Convert.ToInt16(record["ttagindex"]);
            // n 1
            var tagType = Convert.ToInt16(record["tagtype"]);
            // n 2
            var tagDataTyp = Convert.ToInt16(record["tagdatatyp"]);

            return new DataLogTag()
            {
                TagName = tagName,
                TagIndex = ttagIndex,
                TagType = tagType,
                TagDataType = tagDataTyp,
            };
        }
    }
}
