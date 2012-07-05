using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Runtime.Serialization;
using Fantasy.XSerialization;

namespace Fantasy.Jobs.Scheduling
{
    [DataContract]
    [XSerializable("templateAction", NamespaceUri = Consts.ScheduleNamespaceURI)]
    public class TemplateAction : Action
    {

        public override ActionType Type
        {
            get { return ActionType.Template; }
        }

        [DataMember]
        [XAttribute("template")]
        public string Template { get; set; }
    }
}
