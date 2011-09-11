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
        public static BusinessPackage AddBusinessPackage(this IEntityService es, BusinessPackage parent)
        {
            BusinessPackage rs = es.CreateEntity<BusinessPackage>();

            rs.Name = UniqueNameGenerator.GetName(Resources.DefaultNewBusinessPackageName, parent.ChildPackages.Select(p => p.Name));
            rs.CodeName = UniqueNameGenerator.GetCodeName(rs.Name);
            parent.ChildPackages.Add(rs);
            rs.ParentPackage = parent;
            return rs;
        }

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

        public static BusinessEnum AddBusinessEnum(this IEntityService es, BusinessPackage package)
        {
            BusinessEnum rs = es.CreateEntity<BusinessEnum>();
            rs.IsExternal = false;
            rs.IsFlags = false;
            rs.Name = UniqueNameGenerator.GetName(Resources.DefaultNewBusinessEnumName, package.Enums.Select(c => c.Name));
            rs.CodeName = UniqueNameGenerator.GetCodeName(rs.Name);
            package.Enums.Add(rs);
            rs.Package = package;
            return rs;
        }

        public static BusinessEnum AddExternalBusinessEnum(this IEntityService es, BusinessPackage package, Type enumType)
        {
            BusinessEnum rs = es.CreateEntity<BusinessEnum>();
            rs.IsExternal = true;
            rs.IsFlags = enumType.IsDefined(typeof(FlagsAttribute), true);
            rs.Name = UniqueNameGenerator.GetName(enumType.Name, package.Enums.Select(c => c.Name));
            rs.CodeName = enumType.Name;
            rs.ExternalNamespace = enumType.Namespace;
            rs.ExternalAssemblyName = enumType.Assembly.GetName().Name;

            foreach (Enum value in Enum.GetValues(enumType))
            {
                BusinessEnumValue bv = es.CreateEntity<BusinessEnumValue>();
                bv.Name = value.ToString();
                bv.CodeName = value.ToString();
                bv.Value = Convert.ToInt64(value);
                bv.Enum = rs;
                rs.EnumValues.Add(bv);
            }

            package.Enums.Add(rs);
            rs.Package = package;
            return rs;
        }


        public static void SyncExternalEnum(this IEntityService es, BusinessEnum @enum, Type enumType)
        {
            IEnumerable<object> values = new object[0];


            if (enumType != null)
            {
                values = Enum.GetValues(enumType).Cast<object>();
            }

            var toDelete = from ev in @enum.EnumValues where !values.Any(v => v.ToString() == ev.CodeName) select ev;
            foreach (BusinessEnumValue ev in toDelete)
            {
                @enum.EnumValues.Remove(ev);
                ev.Enum = null;
            }

            foreach (BusinessEnumValue ev in @enum.EnumValues)
            {
                Enum value = (Enum)values.First(v => v.ToString() == ev.CodeName);
                ev.Value = Convert.ToInt64(value);
            }

            var toAdd = from v in values where !@enum.EnumValues.Any(ev => ev.CodeName == v.ToString()) select v;

            foreach (Enum value in toAdd)
            {
                BusinessEnumValue bv = es.CreateEntity<BusinessEnumValue>();
                bv.Name = value.ToString();
                bv.CodeName = value.ToString();
                bv.Value = Convert.ToInt64(value);
                bv.Enum = @enum;
                @enum.EnumValues.Add(bv);
            }
        }

        public static  BusinessAssociation AddAssociation(this IEntityService es, BusinessPackage package)
        {
            BusinessAssociation rs = es.CreateEntity<BusinessAssociation>();
            rs.Package = package;
            package.Associations.Add(rs);
            return rs;
        }

        public static BusinessAssociation AddAssociation(this IEntityService es, BusinessPackage package, BusinessClass left, BusinessClass right)
        {
            BusinessAssociation rs = es.CreateEntity<BusinessAssociation>();
            rs.Package = package;
            package.Associations.Add(rs);

            
            rs.Name = UniqueNameGenerator.GetName(left.Name + "_" + right.Name, package.Associations.Select(a => a.Name));
            rs.CodeName = UniqueNameGenerator.GetCodeName(rs.Name);
            
            rs.LeftClass = left;
            rs.LeftRoleName = left.Name;
            rs.LeftRoleCode = left.CodeName;
            rs.LeftCardinality = "0..1";
            rs.LeftNavigatable = true;
            left.LeftAssociations.Add(rs);


            rs.RightClass = right;
            rs.RightRoleName = right.Name;
            rs.RightRoleCode = right.CodeName;
            rs.RightCardinality = "*";
            rs.RightNavigatable = true;
            right.RightAssociations.Add(rs);

            return rs;

        }
    }
}
