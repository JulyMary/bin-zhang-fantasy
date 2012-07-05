using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Fantasy.Jobs
{
    public class TaskItemAggregateReader : ObjectWithSite, ITagValueProvider
    {
        #region ITagValueProvider Members

        public char Prefix
        {
            get { return '@'; }
        }

        public string GetTagValue(string tag, IDictionary<string, object> context)
        {
            string[] expr = tag.Split(new char[] {'.'}, 2, StringSplitOptions.RemoveEmptyEntries);

            IJob job = (IJob)this.Site.GetService(typeof(IJob));
            
            TaskItem[] items = job.GetEvaluatedItemsByCatetory(expr[0]);

            IEnumerable<string> values;
            if(expr.Length > 1)
            {
                values = items.Select(x => x[expr[1]]);
            }
            else
            {
                values = items.Select(x=>x.Name);
            }

            return String.Join(";", values.ToArray());
        }

        public bool HasTag(string tag, IDictionary<string, object> context)
        {
            return true;
        }

        public bool IsEnabled(IDictionary<string, object> context)
        {
            return (bool)context.GetValueOrDefault("EnableTaskItemReader", true) &&
                this.Site != null && this.Site.GetService(typeof(IJob)) != null;
        }

        #endregion
    }
 
 
}
