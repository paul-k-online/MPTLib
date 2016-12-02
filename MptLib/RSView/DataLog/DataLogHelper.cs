using System;
using System.Collections.Generic;
using System.Data;
using MPT.Model;

namespace MPT.RSView.DataLog
{
    public struct RsAlmarmRecord
    {
        // Date (8)
        public DateTime DateTime { get; set; }
        // c 8
        public string Time { get; set; }
        // n 5
        public short Militime { get; set; }
        // c 5
        public string TransType { get; set; }
        // c 255
        public string TagName { get; set; }
        // float
        public double TagValue { get; set; }
        // c 1
        public string TagType { get; set; }
        //float
        public double ThreshVal { get; set; }
        // n 1
        public short ThreshNum { get; set; }
        // c 20
        public string ThreshLabl { get; set; }
        // n 1
        public short Severity { get; set; }
        // c1
        public string DstFlag { get; set; }
        // c20
        public string UserId { get; set; }
        // n1
        public short AlarmType { get; set; }
        // c254
        public string Dscription { get; set; }
        // c15
        public string UserStn { get; set; }
        // c15
        public string LoggingStn { get; set; }



    }

    public static class DatalogHelper
    {
        private static DataLogTag ReadDatalogTag(this IDataRecord record)
        {
            var item = new DataLogTag();
            // c255
            item.TagName = Convert.ToString(record["Tagname"]).Trim();
            // n5
            item.TagIndex = Convert.ToInt16(record["TTagIndex"]);
            // n1
            item.TagType = Convert.ToInt16(record["TagType"]);
            // n2
            item.TagDataType = Convert.ToInt16(record["TagDataTyp"]);
            return item;
        }

        public static List<DataLogTag> GetDatalogTagList(this IDataReader reader)
        {
            var list = new List<DataLogTag>();
            while (reader.Read())
            {
                list.Add(reader.ReadDatalogTag());
            }
            return list;
        }

        private static DataLogFloat ReadDatalogFloat(this IDataRecord reader)
        {
            var item = new DataLogFloat();

            // Date(8)
            var dtS = reader["Date"];
            var dt = Convert.ToDateTime(dtS);
            // c8
            var tsO = reader["Time"];
            var tsS = Convert.ToString(tsO);
            var ts = TimeSpan.Parse(tsS);
            // n3
            var ms = Convert.ToInt16(reader["Millitm"]);

            item.DateTime = new DateTime(dt.Year, dt.Month, dt.Day, ts.Hours, ts.Minutes, ts.Seconds, ms);
            item.Msec = ms;

            // n5
            item.TagIndex = Convert.ToInt16(reader["TagIndex"]);
            // float 17,8
            item.Value = Convert.ToDouble(reader["Value"]);
            // c1
            item.Status = Convert.ToString(reader["Status"]).Trim();
            // c1
            item.Marker = Convert.ToString(reader["Marker"]).Trim();
            // c4
            //Internal = Convert.ToString(reader["Internal"]).Trim(),

            return item;
        }

        public static List<DataLogFloat> GetDataLogFloatList(this IDataReader reader, int projectId = 0)
        {
            var list = new List<DataLogFloat>();
            while (reader.Read())
            {
                var item = reader.ReadDatalogFloat();
                item.ProjectId = projectId;
                list.Add(item);
            }
            return list;
        }

        /*
        public static IEnumerable<T> Select<T>(this IDataReader reader, Func<IDataReader, T> projection)
        {
            while (reader.Read())
            {
                yield return projection(reader);
            }
        }
        */

        /*
        public static Customer FromDataReader(IDataReader reader)
        {
            using (IDataReader reader)
            {
                List<Customer> customers = reader.Select(r => new Customer {
                    CustomerId = r["id"] is DBNull ? null : r["id"].ToString(),
                    CustomerName = r["name"] is DBNull ? null : r["name"].ToString() 
                }).ToList();
            }
        }
         * */
    }
}