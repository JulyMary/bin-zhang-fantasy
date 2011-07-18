using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine;

namespace Fantasy.Studio.BusinessEngine
{
    public class PackageClassesProvider : IPackageChildrenProvider
    {

        #region IPackageChildrenProvider Members

        public IEnumerable<IDocumentTreeViewItem> GetItems(BusinessPackage package)
        {
            return new IDocumentTreeViewItem[] { new ClassFolderTreeViewItem(package) };
        }

        #endregion
    }
}
