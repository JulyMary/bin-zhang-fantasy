using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using Fantasy.Jobs.Management;
using Fantasy.ServiceModel;

namespace Fantasy.Jobs.JobService
{
    public partial class FantasyJobService : ServiceBase
    {

        private bool _stopping = false;
        public FantasyJobService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            
            System.IO.Directory.SetCurrentDirectory(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location));
            (new StartRemotingCommand()).Execute(null);
            JobManager.Default.Start(); 
        }

        protected override void OnStop()
        {
            if (!_stopping)
            {
                _stopping = true;
                JobManager.Default.Stop();
            }
        }

        protected override bool OnPowerEvent(PowerBroadcastStatus powerStatus)
        {

            ILogger logger = JobManager.Default.GetService<ILogger>();
            if (logger != null)
            {
                logger.LogMessage("Manager", "Job service received a power event with value {0}.", powerStatus);
            }

            return base.OnPowerEvent(powerStatus);
        }

        protected override void OnShutdown()
        {

            ILogger logger = JobManager.Default.GetService<ILogger>();
            if (logger != null)
            {
                logger.LogMessage("Manager", "System is shuting down.");
            }
            base.OnShutdown();

            if (!_stopping)
            {
                _stopping = true;
                JobManager.Default.Stop();
            }
        }
    }
}
