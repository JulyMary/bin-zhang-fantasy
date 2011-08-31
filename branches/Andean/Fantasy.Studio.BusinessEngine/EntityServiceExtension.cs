using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine;
using Fantasy.BusinessEngine.Services;
using Fantasy.Studio.BusinessEngine.Properties;
using Fantasy.Utils;

namespace Fantasy.Studio.BusinessEngine
{
    static class EntityServiceExtension 
    {
        public static BusinessClass AddBusinessClass(this IEntityService es, BusinessPackage package)
        {
           
            BusinessClass rs = es.CreateEntity<BusinessClass>();

            rs.Name = UniqueNameGenerator.GetName(Resources.DefaultNewBusinessClassName, package.Classes.Select(c => c.Name));
            rs.ParentClass = es.GetRootClass();
            rs.ParentClass.ChildClasses.Add(rs);
            rs.CodeName = UniqueNameGenerator.GetCodeName(rs.Name);
            package.Classes.Add(rs);
            rs.Package = package;

            return rs;
        }
    }
}
