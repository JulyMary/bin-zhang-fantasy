using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Data
{
    public static class DbCommandExtension
    {
        public static DataTable ExecuteDataTable(this IDbCommand command)
        {
            DataTable rs = new DataTable();

            using (IDataReader reader = command.ExecuteReader())
            {
                rs.Load(reader);
            }

            return rs;
        }

        public static List<T> ExecuteList<T>(this IDbCommand command, T defaultValue = default(T) )
        {
            List<T> rs = new List<T>();
            using (IDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    object value = reader.GetValue(0);
                    if (value is DBNull)
                    {
                        value = defaultValue;
                    }
                    else
                    {
                        value = Convert.ChangeType(value, typeof(T));
                    }

                    rs.Add((T)value);
                }
            }

            return rs;

        }
    }
}
