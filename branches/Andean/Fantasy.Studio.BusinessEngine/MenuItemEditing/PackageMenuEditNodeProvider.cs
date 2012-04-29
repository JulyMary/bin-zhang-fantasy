using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Studio.Controls;
using Fantasy.BusinessEngine;
using Fantasy.Adaption;

namespace Fantasy.Studio.BusinessEngine.MenuItemEditing
{
    public class PackageMenuEditNodeProvider : ObjectWithSite, IChildItemsProvider
    {

        #region IChildItemsProvider Members

        public System.Collections.IEnumerable GetChildren(object parent)
        {

            BusinessPackage package = this.Site.GetRequiredService<IAdapterManager>().GetAdapter<BusinessPackage>(parent);
            if (package.Id == BusinessPackage.RootPackageId)
            {
                return new object[] { MenuEditNode.Instance };
            }
            else
            {
                return new object[0];
            }
        }

        #endregion
    }
}
