using System;
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

namespace Fantasy.Studio.BusinessEngine.ClassDiagramEditing.Model
{
    [XSerializable("class", NamespaceUri= Consts.ClassDiagramNamespace)]
    public class ClassNode : NotifyPropertyChangedObject, IObjectWithSite
    {

        public ClassNode ()
	    {
            this._classListener = new WeakEventListener(this.EntityStateChanged);
            this._propertyListener = new WeakEventListener(this.EntityStateChanged);
            this._propertyCollectionListener = new WeakEventListener(this.PropertyCollectionChanged); 
	    }

        [XAttribute("left")]
        private double _left = 0;

        public double Left
        {
            get { return _left; }
            set
            {
                if (_left != value)
                {
                    _left = value;
                    this.OnPropertyChanged("Left");
                }
            }
        }

        [XAttribute("top")]
        private double _top = 0;
        public double Top
        {
            get { return _top; }
            set
            {
                if (_top != value)
                {
                    _top = value;
                    this.OnPropertyChanged("Top");
                }
            }
        }

        

        [XAttribute("width")]
        private double _width = 180;

        public double Width
        {
            get { return _width; }
            set
            {
                if (_width != value)
                {
                    _width = value;
                    this.OnPropertyChanged("Width");
                }
            }
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
           
            //this.Entity = entity;

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

            CollectionChangedEventManager.AddListener((INotifyCollectionChanged)this.Entity.Properties, this._propertyCollectionListener); 
        
        }

        private bool PropertyCollectionChanged(Type managerType, object sender, EventArgs args)
        {
            NotifyCollectionChangedEventArgs e = (NotifyCollectionChangedEventArgs)args;
            if (e.Action != NotifyCollectionChangedAction.Move)
            {
                foreach (BusinessProperty prop in e.OldItems)
                {
                    EntityStateChangedEventManager.RemoveListener(prop, this._propertyListener);  
                }

                foreach (BusinessProperty prop in e.NewItems)
                {
                    EntityStateChangedEventManager.AddListener(prop, this._propertyListener);  
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


        public ClassDiagram Diagram { get; internal set; }

        public IServiceProvider Site { get; set; }


        private EditingState _editingState = EditingState.Dirty ;

        public EditingState EditingState
        {
            get { return _editingState; }
            set
            {
                if (_editingState != value)
                {
                    _editingState = value;
                    this.OnPropertyChanged("EditingState");
                }
            }
        }

        protected override void OnPropertyChanged(string propertyName)
        {

            base.OnPropertyChanged(propertyName);
            if (propertyName != "EditingState")
            {
                this.EditingState = EditingState.Dirty;
            }
        }

        public void SaveEntity()
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
                this.EditingState = EditingState.Clean;
            }
            catch
            {
                session.EndUpdate(false);
                throw;
            }
        }


    }
}
