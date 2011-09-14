using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Adaption;
using Fantasy.BusinessEngine;
using System.ComponentModel;

namespace Fantasy.Studio.BusinessEngine.ClassDiagramEditing.Model
{
    public class PropertyNodeAdapterFactory : ObjectWithSite, IAdapterFactory
    {
        #region IAdapterFactory Members

        public object GetAdapter(object adaptee, Type targetType)
        {
            if (adaptee is PropertyNode)
            {
                PropertyNode node = (PropertyNode)adaptee;

                if (targetType == typeof(BusinessProperty))
                {
                    return node.Entity;
                }
                else if (targetType == typeof(ICustomTypeDescriptor))
                {
                    return this.Site.GetRequiredService<IAdapterManager>().GetAdapter<ICustomTypeDescriptor>(node.Entity);
                }
            }

            return null;
        }

        public Type[] GetTargetTypes()
        {
            return new Type[] { typeof(BusinessProperty), typeof(ICustomTypeDescriptor) };
        }

        #endregion
    }
}
