using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Studio.Controls;
using System.Collections;

namespace Fantasy.Studio.BusinessEngine.ClassEditing
{
    public class ObjectModelFolderClassesProvider : ObjectWithSite, IChildItemsProvider
    {

        #region IChildItemsProvider Members

        public IEnumerable GetChildren(object parent)
        {
            ObjectModelFolder folder = (ObjectModelFolder)parent;
            return folder.Package.Classes.ToSorted("Name"); 
        }

        #endregion
    }
}
