using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Threading;
using Fantasy.ServiceModel;

namespace Fantasy.Jobs
{
    public abstract class WCFSingletonService : AbstractService
    {
        private ServiceHost _serviceHost;

        public override void InitializeService()
        {
            
            Thread thread = ThreadFactory.CreateThread(() =>
            {
                _serviceHost = new ServiceHost(this);
                _serviceHost.Open();
            }).WithStart();
         
            thread.Join();
            base.InitializeService();
        }

        public override void UninitializeService()
        {

            base.UninitializeService();
            this._serviceHost.Close();
        }
    }
}
