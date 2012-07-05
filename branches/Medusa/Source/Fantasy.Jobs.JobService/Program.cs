using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace Fantasy.Jobs.JobService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            if (String.IsNullOrEmpty(Environment.GetEnvironmentVariable("ProgramData")))
            {
                Environment.SetEnvironmentVariable("ProgramData", Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), EnvironmentVariableTarget.Process);
            }
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
			{ 
				new FantasyJobService() 
			};
            ServiceBase.Run(ServicesToRun);
        }
    }
}
