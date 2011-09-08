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
            domainSetup.ShadowCopyFiles = "true";
            string name;
            lock (typeof(AssemblyLoader))
            {
                name =  "AssemblyLoader" + _index.ToString();
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



        public Assembly ReflectionOnlyLoad(string assemblyString)
        {
            return CreateDomainLoader().ReflectionOnlyLoad(assemblyString);
        }

        public Assembly ReflectionOnlyLoadFrom(string assemblyFile)
        {
            return CreateDomainLoader().ReflectionOnlyLoadFrom(assemblyFile);
        }

        public Type[] ReflectionOnlyGetTypes(Assembly assembly)
        {
            return CreateDomainLoader().ReflectionOnlyGetTypes(assembly); 
        }

        public Type ReflectionOnlyGetType(Assembly assembly, string typeName, bool throwErrors = false, bool ignoreCase = false)
        {
            return CreateDomainLoader().ReflectionOnlyGetType(assembly, typeName, throwErrors, ignoreCase );
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
            AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += new ResolveEventHandler(ReflectionOnlyAssemblyResolve);
          
        }
       
        public Assembly ReflectionOnlyLoad(string assemblyString)
        {
            return Assembly.ReflectionOnlyLoad(assemblyString);
        }

        public Assembly ReflectionOnlyLoadFrom(string assemblyFile)
        {
            return Assembly.ReflectionOnlyLoadFrom(assemblyFile);
        }


        public Type[] ReflectionOnlyGetTypes(Assembly assembly)
        {
             return assembly.GetTypes();
        }

        private Assembly ReflectionOnlyAssemblyResolve(object sender, ResolveEventArgs args)
        {
            Assembly rs = null;

            if (!args.RequestingAssembly.GlobalAssemblyCache)
            {
                string dir = LongPath.GetDirectoryName(args.RequestingAssembly.CodeBase);
                AssemblyName an = new AssemblyName(args.Name);
                string file = LongPath.Combine(dir, an.Name + ".dll");
                if (LongPathFile.Exists(file))
                {
                    rs = Assembly.ReflectionOnlyLoadFrom(file);
                }

            }
            if(rs == null)
            {
                return Assembly.ReflectionOnlyLoad(args.Name); 
            }

            return rs;
        }

        public Type ReflectionOnlyGetType(Assembly assembly, string typeName, bool throwErrors, bool ignoreCase)
        {
            return assembly.GetType(typeName, throwErrors, ignoreCase);
        }
    }
}
