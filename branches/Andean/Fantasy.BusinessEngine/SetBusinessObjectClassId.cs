using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine.Events;
using Fantasy.BusinessEngine.Services;

namespace Fantasy.BusinessEngine
{
    public class SetBusinessObjectClassId : ObjectWithSite, IEntityEventHandler
    {
        #region IEntityEventHandler Members

        public void Execute(EntityEventArgs e)
        {
            BusinessObject obj = (BusinessObject)e.Entity;
            IObjectModelService mc = this.Site.GetRequiredService<IObjectModelService>();
            BusinessClass @class = mc.FindBusinessClassForType(obj.GetType());
            obj.ClassId = @class.Id;
        }

        #endregion
    }
}
