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
            this.Properties.CollectionChanged += new NotifyCollectionChangedEventHandler(PropertiesCollectionChanged);
            foreach (ObjectSecurityPropertyModel prop in this.Properties)
            {
                prop.PropertyChanged += new PropertyChangedEventHandler(ObjectSecurityPropertyChanged);
            }

            InitializeAllState();

        }

        private void InitializeAllState()
        {
            this._updatingAll = true;
            try
            {
                if (!this.Properties.Any(p => p.IsInherited))
                {
                    this.IsAllInherited = false;
                }
                else if (!this.Properties.Any(p => !p.IsInherited))
                {
                    this.IsAllInherited = true;
                }
                else
                {
                    this.IsAllInherited = null;
                }

                if (!this.Properties.Any(p => p.CanRead))
                {
                    this.AllCanRead = false;
                }
                else if (!this.Properties.Any(p => !p.CanRead))
                {
                    this.AllCanRead = true;
                }
                else
                {
                    this.AllCanRead = null;
                }

                if (!this.Properties.Any(p => p.CanWrite))
                {
                    this.AllCanWrite = false;
                }
                else if (!this.Properties.Any(p => !p.CanWrite))
                {
                    this.AllCanWrite = true;
                }
                else
                {
                    this.AllCanWrite = null;
                }

            }
            finally
            {
                this._updatingAll = false;
            }
        }

        void PropertiesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action != NotifyCollectionChangedAction.Move)
            {
                if (e.OldItems != null)
                {
                    foreach (ObjectSecurityPropertyModel prop in e.OldItems)
                    {
                        prop.PropertyChanged -= new PropertyChangedEventHandler(ObjectSecurityPropertyChanged);
                    }
                }
                if (e.NewItems != null)
                {
                    foreach (ObjectSecurityPropertyModel prop in e.NewItems)
                    {
                        prop.PropertyChanged += new PropertyChangedEventHandler(ObjectSecurityPropertyChanged);
                    }
                }
            }
        }


        private bool _updatingAll = false;


        void ObjectSecurityPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (!this._updatingAll)
            {
                this._updatingAll = true;
                try
                {
                    switch (e.PropertyName)
                    {
                        case "IsInherited":
                            if (!this.Properties.Any(p => p.IsInherited))
                            {
                                this.IsAllInherited = false;
                            }
                            else if (!this.Properties.Any(p => !p.IsInherited))
                            {
                                this.IsAllInherited = true;
                            }
                            else
                            {
                                this.IsAllInherited = null;
                            }
                            break;
                        case "CanRead":
                            if (!this.Properties.Any(p => p.CanRead))
                            {
                                this.AllCanRead = false;
                            }
                            else if (!this.Properties.Any(p => !p.CanRead))
                            {
                                this.AllCanRead = true;
                            }
                            else
                            {
                                this.AllCanRead = null;
                            }
                            break;
                           
                        case "CanWrite":
                            if (!this.Properties.Any(p => p.CanWrite))
                            {
                                this.AllCanWrite = false;
                            }
                            else if (!this.Properties.Any(p => !p.CanWrite))
                            {
                                this.AllCanWrite = true;
                            }
                            else
                            {
                                this.AllCanWrite = null;
                            }
                            break;
                    }
                }
                finally
                {
                    this._updatingAll = false;
                }
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

        private bool? _allCanRead;

        public bool? AllCanRead
        {
            get { return _allCanRead; }
            set
            {
                if (_allCanRead != value)
                {
                    _allCanRead = value;

                    if (!this._updatingAll && this._allCanRead != null)
                    {
                        this._updatingAll = true;
                        try
                        {
                            
                            foreach (ObjectSecurityPropertyModel prop in this.Properties)
                            {
                                prop.CanRead = (bool)this._allCanRead;
                            }
                            
                        }
                        finally
                        {
                            this._updatingAll = false;
                        }
                    }


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
                    if (!this._updatingAll && this._allCanWrite != null)
                    {
                        this._updatingAll = true;
                        try
                        {

                            foreach (ObjectSecurityPropertyModel prop in this.Properties)
                            {
                                prop.CanWrite = (bool)this._allCanWrite;
                            }

                        }
                        finally
                        {
                            this._updatingAll = false;
                        }
                    }
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
                    if (!this._updatingAll && this._isAllInherited != null)
                    {
                        this._updatingAll = true;
                        try
                        {

                            foreach (ObjectSecurityPropertyModel prop in this.Properties)
                            {
                                prop.IsInherited = (bool)this._isAllInherited;
                            }

                        }
                        finally
                        {
                            this._updatingAll = false;
                        }
                    }
                    this.OnPropertyChanged("IsAllInherited");
                }
            }
        }


        public ObservableAdapterCollection<ObjectSecurityPropertyModel> Properties { get; private set; }


        private BusinessObjectSecurity _entity;
    }
}
