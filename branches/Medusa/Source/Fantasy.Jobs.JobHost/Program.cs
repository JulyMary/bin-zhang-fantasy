
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Fantasy.Jobs.Management;
using System.Threading;
using System.Xml;
using Fantasy.XSerialization;
using System.Xml.Linq;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Fantasy.Jobs.JobHost
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

             if (CommandArgumentsHelper.HasArgument("pid"))
             {
                 uint pm = UInt32.Parse(CommandArgumentsHelper.GetValue("pid"));
                 SetProcessAffinityMask(Process.GetCurrentProcess().Handle, pm);

             }
           
            //Console.ReadLine(); 
#if Mngr

            JobManager.Default.Start();

#else
            (new StartRemotingCommand()).Execute(null);
            (new RegisterIpcChannelCommand()).Execute(null);
#endif
            
            JobEngine engine = new JobEngine(new Guid(CommandArgumentsHelper.GetValue("id")));
            engine.Initialize();
#if Mngr
            
            MethodInvoker del = delegate { StartJob(engine); };
            del.BeginInvoke(null, null);
#endif
            engine.WaitForExit();
#if Mngr
            JobManager.Default.Stop(); 
#endif
        }

        [DllImport("kernel32.dll", EntryPoint = "SetProcessAffinityMask")]
        private static extern bool SetProcessAffinityMask(IntPtr hProcess,
           uint dwProcessAffinityMask);


        private static void StartJob(JobEngine engine)
        {
            string path = CommandArgumentsHelper.GetValue("s");
            XElement doc = XElement.Load(path);
           
            XSerializer ser = new XSerializer(typeof(JobStartInfo));
            JobStartInfo s = (JobStartInfo)ser.Deserialize(doc);
            if (CommandArgumentsHelper.HasArgument("resume"))
            {
                engine.Resume(s);
            }
            else
            {
                engine.Start(s);
            }
            

        }

    }
}
