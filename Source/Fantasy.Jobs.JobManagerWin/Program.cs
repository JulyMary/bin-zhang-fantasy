using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;

namespace Fantasy.Jobs.JobManagerWin
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (String.IsNullOrEmpty(Environment.GetEnvironmentVariable("ProgramData")))
            {
                Environment.SetEnvironmentVariable("ProgramData", Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), EnvironmentVariableTarget.Process);
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new JobManagerMainForm());
        }
    }
}
