using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Windows;
using Fantasy.BusinessEngine;
using Fantasy.Collections;

namespace Fantasy.Studio.BusinessEngine.ObjectSecurityEditing
{
    public class ObjectSecurityEditorModel : NotifyPropertyChangedObject
    {
        

        public ObjectSecurityEditorModel(BusinessObjectSecurity entity)
        {
            this._entity = entity;
            this.Properties = new ObservableAdapterCollection<ObjectSecurityPropertyModel>(entity.Properties, (ps=>new ObjectSecurityPropertyModel((BusinessObjectPropertySecurity)ps)));
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

                    this.CanCreate = false;
                    this.CanDelete = false;
                    if (this._isInherited)
                    {
                        this._entity.ObjectAccess = null;

                    }
                    this.OnPropertyChanged("IsInherited");
                }
            }
        }
        
        private bool _canCreate;

        public bool CanCreate
        {
            get { return _canCreate; }
            set
            {
                if (_canCreate != value)
                {
                    _canCreate = value;
                    this.OnPropertyChanged("CanCreate");
                }
            }
        }


        private bool _canDelete;

        public bool CanDelete
        {
            get { return _canDelete; }
            set
            {
                if (_canDelete != value)
                {
                    _canDelete = value;
                    this.OnPropertyChanged("CanDelete");
                }
            }
        }

        private bool? _allCanRead;

        public bool? AllCanRead
        {
            get { return _allCanRead; }
            set
            {
                if (_allCanRead != value)
                {
                    _allCanRead = value;
                    this.OnPropertyChanged("AllCanRead");
                }
            }
        }

        private bool? _allCanWrite;

        public bool? AllCanWrite
        {
            get { return _allCanWrite; }
            set
            {
                if (_allCanWrite != value)
                {
                    _allCanWrite = value;
                    this.OnPropertyChanged("AllCanWrite");
                }
            }
        }


        
        private bool? _isAllInherited;

        public bool? IsAllInherited
        {
            get { return _isAllInherited; }
            set
            {
                if (_isAllInherited != value)
                {
                    _isAllInherited = value;
                    this.OnPropertyChanged("IsAllInherited");
                }
            }
        }


        public ObservableAdapterCollection<ObjectSecurityPropertyModel> Properties { get; private set; }


        private BusinessObjectSecurity _entity;
    }
}
