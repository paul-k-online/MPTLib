using System.Collections.Generic;
using System.Linq;

namespace MPT.RSView.ImportExport.Csv
{
    public class CsvDataLog
    {
        public string TagName;
        public string ModelName;

        public override string ToString()
        {
            var fieldList = new List<object>()
            {
                TagName,
                ModelName
            };
            return string.Join(",", fieldList.Select(x => x.ToCsvString()));
        }
    }
}
