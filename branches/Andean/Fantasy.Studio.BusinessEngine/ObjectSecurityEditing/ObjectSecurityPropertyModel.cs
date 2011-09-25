using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine;
using Fantasy.Windows;

namespace Fantasy.Studio.BusinessEngine.ObjectSecurityEditing
{
    public class ObjectSecurityPropertyModel : NotifyPropertyChangedObject
    {
        public ObjectSecurityPropertyModel(BusinessObjectPropertySecurity propertySecurity)
        {
            this._entity = propertySecurity;
            if (propertySecurity.PropertyAccess == null)
            {
                this.IsInherited = true;
            }
            else
            {
                this.CanRead = (propertySecurity.PropertyAccess & BusinessObjectAccess.Read) == BusinessObjectAccess.Read;
                this.CanWrite = (propertySecurity.PropertyAccess & BusinessObjectAccess.Write) == BusinessObjectAccess.Write;
            }
            
        }


        private bool _isInherited;

        public bool IsInherited
        {
            get { return _isInherited; }
            set
            {
                if (_isInherited != value)
                {
                    _isInherited = value;

                    this.CanRead = false;
                    this.CanWrite = false;
                    if (this._isInherited)
                    {
                        this._entity.PropertyAccess = null;

                    }
                    this.OnPropertyChanged("IsInherited");
                }
            }
        }

        private BusinessObjectPropertySecurity _entity;

        private bool _canRead;

        public bool CanRead
        {
            get { return _canRead; }
            set
            {
                if (_canRead != value)
                {
                    _canRead = value;

                    if (_canRead)
                    {
                        _entity.PropertyAccess = _entity.PropertyAccess != null ? _entity.PropertyAccess | BusinessObjectAccess.Read : BusinessObjectAccess.Read;
                    }
                    else
                    {
                        _entity.PropertyAccess = _entity.PropertyAccess != null ? _entity.PropertyAccess &  ~BusinessObjectAccess.Read : BusinessObjectAccess.None;

                    }

                    this.OnPropertyChanged("CanRead");
                }
            }
        }

        private bool _canWrite;

        public bool CanWrite
        {
            get { return _canWrite; }
            set
            {
                if (_canWrite != value)
                {
                    _canWrite = value;

                    if (_canWrite)
                    {
                        _entity.PropertyAccess = _entity.PropertyAccess != null ? _entity.PropertyAccess | BusinessObjectAccess.Write : BusinessObjectAccess.Write;
                    }
                    else
                    {
                        _entity.PropertyAccess = _entity.PropertyAccess != null ? _entity.PropertyAccess & ~BusinessObjectAccess.Write : BusinessObjectAccess.None;

                    }
                    this.OnPropertyChanged("CanWrite");
                }
            }
        }

       
    }
}
