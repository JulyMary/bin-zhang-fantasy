using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine.Services;

namespace Fantasy.BusinessEngine.Events
{
    public class DefaultBusinessObjectCreateHandler : ObjectWithSite, IEntityEventHandler
    {
        #region IEntityEventHandler Members

        public void Execute(EntityEventArgs e)
        {
            BusinessObject obj = (BusinessObject)e.Entity;
            IObjectModelService oms = this.Site.GetRequiredService<IObjectModelService>();
            BusinessClass @class = oms.FindBusinessClassForType(obj.GetType());
            obj.ClassId = @class.Id;

           
        }

        #endregion
    }
}
