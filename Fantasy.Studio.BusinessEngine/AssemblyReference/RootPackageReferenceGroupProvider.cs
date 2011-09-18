using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Studio.Controls;
using Fantasy.BusinessEngine.Services;
using Fantasy.BusinessEngine;
using Fantasy.Adaption;

namespace Fantasy.Studio.BusinessEngine.AssemblyReference
{
    public class RootPackageReferenceGroupProvider : ObjectWithSite, IChildItemsProvider
    {
        #region IChildItemsProvider Members

        public System.Collections.IEnumerable GetChildren(object parent)
        {
            IAdapterManager am = this.Site.GetRequiredService<IAdapterManager>();

            BusinessPackage package = am.GetAdapter<BusinessPackage>(parent);
             
            if (package.Id == BusinessPackage.RootPackageId)
            {
                IEntityService es = this.Site.GetRequiredService<IEntityService>();
                return new object[] { es.GetAssemblyReferenceGroup()};
            }
            else
            {
                return new object[0];
            }
        }

        #endregion
    }
}
