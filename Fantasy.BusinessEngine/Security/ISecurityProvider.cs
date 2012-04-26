using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.BusinessEngine.Security
{
    public interface ISecurityProvider
    {
        BusinessObjectSecurity GetObjectSecurity(ObjectSecurityArgs args);

        BusinessObjectSecurity GetClassSecurity(ClassSecurityArgs args);
    }
}
