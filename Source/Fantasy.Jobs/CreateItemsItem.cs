using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.XSerialization;
using System.Collections.Specialized;
using System.Xml.Linq;

namespace Fantasy.Jobs
{
    [XSerializable("taskitem", NamespaceUri = Consts.XNamespaceURI)]
    public class CreateItemsItem : IConditionalObject, IXSerializable
    {
        public string Name { get; set; }

        public string Category { get; set; }

        public string Condition { get; set; }

        private NameValueCollection _metaData = new NameValueCollection(StringComparer.OrdinalIgnoreCase);
        public NameValueCollection MetaData
        {
            get
            {
                return _metaData;
            }
        }


        public void Load(IServiceProvider context, XElement element)
        {
            this.Name = (string)element.Attribute("name");
            this.Condition = (string)element.Attribute("condition");
            this.Category = element.Name.LocalName;
            foreach (XElement child in element.Elements())
            {
                this._metaData[child.Name.LocalName] = child.Value;
            }
        }

        public void Save(IServiceProvider context, XElement element)
        {
            element.SetAttributeValue("name", this.Name);
            if (!string.IsNullOrEmpty(this.Condition))
            {
                element.SetAttributeValue("condition", this.Condition);
            }
            foreach (string key in this._metaData.AllKeys)
            {
                XElement child = new XElement(element.Name.Namespace + key, this._metaData[key]);

                element.Add(child);
            }
        }
 
    }
}
