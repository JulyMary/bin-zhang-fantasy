using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.IO;
using Fantasy.BusinessEngine;
using Fantasy.BusinessEngine.Services;

namespace Fantasy.Studio.BusinessEngine.Build
{
    public class BusinessRoleItemsReader : ObjectWithSite, IProjectItemsReader
    {
        #region IProjectItemsReader Members

        public void Read(ProjectImportOptions options)
        {
            string include = (string)options.ItemElement.Attribute("Include");
            string codeName = LongPath.GetFileNameWithoutExtension(include);
            
            BusinessRoleData role = options.Package.Roles.FirstOrDefault(r => r.CodeName == codeName && (r.ScriptOptions & ScriptOptions.NoScript) == 0);
            if (role != null)
            {
                options.Handled = true;
                string file = LongPath.Combine(options.ProjectDirectory, include);
                if (options.ReadFile)
                {
                    role.Script = LongPathFile.ReadAllText(file);
                    IEntityService es = this.Site.GetRequiredService<IEntityService>();
                    es.SaveOrUpdate(role);
                }
            }
        }

        #endregion
    }
    
}
