using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.BusinessEngine.Services
{
    public interface ISecurityService
    {
        BusinessObjectSecurity GetObjectSecurity(BusinessObject obj);

        BusinessObjectSecurity GetClassSecurity(BusinessClass @class);
    }
}
