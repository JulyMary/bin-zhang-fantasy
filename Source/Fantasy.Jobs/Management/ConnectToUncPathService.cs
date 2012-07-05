using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.IO;
using Fantasy.ServiceModel;

namespace Fantasy.Jobs.Management
{
    public class ConnectToUncPathService : AbstractService
    {

        public override void InitializeService()
        {
            this.ConnectTo(JobManagerSettings.Default.JobDirectoryFullPath, JobManagerSettings.Default.JobDirectoryUser, JobManagerSettings.Default.JobDirectoryPassword);  

            base.InitializeService();
        }

        private void ConnectTo(string path, string user, string password)
        {
            ILogger logger = this.Site.GetService<ILogger>();
            Uri uri = new Uri(path);
            if (uri.IsUnc)
            {
                bool connected = false;
                try
                {
                    connected = LongPathDirectory.Exists(path);
                }
                catch
                {
                }
                if (!connected)
                {
                    if (logger != null)
                    {
                        logger.LogMessage("Unc", "Connect to UNC path {0}", path);
                    }
                    password = Encryption.Decrypt(password);
                    WNet.AddConnection(path, user, password);
                }
                else
                {
                    if (logger != null)
                    {
                        logger.LogMessage("Unc", "UNC path {0} has already been connected.", path);
                    }
                }
            }
        }
    }
}
