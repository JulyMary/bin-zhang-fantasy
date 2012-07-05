using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using System.IO;

namespace Fantasy.Jobs.Web.Controllers
{
    public class XLogHelper
    {
        public static XElement Load(StreamReader reader)
        {
            List<XElement> nodes = new List<XElement>();
            string line;
            while (!reader.EndOfStream)
            {
                line = reader.ReadLine();

                try
                {
                    XElement node = XElement.Parse(line);
                    nodes.Add(node);
                }
                catch
                {
                }
            }

            XElement rs = new XElement("xlog");
            nodes.Reverse();
            foreach (XElement node in nodes)
            {
                rs.Add(node);
            }

            return rs;
           
           
        }
    }
}