using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Studio.BusinessEngine.CodeEditing;
using Fantasy.BusinessEngine;
using Fantasy.Studio.BusinessEngine.Properties;
using System.Windows;
using System.ComponentModel;
using Fantasy.Windows;

namespace Fantasy.Studio.BusinessEngine.ClassEditing
{
    public class BusinessClassScript : NotifyPropertyChangedObject, IEntityScript, IWeakEventListener
    {

        public BusinessClassScript(BusinessClass @class)
        {
            this.Entity = @class;
            this.IsReadOnly = @class.IsSystem;
            this.Name = this.Entity.Name + ".cs";
            PropertyChangedEventManager.AddListener(this.Entity, this, "Script");
            PropertyChangedEventManager.AddListener(this.Entity, this, "Name");
        }

        public BusinessClass Entity { get; set; }

        public string Content
        {
            get
            {
                return this.Entity.Script;
            }
            set
            {
                this.Entity.Script = value;
            }
        }

        private string _name;

        public string Name
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


        bool IWeakEventListener.ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
        {
            switch (((PropertyChangedEventArgs)e).PropertyName)
            {
                case "Script":
                   this.OnPropertyChanged("Content");
                   return true;
                case "Name":
                    this.Name = this.Entity.Name + ".cs";
                    return true;

                default:
                    return false;
            }

        }


        public override bool Equals(object obj)
        {
            BusinessClassScript other = obj as BusinessClassScript;
            return other != null && other.Entity == this.Entity;
        }

        public override int GetHashCode()
        {
            return unchecked(this.Entity.GetHashCode() + 1000);
        }

        private bool _isReadOnly;

        public bool IsReadOnly
        {
            get { return _isReadOnly; }
            set
            {
                if (_isReadOnly != value)
                {
                    _isReadOnly = value;
                    this.OnPropertyChanged("IsReadOnly");
                }
            }
        }

       

        IBusinessEntity IEntityScript.Entity
        {
            get { return this.Entity; }
        }



      


        public string Extension
        {
            get { return "cs"; }
        }

        
    }
}
