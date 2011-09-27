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

                    if (_canCreate)
                    {
                        _entity.ObjectAccess = _entity.ObjectAccess != null ? _entity.ObjectAccess | BusinessObjectAccess.Create : BusinessObjectAccess.Create;
                    }
                    else
                    {
                        _entity.ObjectAccess = _entity.ObjectAccess != null ? _entity.ObjectAccess & ~BusinessObjectAccess.Create : BusinessObjectAccess.None;

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

                    if (_canCreate)
                    {
                        _entity.ObjectAccess = _entity.ObjectAccess != null ? _entity.ObjectAccess | BusinessObjectAccess.Delete : BusinessObjectAccess.Delete;
                    }
                    else
                    {
                        _entity.ObjectAccess = _entity.ObjectAccess != null ? _entity.ObjectAccess & ~BusinessObjectAccess.Delete : BusinessObjectAccess.None;

                    }
                    this.OnPropertyChanged("CanDelete");
                }
            }
        }

        


        public ObservableAdapterCollection<ObjectSecurityPropertyModel> Properties { get; private set; }


        private BusinessObjectSecurity _entity;
    }
}
