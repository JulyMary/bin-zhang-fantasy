using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine.Collections;

namespace Fantasy.BusinessEngine
{
    public class BusinessClass : BusinessEntity, IGernateCodeBusinessEntity
    {
        public BusinessClass()
        {
            
        }

        public virtual string Name
        {
            get
            {
                return (string)this.GetValue("Name", null);
            }
            set
            {
                this.SetValue("Name", value);
            }
        }


        public virtual string FullName
        {
            get
            {
                return this.Package != null && this.Package.Id != BusinessPackage.RootPackageId ? this.Package.Name + "." + this.Name : this.Name;
            }
        }


        public virtual string FullCodeName
        {
            get
            {
                return this.Package != null ? this.Package.FullCodeName + "." + this.CodeName : this.CodeName;
            }

        }

        public virtual string CodeName
        {
            get
            {
                return (string)this.GetValue("CodeName", null);
            }
            set
            {
                this.SetValue("CodeName", value);
            }
        }

        public virtual BusinessPackage Package
        {
            get
            {
                return (BusinessPackage)this.GetValue("Package", null);
            }
            set
            {
                this.SetValue("Package", value);
            }
        }

        public virtual BusinessClass ParentClass 
        {
            get
            {
                return (BusinessClass)this.GetValue("ParentClass", null);
            }
            set
            {
                this.SetValue("ParentClass", value);
            }
        }



        private IObservableList<BusinessClass> _persistedChildClasses = new ObservableList<BusinessClass>();
        protected internal virtual IObservableList<BusinessClass> PersistedChildClasses
        {
            get { return _persistedChildClasses; }
            private set
            {
                if (_persistedChildClasses != value)
                {
                    _persistedChildClasses = value;
                    _childClasses.Source = value;
                }
            }
        }

        private ObservableListView<BusinessClass> _childClasses;
        public virtual IObservableList<BusinessClass> ChildClasses
        {
            get
            {
                if (this._childClasses == null)
                {
                    this._childClasses = new ObservableListView<BusinessClass>(this._persistedChildClasses);
                }
                return _childClasses;
            }
        }



        private IObservableList<BusinessProperty> _persistedProperties = new ObservableList<BusinessProperty>();
        protected internal virtual IObservableList<BusinessProperty> PersistedProperties
        {
            get { return _persistedProperties; }
            private set
            {
                if (_persistedProperties != value)
                {
                    _persistedProperties = value;
                    _properties.Source = value;
                }
            }
        }

        private ObservableListView<BusinessProperty> _properties;
        public virtual IObservableList<BusinessProperty> Properties
        {
            get
            {
                if (this._properties == null)
                {
                    this._properties = new ObservableListView<BusinessProperty>(this._persistedProperties);
                }
                return _properties;
            }
        }


       


        public virtual string TableName
        {
            get
            {
                return (string)this.GetValue("TableName", null);
            }
            set
            {
                this.SetValue("TableName", value);
            }
        }

        public virtual string TableSchema
        {
            get
            {
                return (string)this.GetValue("TableSchema", null);
            }
            set
            {
                this.SetValue("TableSchema", value);
            }
        }

        public virtual string TableSpace
        {
            get
            {
                return (string)this.GetValue("TableSpace", null);
            }
            set
            {
                this.SetValue("TableSpace", value);
            }
        }

        public virtual bool IsSimple
        {
            get
            {
                return (bool)this.GetValue("IsSimple", false);
            }
            set
            {
                this.SetValue("IsSimple", value);
            }
        }

        public static readonly Guid RootClassId = new Guid("bf0aa7f4-588f-4556-963d-33242e649d57");

        public override EntityState EntityState
        {
            get
            {
                return base.EntityState;
            }
            protected set
            {
                base.EntityState = value;
            }
        }

        private IObservableList<BusinessAssociation> _persistedLeftAssociations = new ObservableList<BusinessAssociation>();
        protected internal virtual IObservableList<BusinessAssociation> PersistedLeftAssociations
        {
            get { return _persistedLeftAssociations; }
            private set
            {
                if (_persistedLeftAssociations != value)
                {
                    _persistedLeftAssociations = value;
                    if (_leftAssocations != null)
                    {
                        _leftAssocations.Source = value;
                    }
                }
            }
        }

        private ObservableListView<BusinessAssociation> _leftAssocations;
        public virtual IObservableList<BusinessAssociation> LeftAssociations
        {
            get
            {
                if (this._leftAssocations == null)
                {
                    this._leftAssocations = new ObservableListView<BusinessAssociation>(this._persistedLeftAssociations);
                }
                return _leftAssocations;
            }
        }




        private IObservableList<BusinessAssociation> _persistedRightAssociations = new ObservableList<BusinessAssociation>();
        protected internal virtual IObservableList<BusinessAssociation> PersistedRightAssociations
        {
            get { return _persistedRightAssociations; }
            private set
            {
                if (_persistedRightAssociations != value)
                {
                    _persistedRightAssociations = value;
                    if (_rightAssociations != null)
                    {
                        _rightAssociations.Source = value;
                    }
                }
            }
        }


        


        private ObservableListView<BusinessAssociation> _rightAssociations;
        public virtual IObservableList<BusinessAssociation> RightAssociations
        {
            get
            {
                if (this._rightAssociations == null)
                {
                    this._rightAssociations = new ObservableListView<BusinessAssociation>(this._persistedRightAssociations);
                }
                return _rightAssociations;
            }
        }

      
    }
}
