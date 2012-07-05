using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Remoting;
using System.Reflection;

namespace Fantasy.Jobs.Management
{
    public class StartRemotingCommand : ICommand
    {
        #region ICommand Members

        public object Execute(object args)
        {
            string configfile = Assembly.GetEntryAssembly().Location + ".config";
            RemotingConfiguration.Configure(configfile, false);
            //bool allow = RemotingConfiguration.IsActivationAllowed(typeof(RemoteJobManagerAccessor));
            return null;
        }

        #endregion
    }
}
