using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.AddIns;
using Fantasy.BusinessEngine.Services;
using Fantasy.IO;
using Fantasy.BusinessEngine;
using System.Xml.Linq;
using Fantasy.Studio.BusinessEngine.Properties;

namespace Fantasy.Studio.BusinessEngine.Build
{
    public class ProjectExporter : ObjectWithSite
    {
        public void Run()
        {
            IProjectItemsGenerator[] generators = AddInTree.Tree.GetTreeNode("fantasy/studio/businessengine/build/projectexporter/itemsgenerators").BuildChildItems<IProjectItemsGenerator>(this, this.Site).ToArray();
            IEntityService es = this.Site.GetRequiredService<IEntityService>();
            ProjectExportOptions runtimeOptions = new ProjectExportOptions() { Project = Projects.Runtime, SolutionPath = Settings.Default.SolutionPath };
            runtimeOptions.ProjectPath = LongPath.Combine(runtimeOptions.SolutionDirectory, Settings.Default.RuntimeAssemblyName + ".csproj"; 
            XElement projectElement = XElement.Load(Settings.ExtractToFullPath(Settings.Default.ClassLibraryTemplatePath));
            foreach(BusinessPackage package in es.GetRootPackage().Flatten(p=>p.ChildPackages))
            {
                foreach(IProjectItemsGenerator generator in generators)
                {
                    generator.CreateItems(package, projectElement);
                }
            }

            projectElement.Save(runtimeOptions.ProjectPath); 

        }
    }
}
