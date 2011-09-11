using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Studio.Controls;
using Fantasy.BusinessEngine;
using System.Collections;
using Fantasy.Studio.BusinessEngine.ClassEditing;

namespace Fantasy.Studio.BusinessEngine.AssociationEditing
{
    public class ObjectModelAssociationFolderProvider : ObjectWithSite, IChildItemsProvider
    {
        #region IChildItemsProvider Members

        
        public IEnumerable GetChildren(object parent)
        {
            ObjectModelFolder om = (ObjectModelFolder)parent;

            BusinessPackage package = om.Package;
            if (package.Id != BusinessPackage.RootPackageId)
            {
                return new object[] { new AssociationFolder(package)};
            }
            else
            {
                return new object[0];
            }
        }

        #endregion
    }
}
