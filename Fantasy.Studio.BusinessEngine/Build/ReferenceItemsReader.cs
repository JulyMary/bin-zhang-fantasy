using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine.Services;
using Fantasy.IO;
using System.Reflection;
using Fantasy.GAC;

namespace Fantasy.Studio.BusinessEngine.Build
{
    public class ReferenceItemsReader : ObjectWithSite, IProjectItemsReader
    {
        #region IProjectItemsReader Members

        public void Read(ProjectImportOptions options)
        {
            if (options.ItemElement.Name == Consts.MSBuildNamespace + "Reference")
            {
                IEntityService es = this.Site.GetRequiredService<IEntityService>();
                string hintPath = (string)options.ItemElement.Element(Consts.MSBuildNamespace + "HintPath");
                if (!string.IsNullOrEmpty(hintPath))
                {
                    string filePath = LongPath.Combine(options.ProjectDirectory, hintPath);
                    es.AddBusinessAssemblyReference(filePath);
                }
                else
                {

                    AssemblyName assemblyRef = new AssemblyName((string)options.ItemElement.Attribute("Include"));
                    es.AddGACAssemblyReference(assemblyRef);
                   
                }
               
                options.Handled = true;
            }
        }

        #endregion
    }
}
