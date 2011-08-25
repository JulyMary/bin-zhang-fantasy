using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine.Collections;

namespace Fantasy.BusinessEngine
{
    public class BusinessClass : BusinessEntity, INamedBusinessEntity
    {
        public BusinessClass()
        {
            this.ChildClasses = new ObservableList<BusinessClass>();
            this.Properties = new ObservableList<BusinessProperty>();
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

        public virtual IObservableList<BusinessClass> ChildClasses { get; private set; }


        public virtual IObservableList<BusinessProperty> Properties { get; private set; }

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

       

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.PreviousState = BusinessEngine.EntityState.Clean;
        }

       

        public override EntityState EntityState
        {
            get
            {
                return base.EntityState;
            }
            protected set
            {

                if (this.EntityState == BusinessEngine.EntityState.Clean && value == BusinessEngine.EntityState.Dirty || value == BusinessEngine.EntityState.Deleted)
                {
                    this.PreviousState = BusinessEngine.EntityState.Clean;
                }
                base.EntityState = value;
            }
        }


        private EntityState _previousState = EntityState.New;
        public virtual EntityState PreviousState
        {
            get
            {
                return _previousState;
            }
            set
            {
                this._previousState = value;
                this._previousValues.Clear();
                foreach (KeyValuePair<string, object> kv in this.Values)
                {
                    this._previousValues.Add(kv.Key, kv.Value);
                }
                this._previousProperties = this.Properties.ToArray();
            }
        }
       
        private BusinessProperty[] _previousProperties = new BusinessProperty[0]; 
        public virtual BusinessProperty[] PreviousProperties
        {
            get
            {
                return _previousProperties ;
            }
        }
        private Dictionary<string, object> _previousValues = new Dictionary<string, object>();
        public virtual T GetPreviousValue<T>(string propertyName, T defaultValue = default(T))
        {
            return (T)this._previousValues.GetValueOrDefault(propertyName, defaultValue);
        }
    }
}
