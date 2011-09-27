using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Windows;
using Fantasy.BusinessEngine;
using Fantasy.Collections;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Fantasy.Studio.BusinessEngine.ObjectSecurityEditing
{
    public class ObjectSecurityEditorModel : NotifyPropertyChangedObject
    {
        

        public ObjectSecurityEditorModel(BusinessObjectSecurity entity)
        {
            this._entity = entity;
            this.Properties = new ObservableAdapterCollection<ObjectSecurityPropertyModel>(entity.Properties, (ps=>new ObjectSecurityPropertyModel((BusinessObjectMemberSecurity)ps)));
            this._isInherited = entity.ObjectAccess == null;
            if (!this._isInherited)
            {
                this._canCreate = (entity.ObjectAccess & BusinessObjectAccess.Create) == BusinessObjectAccess.Create;
                this._canDelete = (entity.ObjectAccess & BusinessObjectAccess.Delete) == BusinessObjectAccess.Delete;
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
                        this._entity.ObjectAccess = null;

                    }
                    else
                    {
                        this._entity.ObjectAccess = BusinessObjectAccess.None;
                    }
                    this._changing = true;
                    try
                    {
                        this.CanCreate = false;
                        this.CanDelete = false;
                    }
                    finally
                    {
                        this._changing = false;
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
                    if (!this._changing)
                    {
                        if (_canCreate)
                        {
                            _entity.ObjectAccess = _entity.ObjectAccess != null ? _entity.ObjectAccess | BusinessObjectAccess.Create : BusinessObjectAccess.Create;
                        }
                        else
                        {
                            _entity.ObjectAccess = _entity.ObjectAccess != null ? _entity.ObjectAccess & ~BusinessObjectAccess.Create : BusinessObjectAccess.None;

                        }
                    }
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
                    if (!this._changing)
                    {
                        if (_canCreate)
                        {
                            _entity.ObjectAccess = _entity.ObjectAccess != null ? _entity.ObjectAccess | BusinessObjectAccess.Delete : BusinessObjectAccess.Delete;
                        }
                        else
                        {
                            _entity.ObjectAccess = _entity.ObjectAccess != null ? _entity.ObjectAccess & ~BusinessObjectAccess.Delete : BusinessObjectAccess.None;

                        }
                    }
                    this.OnPropertyChanged("CanDelete");
                }
            }
        }

        


        public ObservableAdapterCollection<ObjectSecurityPropertyModel> Properties { get; private set; }


        private BusinessObjectSecurity _entity;
    }
}
