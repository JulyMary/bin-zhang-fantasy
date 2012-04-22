using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.ServiceModel;
using Fantasy.BusinessEngine.Services;
using System.Data;

namespace Fantasy.BusinessEngine.MSSQL
{
    public class SQLLastUpdateTimestampService : ServiceBase, ILastUpdateTimestampService
    {
        #region ILastUpdateTimestampService Members

        public long GetLastUpdateSeconds(string name)
        {

            lock (_synRoot)
            {
                IEntityService es = this.Site.GetRequiredService<IEntityService>();

                using (IDbCommand cmd = es.CreateCommand())
                {
                    cmd.CommandText = String.Format("select seconds from BUSINESSLASTUPDATETIMESTAMP where upper(name) = '{0}'", name);
                    object o = cmd.ExecuteScalar();

                    return o is DBNull ? 0L : (long)Convert.ChangeType(o, typeof(Int64));
                }
            }
            
        }

        private object _synRoot = new object();

        #endregion
    }
}
