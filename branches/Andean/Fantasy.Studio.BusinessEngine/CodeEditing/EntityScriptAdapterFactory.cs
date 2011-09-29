using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Adaption;
using Fantasy.BusinessEngine;

namespace Fantasy.Studio.BusinessEngine.CodeEditing
{
    public class EntityScriptAdapterFactory :  ObjectWithSite, IAdapterFactory
    {
        #region IAdapterFactory Members

        public object GetAdapter(object adaptee, Type targetType)
        {
            IEntityScript subFolder = adaptee as IEntityScript;
            if (subFolder != null)
            {
                return subFolder.Entity;
            }
            return null;
        }

        public Type[] GetTargetTypes()
        {
            return new Type[] { typeof(IBusinessEntity)};
        }

        #endregion
    }
   
}
