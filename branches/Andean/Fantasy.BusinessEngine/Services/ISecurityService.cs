using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine.Security;

namespace Fantasy.BusinessEngine.Services
{
    public interface ISecurityService
    {
        BusinessObjectSecurity GetObjectSecurity(ObjectSecurityArgs args);

        BusinessObjectSecurity GetClassSecurity(ClassSecurityArgs args);
    }
}
