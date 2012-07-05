using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Fantasy.Jobs.Management;
using Fantasy.ServiceModel;

namespace Fantasy.Jobs.Solar
{
    [ServiceBehavior(ConcurrencyMode=ConcurrencyMode.Reentrant, InstanceContextMode=InstanceContextMode.PerSession, Namespace=Consts.JobServiceNamespaceURI)]
    public class SatelliteHandler : ISatelliteHandler
    {
        public SatelliteHandler()
        {

        }

        private ISatellite _satellite;
        private string _name;
        #region ISatelliteHandler Members

        public void Connect(string name)
        {
            _name = name;
            _satellite = OperationContext.Current.GetCallbackChannel<ISatellite>();
            SatelliteManager manager = JobManager.Default.GetRequiredService<SatelliteManager>();
            manager.RegisterSatellite(name, _satellite);
        }

        

        public void Echo()
        {
            SatelliteManager manager = JobManager.Default.GetRequiredService<SatelliteManager>();
            if (!manager.IsValid(_satellite))
            {
                throw new FaultException<CallbackExpiredException>(new CallbackExpiredException(), new FaultReason("Satellite handle is invalid."));
            }
            else
            {
                manager.UpdateEchoTime(_name);
            }
        }

        public void Disconnect()
        {
            SatelliteManager manager = JobManager.Default.GetRequiredService<SatelliteManager>();
            manager.UnregisterSatellite(_satellite);
        }

        #endregion

     
    }
}
