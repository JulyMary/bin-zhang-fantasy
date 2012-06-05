using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Windows;
using Fantasy.XSerialization;
using Fantasy.BusinessEngine;
using Fantasy.BusinessEngine.EntityExtensions;

namespace Fantasy.Web.EntityExtensions
{
    [XSerializable("category", NamespaceUri = Consts.ExtensionsNamespace)]
    [ExtensionUsage(AllowMultiple = false)]
    public class Editor : NotifyPropertyChangedObject, IEntityExtension
    {
       
        private string _name;

        public virtual string Name
        {
            get { return _name; }
            set 
            {
                if (_name != value)
                {
                    _name = value;
                    this.OnPropertyChanged("Name");
                }
            }
        }

        private string _description;

        public virtual string Description
        {
            get { return _description; }
            set 
            {
                if (_description != value)
                {
                    _description = value;
                    this.OnPropertyChanged("Description");
                }
            }
        }

       
    }
}
