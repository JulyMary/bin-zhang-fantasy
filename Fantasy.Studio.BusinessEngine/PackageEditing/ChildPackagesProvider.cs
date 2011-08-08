using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Studio.TreeViewModel;
using Fantasy.BusinessEngine;

namespace Fantasy.Studio.BusinessEngine.PackageEditing
{
    public class ChildPackagesProvider : ObjectWithSite, IChildrenProvider
    {
        #region IChildrenProvider Members

        public IEnumerable<object> GetChildren(object parent)
        {
            return ((BusinessPackage)parent).ChildPackages;
        }

        #endregion
    }
}
