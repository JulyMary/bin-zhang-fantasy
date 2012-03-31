using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine.Collections;

namespace Fantasy.BusinessEngine
{
    public class BusinessRoleData : BusinessEntity, INamedBusinessEntity
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

        public virtual string FullCodeName
        {
            get
            {
                return this.Package != null ? this.Package.FullCodeName + "." + this.CodeName : this.CodeName;
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

        public virtual string Description
        {
            get
            {
                return (string)this.GetValue("Description", null);
            }
            set
            {
                this.SetValue("Description", value);
            }
        }

        public virtual bool IsComputed
        {
            get
            {
                return (bool)this.GetValue("IsComputed", false);
            }
            set
            {
                this.SetValue("IsComputed", value);
            }
        }

        public virtual bool IsDisabled
        {
            get
            {
                return (bool)this.GetValue("IsDisabled", false);
            }
            set
            {
                this.SetValue("IsDisabled", value);
            }
        }


        private IObservableList<BusinessUserData> _persistedUsers = new ObservableList<BusinessUserData>();
        protected internal virtual IObservableList<BusinessUserData> PersistedUsers
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

        private ObservableListView<BusinessUserData> _users;
        public virtual IObservableList<BusinessUserData> Users
        {
            get
            {
                if (this._users == null)
                {
                    this._users = new ObservableListView<BusinessUserData>(this._persistedUsers);
                }
                return _users;
            }
        }

        #region INamedBusinessEntity Members


        string INamedBusinessEntity.FullName
        {
            get { return this.Name; }
        }

        #endregion
    }
}
