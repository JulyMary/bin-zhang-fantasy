using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine.Collections;

namespace Fantasy.BusinessEngine
{
    public class BusinessPackage : BusinessEntity, IGernateCodeBusinessEntity
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


        private IObservableList<BusinessEnum> _persistedEnums = new ObservableList<BusinessEnum>();
        protected internal virtual IObservableList<BusinessEnum> PersistedEnums
        {
            get { return _persistedEnums; }
            private set
            {
                if (_persistedEnums != value)
                {
                    _persistedEnums = value;
                    _enums.Source = value;
                }
            }
        }

        private ObservableListView<BusinessEnum> _enums;
        public virtual IObservableList<BusinessEnum> Enums
        {
            get
            {
                if (this._enums == null)
                {
                    this._enums = new ObservableListView<BusinessEnum>(this._persistedEnums);
                }
                return _enums;
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
                    if (_classDiagrams != null)
                    {
                        _classDiagrams.Source = value;
                    }
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


        private IObservableList<BusinessAssociation> _persistedAssociations = new ObservableList<BusinessAssociation>();
        protected internal virtual IObservableList<BusinessAssociation> PersistedAssociations
        {
            get { return _persistedAssociations; }
            private set
            {
                if (_persistedAssociations != value)
                {
                    _persistedAssociations = value;
                    if (_associations != null)
                    {
                        _associations.Source = value;
                    }
                }
            }
        }

        private ObservableListView<BusinessAssociation> _associations;
        public virtual IObservableList<BusinessAssociation> Associations
        {
            get
            {
                if (this._associations == null)
                {
                    this._associations = new ObservableListView<BusinessAssociation>(this._persistedAssociations);
                }
                return _associations;
            }
        }



        private IObservableList<BusinessUser> _persistedUsers = new ObservableList<BusinessUser>();
        protected internal virtual IObservableList<BusinessUser> PersistedUsers
        {
            get { return _persistedUsers; }
            private set
            {
                if (_persistedUsers != value)
                {
                    _persistedUsers = value;
                    if (_users != null)
                    {
                        _users.Source = value;
                    }
                }
            }
        }

        private ObservableListView<BusinessUser> _users;
        public virtual IObservableList<BusinessUser> Users
        {
            get
            {
                if (this._users == null)
                {
                    this._users = new ObservableListView<BusinessUser>(this._persistedUsers);
                }
                return _users;
            }
        }


        private IObservableList<BusinessRole> _persistedRoles = new ObservableList<BusinessRole>();
        protected internal virtual IObservableList<BusinessRole> PersistedRoles
        {
            get { return _persistedRoles; }
            private set
            {
                if (_persistedRoles != value)
                {
                    _persistedRoles = value;
                    if (_roles != null)
                    {
                        _roles.Source = value;
                    }
                }
            }
        }

        private ObservableListView<BusinessRole> _roles;
        public virtual IObservableList<BusinessRole> Roles
        {
            get
            {
                if (this._roles == null)
                {
                    this._roles = new ObservableListView<BusinessRole>(this._persistedRoles);
                }
                return _roles;
            }
        }



        private IObservableList<BusinessApplicationData> _persistedApplications = new ObservableList<BusinessApplicationData>();
        protected internal virtual IObservableList<BusinessApplicationData> PersistedApplications
        {
            get { return _persistedApplications; }
            private set
            {
                if (_persistedApplications != value)
                {
                    _persistedApplications = value;
                    if (_applications != null)
                    {
                        _applications.Source = value;
                    }
                }
            }
        }

        private ObservableListView<BusinessApplicationData> _applications;
        public virtual IObservableList<BusinessApplicationData> Applications
        {
            get
            {
                if (this._applications == null)
                {
                    this._applications = new ObservableListView<BusinessApplicationData>(this._persistedApplications);
                }
                return _applications;
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


        private IObservableList<BusinessWebController> _persistedWebControllers = new ObservableList<BusinessWebController>();
        protected internal virtual IObservableList<BusinessWebController> PersistedWebControllers
        {
            get { return _persistedWebControllers; }
            private set
            {
                if (_persistedWebControllers != value)
                {
                    _persistedWebControllers = value;
                    _webControllers.Source = value;
                }
            }
        }

        private ObservableListView<BusinessWebController> _webControllers;
        public virtual IObservableList<BusinessWebController> WebControllers
        {
            get
            {
                if (this._webControllers == null)
                {
                    this._webControllers = new ObservableListView<BusinessWebController>(this._persistedWebControllers);
                }
                return _webControllers;
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

        private IObservableList<BusinessScript> _persistedScripts = new ObservableList<BusinessScript>();
        protected internal virtual IObservableList<BusinessScript> PersistedScripts
        {
            get { return _persistedScripts; }
            private set
            {
                if (_persistedScripts != value)
                {
                    _persistedScripts = value;
                    _scripts.Source = value;
                }
            }
        }

        private ObservableListView<BusinessScript> _scripts;
        public virtual IObservableList<BusinessScript> Scripts
        {
            get
            {
                if (this._scripts == null)
                {
                    this._scripts = new ObservableListView<BusinessScript>(this._persistedScripts);
                }
                return _scripts;
            }
        }

        public readonly static Guid RootPackageId = new Guid("28f26cdc-9ce5-4d0a-814d-08ce58105e25");
       
    }
}
