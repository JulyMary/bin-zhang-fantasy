using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Adaption;
using Fantasy.BusinessEngine;
using System.ComponentModel;

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
                if(targetType == typeof(IEntityScript))
                {
                    return new BusinessScriptAdapter(entity);
                }
                else if (targetType == typeof(ICustomTypeDescriptor))
                {
                    return this.Site.GetRequiredService<IAdapterManager>().GetAdapter(new BusinessScriptAdapter(entity), typeof(ICustomTypeDescriptor)); 
                }
            }
            return null;
        }

        public Type[] GetTargetTypes()
        {
            return new Type[] { typeof(IEntityScript), typeof(ICustomTypeDescriptor) };
        }

        #endregion
    }
}
