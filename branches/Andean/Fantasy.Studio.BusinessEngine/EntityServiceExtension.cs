using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine;
using Fantasy.BusinessEngine.Services;
using Fantasy.Studio.BusinessEngine.Properties;
using Fantasy.Utils;
using Fantasy.Studio.BusinessEngine.ApplicationEditing;
using Fantasy.Studio.BusinessEngine.UserRoleEditing;
using Fantasy.Studio.BusinessEngine.CodeEditing;
using System.IO;
using System.Xml.Linq;
using Fantasy.Studio.BusinessEngine.CodeGenerating;
using System.CodeDom.Compiler;
using System.Reflection;
using Fantasy.IO;
using Fantasy.Reflection;

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
            rs.TableSchema = Settings.Default.DefaultTableSchema;

            rs.TableName = String.Format("{0}_{1}", Settings.Default.DefaultClassTablePrefix, rs.CodeName).ToUpper();
            rs.TableSpace = String.IsNullOrEmpty(Settings.Default.DefaultTableSpace) ? null : Settings.Default.DefaultTableSpace;
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

        public static BusinessAssociation AddAssociation(this IEntityService es, BusinessPackage package)
        {
            BusinessAssociation rs = es.CreateEntity<BusinessAssociation>();
            rs.Name = UniqueNameGenerator.GetName(Resources.DefaultNewBusinessAssociationName, package.Associations.Select(a => a.Name));
            rs.CodeName = UniqueNameGenerator.GetCodeName(rs.Name);
            rs.Package = package;

            package.Associations.Add(rs);
            return rs;
        }

        public static BusinessAssociation AddBusinessAssociation(this IEntityService es, BusinessPackage package, BusinessClass left, BusinessClass right)
        {
            BusinessAssociation rs = es.CreateEntity<BusinessAssociation>();
            rs.Package = package;
            package.Associations.Add(rs);

            
            rs.Name = UniqueNameGenerator.GetName(left.Name + "_" + right.Name, package.Associations.Select(a => a.Name));
            rs.CodeName = UniqueNameGenerator.GetCodeName(rs.Name);
            rs.TableSchema = Settings.Default.DefaultTableSchema;
            rs.TableSpace = string.IsNullOrEmpty(Settings.Default.DefaultTableSpace) ? null : Settings.Default.DefaultTableSpace;
            rs.TableName = (Settings.Default.DefaultAssociationTablePrefix + "_" + rs.CodeName).ToUpper();
            
            rs.LeftClass = left;
            rs.LeftRoleName = UniqueNameGenerator.GetName(left.Name, right.RightAssociations.Select(a=>a.LeftRoleName)) ;
            rs.LeftRoleCode = UniqueNameGenerator.GetCodeName(rs.LeftRoleName);
            rs.LeftCardinality = "0..1";
            rs.LeftNavigatable = true;
            left.LeftAssociations.Add(rs);


            rs.RightClass = right;
            rs.RightRoleName = UniqueNameGenerator.GetName(right.Name, left.LeftAssociations.Select(a => a.RightRoleName));
            rs.RightRoleCode = UniqueNameGenerator.GetCodeName(rs.RightRoleName);
            rs.RightCardinality = "*";
            rs.RightNavigatable = true;
            right.RightAssociations.Add(rs);

            return rs;

        }

        public static BusinessProperty AddBusinessProperty(this IEntityService es, BusinessClass @class)
        {
            BusinessProperty rs = es.CreateEntity<BusinessProperty>();
            rs.Name = UniqueNameGenerator.GetName(Resources.DefaultNewBusinessPropertyName, @class.Properties.Select(p => p.Name));
            rs.CodeName = UniqueNameGenerator.GetCodeName(rs.Name);
            rs.FieldName = rs.CodeName.ToUpper();
            rs.Class = @class;
            if (es is IObjectWithSite)
            {
                IBusinessDataTypeRepository dtr = ((IObjectWithSite)es).Site.GetRequiredService<IBusinessDataTypeRepository>();
                rs.DataType = dtr.Int32;
                rs.FieldType = rs.DataType.DefaultDatabaseType;
                rs.Length = rs.DataType.DefaultLength;
                rs.Precision = rs.DataType.DefaultPrecision; 

            }
           
            @class.Properties.Add(rs);

            return rs;
        }

        public static BusinessUserData AddBusinessUser(this IEntityService es, BusinessPackage package)
        {
            BusinessUserData rs = es.CreateEntity<BusinessUserData>();
            var query = from p in es.GetRootPackage().Flatten(p => p.ChildPackages)
                        from u in p.Users
                        select u.Name;
            
            rs.Package = package;
            rs.Name = rs.FullName = UniqueNameGenerator.GetName(Resources.DefaultNewBusinessUserName, query);
            rs.CodeName = UniqueNameGenerator.GetCodeName(rs.Name);
            package.Users.Add(rs);

            return rs;
        }

        public static BusinessRoleData AddBusinessRole(this IEntityService es, BusinessPackage package)
        {
            BusinessRoleData rs = es.CreateEntity<BusinessRoleData>();
            var query = from p in es.GetRootPackage().Flatten(p=>p.ChildPackages)
                        from role in p.Roles
                        select role.Name;
            
            rs.Package = package;
            package.Roles.Add(rs);
            rs.Name = UniqueNameGenerator.GetName(Resources.DefaultNewBusinessRoleName, query);
            rs.CodeName = UniqueNameGenerator.GetCodeName(rs.Name);
            return rs;

        }

        public static BusinessApplicationData AddBusinessApplication(this IEntityService es, BusinessPackage package)
        {
            BusinessApplicationData rs = es.CreateEntity<BusinessApplicationData>();
           
            
            package.Applications.Add(rs);
            rs.Package = package;
            rs.Name = UniqueNameGenerator.GetName(Resources.DefaultNewBusinessApplicationName, package.Enums.Select(c => c.Name));
            rs.CodeName = UniqueNameGenerator.GetCodeName(rs.Name);
            return rs;
        }


        public static BusinessApplicationParticipant AddBusinessApplicationParticipant(this IEntityService es, BusinessApplicationData application, BusinessClass @class)
        {
            BusinessApplicationParticipant rs = es.CreateEntity<BusinessApplicationParticipant>();
            rs.Application = application;
            rs.Class = @class;
            application.Participants.Add(rs);
            return rs;

        }


        public static BusinessApplicationACL AddBusinessApplicationACL(this IEntityService es, BusinessApplicationParticipant participant, BusinessRoleData role, BusinessEnumValue state)
        {
            BusinessApplicationACL rs = es.CreateEntity<BusinessApplicationACL>();
            rs.Participant = participant;
            rs.Role = role;
            rs.State = state;
            BusinessObjectSecurity security = new BusinessObjectSecurity();
            security.Sync(participant.Class, null, null);

            participant.ACLs.Add(rs);

            return rs;


        }

        public static BusinessScript[] AddBusinessScript(this IEntityService es, BusinessPackage package, ScriptTemplate template, string fileName)
        {
            List<BusinessScript> rs = new List<BusinessScript>();
            Dictionary<string, string> replacements = new Dictionary<string, string>() 
            { 
                {"fileName", fileName }, 
                {"fileNameWithoutExtension", Path.GetFileNameWithoutExtension(fileName)}, 
                {"extension", Path.GetExtension(fileName)},
                { "namespace", package.FullCodeName } 
            };  

            foreach (ScriptTemplateItem item in template.Items)
            {
                BusinessScript script = es.CreateEntity<BusinessScript>();
                script.Package = package;
                package.Scripts.Add(script);
                script.Name = RepleaceTemplateItem(item.Include, replacements);
                XElement meta = new XElement(item.Name);
                foreach (KeyValuePair<string, string> pair in item.MetaData)
                {
                    meta.Add(new XElement(pair.Key, RepleaceTemplateItem(pair.Value, replacements)));
                }
                script.MetaData = meta.ToString();
                if (item.UseT4)
                {
                    IT4Service t4 = ((IObjectWithSite)es).Site.GetRequiredService<IT4Service>();
                    CompilerErrorCollection errors;
                    script.Script = t4.ProcessTemplate(item.Content, script, out errors);
                }
                else
                {
                    script.Script = item.Content;
                }
                rs.Add(script);
            }

            return rs.ToArray();



        }


        private static string RepleaceTemplateItem(string text, Dictionary<string, string> replacements)
        {
            string rs = text;
            foreach (KeyValuePair<string, string> pair in replacements)
            {
                rs = CaseInsensitiveReplace(rs, "$" + pair.Key + "$", pair.Value);
            }
            return rs;
        }

        private static string CaseInsensitiveReplace(string original,
                    string pattern, string replacement)
        {
            int count, position0, position1;
            count = position0 = position1 = 0;
            string upperString = original.ToUpper();
            string upperPattern = pattern.ToUpper();
            int inc = (original.Length / pattern.Length) *
                      (replacement.Length - pattern.Length);
            char[] chars = new char[original.Length + Math.Max(0, inc)];
            while ((position1 = upperString.IndexOf(upperPattern,
                                              position0)) != -1)
            {
                for (int i = position0; i < position1; ++i)
                    chars[count++] = original[i];
                for (int i = 0; i < replacement.Length; ++i)
                    chars[count++] = replacement[i];
                position0 = position1 + pattern.Length;
            }
            if (position0 == 0) return original;
            for (int i = position0; i < original.Length; ++i)
                chars[count++] = original[i];
            return new string(chars, 0, count);
        }


        public static BusinessAssemblyReference[] AddBusinessAssemblyReference(this IEntityService es, string location)
        {

            string directory = LongPath.GetDirectoryName(location);
            if (!GlobalAssemblyCache.GetGACFolders().Any(f => string.Equals(f, directory, StringComparison.OrdinalIgnoreCase)))
            {
                using (AssemblyLoader loader = new AssemblyLoader())
                {

                    return CreateAssemblyReference(es, location, loader, true).ToArray();
                }
            }
            else
            {

                return new BusinessAssemblyReference[] { AddGACAssemblyReference(es, location) };
                
            }
        }

        private static List<BusinessAssemblyReference> CreateAssemblyReference(IEntityService es, string location, AssemblyLoader loader, bool throwError)
        {
            List<BusinessAssemblyReference> rs = new List<BusinessAssemblyReference>();

            try
            {
                AssemblyName assemblyRef = loader.LoadFrom(location);
               

                BusinessAssemblyReferenceGroup group = es.GetAssemblyReferenceGroup();

                if (!group.References.Any(r => string.Equals(assemblyRef.Name, r.Name, StringComparison.OrdinalIgnoreCase)))
                {
                    BusinessAssemblyReference reference = es.CreateEntity<BusinessAssemblyReference>();
                    reference.Group = group;



                    reference.FullName = assemblyRef.Name;
                    string sysRefPath = Fantasy.BusinessEngine.Properties.Settings.ExtractToFullPath(Fantasy.BusinessEngine.Properties.Settings.Default.SystemReferencesPath);
                    if (location.ToLower().StartsWith(sysRefPath.ToLower()))
                    {
                        
                        reference.Source = BusinessAssemblyReferenceSources.System;
                    }
                    else
                    {
                        
                        reference.Source = BusinessAssemblyReferenceSources.Local;
                        byte[] bytes = File.ReadAllBytes(location);
                        reference.RawAssembly = bytes;
                        string copyPath = LongPath.Combine(Fantasy.BusinessEngine.Properties.Settings.Default.FullReferencesPath, LongPath.GetFileName(location));
                        LongPathFile.Copy(location, copyPath, true);
                    }
                   

                    group.References.Add(reference);
                    es.SaveOrUpdate(reference);
                    
                    rs.Add(reference);
                    
                    if (reference.Source == BusinessAssemblyReferenceSources.Local)
                    {

                        rs.AddRange(AddReferenceAssemblies(es, location, loader));
                    }

                }
            }
            catch
            {
                if (throwError)
                {
                    throw;
                }
            }

            return rs;


        }

        private static List<BusinessAssemblyReference> AddReferenceAssemblies(IEntityService es, string location, AssemblyLoader loader)
        {
            List<BusinessAssemblyReference> rs = new List<BusinessAssemblyReference>();
            string dir = LongPath.GetDirectoryName(location);
            BusinessAssemblyReferenceGroup ag = es.GetAssemblyReferenceGroup();
            var query = from ran in loader.ReflectionOnlyGetReferenceAssemblyNames(location) where ag.References.Any(r => !String.Equals(r.Name, ran.Name, StringComparison.OrdinalIgnoreCase)) select ran;
            foreach (AssemblyName ran in query)
            {
                string file = LongPath.Combine(dir, ran.Name + ".dll");
               
                if (LongPathFile.Exists(file))
                {
                    rs.AddRange(CreateAssemblyReference(es, file, loader, false));
                }

                
            }

            return rs;
        }


        public static BusinessAssemblyReference AddGACAssemblyReference(this IEntityService es, string location)
        {
            using (AssemblyLoader loader = new AssemblyLoader())
            {
                BusinessAssemblyReferenceGroup group = es.GetAssemblyReferenceGroup();
                AssemblyName assemblyRef = loader.LoadFrom(location);

                BusinessAssemblyReference rs = group.References.FirstOrDefault(r => string.Equals(assemblyRef.Name, r.Name, StringComparison.OrdinalIgnoreCase));
                if (rs == null)
                {
                    rs = es.CreateEntity<BusinessAssemblyReference>();
                    rs.FullName = GlobalAssemblyCache.IsInFramework(location) ? assemblyRef.Name : assemblyRef.FullName;
                    rs.Group = group;
                    group.References.Add(rs);
                    rs.Source = BusinessAssemblyReferenceSources.GAC;
                    es.SaveOrUpdate(rs);
                }
                return rs;
            }
        }


        public static BusinessAssemblyReference AddGACAssemblyReference(this IEntityService es, AssemblyName assemblyRef)
        {
            using (AssemblyLoader loader = new AssemblyLoader())
            {
                BusinessAssemblyReferenceGroup group = es.GetAssemblyReferenceGroup();
               

                BusinessAssemblyReference rs = group.References.FirstOrDefault(r => string.Equals(assemblyRef.Name, r.Name, StringComparison.OrdinalIgnoreCase));
                if (rs == null)
                {
                    rs = es.CreateEntity<BusinessAssemblyReference>();

                    string file = LongPath.Combine(GlobalAssemblyCache.FrameworkFolder, assemblyRef.Name + ".dll");
                    rs.FullName = assemblyRef.FullName;
                    if (LongPathFile.Exists(file))
                    {
                        AssemblyName assemblyRef2 = loader.LoadFrom(file);
                        if (assemblyRef2.FullName == assemblyRef.FullName)
                        {
                            rs.FullName = assemblyRef.Name;
                        }
                    }
                    rs.Group = group;
                    group.References.Add(rs);
                    rs.Source = BusinessAssemblyReferenceSources.GAC;
                    es.SaveOrUpdate(rs);
                }
                return rs;
            }
        }

        
    }
}
