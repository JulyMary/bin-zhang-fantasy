using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine.Services;

namespace Fantasy.BusinessEngine.Events
{
    public class BusinessAssociationDropTableHandler : ObjectWithSite, IEntityEventHandler
    {
        public void Execute(EntityEventArgs e)
        {
            BusinessAssociation assn = (BusinessAssociation)e.Entity;
            IDDLService ddl = this.Site.GetRequiredService<IDDLService>();
            ddl.DropAssociationTable(assn);
        }      
    }
}
