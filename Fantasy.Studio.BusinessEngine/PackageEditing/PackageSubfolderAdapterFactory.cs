using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Adaption;
using Fantasy.BusinessEngine;

namespace Fantasy.Studio.BusinessEngine.PackageEditing
{
    public class PackageSubfolderAdapterFactory : ObjectWithSite, IAdapterFactory
    {
        #region IAdapterFactory Members

        public object GetAdapter(object adaptee, Type targetType)
        {
            IPackageSubfolder subFolder = adaptee as IPackageSubfolder;
            if (subFolder != null)
            {
                return subFolder.Package;
            }
            return null;
        }

        public Type[] GetTargetTypes()
        {
            return new Type[] { typeof(BusinessPackage) };
        }

        #endregion
    }
}
