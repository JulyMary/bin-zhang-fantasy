﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.XSerialization;
using Fantasy.Windows;
using Fantasy.BusinessEngine;
using System.Windows;
using System.Collections.Specialized;
using NHibernate;
using Fantasy.BusinessEngine.Services;
using Fantasy.Collections;

namespace Fantasy.Studio.BusinessEngine.ClassDiagramEditing.Model
{
    [XSerializable("class", NamespaceUri= Consts.ClassDiagramNamespace)]
    public class ClassGlyph : ClassDiagramGlyph, IBusinessEntityGlyph
    {

        public ClassGlyph ()
	    {
            this._classListener = new WeakEventListener(this.EntityStateChanged);
            this._propertyListener = new WeakEventListener(this.EntityStateChanged);
            this._propertyCollectionListener = new WeakEventListener(this.PropertyCollectionChanged); 
	    }

        [XAttribute("class")]
        private Guid _classId;

        public Guid ClassId
        {
            get { return _classId; }
            set
            {
                if (_classId != value)
                {
                    _classId = value;
                    this.OnPropertyChanged("ClassId");
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

        [XAttribute("showProperties")]
        private bool _showProperties;

        public bool ShowProperties
        {
            get { return _showProperties; }
            set
            {
                if (_showProperties != value)
                {
                    _showProperties = value;
                    this.OnPropertyChanged("ShowProperties");
                }
            }
        }

      
        [XAttribute("showRelations")]
        private bool _showRelations;

        public bool ShowRelations
        {
            get { return _showRelations; }
            set
            {
                if (_showRelations != value)
                {
                    _showRelations = value;
                    this.OnPropertyChanged("ShowRelations");
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

        private WeakEventListener _classListener;
        private WeakEventListener _propertyListener;
        private WeakEventListener _propertyCollectionListener;


        private bool EntityStateChanged(Type managerType, object sender, EventArgs e)
        {
            if (((IEntity)sender).EntityState != EntityState.Clean)
            {
                this.EditingState = EditingState.Dirty;
            }
            return true;
        }

        private void AttatchEntity(BusinessClass entity)
        {
           
            EditingState state = this.Entity.EntityState == EntityState.Clean ? EditingState.Clean : EditingState.Dirty;
            EntityStateChangedEventManager.AddListener(this.Entity, this._classListener);
            foreach (BusinessProperty prop in entity.Properties)
            {
                EntityStateChangedEventManager.AddListener(prop, this._propertyListener);
                if (prop.EntityState != EntityState.Clean)
                {
                    state = EditingState.Dirty;
                }
            }

            this.Properties = new ObservableAdapterCollection<PropertyNode>(entity.Properties, p => { return new PropertyNode((BusinessProperty)p); });

            this.EditingState = state;


            CollectionChangedEventManager.AddListener((INotifyCollectionChanged)this.Entity.Properties, this._propertyCollectionListener); 
            

        }

        private bool PropertyCollectionChanged(Type managerType, object sender, EventArgs args)
        {
            NotifyCollectionChangedEventArgs e = (NotifyCollectionChangedEventArgs)args;
            if (e.Action != NotifyCollectionChangedAction.Move)
            {
                if (e.OldItems != null)
                {
                    foreach (BusinessProperty prop in e.OldItems)
                    {
                        EntityStateChangedEventManager.RemoveListener(prop, this._propertyListener);
                    }
                }

                if (e.NewItems != null)
                {

                    foreach (BusinessProperty prop in e.NewItems)
                    {
                        EntityStateChangedEventManager.AddListener(prop, this._propertyListener);
                    }
                }
            }

            this.EditingState = EditingState.Dirty;
            return true;
        }



        private BusinessClass _entity;

        public BusinessClass Entity
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
            "EditingState", "Properties", "DisplayIndex"
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
                foreach (BusinessProperty prop in this.Entity.Properties)
                {
                    session.SaveOrUpdate(prop);
                }

                IDDLService dll = this.Site.GetRequiredService<IDDLService>();

                dll.CreateClassTable(this.Entity);

                session.EndUpdate(true);
                base.SaveEntity();
            }
            catch
            {
                session.EndUpdate(false);
                throw;
            }
        }


        private INotifyCollectionChanged _properties;

        public INotifyCollectionChanged Properties
        {
            get { return _properties; }
            private set
            {
                if (_properties != value)
                {
                    _properties = value;
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
