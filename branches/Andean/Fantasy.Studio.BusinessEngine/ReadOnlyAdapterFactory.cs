using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Adaption;
using Fantasy.Studio.Descriptor;
using Fantasy.BusinessEngine;

namespace Fantasy.Studio.BusinessEngine
{
    public abstract class ReadOnlyAdapterFactory : ObjectWithSite, IAdapterFactory
    {
        #region IAdapterFactory Members

        public abstract object GetAdapter(object adaptee, Type targetType);

        public Type[] GetTargetTypes()
        {
            return new Type[] { typeof(IReadOnlyAdapter) };
        }

        #endregion
    }
}
