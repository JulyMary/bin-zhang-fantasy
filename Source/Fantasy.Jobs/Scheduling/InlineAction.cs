using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Fantasy.XSerialization;
using System.Xml.Linq;

namespace Fantasy.Jobs.Scheduling
{
    [DataContract]
    [XSerializable("inlineAction")]
    public class InlineAction : Action, IXSerializable 
    {

        public override ActionType Type
        {
            get { return ActionType.Inline; }
        }

        [DataMember]
        public string Xslt { get; set; }

        #region IXSerializable Members

        public void Load(IServiceProvider context, XElement element)
        {
            XHelper.Default.LoadByXAttributes(context, element, this);
            XElement xsltElement = element.Elements().FirstOrDefault();
            this.Xslt = xsltElement != null ? xsltElement.ToString() : null;
        }

        public void Save(IServiceProvider context, XElement element)
        {
            XHelper.Default.SaveByXAttributes(context, element, this);
            if (!string.IsNullOrEmpty(Xslt))
            {
                element.Add(XElement.Parse(Xslt));
            }
        }

        #endregion
    }
}
