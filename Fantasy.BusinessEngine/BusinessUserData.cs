using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine.Collections;

namespace Fantasy.BusinessEngine
{
    public class BusinessUserData : BusinessEntity, INamedBusinessEntity, IScriptable
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
                return (string)this.GetValue("FullName", null);
            }
            set
            {
                this.SetValue("FullName", value);
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

        protected internal virtual string Password
        {
            get
            {
                return (string)this.GetValue("Password", null);
            }
            set
            {
                this.SetValue("Password", value);
            }
        }


        public virtual void SetPassword(string password)
        {
            this.Password = Encryption.Encrypt(password);
        }

        public virtual bool VerifyPasswod(string password)
        {
            return password == Encryption.Decrypt(this.Password);
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

        private IObservableList<BusinessRoleData> _persistedRoles = new ObservableList<BusinessRoleData>();
        protected internal virtual IObservableList<BusinessRoleData> PersistedRoles
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

        private ObservableListView<BusinessRoleData> _roles;
        public virtual IObservableList<BusinessRoleData> Roles
        {
            get
            {
                if (this._roles == null)
                {
                    this._roles = new ObservableListView<BusinessRoleData>(this._persistedRoles);
                }
                return _roles;
            }
        }

        public virtual ScriptOptions ScriptOptions
        {
            get
            {
                return (ScriptOptions)this.GetValue("ScriptOptions", ScriptOptions.Default);
            }
            set
            {
                this.SetValue("ScriptOptions", value);
            }
        }

        public virtual string ExternalType
        {
            get
            {
                return (string)this.GetValue("ExternalType", null);
            }
            set
            {
                this.SetValue("ExternalType", value);
            }
        }
    }
}
