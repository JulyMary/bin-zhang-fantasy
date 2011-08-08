using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine;

namespace Fantasy.Studio.BusinessEngine.PackageEditing
{
    public class ReadOnlyBusinessPackageFactory : ReadOnlyAdapterFactory
    {
        public override object GetAdapter(object adaptee, Type targetType)
        {
            return new ReadOnlyBusinessPackage((BusinessPackage)adaptee); 
        }
    }
}
