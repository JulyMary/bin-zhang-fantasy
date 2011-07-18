using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine.Collections;
using Fantasy.BusinessEngine;

namespace Fantasy.Studio.BusinessEngine
{
    public class ChildPackageProvider : IPackageChildrenProvider
    {

        #region IPackageChildrenProvider Members

        public IEnumerable<IDocumentTreeViewItem> GetItems(BusinessPackage package)
        {
            return new ObservableAdapterCollection<IDocumentTreeViewItem>(package.ChildPackages,
                p => new PackageTreeViewItem((BusinessPackage)p));
        }

        #endregion
    }
}
