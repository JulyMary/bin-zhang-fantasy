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
    [XSerializable("action", NamespaceUri=Consts.ScheduleNamespaceURI)]
    public abstract class Action
    {
        public abstract  ActionType Type { get; }
    }
}
