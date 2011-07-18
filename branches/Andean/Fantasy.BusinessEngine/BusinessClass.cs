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
    }
}
