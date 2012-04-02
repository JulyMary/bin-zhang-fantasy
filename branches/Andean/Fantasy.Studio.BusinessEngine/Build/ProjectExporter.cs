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
using Fantasy.Studio.BusinessEngine.CodeGenerating;
using System.CodeDom.Compiler;
using System.IO;

namespace Fantasy.Studio.BusinessEngine.Build
{
    public class ProjectExporter : ObjectWithSite
    {
        public void Run()
        {
            IProjectItemsGenerator[] generators = AddInTree.Tree.GetTreeNode("fantasy/studio/businessengine/build/projectexporter/itemsgenerators").BuildChildItems<IProjectItemsGenerator>(this, this.Site).ToArray();
            string slnPath = Settings.ExtractToFullPath(Settings.Default.SolutionPath);
            if (!LongPathFile.Exists(slnPath))
            {
                string slnDir = LongPath.GetDirectoryName(slnPath);
                if(!LongPathDirectory.Exists(slnDir))
                {
                    LongPathDirectory.Create(slnDir);
                }

                LongPathFile.Copy(Settings.ExtractToFullPath(Settings.Default.SolutionTemplatePath), slnPath); 
            }

            string projDir = LongPath.Combine(LongPath.GetDirectoryName(slnPath), LongPath.GetFileNameWithoutExtension(Settings.Default.BusinessDataProjectTemplatePath));
            string projPath = LongPath.Combine(projDir,  LongPath.GetFileName(Settings.Default.BusinessDataProjectTemplatePath));
            if (!LongPathFile.Exists(projPath))
            {
                if(!LongPathDirectory.Exists(projDir))
                {
                    LongPathDirectory.Create(projDir);
                }

                LongPathFile.Copy(Settings.ExtractToFullPath(Settings.Default.BusinessDataProjectTemplatePath), projPath); 
            }

            XElement projectElement = XElement.Load(projPath);
            //Remove Old Items
            foreach (XElement itemGroup in projectElement.Elements(Consts.MSBuildNamespace + "ItemGroup").ToArray())
            {
                itemGroup.Remove();
            }

            XElement insertBefore = projectElement.Element(Consts.MSBuildNamespace + "Import");
            IEntityService es = this.Site.GetRequiredService<IEntityService>();
            ProjectExportOptions dataOptions = new ProjectExportOptions() { SolutionPath = slnPath, ProjectPath=projPath, WriteFile=true };
            foreach (BusinessPackage package in es.GetRootPackage().Flatten(p => p.ChildPackages))
            {
                foreach (IProjectItemsGenerator generator in generators)
                {
                    generator.CreateItems(package, projectElement, insertBefore, dataOptions);
                }
            }

            projectElement.Save(projPath);

        }

        
    }
}
