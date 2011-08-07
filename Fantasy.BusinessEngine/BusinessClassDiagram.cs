using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.BusinessEngine
{
    public class BusinessClassDiagram : BusinessEntity, INamedBusinessEntity
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

        public virtual string Diagram
        {
            get
            {
                return (string)this.GetValue("Diagram", null);
            }
            set
            {
                this.SetValue("Diagram", value);
            }
        }

        #region INamedBusinessEntity Members


        string INamedBusinessEntity.CodeName
        {
            get { return this.Name; }
        }

        string INamedBusinessEntity.FullCodeName
        {
            get { return this.FullName; }
        }

        #endregion
    }
}
