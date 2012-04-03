using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine.Events;
using Fantasy.BusinessEngine;

namespace Fantasy.Studio.BusinessEngine.CodeGenerating
{
    public class ApplicationChangedHandler : ObjectWithSite, IEntityPropertyChangedEventHandler
    {
        #region IEntityPropertyChangedEventHandler Members

        public void Execute(Fantasy.BusinessEngine.EntityPropertyChangedEventArgs e)
        {
            if (e.PropertyName == "CodeName")
            {
                BusinessApplicationData app = (BusinessApplicationData)e.Entity;
                if (app.ScriptOptions == ScriptOptions.Default)
                {
                    IApplicationCodeGenerator gen = this.Site.GetRequiredService<IApplicationCodeGenerator>();
                    gen.Rename(app);
                }
            }
        }

        #endregion
    }
}
