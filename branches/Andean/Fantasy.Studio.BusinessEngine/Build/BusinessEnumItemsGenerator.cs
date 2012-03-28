using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine;
using System.Xml.Linq;
using Fantasy.IO;
using Fantasy.Studio.BusinessEngine.CodeGenerating;
using Fantasy.Studio.BusinessEngine.Properties;
using System.CodeDom.Compiler;

namespace Fantasy.Studio.BusinessEngine.Build
{
    public class BusinessEnumItemsGenerator : ObjectWithSite, IProjectItemsGenerator
    {
        #region IProjectItemsGenerator Members

        public void CreateItems(BusinessPackage package, XElement projectElement, ProjectExportOptions options)
        {
            string itemsFolder = package.GetItemsFolder();
            string itemsFolderFullPath = LongPath.Combine(options.ProjectDirectory, itemsFolder);

            if (package.Classes.Count > 0 && options.WriteFile && !LongPathDirectory.Exists(itemsFolderFullPath))
            {
                LongPathDirectory.Create(itemsFolderFullPath);
            }

            XElement itemsGroup = new XElement(Consts.MSBuildNamespace + "ItemGroup");

            foreach (BusinessEnum @enum in package.Enums.Where(e => !e.IsExternal))
            {
                string itemName = itemsFolder + "\\" + @enum.CodeName + ".cs";
                XElement item = new XElement(Consts.MSBuildNamespace + "Compile");
                item.SetAttributeValue("Include", itemName);
                itemsGroup.Add(item);


                item.SetElementValue(Consts.MSBuildNamespace + "AutoGen", "True");
          

                if (options.WriteFile)
                {
                    IT4Service t4svc = this.Site.GetRequiredService<IT4Service>();
                    CompilerErrorCollection errors;

                    string script = t4svc.ProcessTemplateFile(Settings.ExtractToFullPath(Settings.Default.BusinessEnumT4Path), @enum, out errors, this.Site);
                    LongPathFile.WriteAllText(LongPath.Combine(options.ProjectDirectory, itemName), script, Encoding.UTF8);
                   
                }
            }

            if (itemsGroup.Elements().Count() > 0)
            {
                projectElement.Add(itemsGroup);
            }
        }

        #endregion
    }
}
