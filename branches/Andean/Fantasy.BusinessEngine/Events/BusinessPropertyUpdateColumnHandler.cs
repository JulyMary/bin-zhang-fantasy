using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine.Services;

namespace Fantasy.BusinessEngine.Events
{
    public class BusinessPropertyUpdateColumnHandler : ObjectWithSite, IEntityEventHandler
    {
        #region IEntityEventHandler Members

        public void Execute(EntityEventArgs e)
        {
            BusinessProperty property = (BusinessProperty)e.Entity;
            IDDLService ddl = this.Site.GetRequiredService<IDDLService>();
            ddl.UpdateClassColumn(property);
        }

        #endregion
    }
}
