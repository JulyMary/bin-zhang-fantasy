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

namespace Fantasy.Web.Mvc
{
    public class FantasyHttpApplication : HttpApplication
    {

        protected virtual void Application_Start()
        {


            WatchDependencyPaths();

            string dir = LongPath.GetDirectoryName(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

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

            this.BeginRequest += new EventHandler(Application_BeginRequest);
            this.EndRequest += new EventHandler(Application_EndRequest);
        }

        List<FileSystemWatcher> _dependencyWatchers = new List<FileSystemWatcher>();

        private void WatchDependencyPaths()
        {
            string root = LongPath.GetDirectoryName(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

            Regex regex = new Regex(@"(?<dir>.*)\\?(?<filter>[^\\]*)$", RegexOptions.RightToLeft);

            foreach (string path in DependencyPathSettings.Default.Path.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
            {
                Match match = regex.Match(path);

                string fullPath = LongPath.Combine(root, match.Groups["dir"].Value);
                string filter = match.Groups["filter"].Value;
                FileSystemWatcher watcher = new FileSystemWatcher(fullPath, filter);
                watcher.Changed += new FileSystemEventHandler(DependencyFileChanged);
                watcher.Created += new FileSystemEventHandler(DependencyFileChanged);
                watcher.Deleted += new FileSystemEventHandler(DependencyFileChanged);
                watcher.EnableRaisingEvents = true;
                _dependencyWatchers.Add(watcher);

            }
        }
        private bool _unloaded = false;
        void DependencyFileChanged(object sender, FileSystemEventArgs e)
        {
            if (!_unloaded)
            {
                _unloaded = true;
                HttpRuntime.UnloadAppDomain();
            }
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

            foreach (ICommand command in AddInTree.Tree.GetTreeNode("fantasy/web/request/start").BuildChildItems<ICommand>(null, ServiceManager.Services))
            {
                command.Execute(null);
            }
        }

        protected virtual void Application_End()
        {
            ServiceManager.Services.UninitializeServices();

        }

        protected virtual void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
           
        }

        protected virtual void RegisterRoutes(RouteCollection routes)
        {
            

        }
    }
}
