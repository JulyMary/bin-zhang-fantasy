using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine.Services;
using Fantasy.ServiceModel;
using System.Data.SqlClient;
using System.Data;

namespace Fantasy.BusinessEngine.MSSQL
{
    public class SqlSequenceService : ServiceBase, ISequenceService
    {

        public override void InitializeService()
        {
            this._connectionFactory = this.Site.GetRequiredService<IConnectionFactory>();
            base.InitializeService();
        }

        private IConnectionFactory _connectionFactory;

        #region ISequenceService Members

        public int Next(string name)
        {
            return (int)LongNext(name);
        }

        public long LongNext(string name)
        {
            string sql = string.Format("SELECT next value for {0}", name);

            return ExecuteCommand(sql);
        }

        private long ExecuteCommand(string sql)
        {

            long rs;
            IDbConnection conn = this._connectionFactory.GetConnection();
            try
            {
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    rs = (long)cmd.ExecuteScalar();
                }
            }
            finally
            {
                this._connectionFactory.CloseConnection(conn);
            }

            return rs;
           
        }

        public int Current(string name)
        {
            return (int)LongCurrent(name);
        }

        public long LongCurrent(string name)
        {
            string sql = string.Format("select current_value from sys.sequences where name = '{0}'", name);
            return ExecuteCommand(sql);
        }

        #endregion
    }
}
