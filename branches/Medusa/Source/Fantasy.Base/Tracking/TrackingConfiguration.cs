using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Threading;

namespace Fantasy.Tracking
{
    public static  class TrackingConfiguration
    {

        private static List<ServiceHost> _hosts = new List<ServiceHost>();

        public static void StartTrackingService()
        {

            Thread thread = ThreadFactory.CreateThread(()=>{
            _hosts.AddRange(new ServiceHost[] 
            {
                new ServiceHost(typeof(TrackProviderService)), 
                new ServiceHost(typeof(TrackManagerService)), 
                new ServiceHost(typeof(TrackListenerService))
            });

            foreach (ServiceHost host in _hosts)
            {
                host.Open();
            }}).WithStart();
           
          
            thread.Join();
            
        }

        public static void CloseTrackingService()
        {
            foreach (ServiceHost host in _hosts)
            {
                host.Close();
            }
        }
    }
}
