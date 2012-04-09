using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Reflection;
using System.Web.WebPages;
using Fantasy.Web.Mvc.Properties;

namespace Fantasy.Web.Mvc
{
    public class CompiledViewEngine : ObjectWithSite, IViewEngine
    {
        public CompiledViewEngine()
        {
            this.FileExtensions = new string[] { "cshtml" };
        }


        private Dictionary<string, Type> _viewTypes = new Dictionary<string, Type>();
        

        #region IViewEngine Members

        public ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
        {
            return FindViewInternal(controllerContext, partialViewName); 
        }

        public ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            return FindViewInternal(controllerContext, viewName); 
        }

        private ViewEngineResult FindViewInternal(ControllerContext controllerContext, string viewName)
        {
            ViewEngineResult rs = null;
            Type viewType = this.FindViewType(controllerContext, viewName);
            if (viewType != null)
            {
                rs = new ViewEngineResult(new CompiledMvcView(viewType, true, this.FileExtensions), this);
            }

            return rs;
        }


        public string[] FileExtensions { get; set; }

        private Type FindViewType(ControllerContext controllerContext, string viewName)
        {

            lock (this._viewTypes)
            {
                string key = controllerContext.Controller.ToString() + "|" + viewName;
                if (!this._viewTypes.ContainsKey(key))
                {
                    Type rs = null;
                    Type controllerType = controllerContext.Controller.GetType();
                    Assembly asm = controllerType.Assembly;
                    string controllerName = controllerType.Name;
                    string ns = controllerType.Namespace;
                    if (ns.EndsWith(".Controller"))
                    {
                        ns = ns.Substring(0, ns.Length - ".Controller".Length);

                        string viewTypeName = string.Format("{0}.Views.{1}.{2}", ns, controllerName, viewName);

                        rs = asm.GetType(viewTypeName);

                        if (rs == null)
                        {
                            while (rs == null && ns.Length > 0)
                            {
                                viewTypeName = string.Format("{0}.Views.Shared.{2}", ns, viewName);
                                rs = asm.GetType(viewTypeName);
                                if (rs == null)
                                {
                                    int dotIndex = ns.LastIndexOf('.');
                                    ns = dotIndex > 0 ? ns.Remove(dotIndex) : string.Empty;
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
