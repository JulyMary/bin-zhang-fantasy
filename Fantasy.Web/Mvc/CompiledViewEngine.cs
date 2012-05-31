using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Reflection;
using System.Web.WebPages;
using Fantasy.Web.Properties;
using Fantasy.Reflection;

namespace Fantasy.Web.Mvc
{
    public class CompiledViewEngine : ObjectWithSite, IViewEngine
    {
        public CompiledViewEngine()
        {
            this.FileExtensions = new string[] { "cshtml" };

            this._assemblies = (from assemblyName in Settings.Default.CompiledViewAssemblyNames.Cast<string>()
#pragma warning disable 0618
                                select Assembly.LoadWithPartialName(assemblyName)).ToArray();
#pragma warning restore 0618
        }

        private Assembly[] _assemblies = null;

        private Dictionary<string, Type> _viewTypes = new Dictionary<string, Type>();
        

        #region IViewEngine Members

        public ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
        {
            return FindViewInternal(controllerContext, partialViewName, false); 
        }

        public ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            return FindViewInternal(controllerContext, viewName, !controllerContext.IsChildAction); 
        }

        private ViewEngineResult FindViewInternal(ControllerContext controllerContext, string viewName, bool runViewStartPages)
        {
            ViewEngineResult rs = null;
            List<string> searchedLocations = new List<string>();
            Type viewType = this.FindViewType(controllerContext, viewName, searchedLocations);
            if (viewType != null)
            {
                rs = new ViewEngineResult(new CompiledMvcView(viewType, runViewStartPages, this.FileExtensions), this);
            }
            else
            {
                return new ViewEngineResult(searchedLocations);
            }

            return rs;
        }


        public string[] FileExtensions { get; set; }

        private Type FindViewType(ControllerContext controllerContext, string viewName, List<string> searchedLocation)
        {

            lock (this._viewTypes)
            {
                string key = controllerContext.Controller.ToString() + "|" + viewName;
                if (!this._viewTypes.ContainsKey(key))
                {
                    Type rs = null;
                    Type controllerType = controllerContext.Controller.GetType();
                    Assembly asm = controllerType.Assembly;
                    string rootNamespace = ((RootNamespaceAttribute)asm.GetCustomAttributes(typeof(RootNamespaceAttribute), true).Single()).RootNamespace;
                    string controllerName = controllerType.Name.Substring(0, controllerType.Name.Length - "Controller".Length);
                    string ns = controllerType.Namespace;
                    if (ns.EndsWith(".Controllers"))
                    {
                        ns = ns.Substring(0, ns.Length - ".Controllers".Length);

                        string viewTypeName = string.Format("{0}.Views.{1}.{2}", ns, controllerName, viewName);
                        searchedLocation.Add(ViewTypeNameToVirtualPath(viewTypeName, rootNamespace)); 
                        rs = asm.GetType(viewTypeName, false, true);

                        if (rs == null)
                        {
                           

                            while (rs == null && ns.Length > 0)
                            {
                                viewTypeName = string.Format("{0}.Views.Shared.{1}", ns, viewName);
                                searchedLocation.Add(ViewTypeNameToVirtualPath(viewTypeName, rootNamespace)); 
                                rs = asm.GetType(viewTypeName);
                                if (rs == null)
                                {
                                    int dotIndex = ns.LastIndexOf('.');
                                    ns = dotIndex > 0 ? ns.Remove(dotIndex) : string.Empty;
                                }
                            }

                            if (rs == null)
                            {
                                foreach (Assembly asm2 in this._assemblies.Where(a => a != asm))
                                {
                                    ns = ((RootNamespaceAttribute)asm2.GetCustomAttributes(typeof(RootNamespaceAttribute), true).Single()).RootNamespace;
                                    viewTypeName = string.Format("{0}.Views.Shared.{1}", ns, viewName);
                                    searchedLocation.Add(ViewTypeNameToVirtualPath(viewTypeName, rootNamespace));
                                    rs = asm2.GetType(viewTypeName);
                                    if (rs != null)
                                    {
                                        break;
                                    }


                                }
                            }
                        }


                    }

                    this._viewTypes.Add(key, rs);
                    return rs;
                }
                else
                {
                    return this._viewTypes[key];
                }
            }
        }


        private string ViewTypeNameToVirtualPath(string typeName, string rootNamespace)
        {
            string rs = "~" + typeName.Substring(rootNamespace.Length).Replace('.', '/') + ".cshtml";
            return rs;
          
        }

        public void ReleaseView(ControllerContext controllerContext, IView view)
        {
            IDisposable disposable = view as IDisposable;
            if (disposable != null)
            {
                disposable.Dispose();
            }

        }

        #endregion

       
    }
}
