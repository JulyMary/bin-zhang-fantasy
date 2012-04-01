using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Fantasy.IO;
using Fantasy.BusinessEngine;

namespace Fantasy.Studio.BusinessEngine.Build
{
    public class BusinessScriptItemsGenerator : ObjectWithSite, IProjectItemsGenerator
    {
        public void CreateItems(Fantasy.BusinessEngine.BusinessPackage package, XElement projectElement, ProjectExportOptions options)
        {

            string itemsFolder = package.GetItemsFolder();
            string itemsFolderFullPath = LongPath.Combine(options.ProjectDirectory, itemsFolder);

            if (package.Scripts.Count > 0 && options.WriteFile && !LongPathDirectory.Exists(itemsFolderFullPath))
            {
                LongPathDirectory.Create(itemsFolderFullPath);
            }

            XElement itemsGroup = new XElement(Consts.MSBuildNamespace + "ItemGroup");

            foreach (BusinessScript script in package.Scripts)
            {
                string itemName = itemsFolder + "\\" + script.CodeName + ".cs";
                XElement meta = !string.IsNullOrEmpty(script.MetaData) ? XElement.Parse(script.MetaData) : new XElement("None"); 
                XElement item = new XElement(Consts.MSBuildNamespace + meta.Name.LocalName);
                item.SetAttributeValue("Include", itemName);
                foreach (XElement child in meta.Elements())
                {
                    item.Add(new XElement(Consts.MSBuildNamespace + child.Name.LocalName, child.Value));
                }
                itemsGroup.Add(item);




                if (options.WriteFile)
                {
                    LongPathFile.WriteAllText(LongPath.Combine(options.ProjectDirectory, itemName), script.Script, Encoding.UTF8);

                }


            }

            if (itemsGroup.Elements().Count() > 0)
            {

                projectElement.Add(itemsGroup);
            }
        }
    }
}
