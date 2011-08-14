using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Studio.Controls;
using Fantasy.BusinessEngine;

namespace Fantasy.Studio.BusinessEngine.ClassEditing
{
    public class PackageClassesProvider : ObjectWithSite, IChildItemsProvider
    {
        #region IChildItemsProvider Members

        public IEnumerable<object> GetChildren(object parent)
        {
            return ((BusinessPackage)parent).Classes; 
        }

        #endregion
    }
}
