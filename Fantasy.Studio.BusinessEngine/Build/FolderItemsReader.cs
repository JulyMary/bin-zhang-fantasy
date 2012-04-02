using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.Studio.BusinessEngine.Build
{
    public class FolderItemsReader : ObjectWithSite, IProjectItemsReader
    {
        #region IProjectItemsReader Members

        public void Read(ProjectImportOptions options)
        {
            options.Handled = options.ItemElement.Name == Consts.MSBuildNamespace + "Folder";
        }

        #endregion
    }
}
