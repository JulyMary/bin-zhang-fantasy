using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.IO;
using Fantasy.Studio.BusinessEngine.Properties;
using System.Xml.Linq;
using Fantasy.AddIns;
using Fantasy.BusinessEngine;
using Fantasy.BusinessEngine.Services;

namespace Fantasy.Studio.BusinessEngine.Build
{
    public class ProjectImporter : ObjectWithSite
    {
        public void Run()
        {
            string slnPath = Settings.ExtractToFullPath(Settings.Default.SolutionPath);
            string projDir = LongPath.Combine(LongPath.GetDirectoryName(slnPath), LongPath.GetFileNameWithoutExtension(Settings.Default.BusinessDataProjectTemplatePath));
            string projPath = LongPath.Combine(projDir, LongPath.GetFileName(Settings.Default.BusinessDataProjectTemplatePath));

            XElement projectElement = XElement.Load(projPath);


            string rootNamespace = (string)(from ele in projectElement.Elements(Consts.MSBuildNamespace + "PropertyGroup")
                                            from prop in ele.Elements()
                                            where prop.Name == Consts.MSBuildNamespace + "RootNamespace"
                                            select prop).SingleOrDefault();

            IProjectItemsReader[] readers = AddInTree.Tree.GetTreeNode("fantasy/studio/businessengine/build/projectimporter/itemsreaders").BuildChildItems<IProjectItemsReader>(this, this.Site).ToArray();
            var itemElements = from itemGroup in projectElement.Elements(Consts.MSBuildNamespace + "ItemGroup")
                               from item in itemGroup.Elements()
                               select item;


            this._allPackages = this.Site.GetRequiredService<IEntityService>().Query<BusinessPackage>().ToList();


            foreach (XElement itemElement in itemElements)
            {
                BusinessPackage package = this.FindOrCreateParentPackageForItem(rootNamespace, itemElement);
                ProjectImportOptions options = new ProjectImportOptions()
                {
                    SolutionPath = slnPath,
                    ProjectPath = projPath,
                    ItemElement = itemElement,
                    Package = package
                };

                foreach (IProjectItemsReader reader in readers)
                {
                    reader.Read(options);
                    if (options.Handled)
                    {
                        break;
                    }
                }


            }

        }

        private List<BusinessPackage> _allPackages;

        private BusinessPackage FindOrCreateParentPackageForItem(string rootNamespace, XElement itemElement)
        {
            string include = (string)itemElement.Attribute("Include");
            string dir = LongPath.GetDirectoryName(include);
            
            string packageFullCodeName = !String.IsNullOrEmpty(dir) ?  rootNamespace + "." + dir.Replace('\\', '.') : rootNamespace;
            IEntityService es = this.Site.GetRequiredService<IEntityService>();
            return this.FindOrCreatePackageRecursive(packageFullCodeName);
            
        }

        private BusinessPackage FindOrCreatePackageRecursive(string packageFullCodeName)
        {
            BusinessPackage rs = _allPackages.FirstOrDefault(p => p.FullCodeName == packageFullCodeName);
            if (rs == null)
            {
                int dotIndex = packageFullCodeName.LastIndexOf('.');
                string parentFullCodeName = packageFullCodeName.Substring(0, dotIndex);
                string codeName = packageFullCodeName.Substring(dotIndex + 1);
                BusinessPackage parent = this.FindOrCreatePackageRecursive(parentFullCodeName);
                IEntityService es = this.Site.GetRequiredService<IEntityService>();
                rs = es.CreateEntity<BusinessPackage>();

                rs.Name = rs.CodeName = codeName;
               
                parent.ChildPackages.Add(rs);
                rs.ParentPackage = parent;
                es.SaveOrUpdate(rs);
                this._allPackages.Add(rs);
               
            }
            return rs;
        }
    }
}
