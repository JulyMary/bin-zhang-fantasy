using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.IO;
using Fantasy.BusinessEngine;
using Fantasy.BusinessEngine.Services;

namespace Fantasy.Studio.BusinessEngine.Build
{
    public class BusinessClassItemsReader : ObjectWithSite, IProjectItemsReader
    {

        #region IProjectItemsReader Members

        public void Read(ProjectImportOptions options)
        {
            string include = (string)options.ItemElement.Attribute("Include");
            string name = LongPath.GetFileNameWithoutExtension(include);
            string codeName;
            bool isAutoScript = false;
            if (name.ToLower().EndsWith(".designer"))
            {
                isAutoScript = true;
                codeName = name.Remove(name.Length - ".designer".Length);
            }
            else
            {
                codeName = name;
            }

            BusinessClass @class = options.Package.Classes.FirstOrDefault(c => c.CodeName == codeName && (c.ScriptOptions & ScriptOptions.NoScript) == 0 );
            if (@class != null)
            {
                options.Handled = true;
                string file = LongPath.Combine(options.ProjectDirectory, include);
                if (options.ReadFile && !isAutoScript)
                {
                    @class.Script = LongPathFile.ReadAllText(file);
                    IEntityService es = this.Site.GetRequiredService<IEntityService>();
                    es.SaveOrUpdate(@class);
                }
            }

        }

        #endregion
    }
}
