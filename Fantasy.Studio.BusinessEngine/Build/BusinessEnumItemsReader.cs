using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.IO;
using Fantasy.BusinessEngine;

namespace Fantasy.Studio.BusinessEngine.Build
{
    public class BusinessEnumItemsReader : ObjectWithSite, IProjectItemsReader
    {
        #region IProjectItemsReader Members

        public void Read(ProjectImportOptions options)
        {
            string include = (string)options.ItemElement.Attribute("Include");
            string codeName = LongPath.GetFileNameWithoutExtension(include);
            options.Handled =  options.Package.Enums.Any(e =>! e.IsExternal &&  e.CodeName == codeName);
        }

        #endregion
    }
}
