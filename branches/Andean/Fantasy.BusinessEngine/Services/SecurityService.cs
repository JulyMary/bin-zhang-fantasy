using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.ServiceModel;

namespace Fantasy.BusinessEngine.Services
{
    public class SecurityService : ServiceBase, ISecurityService
    {
        #region ISecurityService Members

        public BusinessObjectSecurity GetObjectSecurity(BusinessObject obj)
        {
            throw new NotImplementedException();
        }

        public BusinessObjectSecurity GetClassSecurity(BusinessClass @class)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
