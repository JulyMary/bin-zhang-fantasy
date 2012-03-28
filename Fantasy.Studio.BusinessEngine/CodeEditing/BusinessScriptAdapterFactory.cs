using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Adaption;
using Fantasy.BusinessEngine;

namespace Fantasy.Studio.BusinessEngine.CodeEditing
{
    public class BusinessScriptAdapterFactory : ObjectWithSite, IAdapterFactory
    {
        #region IAdapterFactory Members

        public object GetAdapter(object adaptee, Type targetType)
        {
            BusinessScript entity = adaptee as BusinessScript;
            if (entity != null)
            {
                return new BusinessScriptAdapter(entity);
            }
            return null;
        }

        public Type[] GetTargetTypes()
        {
            return new Type[] { typeof(IEntityScript) };
        }

        #endregion
    }
}
