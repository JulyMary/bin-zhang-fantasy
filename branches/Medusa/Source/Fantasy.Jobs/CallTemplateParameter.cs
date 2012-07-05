using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.XSerialization;

namespace Fantasy.Jobs
{
    [XSerializable("parameters", NamespaceUri = Consts.XNamespaceURI)]
    public class CallTemplateParameter : IConditionalObject
    {
        [XAttribute("itemCategory")]
        public string ItemCategory { get; set; }

        [XAttribute("include")]
        public string Include { get; set; }

        [XAttribute("condition")]
        public string Condition { get; set; }

        [XAttribute("propertyName")]
        public string PropertyName { get; set; }

        [XAttribute("value")]
        public string Value { get; set; }
    }
}
