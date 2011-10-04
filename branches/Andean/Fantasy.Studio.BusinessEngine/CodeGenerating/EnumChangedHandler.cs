using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine.Events;
using Fantasy.BusinessEngine;

namespace Fantasy.Studio.BusinessEngine.CodeGenerating
{
    public class EnumChangedHandler : ObjectWithSite, IEntityEventHandler
    {

        #region IEntityEventHandler Members

        public void Execute(EntityEventArgs e)
        {
            BusinessEnum @enum = (BusinessEnum)e.Entity;

            IBusinessEnumCodeGenerator svc = this.Site.GetRequiredService<IBusinessEnumCodeGenerator>();
            if (!(@enum.IsSystem || @enum.IsExternal))
            {
                svc.RegisterEnum(@enum);
            }
        }

        #endregion
    }
}
