using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine.Collections;

namespace Fantasy.BusinessEngine
{
    public class BusinessEnum : BusinessEntity
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

        public virtual bool IsFlags
        {
            get
            {
                return (bool)this.GetValue("IsFlags", false);
            }
            set
            {
                this.SetValue("IsFlags", value);
            }
        }

        public virtual bool IsExternal
        {
            get
            {
                return (bool)this.GetValue("IsExternal", false);
            }
            set
            {
                this.SetValue("IsExternal", value);
            }
        }

        public virtual string ExternalAssemblyName
        {
            get
            {
                return (string)this.GetValue("ExternalAssemblyName", null);
            }
            set
            {
                this.SetValue("ExternalAssemblyName", value);
            }
        }

        public virtual IObservableList<BusinessEnumValue> EnumValues { get; private set; }

        
    }
}
