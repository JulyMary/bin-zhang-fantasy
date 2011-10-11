using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine;
using Fantasy.BusinessEngine.Services;
using System.Xml.Linq;
using Fantasy.IO;

namespace Fantasy.Studio.BusinessEngine.Build
{
    public class ReferenceItemsGenerator : ObjectWithSite, IProjectItemsGenerator
    {
        #region IProjectItemsGenerator Members

        public void CreateItems(Fantasy.BusinessEngine.BusinessPackage package, System.Xml.Linq.XElement projectElement, ProjectExportOptions options)
        {
            if (package.Id == BusinessPackage.RootPackageId)
            {
                IEntityService es = this.Site.GetRequiredService<IEntityService>();
                XElement itemsGroup = new XElement(Consts.MSBuildNamespace + "ItemGroup");
                foreach (BusinessAssemblyReference br in es.GetAssemblyReferenceGroup().References)
                {


                    XElement reference = new XElement(Consts.MSBuildNamespace + "Reference");
                    reference.SetAttributeValue("Include", br.Name);
                    if (br.Source == BusinessAssemblyReferenceSources.Local)
                    {
                        string path = LongPath.Combine(Fantasy.BusinessEngine.Properties.Settings.Default.FullReferencesPath, br.Name + ".dll");
                        string hintPath = LongPath.GetRelativePath(options.ProjectDirectory + "\\", path);
                        reference.SetElementValue(Consts.MSBuildNamespace + "HintPath", hintPath);
                    }
                    else if (br.Source == BusinessAssemblyReferenceSources.System)
                    {
                        string path = LongPath.Combine(Fantasy.BusinessEngine.Properties.Settings.ExtractToFullPath(Fantasy.BusinessEngine.Properties.Settings.Default.SystemReferencesPath), br.Name + ".dll");
                        string hintPath = LongPath.GetRelativePath(options.ProjectDirectory + "\\", path);
                        reference.SetElementValue(Consts.MSBuildNamespace + "HintPath", hintPath);
                    }
                    itemsGroup.Add(reference); 
                }

                projectElement.Add(itemsGroup);
            }
        }

        #endregion
    }
}
