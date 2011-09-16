using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine.Services;

namespace Fantasy.BusinessEngine.Events
{
    public class BusinessAssociationCreateTableHandler : ObjectWithSite, IEntityEventHandler
    {
        #region IEntityEventHandler Members

        public void Execute(EntityEventArgs e)
        {
            BusinessAssociation assn = (BusinessAssociation)e.Entity;
            IDDLService ddl = this.Site.GetRequiredService<IDDLService>();
            ddl.CreateAssoicationTable(assn);
        }

        #endregion
    }
}
