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
            ExportBusinessDataProject(generators);

            XElement solution = new XElement("solution",
                new XElement("project",
                    new XAttribute("id", Settings.Default.BusinessDataProjectId),
                    new XAttribute("name", Settings.Default.BusinessDataAssemblyName)));
            IT4Service t4 = this.Site.GetRequiredService<IT4Service>();
            CompilerErrorCollection errors;
            string solutionContent = t4.ProcessTemplateFile(Settings.ExtractToFullPath(Settings.Default.SolutionTemplatePath), solution, out errors);

            Stream fs = new FileStream(Settings.ExtractToFullPath(Settings.Default.SolutionPath), FileMode.Create);
            fs.WriteByte(239);
            fs.WriteByte(187);
            fs.WriteByte(191);
            fs.WriteByte(13);
            fs.WriteByte(10);
            StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
            sw.Write(solutionContent);
            sw.Close();

            //LongPathFile.WriteAllText(Settings.ExtractToFullPath(Settings.Default.SolutionPath), solutionContent, Encoding.UTF8); 
        }

        private void ExportBusinessDataProject(IProjectItemsGenerator[] generators)
        {
            IEntityService es = this.Site.GetRequiredService<IEntityService>();
            ProjectExportOptions dataOptions = new ProjectExportOptions() {SolutionPath = Settings.Default.SolutionPath };
            dataOptions.ProjectPath = LongPath.Combine(dataOptions.SolutionDirectory, Settings.Default.BusinessDataAssemblyName,  Settings.Default.BusinessDataAssemblyName + ".csproj");

            if (!LongPathDirectory.Exists(dataOptions.ProjectDirectory))
            {
                LongPathDirectory.Create(dataOptions.ProjectDirectory);
            }
            XElement projectElement = XElement.Load(Settings.ExtractToFullPath(Settings.Default.ClassLibraryTemplatePath));

            XElement firstGroup = projectElement.Element(Consts.MSBuildNamespace + "PropertyGroup");
            firstGroup.SetElementValue(Consts.MSBuildNamespace + "ProjectGuid", Settings.Default.BusinessDataProjectId);
            firstGroup.SetElementValue(Consts.MSBuildNamespace +"AssemblyName", Settings.Default.BusinessDataAssemblyName);


            foreach (BusinessPackage package in es.GetRootPackage().Flatten(p => p.ChildPackages))
            {
                foreach (IProjectItemsGenerator generator in generators)
                {
                    generator.CreateItems(package, projectElement, dataOptions);
                }
            }
           
            projectElement.Add(new XElement(Consts.MSBuildNamespace + "Import", new XAttribute("Project", "$(MSBuildToolsPath)\\Microsoft.CSharp.targets")));  

            projectElement.Save(dataOptions.ProjectPath);


          

            
        }
    }
}
