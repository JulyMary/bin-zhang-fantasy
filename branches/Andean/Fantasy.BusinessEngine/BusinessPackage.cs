using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine.Collections;

namespace Fantasy.BusinessEngine
{
    public class BusinessPackage : BusinessEntity, INamedBusinessEntity
    {
        public BusinessPackage()
        {
            this.ChildPackages = new ObservableList<BusinessPackage>();
            this.Classes = new ObservableList<BusinessClass>();
            this.ClassDiagrams = new ObservableList<BusinessClassDiagram>();
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
                return (string)this.GetValue("CodeName", null);
            }
            set
            {
                this.SetValue("CodeName", value);
            }
        }

        public virtual IObservableList<BusinessPackage> ChildPackages { get; private set; }

        public virtual IObservableList<BusinessClass> Classes { get; private set; }

        public virtual IObservableList<BusinessClassDiagram> ClassDiagrams { get; private set; }

        public virtual BusinessPackage ParentPackage
        {
            get
            {
                return (BusinessPackage)this.GetValue("ParentPackage", null);
            }
            set
            {
                this.SetValue("ParentPackage", value);
            }
        }


        public virtual string FullName
        {
            get 
            { 
                return this.ParentPackage != null && this.ParentPackage.Id != RootPackageId  ? this.ParentPackage.Name + "." +  this.Name : this.Name; 
            }
        }

       
        public virtual string FullCodeName
        {
            get
            {
                return  this.ParentPackage != null ? this.ParentPackage.FullCodeName + "." +  this.CodeName : this.CodeName;
            }
    
        }


        public virtual bool BuildAsAssembly
        {
            get
            {
                return (bool)this.GetValue("BuildAsAssembly", false);
            }
            set
            {
                this.SetValue("BuildAsAssembly", value);
            }
        }


        public readonly static Guid RootPackageId = new Guid("28f26cdc-9ce5-4d0a-814d-08ce58105e25");
       
    }
}
