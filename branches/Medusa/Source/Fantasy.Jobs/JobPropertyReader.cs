using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.Jobs
{
    public class JobPropertyReader : ObjectWithSite , ITagValueProvider 
    {
        #region ITagValueProvider Members

        public char Prefix
        {
            get { return '$'; }
        }

        public string GetTagValue(string tag, IDictionary<string, object> context)
        {
            Job job = (Job)this.Site.GetService(typeof(IJob));
            return job.GetProperty(tag); 

        }

        public bool HasTag(string tag, IDictionary<string, object> context)
        {
            Job job = (Job)this.Site.GetService(typeof(IJob));
            return job != null ? job.HasProperty(tag) : false;
        }

        public bool IsEnabled(IDictionary<string, object> context)
        {
            return this.Site != null && this.Site.GetService(typeof(IJob)) != null;
        }

        #endregion
    }
}
