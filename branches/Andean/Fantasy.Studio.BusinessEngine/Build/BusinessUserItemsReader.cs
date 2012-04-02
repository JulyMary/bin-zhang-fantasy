using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.IO;
using Fantasy.BusinessEngine;
using Fantasy.BusinessEngine.Services;

namespace Fantasy.Studio.BusinessEngine.Build
{
    public class BusinessUserItemsReader : ObjectWithSite, IProjectItemsReader
    {
        #region IProjectItemsReader Members

        public void Read(ProjectImportOptions options)
        {
            string include = (string)options.ItemElement.Attribute("Include");
            string codeName = LongPath.GetFileNameWithoutExtension(include);
            
           

            BusinessUserData user = options.Package.Users.FirstOrDefault(u => u.CodeName == codeName && (u.ScriptOptions & ScriptOptions.NoScript) == 0);
            if (user != null)
            {
                options.Handled = true;
                string file = LongPath.Combine(options.ProjectDirectory, include);
                if (options.ReadFile)
                {
                    user.Script = LongPathFile.ReadAllText(file);
                    IEntityService es = this.Site.GetRequiredService<IEntityService>();
                    es.SaveOrUpdate(user);
                }
            }
        }

        #endregion
    }
    
}
