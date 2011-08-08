using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Studio.TreeViewModel;
using Fantasy.BusinessEngine;

namespace Fantasy.Studio.BusinessEngine.ClassEditing
{
    public class PackageClassFolderProvider : ObjectWithSite, IChildrenProvider
    {
        #region IChildrenProvider Members

        
        public IEnumerable<object> GetChildren(object parent)
        {
            BusinessPackage package = (BusinessPackage)parent;
            if (package.Id != BusinessPackage.RootPackageId)
            {
                return new object[] { new ClassFolder(package)};
            }
            else
            {
                return new object[0];
            }
        }

        #endregion
    }
}
