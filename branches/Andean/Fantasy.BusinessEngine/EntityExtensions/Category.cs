using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Windows;
using Fantasy.XSerialization;
using System.ComponentModel;

namespace Fantasy.BusinessEngine.EntityExtensions
{
    [XSerializable("category", NamespaceUri=Consts.ExtensionsNamespace)]
    [Browsable(false)]
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

        #region IEntityExtension Members

        public string Name
        {
            get { return "Category"; }
        }

        public string Description
        {
            get { return this.Value; }
        }

        #endregion

       
    }
}
