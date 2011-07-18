using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine;

namespace Fantasy.Studio.BusinessEngine
{
    public interface IPackageChildrenProvider
    {
        IEnumerable<IDocumentTreeViewItem> GetItems(BusinessPackage package);

    }
}
