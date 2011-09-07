using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

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


        #region IDisposable Members

        public void Dispose()
        {
            AppDomain.Unload(_domain);
        }

        #endregion
    }

    public class DomainAssemblyLoader : MarshalByRefObject
    {
        public Assembly ReflectionOnlyLoad(string assemblyString)
        {
            return Assembly.ReflectionOnlyLoad(assemblyString);
        }

        public Assembly ReflectionOnlyLoadFrom(string assemblyFile)
        {
            return Assembly.ReflectionOnlyLoadFrom(assemblyFile);
        }

    }
}
