using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Configuration;
using System.Threading;
using Fantasy.ServiceModel;

namespace Fantasy.Jobs.Management
{
    public class WCFHostService : AbstractService
    {
        private ServiceHost[] _serviceHosts;


        public override void InitializeService()
        {
            Thread thread = ThreadFactory.CreateThread(() =>
            {
                Type[] types = (Type[])AddIn.GetTypes("jobService/wcf/serviceHost");
                this._serviceHosts = new ServiceHost[types.Length];
                for (int i = 0; i < types.Length; i++)
                {
                    this._serviceHosts[i] = new ServiceHost(types[i]);
                    this._serviceHosts[i].Open();
                }
            }).WithStart();
           
           
            base.InitializeService();
        }

        public override void UninitializeService()
        {
            //foreach (ServiceHost host in this._serviceHosts)
            //{
            //    host.Close();
            //}
            base.UninitializeService();
        }
    }
}
