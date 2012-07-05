using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Fantasy.XSerialization;

namespace Fantasy.Jobs.Scheduling
{
    [DataContract]
    [XSerializable("customAction", NamespaceUri = Consts.ScheduleNamespaceURI)]
    public class CustomAction : Action
    {
        [DataMember]
        [XAttribute("customType")]
        public string CustomType { get; set; }

        public override ActionType Type
        {
            get { return ActionType.Custom; }
        }
    }
}
