using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine.Services;

namespace Fantasy.BusinessEngine.Events
{
    public class BusinessEnumValueCreateOrderHandler : ObjectWithSite, IEntityEventHandler
    {



        public void Execute(EntityEventArgs e)
        {
            BusinessEnumValue ev = (BusinessEnumValue)e.Entity;

            ISequenceService seq = this.Site.GetRequiredService<ISequenceService>();

            ev.DisplayOrder = seq.LongNext(WellknownSequences.MetaModel);
        }


    }
}
