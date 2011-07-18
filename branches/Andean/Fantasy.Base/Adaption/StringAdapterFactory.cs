using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.Adaption
{
    public class StringAdapterFactory : IAdapterFactory
    {
       
        #region IAdapterFactory Members

        public object GetAdapter(object adaptee, Type targetType)
        {
            return adaptee != null ? adaptee.ToString() : null;
        }

        public Type[] GetTargetTypes()
        {
            return new Type[] { typeof(string) };
        }

        #endregion
    }
}
