using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.ServiceModel;
using System.Web.Mvc;
using Fantasy.AddIns;

namespace Fantasy.Web.Mvc
{
    public class ChainedViewEngine : ServiceBase, IViewEngine
    {

        private IViewEngine[] _viewEngines;
        private Dictionary<IView, IViewEngine> _views = new Dictionary<IView, IViewEngine>();
        private object _syncRoot = new object();

        public override void InitializeService()
        {
            this._viewEngines = AddInTree.Tree.GetTreeNode("fantasy/web/viewengines").BuildChildItems<IViewEngine>(this, this.Site).ToArray();

            base.InitializeService();
        }

        #region IViewEngine Members

        public ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
        {
            ViewEngineResult rs = null;
            foreach (IViewEngine engine in this._viewEngines)
            {
                rs = engine.FindPartialView(controllerContext, partialViewName, useCache);
                if (rs != null)
                {
                    lock (_syncRoot)
                    {
                        _views.Add(rs.View, engine);
                    }

                    break;
                }
            }


            return rs;
        }

        public ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            ViewEngineResult rs = null;
            foreach (IViewEngine engine in this._viewEngines)
            {
                rs = engine.FindView(controllerContext, viewName, masterName, useCache);
                if (rs != null)
                {
                    lock (_syncRoot)
                    {
                        _views.Add(rs.View, engine);
                    }
                    break;
                }
            }
            return rs;
        }

       

        public void ReleaseView(ControllerContext controllerContext, IView view)
        {
            IViewEngine engine = this._views.GetValueOrDefault(view);
            if (engine != null)
            {
                engine.ReleaseView(controllerContext, view);
                lock (_syncRoot)
                {
                    this._views.Remove(view);
                }
               
            }
        }

        #endregion
    }
}
