using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

namespace Fantasy.Jobs.Management
{
    public sealed partial class JobManagerSettings 
    {
        public string JobTemplateDirectoryFullPath
        {
            get
            {
               return this.ExtractToFullPath(this.JobTemplateDirectory);
            }
        }


        private string ExtractToFullPath(string value)
        {
            string rs = Environment.ExpandEnvironmentVariables(value);

            if (!Path.IsPathRooted(rs))
            {
                Assembly asm = Assembly.GetEntryAssembly();
                string entryPath = Path.GetDirectoryName(asm.Location);

                rs = entryPath + Path.DirectorySeparatorChar  + rs;
            }

            return new Uri(rs).LocalPath;
        }

        public string LogDirectoryFullPath
        {
            get
            {
                return this.ExtractToFullPath(this.LogDirectory); 
            }
        }

        public string JobDirectoryFullPath
        {
            get
            {
                return this.ExtractToFullPath(JobDirectory); 
            }
        }

        public string JobHostFullPath
        {
            get
            {
                return this.ExtractToFullPath(this.JobHostPath);
            }
        }

        public string ScheduleDirectoryFullPath
        {
            get
            {
                return this.ExtractToFullPath(this.ScheduleDirectory);
            }
        }

        public string ScheduleTemplateDirectoryFullPath
        {
            get
            {
                return this.ExtractToFullPath(this.ScheduleTemplateDirectory); 
            }
        }


    }
}
