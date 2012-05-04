using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Connection;
using Fantasy.ServiceModel;


namespace Fantasy.BusinessEngine.Services
{
    public class NHConnectionProvider : UserSuppliedConnectionProvider
    {

        private IConnectionFactory _factory;

        #region IConnectionProvider Members

        public override void CloseConnection(System.Data.IDbConnection conn)
        {
            _factory.CloseConnection(conn);
        }

        public override System.Data.IDbConnection GetConnection()
        {
            if (_factory == null)
            {
                _factory = ServiceManager.Services.GetRequiredService<IConnectionFactory>();
            }
            return _factory.GetConnection();

           
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            
        }

        #endregion
    }
}
