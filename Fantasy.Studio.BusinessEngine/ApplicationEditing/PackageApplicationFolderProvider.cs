using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Studio.Controls;
using Fantasy.Adaption;
using Fantasy.BusinessEngine;

namespace Fantasy.Studio.BusinessEngine.ApplicationEditing
{
    public class PackageApplicationFolderProvider : ObjectWithSite, IChildItemsProvider
    {

        #region IChildItemsProvider Members

        public System.Collections.IEnumerable GetChildren(object parent)
        {
            IAdapterManager am = this.Site.GetRequiredService<IAdapterManager>();

            BusinessPackage package = am.GetAdapter<BusinessPackage>(parent);

            return package.Id != BusinessPackage.RootPackageId ? new object[] { new ApplicationFolder(package) } : new object[0];
        }

        #endregion
    }
}
