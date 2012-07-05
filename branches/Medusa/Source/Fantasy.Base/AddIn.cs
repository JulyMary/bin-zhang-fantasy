using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Xml;

namespace Fantasy
{
    public class AddIn : IConfigurationSectionHandler
    {
        #region IConfigurationSectionHandler Members

        public object Create(object parent, object configContext, System.Xml.XmlNode section)
        {
            return section;
        }

        #endregion


        public static T[] CreateObjects<T>(string xpath)
        {
            object[] temp = CreateObjects(xpath);
            T[] rs = new T[temp.Length];
            temp.CopyTo(rs, 0);
            return rs;
        }

        public static Type[] GetTypes(string xpath)
        {
           
            XmlElement root = (XmlElement)ConfigurationManager.GetSection("addin");

            xpath += "/@type";
            

            List<Type> rs = new List<Type>();

            foreach(XmlNode node in root.SelectNodes(xpath))
            {
                rs.Add(Type.GetType(node.Value, true));
            }
            return rs.ToArray();
        }

        public static object[] CreateObjects(string xpath)
        {
            Type[] types = GetTypes(xpath);
            object[] rs;
            if (types != null)
            {
                rs = new object[types.Length];
                for (int i = 0; i < rs.Length; i++)
                {
                    rs[i] = Activator.CreateInstance(types[i]);
                }
            }
            else
            {
                rs = new object[0];
            }

            return rs;
        }
    }
}
