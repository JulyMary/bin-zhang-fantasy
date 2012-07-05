using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Reflection;

namespace Fantasy.Jobs.Resources
{
    [Serializable]
    public class ResourceParameter
    {
        public ResourceParameter(string name, object values = null)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            this.Name = name;

           
            if (values != null)
            {
                if (values is IDictionary<string, string>)
                {
                    foreach (KeyValuePair<string, string> kv in (IDictionary<string, string>)values)
                    {
                        _values.Add(kv.Key, kv.Value);
                    }
                }
                else
                {
                    Type t = values.GetType();
                    foreach (PropertyInfo pi in t.GetProperties())
                    {
                        object v = pi.GetValue(values, null);
                        string s = v != null ? Convert.ToString(v) : null;
                        this._values.Add(pi.Name, s);
                    }
                }
                
            }
        }

        internal ResourceParameter()
        {

        }

        public string Name { get; internal set; }


        private Dictionary<string, string> _values = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        public Dictionary<string,string> Values
        {
            get { return _values; }
        }

        public override string ToString()
        {
            StringBuilder rs = new StringBuilder();
            rs.AppendFormat("name={0}", this.Name);
            foreach (KeyValuePair<string, string> item in this._values)
            {
                rs.AppendFormat(";{0}={1}", item.Key, item.Value);
            }

            return rs.ToString();

        }

    }
}
