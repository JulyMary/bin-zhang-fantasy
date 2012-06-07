using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Windows;
using Fantasy.XSerialization;
using Fantasy.BusinessEngine;
using Fantasy.BusinessEngine.EntityExtensions;
using Fantasy.ComponentModel;
using Fantasy.Web.Mvc.Html;
using System.Web.Mvc;

namespace Fantasy.Web.EntityExtensions
{
    [XSerializable("editor", NamespaceUri = Consts.ExtensionsNamespace)]
    [ExtensionUsage(AllowMultiple = false)]
    [Icon("Fantasy.Web.Design.Properties.Resources, Fantasy.Web.Design", "EditorIcon")]
    public abstract class EditorExtension : NotifyPropertyChangedObject, IEntityExtension
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



        #region IEntityExtension Members


        public virtual bool ApplyTo(object context)
        {
            return false;
        }

        #endregion

        public abstract UserControlFactory CreateEditor(HtmlHelper htmlHelper);  


    }
}
