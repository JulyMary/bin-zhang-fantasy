using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.Jobs
{
    public class EnvironmentVariablesReader : ITagValueProvider
    {

        #region ITagValueProvider Members

        public string GetTagValue(string tag, IDictionary<string, object> context)
        {
            return Environment.ExpandEnvironmentVariables(string.Format("%{0}%", tag));
        }

        public bool HasTag(string tag, IDictionary<string, object> context)
        {
            return Environment.GetEnvironmentVariables().Contains(tag);
        }

        public char Prefix
        {
            get { return '$'; }
        }

     

        public bool IsEnabled(IDictionary<string, object> context)
        {
            return true;
        }

        #endregion
    }
}
