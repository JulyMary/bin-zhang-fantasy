using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine.Events;
using Fantasy.BusinessEngine;

namespace Fantasy.Studio.BusinessEngine.CodeGenerating
{
    public class RoleChangedHandler : ObjectWithSite, IEntityPropertyChangedEventHandler
    {
        #region IEntityPropertyChangedEventHandler Members

        public void Execute(Fantasy.BusinessEngine.EntityPropertyChangedEventArgs e)
        {
            if (e.PropertyName == "CodeName")
            {
                BusinessRoleData role = (BusinessRoleData)e.Entity as BusinessRoleData;
                if (role.ScriptOptions == ScriptOptions.Default)
                {
                    IBusinessRoleCodeGenerator gen = this.Site.GetRequiredService<IBusinessRoleCodeGenerator>();
                    gen.Rename(role);
                }
            }
        }

        #endregion
    }
}
