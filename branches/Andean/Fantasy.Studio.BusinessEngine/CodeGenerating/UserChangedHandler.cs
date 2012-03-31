using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine.Events;
using Fantasy.BusinessEngine;

namespace Fantasy.Studio.BusinessEngine.CodeGenerating
{
    public class UserChangedHandler : ObjectWithSite, IEntityPropertyChangedEventHandler
    {
        #region IEntityPropertyChangedEventHandler Members

        public void Execute(Fantasy.BusinessEngine.EntityPropertyChangedEventArgs e)
        {
            if (e.PropertyName == "CodeName")
            {
                IBusinessUserCodeGenerator gen = this.Site.GetRequiredService<IBusinessUserCodeGenerator>();
                gen.Rename((BusinessUserData)e.Entity);
            }
        }

        #endregion
    }
}
