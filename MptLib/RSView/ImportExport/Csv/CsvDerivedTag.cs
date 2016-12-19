using System.Collections.Generic;
using System.Linq;

namespace MPT.RSView.ImportExport.Csv
{
    public class CsvDerivedTag
    {
        //;Tag Name,            Expression,                                                 Description,    DerivedTag File Name
        //"TimeConv\trend_m",   "((trend\timerange - trend\timerange % 60) % 3600) / 60",   "минуты",       "timeconv"

        public string DerivedTagName;
        public string Expression;
        public string Description;
        public string DerivedTagFileName;
        
        public override string ToString()
        {
            var fieldList = new List<object>()
            {
                DerivedTagName,
                Expression,
                Description,
                DerivedTagFileName,
            };
            return string.Join(",", fieldList.Select(x => x.ToCsvString()));
        }
    }
}
