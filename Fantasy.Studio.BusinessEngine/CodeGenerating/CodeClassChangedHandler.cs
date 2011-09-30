using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine.Events;
using Fantasy.BusinessEngine;

namespace Fantasy.Studio.BusinessEngine.CodeGenerating
{
    public class CodeClassChangedHandler : ObjectWithSite, IEntityEventHandler
    {

        #region IEntityEventHandler Members

        public void Execute(EntityEventArgs e)
        {
            BusinessClass @class = (BusinessClass)e.Entity;

            IBusinessClassT4Service svc = this.Site.GetRequiredService<IBusinessClassT4Service>();
            if (!@class.IsSystem)
            {
                svc.RegisterClass(@class);
            }
        }

        #endregion
    }
}
