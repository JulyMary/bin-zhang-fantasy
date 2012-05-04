using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.IO;
using System.Data.SqlClient;
using Fantasy.BusinessEngine.Services;

namespace Fantasy.BusinessEngine.MSSQL
{
    public class DebugConnectionFactory : IConnectionFactory
    {

        private string _connectionString = "Data Source=.\\SQLEXPRESS;Database=Fantasy;Initial Catalog=Fantasy;Integrated Security=True;";

        #region IConnectionFactory Members

        public System.Data.IDbConnection GetConnection()
        {
            SqlConnection rs = new SqlConnection(this._connectionString);
            rs.Open();
            return rs;
        }

        public void CloseConnection(System.Data.IDbConnection connection)
        {
            connection.Dispose();
        }

        #endregion
    }
}
