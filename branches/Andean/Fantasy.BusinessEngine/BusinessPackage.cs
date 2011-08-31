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

        

        private IObservableList<BusinessPackage> _persistedChildPackages = new ObservableList<BusinessPackage>();
        protected internal virtual IObservableList<BusinessPackage> PersistedChildPackages
        {
            get { return _persistedChildPackages; }
            private set
            {
                if (_persistedChildPackages != value)
                {
                    _persistedChildPackages = value;
                    _childPackages.Source = value;
                }
            }
        }

        private ObservableListView<BusinessPackage> _childPackages;
        public virtual IObservableList<BusinessPackage> ChildPackages
        {
            get
            {
                if (this._childPackages == null)
                {
                    this._childPackages = new ObservableListView<BusinessPackage>(this._persistedChildPackages);
                }
                return _childPackages;
            }
        }

        private IObservableList<BusinessClass> _persistedClasses = new ObservableList<BusinessClass>();
        protected internal virtual IObservableList<BusinessClass> PersistedClasses
        {
            get { return _persistedClasses; }
            private set
            {
                if (_persistedClasses != value)
                {
                    _persistedClasses = value;
                    _classes.Source = value;
                }
            }
        }

        private ObservableListView<BusinessClass> _classes;
        public virtual IObservableList<BusinessClass> Classes
        {
            get
            {
                if (this._classes == null)
                {
                    this._classes = new ObservableListView<BusinessClass>(this._persistedClasses); 
                }
                return _classes;
            }
        }


        private IObservableList<BusinessClassDiagram> _persistedClassDiagrams = new ObservableList<BusinessClassDiagram>();
        protected internal virtual IObservableList<BusinessClassDiagram> PersistedClassDiagrams
        {
            get { return _persistedClassDiagrams; }
            private set
            {
                if (_persistedClassDiagrams != value)
                {
                    _persistedClassDiagrams = value;
                    _classDiagrams.Source = value;
                }
            }
        }

        private ObservableListView<BusinessClassDiagram> _classDiagrams;
        public virtual IObservableList<BusinessClassDiagram> ClassDiagrams
        {
            get
            {
                if (this._classDiagrams == null)
                {
                    this._classDiagrams = new ObservableListView<BusinessClassDiagram>(this._persistedClassDiagrams);
                }
                return _classDiagrams;
            }
        }


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
