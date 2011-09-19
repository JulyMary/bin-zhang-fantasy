using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine.Collections;

namespace Fantasy.BusinessEngine
{
    public class BusinessEnum : BusinessEntity, IGernateCodeBusinessEntity
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
                if (this.IsExternal)
                {
                    return this.ExternalNamespace + "." + this.Name;
                }
                else
                {
                    return this.Package != null && this.Package.Id != BusinessPackage.RootPackageId ? this.Package.Name + "." + this.Name : this.Name;
                }
            }
        }


        public virtual string FullCodeName
        {
            get
            {
                if (this.IsExternal)
                {
                    return this.ExternalNamespace + "." + this.CodeName;

                }
                else
                {
                    return this.Package != null ? this.Package.FullCodeName + "." + this.CodeName : this.CodeName;
                }
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


        public virtual string ExternalNamespace
        {
            get
            {
                return (string)this.GetValue("ExternalNamespace", null);
            }
            set
            {
                this.SetValue("ExternalNamespace", value);
            }
        }

        private IObservableList<BusinessEnumValue> _persistedEnumValues = new ObservableList<BusinessEnumValue>();
        protected internal virtual IObservableList<BusinessEnumValue> PersistedEnumValues
        {
            get { return _persistedEnumValues; }
            private set
            {
                if (_persistedEnumValues != value)
                {
                    _persistedEnumValues = value;
                    _enumValues.Source = value;
                }
            }
        }

        private ObservableListView<BusinessEnumValue> _enumValues;
        public virtual IObservableList<BusinessEnumValue> EnumValues
        {
            get
            {
                if (this._enumValues == null)
                {
                    this._enumValues = new ObservableListView<BusinessEnumValue>(this._persistedEnumValues);
                }
                return _enumValues;
            }
        }

        
    }
}
