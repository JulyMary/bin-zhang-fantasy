using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine.Events;
using Fantasy.BusinessEngine;

namespace Fantasy.Studio.BusinessEngine.CodeGenerating
{
    public class ClassChangedHandler : ObjectWithSite, IEntityEventHandler
    {

        #region IEntityEventHandler Members

        public void Execute(EntityEventArgs e)
        {
            BusinessClass @class = (BusinessClass)e.Entity;

            IBusinessClassCodeGenerator svc = this.Site.GetRequiredService<IBusinessClassCodeGenerator>();
            if (@class.ScriptOptions == ScriptOptions.Default)
            {
                svc.RegisterClass(@class);
            }
        }

        #endregion
    }
}
