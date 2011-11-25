using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Studio.Controls;
using Fantasy.Adaption;
using Fantasy.BusinessEngine;
using System.Collections.ObjectModel;

namespace Fantasy.Studio.BusinessEngine.Web
{
    public class PackageWebFolderProvider : ObjectWithSite, IChildItemsProvider
    {

        #region IChildItemsProvider Members

        public System.Collections.IEnumerable GetChildren(object parent)
        {
            IAdapterManager am = this.Site.GetRequiredService<IAdapterManager>();

            BusinessPackage package =  am.GetAdapter<BusinessPackage>(parent);



            return package.WebFolders;

        }

        #endregion
    }
}
