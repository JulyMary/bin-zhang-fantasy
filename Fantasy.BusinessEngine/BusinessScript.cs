using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.IO;

namespace Fantasy.BusinessEngine
{
    public class BusinessScript : BusinessEntity, IGernateCodeBusinessEntity
    {
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


        public virtual string Script
        {
            get
            {
                return (string)this.GetValue("Script", null);
            }
            set
            {
                this.SetValue("Script", value);
            }
        }

       

        public virtual string MetaData
        {
            get
            {
                return (string)this.GetValue("MetaData", null);
            }
            set
            {
                this.SetValue("MetaData", value);
            }
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

        public virtual string CodeName
        {
            get
            {
                return LongPath.GetFileNameWithoutExtension(this.Name);
            }
        }

        public virtual string FullName
        {
            get
            {
                return this.Package != null ? this.Package.Name + "." + this.Name : this.Name;
            }
        }


        public virtual string FullCodeName
        {
            get
            {
                return this.Package != null ? this.Package.FullCodeName + "." + this.CodeName : this.CodeName;
            }

        }
    }
}
