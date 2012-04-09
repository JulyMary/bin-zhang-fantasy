using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.WebPages;
using Fantasy.Web.Mvc.Properties;
using System.Reflection;

namespace Fantasy.Web.Mvc
{
    public class CompiledVirtualPathFactory : ObjectWithSite, IVirtualPathFactory
    {

        public CompiledVirtualPathFactory()
        {
            this._virtualPathTypes = (from assemblyName in Settings.Default.CompiledViewAssemblyNames.Cast<string>()
#pragma warning disable 0618
                                      let assembly = Assembly.LoadWithPartialName(assemblyName)
#pragma warning restore 0618
                                      from type in assembly.GetTypes()
                                      where typeof(WebPageRenderingBase).IsAssignableFrom(type)
                                      let pageVirtualPath = type.GetCustomAttributes(inherit: false).OfType<PageVirtualPathAttribute>().FirstOrDefault()
                                      where pageVirtualPath != null
                                      select new { path = pageVirtualPath.VirtualPath, type = type }
                        ).ToDictionary(t => t.path, t => t.type, StringComparer.OrdinalIgnoreCase);
        }


        private IDictionary<string, Type> _virtualPathTypes = new Dictionary<string, Type>();
        #region IVirtualPathFactory Members

        public object CreateInstance(string virtualPath)
        {
            Type t = this._virtualPathTypes.GetValueOrDefault(virtualPath);
            return t != null ? Activator.CreateInstance(t) : null;
        }

        public bool Exists(string virtualPath)
        {
            return _virtualPathTypes.ContainsKey(virtualPath); 
        }

        #endregion
    }
}
