using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Fantasy.IO;

namespace Fantasy.Reflection
{
    
    public class AssemblyLoader : IDisposable
    {
        private AppDomain _domain;

        private static int _index = 0;
        public AssemblyLoader()
        {
            AppDomainSetup domainSetup = new AppDomainSetup();

            domainSetup.ApplicationBase = AppDomain.CurrentDomain.BaseDirectory;
            domainSetup.ShadowCopyFiles = "false";
            string name;
            lock (typeof(DomainAssemblyLoader))
            {
                name = "AssemblyLoader" + _index.ToString();
                _index++;
            }

            _domain = AppDomain.CreateDomain(name, null, domainSetup);
           
            AssemblyName currentAssembly = Assembly.GetExecutingAssembly().GetName();
            _domain.Load(Assembly.GetExecutingAssembly().GetName());

        }

        private DomainAssemblyLoader CreateDomainLoader()
        {
            DomainAssemblyLoader rs = (DomainAssemblyLoader)_domain.CreateInstanceAndUnwrap(Assembly.GetExecutingAssembly().GetName().FullName, typeof(DomainAssemblyLoader).FullName);
            return rs;

        }



        public AssemblyName Load(string assemblyString)
        {
            return CreateDomainLoader().Load(assemblyString);
            
        }

        public AssemblyName LoadFrom(string assemblyFile)
        {
            return CreateDomainLoader().LoadFrom(assemblyFile);
            
        }

        public string GetImageRuntimeVersion(AssemblyName assemblyRef)
        {
            return CreateDomainLoader().GetImageRuntimeVersion(assemblyRef);
        }

        public Type[] GetTypes(AssemblyName assemblyRef)
        {
            try
            {
                return CreateDomainLoader().GetTypes(assemblyRef);
            }
            catch
            {
                return new Type[0];
            }
        }

        public Type GetType(AssemblyName assembly, string typeName, bool throwErrors = false, bool ignoreCase = false)
        {
            return CreateDomainLoader().GetType(assembly, typeName, throwErrors, ignoreCase);
           
        }

        public bool IsInGAC(AssemblyName assemblyRef)
        {
            return CreateDomainLoader().IsInGAC(assemblyRef);
        }

      

       

        public AssemblyName[] ReflectionOnlyGetReferenceAssemblyNames(string location)
        {

            return CreateDomainLoader().GetReferenceAssemblyNames(location);
        }


        #region IDisposable Members

        public void Dispose()
        {
            AppDomain.Unload(_domain);
        }

        #endregion


       

       
    }

    public class DomainAssemblyLoader : MarshalByRefObject
    {
        public DomainAssemblyLoader()
        {
         
          
        }

        public AssemblyName Load(string assemblyString)
        {
#pragma warning disable 0618
            Assembly asm = Assembly.LoadWithPartialName(assemblyString);
#pragma warning restore 0618
            return asm.GetName();
        }

        public AssemblyName LoadFrom(string assemblyFile)
        {
            Assembly asm = Assembly.LoadFrom(assemblyFile);
            return asm.GetName();
        }

        public bool IsInGAC(AssemblyName assemblyRef)
        {
            var query = from dir in GlobalAssemblyCache.GetGACFolders()
                        let file = LongPath.Combine(dir, assemblyRef.Name + ".dll")
                        where LongPathFile.Exists(file) && Assembly.ReflectionOnlyLoadFrom(file).FullName == assemblyRef.FullName
                        select file;
            return query.Any();
        }

        public bool IsInFramework(AssemblyName assemblyRef)
        {
            bool rs = false;
            string file = LongPath.Combine(GlobalAssemblyCache.FrameworkFolder, assemblyRef.Name + ".dll");
            if (LongPathFile.Exists(file))
            {
                rs = Assembly.ReflectionOnlyLoadFrom(file).FullName == assemblyRef.FullName;
            }

            return rs;


        }

        public AssemblyName[] GetReferenceAssemblyNames(string location)
        {
            Assembly assembly = Assembly.Load(location);
            return assembly.GetReferencedAssemblies();
        }

        public Type[] GetTypes(AssemblyName assemblyRef)
        {
            Assembly assembly = Assembly.Load(assemblyRef);
            return assembly.GetTypes();
        }

        public Type GetType(AssemblyName assemblyRef, string typeName, bool throwErrors, bool ignoreCase)
        {
            Assembly assembly = Assembly.Load(assemblyRef);
            return assembly.GetType(typeName, throwErrors, ignoreCase);
        }

        public string GetImageRuntimeVersion(AssemblyName assemblyRef)
        {
            Assembly assembly = Assembly.Load(assemblyRef);
            return assembly.ImageRuntimeVersion;
        }
    }
}
