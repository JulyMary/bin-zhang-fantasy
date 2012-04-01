using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.IO;
using System.Xml.Linq;
using Fantasy.BusinessEngine;

namespace Fantasy.Studio.BusinessEngine.Build
{
    public class BusinessApplicationItemsGenerator :  ObjectWithSite, IProjectItemsGenerator
    {
        public void CreateItems(Fantasy.BusinessEngine.BusinessPackage package, XElement projectElement, ProjectExportOptions options)
        {

            string itemsFolder = package.GetItemsFolder();
            string itemsFolderFullPath = LongPath.Combine(options.ProjectDirectory, itemsFolder);

            if (package.Applications.Count > 0 && options.WriteFile && !LongPathDirectory.Exists(itemsFolderFullPath))
            {
                LongPathDirectory.Create(itemsFolderFullPath);
            }

            XElement itemsGroup = new XElement(Consts.MSBuildNamespace + "ItemGroup");

            foreach (BusinessApplicationData app in package.Applications.Where(a => (a.ScriptOptions & ScriptOptions.NoScript) != ScriptOptions.NoScript))
            {
                string itemName = itemsFolder + "\\" + app.CodeName + ".cs";
                XElement item = new XElement(Consts.MSBuildNamespace + "Compile");
                item.SetAttributeValue("Include", itemName);
                itemsGroup.Add(item);


               

                if (options.WriteFile)
                {
                    LongPathFile.WriteAllText(LongPath.Combine(options.ProjectDirectory, itemName), app.Script, Encoding.UTF8);
                   
                }

               
            }

            if (itemsGroup.Elements().Count() > 0)
            {

                projectElement.Add(itemsGroup);
            }
        }

    }
}
