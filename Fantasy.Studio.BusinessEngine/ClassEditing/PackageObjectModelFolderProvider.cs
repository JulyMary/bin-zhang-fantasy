using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Studio.Controls;
using Fantasy.BusinessEngine;
using System.Collections;

namespace Fantasy.Studio.BusinessEngine.ClassEditing
{
    public class PackageObjectModelFolderProvider : ObjectWithSite, IChildItemsProvider
    {
        #region IChildItemsProvider Members

        
        public IEnumerable GetChildren(object parent)
        {
            BusinessPackage package = (BusinessPackage)parent;
            if (package.Id != BusinessPackage.RootPackageId)
            {
                return new object[] { new ObjectModelFolder(package)};
            }
            else
            {
                return new object[0];
            }
        }

        #endregion
    }
}
