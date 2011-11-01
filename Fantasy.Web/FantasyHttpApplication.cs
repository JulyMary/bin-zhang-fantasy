using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Fantasy.IO;
using Fantasy.AddIns;
using Fantasy.ServiceModel;

namespace Fantasy.Web
{
    public class FantasyHttpApplication : HttpApplication
    {

        protected virtual void Application_Start()
        {

            string dir = LongPath.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);

            DefaultAddInTree.Initialize(LongPathDirectory.EnumerateAllFiles(dir, "*.addin.xaml"));

            IEnumerable<object> services = AddInTree.Tree.GetTreeNode("fantasy/services").BuildChildItems<object>(null, ServiceManager.Services);
            ServiceManager.Services.InitializeServices(services.ToArray());

            foreach (ICommand command in AddInTree.Tree.GetTreeNode("fantasy/startup").BuildChildItems<ICommand>(null, ServiceManager.Services))
            {
                command.Execute(null);
            }

           
            AreaRegistration.RegisterAllAreas();
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }

        protected virtual void Application_End()
        {

        }

        protected virtual void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
           
        }

        protected virtual void RegisterRoutes(RouteCollection routes)
        {
            

        }
    }
}
