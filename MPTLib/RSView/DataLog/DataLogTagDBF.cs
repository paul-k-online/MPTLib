using System.Collections.Generic;
using System;
using System.Data;

namespace MPT.RSView.DataLog
{
    public partial class DataLogTagDBF
    {
        // n5
        public short TTagIndex { get; set; }
        // c255
        public string TagName { get; set; }
        // n1
        public short TagType { get; set; }
        // n2
        public short TagDataTyp { get; set; }


        public static DataLogTagDBF FromReader(IDataRecord record)
        {
            var item = new DataLogTagDBF()
            {
                TagName = Convert.ToString(record["tagname"]).Trim(),
                TTagIndex = Convert.ToInt16(record["ttagindex"]),
                TagType = Convert.ToInt16(record["tagtype"]),
                TagDataTyp = Convert.ToInt16(record["tagdatatyp"]),
            };
            return item;
        }
    }
}
