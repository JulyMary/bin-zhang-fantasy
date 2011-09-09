using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.XSerialization;
using Fantasy.Windows;
using Fantasy.BusinessEngine;
using System.Collections.Specialized;
using Fantasy.Collections;
using NHibernate;
using Fantasy.BusinessEngine.Services;

namespace Fantasy.Studio.BusinessEngine.ClassDiagramEditing.Model
{
    [XSerializable("enum", NamespaceUri= Consts.ClassDiagramNamespace)]
    public class EnumGlyph : ClassDiagramGlyph, Fantasy.Studio.BusinessEngine.ClassDiagramEditing.Model.IBusinessEntityGlyph
    {
         public EnumGlyph()
	    {
            this._enumListener = new WeakEventListener(this.EntityStateChanged);
            this._enumValueListener = new WeakEventListener(this.EntityStateChanged);
            this._enumValueCollectionListener = new WeakEventListener(this.EnumValueCollectionChanged); 
	    }


       

        [XAttribute("enum")]
        private Guid _enumId;

        public Guid EnumId
        {
            get { return _enumId; }
            set
            {
                if (_enumId != value)
                {
                    _enumId = value;
                    this.OnPropertyChanged("EnumId");
                }
            }
        }
      
          
        [XAttribute("showMember")]
        private bool _showMember = true;

        public bool ShowMember
        {
            get { return _showMember; }
            set
            {
                if (_showMember != value)
                {
                    _showMember = value;
                    this.OnPropertyChanged("ShowMember");
                }
            }
        }

       

        private bool? _isShortCut;

        public bool IsShortCut
        {
            get 
            {
                if (_isShortCut == null)
                {
                    _isShortCut = this.Entity.Package != this.Diagram.Entity.Package;
                }
                return (bool)_isShortCut; 
            }
        }

        private WeakEventListener _enumListener;
        private WeakEventListener _enumValueListener;
        private WeakEventListener _enumValueCollectionListener;


        private bool EntityStateChanged(Type managerType, object sender, EventArgs e)
        {
            if (((IEntity)sender).EntityState != EntityState.Clean)
            {
                this.EditingState = EditingState.Dirty;
            }
            return true;
        }

        private void AttatchEntity(BusinessEnum entity)
        {
           
            EditingState state = this.Entity.EntityState == EntityState.Clean ? EditingState.Clean : EditingState.Dirty;
            EntityStateChangedEventManager.AddListener(this.Entity, this._enumListener);
            foreach (BusinessEnumValue ev in entity.EnumValues)
            {
                EntityStateChangedEventManager.AddListener(ev, this._enumValueListener);
                if (ev.EntityState != EntityState.Clean)
                {
                    state = EditingState.Dirty;
                }
            }

            this.EnumValues = new ObservableAdapterCollection<EnumValueNode>(entity.EnumValues, v => { return new EnumValueNode((BusinessEnumValue)v); });

            this.EditingState = state;


            CollectionChangedEventManager.AddListener((INotifyCollectionChanged)this.Entity.EnumValues, this._enumValueCollectionListener); 
            

        }

        private bool EnumValueCollectionChanged(Type managerType, object sender, EventArgs args)
        {
            NotifyCollectionChangedEventArgs e = (NotifyCollectionChangedEventArgs)args;
            if (e.Action != NotifyCollectionChangedAction.Move)
            {
                if (e.OldItems != null)
                {
                    foreach (BusinessEnumValue value in e.OldItems)
                    {
                        EntityStateChangedEventManager.RemoveListener(value, this._enumValueListener);
                    }
                }

                if (e.NewItems != null)
                {

                    foreach (BusinessEnumValue value in e.NewItems)
                    {
                        EntityStateChangedEventManager.AddListener(value, this._enumValueListener);
                    }
                }
            }

            this.EditingState = EditingState.Dirty;
            return true;
        }



        private BusinessEnum _entity;

        public BusinessEnum Entity
        {
            get { return _entity; }
            set 
            {
                _entity = value;
                if (value != null)
                {
                    this.AttatchEntity(_entity);
                }
            }
        }


       


        private int _displayIndex;

        public int DisplayIndex
        {
            get { return _displayIndex; }
            set
            {
                if (_displayIndex != value)
                {
                    _displayIndex = value;
                    this.OnPropertyChanged("DisplayIndex");
                }
            }
        }


        private static string[] NoneSerializeProperties = new string[] {
            "EditingState", "EnumValues", "DisplayIndex"
        };

        protected override void OnPropertyChanged(string propertyName)
        {

            base.OnPropertyChanged(propertyName);
            if (Array.IndexOf(NoneSerializeProperties, propertyName) < 0)
            {
                this.EditingState = EditingState.Dirty;
            }
        }

        public override void SaveEntity()
        {
            ISession session = this.Site.GetRequiredService<IEntityService>().DefaultSession;

         
            session.BeginUpdate();
            try
            {


                session.SaveOrUpdate(this.Entity);
                foreach (BusinessEnumValue prop in this.Entity.EnumValues)
                {
                    session.SaveOrUpdate(prop);
                }

                IDDLService dll = this.Site.GetRequiredService<IDDLService>();
                session.EndUpdate(true);
                base.SaveEntity();
            }
            catch
            {
                session.EndUpdate(false);
                throw;
            }
        }


        private INotifyCollectionChanged _enumValues;

        public INotifyCollectionChanged EnumValues
        {
            get { return _enumValues; }
            private set
            {
                if (_enumValues != value)
                {
                    _enumValues = value;
                    this.OnPropertyChanged("Properties");
                }
            }
        }

       

        IBusinessEntity IBusinessEntityGlyph.Entity
        {
            get { return this.Entity; }
        }

       
    }
}
