using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine.Services;
using Fantasy.BusinessEngine.Security;

namespace Fantasy.BusinessEngine.Applications
{
    public class AdministrativeApplication : BusinessApplication
    {

        public override BusinessObjectSecurity GetObjectSecurity(BusinessObject obj)
        {
            BusinessClass @class = BusinessEngineContext.Current.GetRequiredService<IObjectModelService>().FindBusinessClass(obj.ClassId);
            return this.GetClassSecurity(@class);
        }

        public override BusinessObjectSecurity GetClassSecurity(BusinessClass @class)
        {
            IBusinessUserRoleService urs = BusinessEngineContext.Current.GetRequiredService<IBusinessUserRoleService>();
            
            if (BusinessEngineContext.Current.User.IsInRole(urs.WellknownRoles.Administrators))
            {

                return BusinessObjectSecurity.Create(@class, true, true, true, true);
            }
            else
            {
                return BusinessObjectSecurity.Create(@class, false, false, false, false);
            }


            
        }
    }
}
