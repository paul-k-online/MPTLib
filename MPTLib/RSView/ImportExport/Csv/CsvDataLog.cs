using System.Collections.Generic;
using System.Linq;

namespace MPT.RSView.ImportExport.Csv
{
    public class CsvDataLog
    {
        public string TagName { get; set; }
        public string ModelName { get; set; }

        public string ToCsvString()
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
