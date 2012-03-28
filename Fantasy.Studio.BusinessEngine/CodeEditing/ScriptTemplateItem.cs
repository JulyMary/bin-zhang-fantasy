using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.XSerialization;
using System.Xml.Linq;

namespace Fantasy.Studio.BusinessEngine.CodeEditing
{
    [XSerializable("item", NamespaceUri = Consts.ScriptTemplateNamespace)]
    public class ScriptTemplateItem : IXSerializable
    {
        public ScriptTemplateItem()
        {
            this.MetaData = new Dictionary<string, string>();
        }

        public string Name { get; set; }
        [XAttribute("include")]
        public string Include { get; set; }
        [XAttribute("tt")]
        public string TT { get; set; }
        public IDictionary<string, string> MetaData { get; private set; }

        [XAttribute("autoOpen")]
        public bool AutoOpen { get; set; }

        public string TTContent { get; set; }

        #region IXSerializable Members

        public void Load(IServiceProvider context, System.Xml.Linq.XElement element)
        {
            XHelper.Default.LoadByXAttributes(context, element, this);
            this.Name = element.Name.LocalName;
            foreach (XElement meta in element.Elements())
            {
                this.MetaData[meta.Name.LocalName] = (string)meta;
            }
            

        }

        public void Save(IServiceProvider context, System.Xml.Linq.XElement element)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}
