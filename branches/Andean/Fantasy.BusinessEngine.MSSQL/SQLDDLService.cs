using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.ServiceModel;
using Fantasy.BusinessEngine.Services;
using System.Data;

namespace Fantasy.BusinessEngine.MSSQL
{
    public class SqlDDLService : ServiceBase, IDDLService
    {
        public override void InitializeService()
        {
            LoadDataTypes();
            base.InitializeService();
        }

        private void LoadDataTypes()
        {
            IEntityService es = this.Site.GetRequiredService<IEntityService>();
            using (IDbCommand cmd = es.DefaultSession.Connection.CreateCommand())
            {
                cmd.CommandText = "select name from sys.types order by name";
                List<string> types = new List<string>();
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        types.Add(reader.GetString(0));
                    }
                }
                this.DataTypes = types.ToArray();

            }
        }

        public string[] DataTypes { get; private set; }
        
    }
}
