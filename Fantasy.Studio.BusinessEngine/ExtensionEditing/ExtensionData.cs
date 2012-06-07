using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Windows;
using Fantasy.BusinessEngine;
using System.Windows;
using System.ComponentModel;
using Fantasy.Studio.BusinessEngine.Properties;
using Fantasy.BusinessEngine.EntityExtensions;

namespace Fantasy.Studio.BusinessEngine.ExtensionEditing
{
    public abstract class ExtensionData : NotifyPropertyChangedObject
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
            this.Name = property.Class.Name + "." + property.Name + Resources.EntityExtensionsSuffix;
            PropertyChangedEventManager.AddListener(property, this, "Name");
            PropertyChangedEventManager.AddListener(property.Class, this, "Name");
            this.Type = "Property.Extensions";
        }

        #region IWeakEventListener Members

        public bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
        {
            switch (((PropertyChangedEventArgs)e).PropertyName)
            {
                case "Name":
                    BusinessProperty prop = (BusinessProperty)this.Entity;
                    this.Name = prop.Class.Name + "." + prop.Name + Resources.EntityExtensionsSuffix;
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
            this.Name = assn.RightClass + "." + assn.LeftRoleName + Resources.EntityExtensionsSuffix;
            PropertyChangedEventManager.AddListener(assn, this, "LeftRoleName");
            PropertyChangedEventManager.AddListener(assn.RightClass, this, "Name");
            this.Type = "Role.Extensions";
        }

        #region IWeakEventListener Members

        public bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
        {
            switch (((PropertyChangedEventArgs)e).PropertyName)
            {
                case "Name":
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
            this.Name = assn.LeftClass.Name + "." + assn.RightRoleName + Resources.EntityExtensionsSuffix;
            PropertyChangedEventManager.AddListener(assn, this, "RightRoleName");
            PropertyChangedEventManager.AddListener(assn.LeftClass, this, "Name");
            this.Type = "Role.Extensions";
        }

        #region IWeakEventListener Members

        public bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
        {
            switch (((PropertyChangedEventArgs)e).PropertyName)
            {
                case "Name":   
                case "RightRoleName":
                    BusinessAssociation assn = (BusinessAssociation)this.Entity;
                    this.Name = assn.LeftClass.Name + "." + assn.RightRoleName + Resources.EntityExtensionsSuffix;
                    return true;
                default:
                    return false;
            }
        }

      

        #endregion
    }
}
