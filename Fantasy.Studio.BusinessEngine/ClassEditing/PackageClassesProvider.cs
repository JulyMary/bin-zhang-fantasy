using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Studio.TreeViewModel;
using Fantasy.BusinessEngine;

namespace Fantasy.Studio.BusinessEngine.ClassEditing
{
    public class PackageClassesProvider : ObjectWithSite, IChildrenProvider
    {
        #region IChildrenProvider Members

        public IEnumerable<object> GetChildren(object parent)
        {
            return ((BusinessPackage)parent).Classes; 
        }

        #endregion
    }
}
