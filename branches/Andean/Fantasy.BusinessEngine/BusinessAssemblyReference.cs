using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Fantasy.BusinessEngine
{
    public class BusinessAssemblyReference : BusinessEntity
    {

        public virtual BusinessAssemblyReferenceGroup Group
        {
            get
            {
                return (BusinessAssemblyReferenceGroup)this.GetValue("Group", null);
            }
            set
            {
                this.SetValue("Group", value);
            }
        }

        protected internal virtual string FullName
        {
            get
            {
                return (string)this.GetValue("FullName", null);
            }
            set
            {
                if (value != this.FullName)
                {
                    this.SetValue("FullName", value);
                    if (value != null)
                    {
                        this.AssemblyName = new AssemblyName(value);
                    }
                    else
                    {
                        this.AssemblyName = null;
                    }
                }
            }
        }


        private AssemblyName _assemblyName;

        public virtual  AssemblyName AssemblyName
        {
            get { return _assemblyName; }
            set
            {
                if (_assemblyName != value)
                {
                    _assemblyName = value;
                    if (value != null)
                    {
                        this.FullName = value.FullName;
                    }
                    else
                    {
                        this.FullName = null;
                    }
                    this.OnNotifyPropertyChangedPropertyChanged("AssemblyName");
                }
            }
        }

        public virtual byte[] RawAssembly
        {
            get
            {
                return (byte[])this.GetValue("RawAssembly", null);
            }
            set
            {
                this.SetValue("RawAssembly", value);
            }
        }


        public virtual bool CopyLocal
        {
            get
            {
                return (bool)this.GetValue("CopyLocal", false);
            }
            set
            {
                this.SetValue("CopyLocal", value);
            }
        }

        
    }
}
