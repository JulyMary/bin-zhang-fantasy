using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace Fantasy.Jobs.Demo
{
    static class Db
    {
        public static object UsingCommand(string connectionString, Func<SqlCommand, object> callback)
        {
            using (SqlConnection cnnt = new SqlConnection(connectionString))
            {
                cnnt.Open();
                using (SqlCommand cmd = cnnt.CreateCommand())
                {
                    return callback(cmd);
                }
            }

        }
    }
}
