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
        //private AppDomain _domain;

        private static int _index = 0;
        public AssemblyLoader()
        {
            //AppDomainSetup domainSetup = new AppDomainSetup();

            //domainSetup.ApplicationBase = AppDomain.CurrentDomain.BaseDirectory;
            //domainSetup.ShadowCopyFiles = "false";
            //string name;
            //lock (typeof(AssemblyLoader))
            //{
            //    name =  "AssemblyLoader" + _index.ToString();
            //    _index++;
            //}

            //_domain = AppDomain.CreateDomain(name, null, domainSetup);
            ////_domain.ReflectionOnlyAssemblyResolve += new ResolveEventHandler(ReflectionOnlyAssemblyResolve);
            //AssemblyName currentAssembly = Assembly.GetExecutingAssembly().GetName();
            //_domain.Load(Assembly.GetExecutingAssembly().GetName());

        }

        //private DomainAssemblyLoader CreateDomainLoader()
        //{
        //    DomainAssemblyLoader rs = (DomainAssemblyLoader)_domain.CreateInstanceAndUnwrap(Assembly.GetExecutingAssembly().GetName().FullName, typeof(DomainAssemblyLoader).FullName);
        //    return rs;

        //}



        public Assembly ReflectionOnlyLoad(string assemblyString)
        {
            //return CreateDomainLoader().ReflectionOnlyLoad(assemblyString);
            return Assembly.Load(assemblyString);
        }

        public Assembly ReflectionOnlyLoadFrom(string assemblyFile)
        {
            //return CreateDomainLoader().ReflectionOnlyLoadFrom(assemblyFile);
            return Assembly.LoadFrom(assemblyFile);
        }

        public Type[] ReflectionOnlyGetTypes(Assembly assembly)
        {
            ////AssemblyName name = assembly.GetName();
            ////return CreateDomainLoader().ReflectionOnlyGetTypes(name);
            //lock (typeof(AssemblyLoader))
            //{
            //    AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += new ResolveEventHandler(ReflectionOnlyAssemblyResolve);
            //    try
            //    {
            //        if (!assembly.ReflectionOnly)
            //        {
            //            assembly = Assembly.ReflectionOnlyLoadFrom(assembly.CodeBase);
            //        }
            //       
                    
            //    }
            //    finally
            //    {
            //        AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve -= new ResolveEventHandler(ReflectionOnlyAssemblyResolve);
            //    }
            //}

            try
            {
                return assembly.GetTypes();
            }
            catch
            {
                return new Type[0];
            }
        }

        public Type ReflectionOnlyGetType(Assembly assembly, string typeName, bool throwErrors = false, bool ignoreCase = false)
        {
            return assembly.GetType(typeName, throwErrors, ignoreCase);

            //return CreateDomainLoader().ReflectionOnlyGetType(assembly, typeName, throwErrors, ignoreCase );
        }

        public AssemblyName ReflectionOnlyLoadNameFrom(string assemblyFile)
        {
            //return CreateDomainLoader().RefectionOnlyLoadNameFrom(assemblyFile);
            return Assembly.LoadFrom(assemblyFile).GetName();
        }

        public bool ReflectionOnlyIsInGAC(AssemblyName assemblyRef)
        {
            return Assembly.Load(assemblyRef.FullName).GlobalAssemblyCache;
        }

        public AssemblyName[] ReflectionOnlyGetReferenceAssemblyNames(AssemblyName assemblyRef)
        {
            Assembly assembly = Assembly.LoadFrom(assemblyRef.CodeBase);
            return assembly.GetReferencedAssemblies();
            //return CreateDomainLoader().ReflectionOnlyGetReferenceAssemblyNames(assemblyRef);
        }


        #region IDisposable Members

        public void Dispose()
        {
            //AppDomain.Unload(_domain);
        }

        #endregion


        private static Assembly ReflectionOnlyAssemblyResolve(object sender, ResolveEventArgs args)
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
            if (rs == null)
            {
                return Assembly.ReflectionOnlyLoad(args.Name);
            }

            return rs;
        }

       
    }

    public class DomainAssemblyLoader : MarshalByRefObject
    {
        public DomainAssemblyLoader()
        {
            AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += new ResolveEventHandler(ReflectionOnlyAssemblyResolve);
          
        }

        private static Assembly ReflectionOnlyAssemblyResolve(object sender, ResolveEventArgs args)
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
            if (rs == null)
            {
                return Assembly.ReflectionOnlyLoad(args.Name);
            }

            return rs;
        }
       
        public Assembly ReflectionOnlyLoad(string assemblyString)
        {
            return Assembly.ReflectionOnlyLoad(assemblyString);
        }

        public Assembly ReflectionOnlyLoadFrom(string assemblyFile)
        {
            return Assembly.ReflectionOnlyLoadFrom(assemblyFile);
        }

        public AssemblyName RefectionOnlyLoadNameFrom(string assemblyFile)
        {
            return Assembly.ReflectionOnlyLoadFrom(assemblyFile).GetName();
        }


        public Type[] ReflectionOnlyGetTypes(AssemblyName assemblyRef)
        {

            Assembly assembly = assemblyRef.CodeBase != null ? Assembly.ReflectionOnlyLoadFrom(assemblyRef.CodeBase) : Assembly.ReflectionOnlyLoad(assemblyRef.FullName); 
            return assembly.GetTypes();
        }

        

        public Type ReflectionOnlyGetType(Assembly assembly, string typeName, bool throwErrors, bool ignoreCase)
        {
            return assembly.GetType(typeName, throwErrors, ignoreCase);
        }

        public bool ReflectionOnlyIsInGAC(AssemblyName assemblyRef)
        {
            Assembly asm = Assembly.ReflectionOnlyLoadFrom(assemblyRef.CodeBase);
            return asm.GlobalAssemblyCache;
        }

        public AssemblyName[] ReflectionOnlyGetReferenceAssemblyNames(AssemblyName assemblyRef)
        {
            Assembly assembly = Assembly.ReflectionOnlyLoadFrom(assemblyRef.CodeBase);
            return assembly.GetReferencedAssemblies();
        }
    }
}
