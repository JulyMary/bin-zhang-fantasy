using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Fantasy.ServiceModel;
using System.Web.Routing;
using System.Web;
using Fantasy.AddIns;

namespace Fantasy.Web.Mvc
{
    public class ChainedControllerFactory : ServiceBase, IControllerFactory 
    {
        const string CHAINEDCONTROLLERFACTORY = "__chainedControllerFactory";

        protected List<IControllerFactory> _factories = new List<IControllerFactory>();


        public override void InitializeService()
        {
            this._factories.AddRange(AddInTree.Tree.GetTreeNode("fantasy/web/controllerfactories").BuildChildItems<IControllerFactory>(this, this.Site));

            base.InitializeService();
           
        }

        public IController CreateController(RequestContext requestContext, string controllerName)
        {
            IController controller = null;
            foreach (IControllerFactory factory in _factories)
            {
                controller = factory.CreateController(requestContext, controllerName);
                if (controller != null)
                {
                    requestContext.HttpContext.Items[CHAINEDCONTROLLERFACTORY] = factory;
                    return controller;
                }
            }

            return null;
        }

        public void ReleaseController(IController controller)
        {
            IControllerFactory factory =
                HttpContext.Current.Items[CHAINEDCONTROLLERFACTORY] as IControllerFactory;
            if (factory != null)
                factory.ReleaseController(controller);
        }



        #region IControllerFactory Members


        public System.Web.SessionState.SessionStateBehavior GetControllerSessionBehavior(RequestContext requestContext, string controllerName)
        {
            return System.Web.SessionState.SessionStateBehavior.Default;
        }

        #endregion
    }
}
