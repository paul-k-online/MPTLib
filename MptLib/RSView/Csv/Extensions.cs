using System.Data.Entity.ModelConfiguration.Conventions;

namespace MPT.RSView.Csv
{
    public static class Extensions
    {
        /*
        public static string ToRS(this RSTagType value)
        {
            return value.ToString().ToRS();
        }

        public static string ToRS(this RSDataSource value)
        {
            return value.ToString().ToRS();
        }

        public static string ToRS(this RSBool value)
        {
            return value.ToString().ToRS();
        }
        */
        
        /*
        public static string ToRS(this string value)
        {
            return value == null ? "" : string.Format("\"{0}\"", value);
        }
        */
        /*
        public static string ToRS(this double? value)
        {
            return value == null ? "" : value.ToString();
        }
        */

        public static string ToRS(this object value)
        {           
            if (value == null)
                return "";
            
            if (value is string)
                //return (value as string).ToRS();
                //return value == null ? "" : string.Format("\"{0}\"", value);
                return string.Format("\"{0}\"", value);

            if (value is double)
                //return (value as double?).ToRS();
                //return value == null ? "" : value.ToString();
                return value.ToString();

            if (value is int)
                //return (value as double?).ToRS();
                //return value == null ? "" : value.ToString();
                return value.ToString();


            if (value is AlarmMessage)
                return value.ToString();

            if (value is CsvAlarmTreshold)
                return value.ToString();
            
            /*
            if (value is RSTagType)
                return ((RSTagType)value).ToRS();

            if (value is RSDataSource)
                return ((RSDataSource)value).ToRS();

            if (value is RSBool)
                return ((RSBool)value).ToRS();
            */

            //return "";
            var a = value.ToString();
            var b = a.ToRS();
            return b;
            //return value.ToString().ToRS();
        }
    }
}