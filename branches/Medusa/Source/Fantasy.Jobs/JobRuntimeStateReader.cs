using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Fantasy.XSerialization;

namespace Fantasy.Jobs
{
    public class JobRuntimeStateReader :ObjectWithSite, ITagValueProvider 
    {
        #region ITagValueProvider Members

        public char Prefix
        {
            get { return '#'; }
        }

        public string GetTagValue(string tag, IDictionary<string, object> context)
        {
            string rs;
            this.TryGetValue(tag, context, out rs);
            return rs;
        }


        private bool TryGetValue(string name, IDictionary<string, object> context, out string value)
        {
            bool rs = false;
            value = null;
            if (this.Site != null)
            {

                string[] names = name.Split(new char[] { '.' }, 2);
                name = names[0];

                string meta = names.Length > 1 ? names[1] : null;

                IJob job = (Job)this.Site.GetService(typeof(IJob));

                object o;

                rs = job.RuntimeStatus.TryGetValue(name, out o);

                if (rs)
                {
                    if (o is TaskItem)
                    {
                        TaskItem item = (TaskItem)o;
                        value = meta != null ? item[meta] : item.Name;
                    }
                    else if(meta != null)
                    {
                        TaskItem item = job.GetEvaluatedItemByName((string)o);
                        if(item != null)
                        {
                            value = item[meta];
                        }
                    }
                    else if(o != null)
                    {
                        TypeConverter cvt = XHelper.Default.CreateXConverter(o.GetType());
                        value = cvt.ConvertToString(o); 
                    }
                }
                
            }

           
            return rs;
        }

        public bool HasTag(string tag, IDictionary<string, object> context)
        {

            string value;
            return this.TryGetValue(tag, context, out value);
           
        }

        #endregion

        #region ITagValueProvider Members


        public bool IsEnabled(IDictionary<string, object> context)
        {
            return (bool)context.GetValueOrDefault("EnableTaskItemReader", true) &&
                this.Site != null && this.Site.GetService(typeof(IJob)) != null;
        }

        #endregion
    }
}
