using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Text.RegularExpressions;
using System.ServiceProcess;


namespace Fantasy.Jobs.JobService
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();

            string svc = CommandArgumentsHelper.GetValue("svc");
            if (!string.IsNullOrEmpty(svc))
            {
                this.FantasyJobServiceInstaller.DisplayName = svc;
                this.FantasyJobServiceInstaller.ServiceName = svc;
            }

            string st = CommandArgumentsHelper.GetValue("start");
            if (!string.IsNullOrEmpty(st))
            {
                ServiceStartMode m = (ServiceStartMode)Enum.Parse(typeof(ServiceStartMode), st);
                this.FantasyJobServiceInstaller.StartType = m;

            }

            
        }
    }
}
