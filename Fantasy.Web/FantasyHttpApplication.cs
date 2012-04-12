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
using Fantasy.BusinessEngine;
using System.IO;
using System.Text.RegularExpressions;
using Fantasy.Web.Properties;
using Fantasy.BusinessEngine.Services;

namespace Fantasy.Web
{
    public class FantasyHttpApplication : HttpApplication
    {

        protected virtual void Application_Start()
        {
            string dir = LongPath.GetDirectoryName(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

            DefaultAddInTree.Initialize(LongPathDirectory.EnumerateAllFiles(dir, "*.addin.xaml"));

            IEnumerable<object> services = AddInTree.Tree.GetTreeNode("fantasy/services").BuildChildItems<object>(null, ServiceManager.Services);
            ServiceManager.Services.InitializeServices(services.ToArray());

            foreach (ICommand command in AddInTree.Tree.GetTreeNode("fantasy/startup").BuildChildItems<ICommand>(null, ServiceManager.Services))
            {
                command.Execute(null);
            }

            this.BeginRequest += new EventHandler(Application_BeginRequest);
            this.EndRequest += new EventHandler(Application_EndRequest);
        }

        void Application_EndRequest(object sender, EventArgs e)
        {

            foreach (ICommand command in AddInTree.Tree.GetTreeNode("fantasy/web/request/end").BuildChildItems<ICommand>(null, ServiceManager.Services))
            {
                command.Execute(null);
            }
            AutoInitServiceContainer services = (AutoInitServiceContainer)HttpContext.Current.Items[BusinessEngineContextServicesKey];
            services.UninitializeServices();
        }


        

        private const string BusinessEngineContextServicesKey = "BUSINESSENGINECONTEXTSERVICES";

        void Application_BeginRequest(object sender, EventArgs e)
        {
            AutoInitServiceContainer services = new AutoInitServiceContainer();
            services.InitializeServices(ServiceManager.Services, AddInTree.Tree.GetTreeNode("fantasy/context/services").BuildChildItems<object>(null, services).ToArray());
            BusinessEngineContext context = new BusinessEngineContext(services);

            HttpContext.Current.Items[BusinessEngineContextServicesKey] = services; 

            BusinessEngineContext.Current = context;


            IBusinessUserRoleService userRoleSvc = ServiceManager.Services.GetRequiredService<IBusinessUserRoleService>();
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                context.User = userRoleSvc.UserByName(HttpContext.Current.User.Identity.Name);
            }
            else
            {
                context.User = userRoleSvc.WellknownUsers.Guest;
            }

            foreach (ICommand command in AddInTree.Tree.GetTreeNode("fantasy/web/request/start").BuildChildItems<ICommand>(null, ServiceManager.Services))
            {
                command.Execute(null);
            }
        }

        protected virtual void Application_End()
        {
            ServiceManager.Services.UninitializeServices();

        }

      

       
    }
}
