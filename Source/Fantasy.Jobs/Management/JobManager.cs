using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Fantasy.ServiceModel;

namespace Fantasy.Jobs.Management
{
    public class JobManager : MarshalByRefObject,  IJobManager 
    {
        private JobManager()
        {

        }


        public override object InitializeLifetimeService()
        {
            return null;
        }

        private static JobManager _default = new JobManager();
        public static JobManager Default
        {
            get
            {
                return _default;
            }
        }

        private ServiceContainer _serviceContainer = new ServiceContainer();
        
        public void Start(IServiceProvider parentServices = null)
        {

            List<object> services = new List<object>(AddIn.CreateObjects("jobService/services/service"));
            services.Add(this);
            _serviceContainer.InitializeServices(parentServices, services.ToArray());

            object[] commands = AddIn.CreateObjects("jobService/startupCommands/command");

            foreach (ICommand command in commands)
            {
                if (command is IObjectWithSite)
                {
                    ((IObjectWithSite)command).Site = this;
                }
                command.Execute(null);
            }
        }

        public void Stop()
        {
            this._serviceContainer.UninitializeServices(); 
        }

        #region IServiceProvider Members

        public object GetService(Type serviceType)
        {
            return _serviceContainer.GetService(serviceType); 
        }

        #endregion
    }
}
