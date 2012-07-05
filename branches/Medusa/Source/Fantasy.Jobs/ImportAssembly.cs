using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Reflection;
using Fantasy.XSerialization;

namespace Fantasy.Jobs
{
    [XSerializable("import", NamespaceUri = Consts.XNamespaceURI )]  
    public class ImportAssembly 
    {
        [XAttribute("name")]
        public string Name { get; set; }

        [XAttribute("path")]
        public string Path { get; set; }

        [XAttribute("src")]
        public string Src { get; set; }

        public Assembly LoadAssembly(IStringParser parser)
        {
            Assembly rs;
            if (!string.IsNullOrEmpty(this.Name))
            {
                if (parser != null)
                {
                    this.Name = parser.Parse(this.Name);
                }
                rs = Assembly.Load(this.Name);
            }
            else if (!string.IsNullOrEmpty(this.Path))
            {
                if (parser != null)
                {
                    this.Path = parser.Parse(this.Path);
                }

                string fullPath = this.Path;

                if (!System.IO.Path.IsPathRooted(fullPath))
                {
                    fullPath = Fantasy.IO.LongPath.Combine(Src, fullPath);
                }

                rs = Assembly.LoadFile(fullPath);
            }
            else
            {
                throw new InvalidOperationException("\"import\" element must has name or path attribute.");
            }

            return rs;

        }

        
    }
}
