using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Studio.Controls;
using Fantasy.BusinessEngine;
using Fantasy.Adaption;

namespace Fantasy.Studio.BusinessEngine.UserRoleEditing
{
    public class PackageUserFolderProvider : ObjectWithSite, IChildItemsProvider
    {


      

        #region IChildItemsProvider Members

        public System.Collections.IEnumerable GetChildren(object parent)
        {
            IAdapterManager am = this.Site.GetRequiredService<IAdapterManager>();

            BusinessPackage package = am.GetAdapter<BusinessPackage>(parent);

            return new object[] { new UserFolder(package) };
        }

        #endregion
    }
}
