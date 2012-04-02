using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.IO;
using Fantasy.BusinessEngine;
using Fantasy.BusinessEngine.Services;
using System.Xml.Linq;

namespace Fantasy.Studio.BusinessEngine.Build
{
    public class BusinessScriptItemsReader : ObjectWithSite, IProjectItemsReader
    {
        #region IProjectItemsReader Members

        public void Read(ProjectImportOptions options)
        {

            string include = (string)options.ItemElement.Attribute("Include");
            string name = LongPath.GetFileName(include);
            IEntityService es = this.Site.GetRequiredService<IEntityService>();
            BusinessScript script = options.Package.Scripts.FirstOrDefault(s => String.Equals(s.Name, name, StringComparison.OrdinalIgnoreCase));
            if (script == null)
            {
                script = es.CreateEntity<BusinessScript>();
                script.Package = options.Package;
                script.Name = name;
                options.Package.Scripts.Add(script);
                
            }

           

            XElement meta = new XElement(options.ItemElement.Name.LocalName);
            foreach (XElement child in options.ItemElement.Elements())
            {
                meta.Add(new XElement(child.Name.LocalName, child.Value));
            }
            script.MetaData = meta.ToString(SaveOptions.OmitDuplicateNamespaces);


            string path = (string)options.ItemElement.Element(Consts.MSBuildNamespace + "HintPath");
            if(path == null)
            {
                path = include;
            }
            string fullPath = LongPath.Combine(options.ProjectDirectory, path);

            script.Script = LongPathFile.ReadAllText(fullPath);

            es.SaveOrUpdate(script);
            options.Handled = true;
        }

        #endregion
    }
}
