using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MPT.PrimitiveType
{
    public static class DataRowExtension
    {
        /// <summary>
        /// set the given object from the given data row
        /// </summary>
        /// <param name="item"></param>
        /// <param name="row"></param>
        public static void SetItemFromRow<T>(T item, DataRow row) where T : new()
        {
            foreach (DataColumn c in row.Table.Columns)
            {
                // find the property for the column
                var p = item.GetType().GetProperty(c.ColumnName);

                // if exists, set the value
                if (p != null && row[c] != DBNull.Value)
                {
                    p.SetValue(item, row[c], null);
                }
            }
        }

        // function that creates an object from the given data row
        public static T CreateItemFromRow<T>(DataRow row) where T : new()
        {
            // create a new object
            var item = new T();

            // set the item
            SetItemFromRow(item, row);

            // return 
            return item;
        }


        // function that creates a list of an object from the given data table
        public static List<T> CreateListFromTable<T>(DataTable tbl) where T : new()
        {
            var list = new List<T>();
            foreach (DataRow r in tbl.Rows)
            {
                list.Add(CreateItemFromRow<T>(r));
            }
            return list;
        }
    }
}
