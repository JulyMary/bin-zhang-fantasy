using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;
using Fantasy.IO;

namespace System.Xml.Linq
{
    public static class LongPathXNode
    {
        public static XElement LoadXElement(string url)
        {
            XElement rs;
            FileStream fs = LongPathFile.Open(url, FileMode.Open, FileAccess.Read, FileShare.Read);
            try
            {

                rs = XElement.Load(XmlReader.Create(fs));
            }
            finally
            {
                fs.Close();
            }
            return rs;
        }

        public static XDocument LoadXDocument(string url)
        {
            XDocument rs;
            FileStream fs = LongPathFile.Open(url, FileMode.Open, FileAccess.Read, FileShare.Read);
            try
            {
                rs = XDocument.Load(XmlReader.Create(fs));
            }
            finally
            {
                fs.Close();
            }

            return rs;
        }

        
    }
}
