using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.BusinessEngine
{
    public class BusinessAssociation : BusinessEntity
    {

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

        public virtual BusinessClass LeftClass
        {
            get
            {
                return (BusinessClass)this.GetValue("LeftClass", null);
            }
            set
            {
                this.SetValue("LeftClass", value);
            }
        }


        public virtual string LeftRoleName
        {
            get
            {
                return (string)this.GetValue("LeftRoleName", null);
            }
            set
            {
                this.SetValue("LeftRoleName", value);
            }
        }

        public virtual string LeftRoleCode
        {
            get
            {
                return (string)this.GetValue("LeftRoleCode", 0);
            }
            set
            {
                this.SetValue("LeftRoleCode", value);
            }
        }

        public virtual string LeftCardinality
        {
            get
            {
                return (string)this.GetValue("LeftCardinality", "1");
            }
            set
            {
                this.SetValue("LeftCardinality", value);
            }
        }


        public virtual bool LeftNavigatable
        {
            get
            {
                return (bool)this.GetValue("LeftNavigatable", true);
            }
            set
            {
                this.SetValue("LeftNavigatable", value);
            }
        }


        public virtual BusinessClass RightClass
        {
            get
            {
                return (BusinessClass)this.GetValue("RightClass", null);
            }
            set
            {
                this.SetValue("RightClass", value);
            }
        }


        public virtual string RightRoleName
        {
            get
            {
                return (string)this.GetValue("RightRoleName", null);
            }
            set
            {
                this.SetValue("RightRoleName", value);
            }
        }

        public virtual string RightRoleCode
        {
            get
            {
                return (string)this.GetValue("RightRoleCode", null);
            }
            set
            {
                this.SetValue("RightRoleCode", value);
            }
        }


        public virtual string RightCardinality
        {
            get
            {
                return (string)this.GetValue("RightCardinality", "*");
            }
            set
            {
                this.SetValue("RightCardinality", value);
            }
        }


        public virtual bool RightNavigatable
        {
            get
            {
                return (bool)this.GetValue("RightNavigatable", true);
            }
            set
            {
                this.SetValue("RightNavigatable", value);
            }
        }



    }
}
