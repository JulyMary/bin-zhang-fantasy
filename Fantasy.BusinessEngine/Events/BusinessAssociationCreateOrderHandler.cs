using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine.Services;

namespace Fantasy.BusinessEngine.Events
{
    public class BusinessAssociationCreateOrderHandler : ObjectWithSite, IEntityEventHandler
    {
        #region IEntityEventHandler Members

        public void Execute(EntityEventArgs e)
        {
            BusinessAssociation ass = (BusinessAssociation)e.Entity;

            ISequenceService seq = this.Site.GetRequiredService<ISequenceService>();

            ass.LeftRoleDisplayOrder = seq.LongNext(WellknownSequences.MetaModel);
            ass.RightRoleDisplayOrder = seq.LongNext(WellknownSequences.MetaModel);
        }

        #endregion
    }
}
