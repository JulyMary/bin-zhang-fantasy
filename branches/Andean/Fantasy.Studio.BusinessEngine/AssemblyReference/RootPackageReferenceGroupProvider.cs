using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Studio.Controls;
using Fantasy.BusinessEngine.Services;
using Fantasy.BusinessEngine;

namespace Fantasy.Studio.BusinessEngine.AssemblyReference
{
    public class RootPackageReferenceGroupProvider : ObjectWithSite, IChildItemsProvider
    {
        #region IChildItemsProvider Members

        public System.Collections.IEnumerable GetChildren(object parent)
        {
            BusinessPackage package = (BusinessPackage)parent;
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
