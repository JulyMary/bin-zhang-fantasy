using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Windows;
using Fantasy.BusinessEngine;
using System.Windows;
using System.ComponentModel;
using Fantasy.Studio.BusinessEngine.Properties;

namespace Fantasy.Studio.BusinessEngine.ExtensionEditing
{
    public class ExtensionData : NotifyPropertyChangedObject
    {
        private BusinessEntity _entity;

        public BusinessEntity Entity
        {
            get { return _entity; }
            set
            {
                if (_entity != value)
                {
                    _entity = value;
                    this.OnPropertyChanged("Entity");
                }
            }
        }

        private IList<IEntityExtension> _extensions;

        public IList<IEntityExtension> Extensions
        {
            get { return _extensions; }
            set
            {
                if (_extensions != value)
                {
                    _extensions = value;
                    this.OnPropertyChanged("Extensions");
                }
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

        private string _type;

        public string Type
        {
            get { return _type; }
            set
            {
                if (_type != value)
                {
                    _type = value;
                    this.OnPropertyChanged("Type");
                }
            }
        }


        public override bool Equals(object obj)
        {
            ExtensionData other = obj as ExtensionData;
            if (other != null)
            {
                return other.Entity == this.Entity && other.Extensions == this.Extensions;
            }
            else
            {
                return false;
            }
           
        }

        public override int GetHashCode()
        {
            return this.Entity.GetHashCode() ^ this.Extensions.GetHashCode();
        }
    }

    public class PropertyExtensionData : ExtensionData, IWeakEventListener
    {
        public PropertyExtensionData(BusinessProperty property)
        {
            this.Entity = property;
            this.Extensions = property.Extensions;
            this.Name = ((BusinessProperty)this.Entity).Name + Resources.EntityExtensionsSuffix;
            PropertyChangedEventManager.AddListener(property, this, "Name");
            this.Type = "Property.Extensions";
        }

        #region IWeakEventListener Members

        public bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
        {
            switch (((PropertyChangedEventArgs)e).PropertyName)
            {
                case "Name":
                    this.Name = ((BusinessProperty)this.Entity).Name + Resources.EntityExtensionsSuffix;
                    return true;

                default:
                    return false;
            }
        }

        #endregion
    }

    public class AssociationLeftRoleExtensionData : ExtensionData, IWeakEventListener
    {
        public AssociationLeftRoleExtensionData(BusinessAssociation assn)
        {
            this.Entity = assn;
            this.Extensions = assn.LeftExtensions;
            this.Name = ((BusinessAssociation)this.Entity).LeftRoleName + Resources.EntityExtensionsSuffix;
            PropertyChangedEventManager.AddListener(assn, this, "LeftRoleName");
            this.Type = "Role.Extensions";
        }

        #region IWeakEventListener Members

        public bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
        {
            switch (((PropertyChangedEventArgs)e).PropertyName)
            {
                case "LeftRoleName":
                    this.Name = ((BusinessAssociation)this.Entity).LeftRoleName + Resources.EntityExtensionsSuffix;
                    return true;
                default:
                    return false;
            }
        }

        #endregion
    }

    public class AssociationRightRoleExtensionData : ExtensionData, IWeakEventListener
    {
        public AssociationRightRoleExtensionData(BusinessAssociation assn)
        {
            this.Entity = assn;
            this.Extensions = assn.RightExtensions;
            this.Name = ((BusinessAssociation)this.Entity).RightRoleName + Resources.EntityExtensionsSuffix;
            PropertyChangedEventManager.AddListener(assn, this, "RightRoleName");
            this.Type = "Role.Extensions";
        }

        #region IWeakEventListener Members

        public bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
        {
            switch (((PropertyChangedEventArgs)e).PropertyName)
            {
                case "RightRoleName":
                    this.Name = ((BusinessAssociation)this.Entity).RightRoleName + Resources.EntityExtensionsSuffix;
                    return true;
                default:
                    return false;
            }
        }

        #endregion
    }
}
