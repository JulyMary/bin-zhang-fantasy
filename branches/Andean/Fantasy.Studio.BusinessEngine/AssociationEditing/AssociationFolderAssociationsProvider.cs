using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Studio.Controls;
using System.Collections;
using Fantasy.BusinessEngine;

namespace Fantasy.Studio.BusinessEngine.AssociationEditing
{
    public class AssociationFolderAssociationsProvider  : ObjectWithSite, IChildItemsProvider
    {
        #region IChildItemsProvider Members

        
        public IEnumerable GetChildren(object parent)
        {
            AssociationFolder om = (AssociationFolder)parent;
            BusinessPackage package = om.Package;
            return package.Associations.ToSorted("Name");
        }

        #endregion
    }
}
