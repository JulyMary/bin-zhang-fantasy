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
        public ObjectSecurityPropertyModel(BusinessObjectMemberSecurity propertySecurity)
        {
            this.Entity = propertySecurity;
            if (propertySecurity.PropertyAccess == null)
            {
                this.IsInherited = true;
            }
            else
            {
                this._canRead = (propertySecurity.PropertyAccess & BusinessObjectAccess.Read) == BusinessObjectAccess.Read;
                this._canRead = (propertySecurity.PropertyAccess & BusinessObjectAccess.Write) == BusinessObjectAccess.Write;
            }
            
        }

        private bool _changing = false;
        private bool _isInherited;

        public bool IsInherited
        {
            get { return _isInherited; }
            set
            {
                if (_isInherited != value)
                {
                    _isInherited = value;

                    
                    if (this._isInherited)
                    {
                        this.Entity.PropertyAccess = null;

                    }
                    else
                    {
                        this.Entity.PropertyAccess = BusinessObjectAccess.None;
                    }
                    if (!this._changing)
                    {
                        this._changing = true;
                        try
                        {
                            this.CanRead = false;
                            this.CanWrite = false;
                            
                        }
                        finally
                        {
                            this._changing = false;
                        }
                    }
                    
                    this.OnPropertyChanged("IsInherited");
                }
            }
        }

        public BusinessObjectMemberSecurity Entity {get;private set;}

        private bool _canRead;

        public bool CanRead
        {
            get { return _canRead; }
            set
            {
                if (_canRead != value)
                {
                    _canRead = value;

                    if (!this._changing)
                    {
                        if (_canRead)
                        {
                            Entity.PropertyAccess = Entity.PropertyAccess != null ? Entity.PropertyAccess | BusinessObjectAccess.Read : BusinessObjectAccess.Read;
                        }
                        else
                        {
                            Entity.PropertyAccess = Entity.PropertyAccess != null ? Entity.PropertyAccess & ~BusinessObjectAccess.Read : BusinessObjectAccess.None;

                        }
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
                    if (!this._changing)
                    {
                        if (_canWrite)
                        {
                            Entity.PropertyAccess = Entity.PropertyAccess != null ? Entity.PropertyAccess | BusinessObjectAccess.Write : BusinessObjectAccess.Write;
                        }
                        else
                        {
                            Entity.PropertyAccess = Entity.PropertyAccess != null ? Entity.PropertyAccess & ~BusinessObjectAccess.Write : BusinessObjectAccess.None;

                        }
                    }
                    this.OnPropertyChanged("CanWrite");
                }
            }
        }

       
    }
}
