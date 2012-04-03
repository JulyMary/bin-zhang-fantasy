using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Fantasy.IO;
using Fantasy.BusinessEngine;

namespace Fantasy.Studio.BusinessEngine.Build
{
    public class BusinessRoleItemsGenerator : ObjectWithSite, IProjectItemsGenerator
    {
        public void CreateItems(Fantasy.BusinessEngine.BusinessPackage package, XElement projectElement, XElement insertBefore, ProjectExportOptions options)
        {

            string itemsFolder = package.GetItemsFolder();
            string itemsFolderFullPath = LongPath.Combine(options.ProjectDirectory, itemsFolder);

            if (package.Roles.Count > 0 && options.WriteFile && !LongPathDirectory.Exists(itemsFolderFullPath))
            {
                LongPathDirectory.Create(itemsFolderFullPath);
            }

            XElement itemsGroup = new XElement(Consts.MSBuildNamespace + "ItemGroup");

            foreach (BusinessRoleData role in package.Roles.Where(r => r.ScriptOptions == ScriptOptions.Default))
            {
                string itemName = itemsFolder + "\\" + role.CodeName + ".cs";
                XElement item = new XElement(Consts.MSBuildNamespace + "Compile");
                item.SetAttributeValue("Include", itemName);
                itemsGroup.Add(item);

                if (options.WriteFile)
                {
                    LongPathFile.WriteAllText(LongPath.Combine(options.ProjectDirectory, itemName), role.Script, Encoding.UTF8);

                }


            }

            if (itemsGroup.Elements().Count() > 0)
            {

                insertBefore.AddBeforeSelf(itemsGroup);
            }
        }

    }
}
