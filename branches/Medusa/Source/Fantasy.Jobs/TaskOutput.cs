using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.XSerialization;
namespace Fantasy.Jobs
{
    [XSerializable("output", NamespaceUri=Consts.XNamespaceURI )]
    public class TaskOutput : IConditionalObject
    {
        [XAttribute("taskParameter")]
        public string TaskParameter { get; set; }

        [XAttribute("propertyName")]
        public string PropertyName { get; set; }

        [XAttribute("itemCategory")]
        public string ItemCategory { get; set; }

        [XAttribute("condition")]
        public string Condition { get; set; }
    }
}
