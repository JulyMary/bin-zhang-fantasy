using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.IO;

namespace Fantasy.Jobs
{
    [DataContract]
    [Serializable]
    public class JobTemplate
    {
        public JobTemplate()
        {
            this.Name = string.Empty;
        }

        [DataMember]
        public int id { get; set; }

        [DataMember]
        public string Location { get; set; }

        [DataMember]
        public string Content { get; set; }

        [DataMember]
        public string Name { get; set; }


        [DataMember]
        public bool Valid { get; set; }

        public string ToAbsolutPath(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path"); 
            }
            string rs = path;
            if (!Path.IsPathRooted(rs))
            {
                string dir = Path.GetDirectoryName(this.Location);
                rs = dir + Path.DirectorySeparatorChar + rs;

                rs = Path.GetFullPath(path); 
            }

            return rs;
            
        }
    }
}
