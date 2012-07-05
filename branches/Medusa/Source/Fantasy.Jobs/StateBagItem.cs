using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.XSerialization;
using System.Xml;
using System.ComponentModel;
using System.Collections;
using System.Xml.Linq;

namespace Fantasy.Jobs
{
    
    [XSerializable("state", NamespaceUri= Consts.XNamespaceURI)] 
    public class StateBagItem : IXSerializable 
    {
       
        public string Name { get; set; }

        private object _value;

        public object Value
        {
            get { return _value; }
            set { _value = value; }
        }


        #region IXSerializable Members

        void IXSerializable.Load(IServiceProvider context, XElement element)
        {
            this.Name = element.Name.LocalName;
            XNamespace ns = Consts.XNamespaceURI;
            if(! String.IsNullOrEmpty((string)element.Attribute(ns + "type")))
            {
                
                Type t = Type.GetType((string)element.Attribute(ns + "type"), true);
                if (t.IsDefined(typeof(XSerializableAttribute), false))
                {
                   
                    XSerializer ser = new XSerializer(t);
                    this.Value = ser.Deserialize(element);
                }
                else
                {
                    TypeConverter convert = XHelper.Default.CreateXConverter(t);
                    this.Value = convert.ConvertTo(element.Value, t); 
                }
            }

        }

        void IXSerializable.Save(IServiceProvider context, XElement element)
        {
 
            if (Value != null)
            {
                XNamespace ns = Consts.XNamespaceURI;
                Type t = Value.GetType();
                element.SetAttributeValue(ns + "type", string.Format("{0}, {1}" , t.FullName, t.Assembly.GetName().Name) );

                if (t.IsDefined(typeof(XSerializableAttribute), false))
                {
                    XSerializer ser = new XSerializer(t);
                    ser.Serialize(element, Value);
                }
                else
                {
                    TypeConverter convert = XHelper.Default.CreateXConverter(t);
                    string s = (string)convert.ConvertFrom(this.Value);
                    element.Value = s;
                }


            }
        }

        #endregion
    }

    public class StateBagItemsSerializer : IXCollectionSerializer
    {
        public void Save(IServiceProvider context, XElement element, IEnumerable collection)
        {
          
            foreach (StateBagItem item in collection)
            {
                XElement childElement = new XElement(element.Name.Namespace + item.Name);
                element.Add(childElement);
                ((IXSerializable)item).Save(context, childElement);
            }
        }

        public IEnumerable Load(IServiceProvider context, XElement element)
        {
            ArrayList rs = new ArrayList();
            foreach (XElement childElement in element.Elements())
            {
                StateBagItem item = new StateBagItem();
                ((IXSerializable)item).Load(context, childElement);
                rs.Add(item);
            }

            return rs;
        }
    }

}
