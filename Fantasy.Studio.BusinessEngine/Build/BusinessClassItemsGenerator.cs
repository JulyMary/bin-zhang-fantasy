using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Fantasy.BusinessEngine;
using Fantasy.IO;

namespace Fantasy.Studio.BusinessEngine.Build
{
    public class BusinessClassItemsGenerator : ObjectWithSite, IProjectItemsGenerator
    {
        #region IProjectItemsGenerator Members

        public void CreateItems(Fantasy.BusinessEngine.BusinessPackage package, XElement projectElement, XElement insertBefore, ProjectExportOptions options)
        {

            string itemsFolder = package.GetItemsFolder();
            string itemsFolderFullPath = LongPath.Combine(options.ProjectDirectory, itemsFolder);

            if (package.Classes.Count > 0 && options.WriteFile && !LongPathDirectory.Exists(itemsFolderFullPath) )
            {
                LongPathDirectory.Create(itemsFolderFullPath);
            }

            XElement itemsGroup = new XElement(Consts.MSBuildNamespace + "ItemGroup");

            foreach (BusinessClass @class in package.Classes.Where(c=>(c.ScriptOptions & ScriptOptions.NoScript ) != ScriptOptions.NoScript  ))
            {
                string itemName = itemsFolder + "\\" + @class.CodeName + ".cs";
                XElement item = new XElement(Consts.MSBuildNamespace + "Compile");
                item.SetAttributeValue("Include", itemName);
                itemsGroup.Add(item);
               

                string designerItemName = itemsFolder + "\\" + @class.CodeName + ".Designer.cs";
                XElement designerItem = new XElement(Consts.MSBuildNamespace + "Compile");
                designerItem.SetAttributeValue("Include", designerItemName);
                designerItem.SetElementValue(Consts.MSBuildNamespace + "AutoGen", "True");
                designerItem.SetElementValue(Consts.MSBuildNamespace + "DependentUpon", @class.CodeName + ".cs");
                designerItem.SetElementValue(Consts.MSBuildNamespace + "DesignTime", "True");

                if (options.WriteFile)
                {
                    LongPathFile.WriteAllText(LongPath.Combine(options.ProjectDirectory, itemName), @class.Script, Encoding.UTF8);
                    LongPathFile.WriteAllText(LongPath.Combine(options.ProjectDirectory, designerItemName), @class.AutoScript, Encoding.UTF8); 
                }

                itemsGroup.Add(designerItem); 
            }

            if (itemsGroup.Elements().Count() > 0)
            {

                insertBefore.AddBeforeSelf(itemsGroup);
            }
        }

        #endregion
    }
}
