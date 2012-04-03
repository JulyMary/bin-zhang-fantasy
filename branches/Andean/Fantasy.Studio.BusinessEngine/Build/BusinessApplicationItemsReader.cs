using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.IO;
using Fantasy.BusinessEngine;
using Fantasy.BusinessEngine.Services;

namespace Fantasy.Studio.BusinessEngine.Build
{
    public class BusinessApplicationItemsReader : ObjectWithSite, IProjectItemsReader
    {
        #region IProjectItemsReader Members

        public void Read(ProjectImportOptions options)
        {
            string include = (string)options.ItemElement.Attribute("Include");
            string codeName = LongPath.GetFileNameWithoutExtension(include);
            
           

            BusinessApplicationData app = options.Package.Applications.FirstOrDefault(a => a.CodeName == codeName && a.ScriptOptions == ScriptOptions.Default);
            if (app != null)
            {
                options.Handled = true;
                string file = LongPath.Combine(options.ProjectDirectory, include);
                if (options.ReadFile)
                {
                    app.Script = LongPathFile.ReadAllText(file);
                    IEntityService es = this.Site.GetRequiredService<IEntityService>();
                    es.SaveOrUpdate(app);
                }
            }
        }

        #endregion
    }
}
