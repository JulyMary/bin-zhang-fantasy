using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine.Services;

namespace Fantasy.BusinessEngine.Events
{
    public class BusinessPropertyCreateOrderHandler : ObjectWithSite, IEntityEventHandler
    {


        #region IEntityEventHandler Members

        public void Execute(EntityEventArgs e)
        {
            BusinessProperty property = (BusinessProperty)e.Entity;

            ISequenceService seq = this.Site.GetRequiredService<ISequenceService>();

            property.Order = seq.LongNext(WellknownSequences.MetaModel);
        }

        #endregion
    }
}
