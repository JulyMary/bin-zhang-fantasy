using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Properties;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml;

namespace Fantasy.XSerialization
{
    public class XSerializer
    {
        public XSerializer(Type targetType)
        {
            if (targetType == null)
            {
                throw new ArgumentNullException("type"); 
            }
            this.TargetType = targetType;
            this._map = XHelper.Default.GetTypeMap(targetType);
            
        }

        public IServiceProvider Context { get; set; }

        public Type TargetType { get; private set; }

        XTypeMap _map;

        public void Serialize(XElement element, object o)
        {
            this._map.Save(this.Context , element, o);
        }

        public XElement Serialize(object o, XmlSerializerNamespaces namespaces = null)
        {
            if (o == null)
            {
                throw new ArgumentNullException("o"); 
            }

            string ns = null;
            string name = null;
           
            name = this._map.SerializableAttribute.Name;
            ns = this._map.SerializableAttribute.NamespaceUri;
            if (string.IsNullOrEmpty(name))
            {
                throw new XException(String.Format(Resources.TypeMissingElementNameText, this.TargetType)); 
            }

          
            XElement rs = new XElement((XNamespace)ns + name);
            

            if (namespaces != null)
            {
                foreach (XmlQualifiedName qn in namespaces.ToArray())
                {
                    if (qn.Name != "")
                    {
                        rs.Add(new XAttribute(XNamespace.Xmlns + qn.Name, qn.Namespace));
                    }
                    else
                    {
                        rs.Add(new XAttribute("xmlns", qn.Namespace));
                    }
                   
                }
            }
            this._map.Save(Context, rs, o);
            return rs;
        }

        

        public object Deserialize(XElement element)
        {
            object rs = Activator.CreateInstance(this.TargetType);
            this.Deserialize(element, rs);
            return rs;
        }

        public void Deserialize(XElement element, object instance)
        {
            this._map.Load(this.Context, element, instance); 
        }
    }
}
