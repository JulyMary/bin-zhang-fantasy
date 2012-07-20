using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Fantasy.ServiceModel;
using System.Threading;
using System.IO;
using System.Text.RegularExpressions;

namespace Fantasy.Jobs.Tasks
{
    [Task("execute", Consts.XNamespaceURI, Description="Execute an external process with command line")] 
    public class ExecuteTask : ObjectWithSite, ITask
    {

        public ExecuteTask()
        {
            Timeout = TimeSpan.MaxValue;
            this.WorkingDirectory = string.Empty;
            this.Arguments = string.Empty;
            this.WaitForExit = false;
            this.UseShellExecute = false;
        }
        #region ITask Members

        public bool Execute()
        {
            ILogger logger = this.Site.GetService<ILogger>();
            ProcessStartInfo si = new ProcessStartInfo()
            {
                FileName = this.FileName,
                Arguments = this.Arguments,
                CreateNoWindow = true,
                ErrorDialog = false,
                WorkingDirectory = this.WorkingDirectory,
                RedirectStandardError = this.WaitForExit && !this.UseShellExecute,
                RedirectStandardOutput = this.WaitForExit && !this.UseShellExecute,
                UseShellExecute = false
            };

            Process process = Process.Start(si);

          

            if (this.WaitForExit)
            {
                WaitProcess(process);
            }

            return true;

            
        }


        private void WriteOutput(object state)
        {
            try
            {
                ILogger logger = this.Site.GetService<ILogger>();
                StreamReader reader = (StreamReader)state;
                Regex regex = new Regex(@"\{\d+(:[^\}]*)?\}");
                string text;

                do
                {
                    text = reader.ReadLine();
                    if (text != null)
                    {
                        text = regex.Replace(text, string.Empty);
                        logger.SafeLogMessage("Execute", text);
                    }
                } while (text != null);
            }
            catch
            {
            }

        }

        private Thread _outputThread;

        private Thread _errorThread;
       
        private void WaitProcess(Process process)
        {


            this._outputThread =  ThreadFactory.CreateThread(WriteOutput).WithStart(process.StandardOutput);
            this._errorThread = ThreadFactory.CreateThread(WriteOutput).WithStart(process.StandardError);
            DateTime startTime = DateTime.Now;
            bool exited = false;
            do
            {
                DateTime now = DateTime.Now;
                TimeSpan consumed = now - startTime;

                TimeSpan waitTime = Timeout - consumed;

                if (waitTime > _refreshInterval)
                {
                    waitTime = _refreshInterval;
                }

                try
                {
                    exited = process.HasExited;
                }
                catch
                {
                }

                if (!exited)
                {

                    exited = process.WaitForExit(waitTime.Milliseconds);
                }

            } while (!exited);

            try
            {
                this.ExitCode = process.ExitCode;
            }
            catch
            {
            }
        }

        #endregion

        private readonly static TimeSpan _refreshInterval = TimeSpan.FromSeconds(15); 
        
        [TaskMember("file", Flags=TaskMemberFlags.Input | TaskMemberFlags.Required, Description="The application to start.")] 
        public string FileName { get; set; }

        [TaskMember("arguments", Description="The command-line arguments to use when starting the application.")]
        public string Arguments { get; set; }

        [TaskMember("exitCode", Flags=TaskMemberFlags.Output, Description="The code that the associated process specified when it terminated.")]
        public int ExitCode { get; set; }

        [TaskMember("waitForExit", Description="true if the task to wait for application to exit; otherwise false.")]
        public bool WaitForExit { get; set; }

        [TaskMember("workingDirectory", Description="The initial directory for the process to be started.")]
        public string WorkingDirectory { get; set; }

        [TaskMember("timeout", Description="The amount of time to wai for application to exit." )]
        public TimeSpan Timeout { get; set; }

        [TaskMember("useShellExecute")]
        public bool UseShellExecute { get; set; }

        

    }
}
