using System.Collections.Generic;
using System.Linq;

namespace MPT.RSView.Csv
{
    public class DataLog
    {
        public string TagName;
        public string ModelName;

        public override string ToString()
        {
            var fieldList =  new List<object>()
                       {
                           TagName,
                           ModelName
                       };
            return string.Join(",", fieldList.Select(x => x.ToRS()));
        }

        
        public static IEnumerable<string> GetRSViewCsvDataLogFile(IEnumerable<DataLog> dataLogList)
        {
            const string header =
                ";###001 - THIS LINE CONTAINS VERSION INFORMATION. DO NOT REMOVE!!!" + 
                "\n" + 
                "\n;" +
                "Tag Name, Model Name" + 
                "\n";

            var list = new List<string>()
                       {
                           header
                       };
            list.AddRange(dataLogList.Select(dataLog => dataLog.ToString()));

            return list;
        }
    }
}
