using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine;

namespace Fantasy.Studio.BusinessEngine
{
    public class ReadOnlyBusinessClassFactory : ReadOnlyAdapterFactory
    {

        public override object GetAdapter(object adaptee, Type targetType)
        {
            return new ReadOnlyBusinessClass((BusinessClass)adaptee);
        }
    }
}
