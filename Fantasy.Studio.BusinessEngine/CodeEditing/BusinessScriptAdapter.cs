using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Windows;
using Fantasy.BusinessEngine;
using System.Windows;
using System.ComponentModel;

namespace Fantasy.Studio.BusinessEngine.CodeEditing
{
    public class BusinessScriptAdapter : NotifyPropertyChangedObject, IEntityScript, IWeakEventListener
    {

        public BusinessScriptAdapter(BusinessScriptBase script)
        {
            this.Entity = script;
            this.IsReadOnly = script.IsSystem;
            this.Name = this.Entity.Name;
            PropertyChangedEventManager.AddListener(this.Entity, this, "Script");
            PropertyChangedEventManager.AddListener(this.Entity, this, "Name");
        }

        public BusinessScriptBase Entity { get; set; }

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
                    this.Name = this.Entity.Name;
                    return true;

                default:
                    return false;
            }

        }


        public override bool Equals(object obj)
        {
            BusinessScriptAdapter other = obj as BusinessScriptAdapter;
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


        #region IEntityScript Members


        public string Extension
        {
            get { return this.Name.Substring(this.Name.LastIndexOf('.') + 1);}
        }

        #endregion
    }
}
