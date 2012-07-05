using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.IO;
using System.Reflection;
using Fantasy.Jobs.Management;

namespace Fantasy.Jobs
{
    public class ReservedPropertiesTagProvider: ObjectWithSite, ITagValueProvider
    {
        #region ITagValueProvider Members

        public char Prefix
        {
            get { return '$'; }
        }


        private IJobEngine _engine;

        private IJobEngine Engine
        {
            get 
            {
                if (_engine == null)
                {
                    _engine = this.Site.GetRequiredService<IJobEngine>();
                    
                }
                return _engine;
            }
        }

        


        public string GetTagValue(string tag, IDictionary<string, object> context)
        {
            switch (tag.ToLower())
            {
                case "jobdir":
                    return this.Engine.JobDirectory;
                case "intermediatedir":
                    return this.Engine.JobDirectory;
                case "jobhostdir":
                    return LongPath.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                case "templatedir":
                    return this.Site.GetRequiredService<IJobManagerSettingsReader>().GetSetting<string>("JobTemplateDirectoryFullPath");
                case "jobid":
                    return this.Engine.JobId.ToString();
                case "template":
                    IJob job = this.Site.GetRequiredService<IJob>();
                    return job.TemplateName;
                default:
                    return "";
            }
        }

        public bool HasTag(string tag, IDictionary<string, object> context)
        {
            string[] tags = new string[] { "jobdir", "intermediatedir", "jobhostdir", "templatedir", "jobid", "templatename"};
            return Array.IndexOf(tags, tag.ToLower()) >= 0;
        }

        public bool IsEnabled(IDictionary<string, object> context)
        {
            return true;
        }

        #endregion
    }
}
