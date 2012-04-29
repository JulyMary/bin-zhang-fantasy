using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine.Services;

namespace Fantasy.BusinessEngine.Events
{
    public class BusinessMenuItemCreateOrderHander : ObjectWithSite, IEntityEventHandler
    {
        #region IEntityEventHandler Members

        public void Execute(EntityEventArgs e)
        {
            BusinessMenuItem mi = (BusinessMenuItem)e.Entity;

            ISequenceService seq = this.Site.GetRequiredService<ISequenceService>();

            mi.DisplayOrder = seq.LongNext(WellknownSequences.MetaModel);
           
        }

        #endregion
    }
}
