using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Tracking;

namespace Fantasy.Jobs.Management
{
    public class StartTrackingCommand : ICommand
    {
        #region ICommand Members

        public object Execute(object args)
        {
            TrackingConfiguration.StartTrackingService();
            return null;
        }

        #endregion
    }
}
