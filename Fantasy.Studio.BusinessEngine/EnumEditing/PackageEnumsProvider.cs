using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Fantasy.Studio.Controls;
using Fantasy.BusinessEngine;

namespace Fantasy.Studio.BusinessEngine.EnumEditing
{
    public class PackageEnumsProvider : ObjectWithSite, IChildItemsProvider
    {

        #region IChildItemsProvider Members

        public IEnumerable GetChildren(object parent)
        {
            BusinessPackage package = (BusinessPackage)parent;
            return package.Enums.ToSorted("Name");
        }

        #endregion
    }
}
