using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Studio.Controls;

namespace Fantasy.Studio.BusinessEngine.ClassEditing
{
    public class ClassFolderClassesProvider : ObjectWithSite, IChildItemsProvider
    {

        #region IChildItemsProvider Members

        public IEnumerable<object> GetChildren(object parent)
        {
            ClassFolder folder = (ClassFolder)parent;
            return folder.Package.Classes; 
        }

        #endregion
    }
}
