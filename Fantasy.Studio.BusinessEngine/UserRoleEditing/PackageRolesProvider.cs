using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Adaption;
using Fantasy.BusinessEngine;
using Fantasy.Studio.Controls;

namespace Fantasy.Studio.BusinessEngine.UserRoleEditing
{
    public class PackageRolesProvider : ObjectWithSite, IChildItemsProvider
    {

        #region IChildItemsProvider Members

        public System.Collections.IEnumerable GetChildren(object parent)
        {
            IAdapterManager am = this.Site.GetRequiredService<IAdapterManager>();

            BusinessPackage package = am.GetAdapter<BusinessPackage>(parent);

            return package.Roles.ToSorted("Name");
        }

        #endregion
    }
}
