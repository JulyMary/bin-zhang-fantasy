using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine.Security;
using Fantasy.BusinessEngine.Services;

namespace Fantasy.BusinessEngine.Applications
{
    public class VoidApplication : BusinessApplication
    {
        public sealed override BusinessObject EntryObject
        {
            get
            {
                return null;
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        public static readonly Guid VoidApplicationId = new Guid("fcf49a56-7c78-4275-984e-36db16bb889a");

      
        public sealed override Security.BusinessObjectSecurity GetClassSecurity(BusinessClass @class)
        {
            return BusinessObjectSecurity.Create(@class, false, false, false, false);
        }

        public sealed override Security.BusinessObjectSecurity GetObjectSecurity(BusinessObject obj)
        {
            IObjectModelService oms = BusinessEngineContext.Current.GetRequiredService<IObjectModelService>();
            BusinessClass @class = oms.FindBusinessClass(obj.ClassId);
            return this.GetClassSecurity(@class);
        }
    }
}
