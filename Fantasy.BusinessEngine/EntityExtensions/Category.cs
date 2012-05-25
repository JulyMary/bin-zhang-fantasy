using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Windows;
using Fantasy.XSerialization;

namespace Fantasy.BusinessEngine.EntityExtensions
{
    [XSerializable("category", NamespaceUri=Consts.ExtensionsNamespace)]
    public class Category : NotifyPropertyChangedObject, IEntityExtension
    {
        [XAttribute("value")]
        private string _value;

        public string Value
        {
            get { return _value; }
            set
            {
                if (_value != value)
                {
                    _value = value;
                    this.OnPropertyChanged("Value");
                }
            }
        }
    }
}
