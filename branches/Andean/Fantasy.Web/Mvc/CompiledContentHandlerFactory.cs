using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.WebPages;
using Fantasy.ServiceModel;
using System.Reflection;
using Fantasy.Web.Properties;
using Fantasy.Reflection;
using System.IO;

namespace Fantasy.Web.Mvc
{
    public class CompiledContentHandlerFactory : ServiceBase, IVirtualPathFactory, IHttpHandlerFactory
    {


        public override void InitializeService()
        {

            this._assemblies =  (from assemblyName in Settings.Default.CompiledViewAssemblyNames.Cast<string>()
#pragma warning disable 0618
                                          select Assembly.LoadWithPartialName(assemblyName)).ToArray();
#pragma warning restore 0618

            base.InitializeService();
        }

        private Assembly[] _assemblies;


        #region IVirtualPathFactory Members

        public object CreateInstance(string virtualPath)
        {
            return this;
        }

        public bool Exists(string virtualPath)
        {
            Assembly asm;
            string name;
            GetResourceName(virtualPath, out asm, out name);
            return name != null;
        }


        private void GetResourceName(string virtualPath, out Assembly assembly, out string resourceName)
        {
            virtualPath = virtualPath.TrimStart('~');

            assembly = null;
            resourceName = null;

            string name = virtualPath.Replace('/', '.');
            foreach (Assembly asm in this._assemblies)
            {

                string rns = asm.GetCustomAttributes(typeof(RootNamespaceAttribute), true).Cast<RootNamespaceAttribute>().Single().RootNamespace;
                string rsName = rns + name;

                resourceName = asm.GetManifestResourceNames().SingleOrDefault(s=>string.Equals(s, rsName, StringComparison.OrdinalIgnoreCase));

                if(resourceName != null)
                {
                    assembly = asm;
                    return;
                }
            }
        }


        public IHttpHandler GetHandler(HttpContextBase context)
        {
            string virtualPath = context.Request.FilePath;
            Assembly asm;
            string name;
            GetResourceName(virtualPath, out asm, out name);
            Stream stream = asm.GetManifestResourceStream(name);
            return new CompiledContentHandler(name.Substring(name.LastIndexOf('.')), stream);
            
        }


        #endregion

        #region IHttpHandlerFactory Members

        IHttpHandler IHttpHandlerFactory.GetHandler(HttpContext context, string requestType, string url, string pathTranslated)
        {
            return this.GetHandler(new HttpContextWrapper(context));
        }

        void IHttpHandlerFactory.ReleaseHandler(IHttpHandler handler)
        {
            
        }

        #endregion


       
    }
}