using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Studio.TreeViewModel;

namespace Fantasy.Studio.BusinessEngine
{
    public class ClassFolderClassesProvider : ObjectWithSite, IChildrenProvider
    {

        #region IChildrenProvider Members

        public IEnumerable<object> GetChildren(object parent)
        {
            ClassFolder folder = (ClassFolder)parent;
            return folder.Package.Classes; 
        }

        #endregion
    }
}
