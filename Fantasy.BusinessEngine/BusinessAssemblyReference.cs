using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Security.Cryptography;
using System.Globalization;

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

        public virtual string FullName
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

        private AssemblyName AssemblyName
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
                        this.Name = _assemblyName.Name;
                        this.CultureInfo = _assemblyName.CultureInfo;
                        this.Version = _assemblyName.Version;
                        byte[] pc = _assemblyName.GetPublicKeyToken();
                        if (pc != null)
                        {
                            this.PublicToken = BitConverter.ToString(pc).Replace("-", string.Empty);
                        }
                        else
                        {
                            this.PublicToken = null;
                        }
                    }
                    else
                    {
                        this.FullName = null;
                    }
                   
                }
            }
        }


        private string _name;

        public virtual string Name
        {
            get { return _name; }
            private set
            {
                if (_name != value)
                {
                    _name = value;
                    this.OnNotifyPropertyChangedPropertyChanged("Name");
                }
            }
        }


        private  CultureInfo _cultureInfo;

        public virtual CultureInfo CultureInfo
        {
            get { return _cultureInfo; }
            private set
            {
                if (_cultureInfo != value)
                {
                    _cultureInfo = value;
                    this.OnNotifyPropertyChangedPropertyChanged("CultureInfo");
                }
            }
        }

        private Version _version;

        public virtual Version Version
        {
            get { return _version; }
            private set
            {
                if (_version != value)
                {
                    _version = value;
                    this.OnNotifyPropertyChangedPropertyChanged("Version");
                }
            }
        }

        private string _publicToken;

        public virtual string PublicToken
        {
            get { return _publicToken; }
            private set
            {
                if (_publicToken != value)
                {
                    _publicToken = value;
                    this.OnNotifyPropertyChangedPropertyChanged("PublicToken");
                }
            }
        }


        protected internal virtual byte[] PersitedRawAssembly
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


        public virtual byte[] RawAssembly
        {
            get
            {
                return (byte[])this.GetValue("RawAssembly", null);
            }
            set
            {
                this.SetValue("RawAssembly", value);
                if (value == null)
                {
                    this.RawHash = null;
                }
                else
                {
                    using (MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider())
                    {
                        byte[] bitHash = provider.ComputeHash(value);
                        this.RawHash = BitConverter.ToString(bitHash).Replace("-", string.Empty);
                    }
                }
            }
        }



        public virtual string RawHash
        {
            get
            {
                return (string)this.GetValue("RawHash", null);
            }
            private set
            {
                this.SetValue("RawHash", value);
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
