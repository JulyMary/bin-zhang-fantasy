using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.XSerialization;

namespace Fantasy.Jobs
{
    [XSerializable("parameters", NamespaceUri = Consts.XNamespaceURI)]
    internal class CallTemplateParameter : IConditionalObject
    {
        [XAttribute("itemCategory")]
        public string ItemCategory = null;

        [XAttribute("include")]
        public string Include = null;

        [XAttribute("condition")]
        private string _condition = null;
        string IConditionalObject.Condition
        {
            get
            {
                return this._condition;
            }
            set
            {
                this._condition = value;
            }
        }

        [XAttribute("propertyName")]
        public string PropertyName = null;
        [XAttribute("value")]
        public string Value = null;
    }
}
