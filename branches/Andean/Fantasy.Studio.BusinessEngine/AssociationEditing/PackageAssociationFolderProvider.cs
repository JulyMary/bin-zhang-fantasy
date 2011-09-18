using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Studio.Controls;
using Fantasy.BusinessEngine;
using System.Collections;
using Fantasy.Studio.BusinessEngine.ClassEditing;
using Fantasy.Adaption;

namespace Fantasy.Studio.BusinessEngine.AssociationEditing
{
    public class PackageAssociationFolderProvider : ObjectWithSite, IChildItemsProvider
    {
        #region IChildItemsProvider Members

        
        public IEnumerable GetChildren(object parent)
        {
            IAdapterManager am = this.Site.GetRequiredService<IAdapterManager>();

            BusinessPackage package = (BusinessPackage)am.GetAdapter<BusinessPackage>(parent);

          
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
