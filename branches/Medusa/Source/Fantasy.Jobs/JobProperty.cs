using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.XSerialization;
using System.Collections;
using System.Xml.Linq;

namespace Fantasy.Jobs
{
    [XSerializable("property", NamespaceUri=Consts.XNamespaceURI)]
    [Serializable]
    public class JobProperty: IXSerializable, ICloneable , IConditionalObject  
    {
        public string Name { get; set; }

        [XValue]
        public string Value { get; set; }

        public object Clone()
        {
            return new JobProperty { Name = this.Name, Value = this.Value, Condition = this.Condition }; 
        }

        public void Load(IServiceProvider context, XElement element)
        {
            this.Name = element.Name.LocalName;
            XHelper.Default.LoadByXAttributes(context, element, this);
        }

        public void Save(IServiceProvider context, XElement element)
        {
            XHelper.Default.SaveByXAttributes(context, element, this);
        }

        [XAttribute("condition")] 
        public string Condition { get; set; }
       
    }


    class JobPropertiesSerializer : IXCollectionSerializer
    {

        public void Save(IServiceProvider context, XElement element, IEnumerable collection)
        {
            

            foreach (JobProperty item in collection)
            {
                XElement childElement = new XElement(element.Name.Namespace + item.Name);
                element.Add(childElement);
                item.Save(context, childElement);
            }
        }

        public IEnumerable Load(IServiceProvider context, XElement element)
        {
            ArrayList rs = new ArrayList();
            foreach (XElement childElement in element.Elements())
            {
                JobProperty item = new JobProperty();
                item.Load(context, childElement);
                rs.Add(item);
            }

            return rs;
        }
    }
}
