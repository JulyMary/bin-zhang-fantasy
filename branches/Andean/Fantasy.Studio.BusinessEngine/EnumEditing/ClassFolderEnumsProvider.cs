using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Studio.Controls;
using System.Collections;
using Fantasy.Studio.BusinessEngine.ClassEditing;

namespace Fantasy.Studio.BusinessEngine.EnumEditing
{
    public class ClassFolderEnumsProvider : ObjectWithSite, IChildItemsProvider
    {

        #region IChildItemsProvider Members

        public IEnumerable GetChildren(object parent)
        {
            ClassFolder folder = (ClassFolder)parent;
            return folder.Package.Enums.ToSorted("Name"); 
        }

        #endregion
    }
}
