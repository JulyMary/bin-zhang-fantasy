using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using Fantasy.XSerialization;
using System.Xml.Linq;

namespace Fantasy.Jobs
{
    [Serializable]
    [XSerializable("taskitem",NamespaceUri = Consts.XNamespaceURI) ] 
    public class TaskItem : IXSerializable, ICloneable, IConditionalObject  
    {
        
        public string Name { get; set; }

        public string Category { get; set; }

        private NameValueCollection _metaData = new NameValueCollection(StringComparer.OrdinalIgnoreCase); 

        public string this[string index] { 
            get 
            {
                if (index == null)
                {
                    throw new ArgumentNullException("index"); 
                }

                ITaskItemMetaDataProvider  provider = (from p in MetaDataProviders
                                where p.GetNames(this).ToList().Exists(x => StringComparer.OrdinalIgnoreCase.Compare(x, index) == 0)
                                select p).FirstOrDefault();

                if (provider != null)
                {
                    return provider.GetData(this, index);
                }
                else
                {
                    return this._metaData[index];
                }
                

            }
            set 
            {
                if (index == null)
                {
                    throw new ArgumentNullException("index");
                }

                ITaskItemMetaDataProvider provider = (from p in MetaDataProviders
                                                     where p.GetNames(this).ToList().Exists(x => StringComparer.OrdinalIgnoreCase.Compare(x, index) == 0)
                                                     select p).FirstOrDefault();

                if (provider == null)
                {
                    this._metaData[index] = value;
                }
                
            }
        }

        public int MetaDataCount
        {
            get
            {
                return this.MetaDataNames.Length; 
            }
        }

        public string[] MetaDataNames
        {
            get
            {
                return (from provider in MetaDataProviders
                       let names = provider.GetNames(this)
                       from name in names select name).Union(this._metaData.AllKeys).ToArray(); 
            }
        }

        

        public bool HasMetaData(string name)
        {
            var query = from key in MetaDataNames where (StringComparer.OrdinalIgnoreCase.Compare(name, key) == 0) select key;
            return query.Count() > 0;
        }

        public void RemoveMetaData(string name)
        {
            this._metaData.Remove(name);
        }

        public void CopyMetaDataTo(TaskItem destinationItem)
        {
            foreach (string key in this._metaData.AllKeys)
            {
                destinationItem[key] = this._metaData[key];
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

        #region ICloneable Members

        public object Clone()
        {
            TaskItem rs = new TaskItem();
            rs.Name = this.Name;
            rs.Category = this.Category;
            this.CopyMetaDataTo(rs);
            return rs;
        }

        #endregion

        #region IConditionalObject Members

        [XAttribute("condition")] 
        public string Condition
        {
            get;set;
        }

        #endregion


        private static ITaskItemMetaDataProvider[] MetaDataProviders = new ITaskItemMetaDataProvider[] { new NameAndCategoryMetaDataProvider(), new FileInfoMetaDataProvider() }; 

       
 
    }
}
