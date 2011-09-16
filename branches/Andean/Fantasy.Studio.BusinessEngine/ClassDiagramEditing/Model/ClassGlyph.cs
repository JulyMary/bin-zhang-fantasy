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
using Fantasy.Collections;
using System.ComponentModel;

namespace Fantasy.Studio.BusinessEngine.ClassDiagramEditing.Model
{
    [XSerializable("class", NamespaceUri= Consts.ClassDiagramNamespace)]
    public class ClassGlyph : ClassDiagramGlyph, IBusinessEntityGlyph
    {

        public ClassGlyph ()
	    {
            this._classListener = new WeakEventListener((t, sender, e) =>
            {
                if (((IEntity)sender).EntityState != EntityState.Clean)
                {
                    this.EditingState = EditingState.Dirty;
                }
                return true;
            });

            this._memberListener = new WeakEventListener((t, sender, e) =>
            {
                if (((MemberNode)sender).EditingState == Studio.EditingState.Dirty)
                {
                    this.EditingState = Studio.EditingState.Dirty;
                }
                return true;
            });

            this._memberCollectionListener = new WeakEventListener((t, sender, args) =>
            {
                NotifyCollectionChangedEventArgs e = (NotifyCollectionChangedEventArgs)args;
                if (e.Action != NotifyCollectionChangedAction.Move)
                {
                    if (e.OldItems != null)
                    {
                        foreach (MemberNode member in e.OldItems)
                        {
                            PropertyChangedEventManager.RemoveListener(member, this._memberListener, "EditingState");
                        }
                    }

                    if (e.NewItems != null)
                    {

                        foreach (MemberNode member in e.NewItems)
                        {
                            PropertyChangedEventManager.AddListener(member, this._memberListener, "EditingState");
                        }
                    }
                }

                this.EditingState = EditingState.Dirty;
                return true;
            });

            this._inheritanceChangedListener = new WeakEventListener((t, sender, args) =>
            {
                this.Members.Lock();
                try
                {
                    this.DetachInheritedData();
                    this.AttachInheritedData();
                }
                finally
                {
                    this.Members.Unlock();
                }
                return true;
            });
	    }

        [XAttribute("id")]
        private Guid _id;

        public Guid Id
        {
            get { return _id; }
            set
            {
                if (_id != value)
                {
                    _id = value;
                    this.OnPropertyChanged("Id");
                }
            }
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

        [XAttribute("showInherited")]
        private bool _showInheritedMembers;

        public bool ShowInheritedMembers
        {
            get { return _showInheritedMembers; }
            set
            {
                if (_showInheritedMembers != value)
                {
                    _showInheritedMembers = value;
                    this.OnPropertyChanged("ShowInheritedMembers");
                    if (this.Entity != null)
                    {
                        if (value)
                        {
                            this.AttachInheritedData();
                        }
                        else
                        {
                            this.DetachInheritedData();
                        }
                    }
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
        private WeakEventListener _memberListener;
        private WeakEventListener _memberCollectionListener;


        private ObservableAdapterCollection<PropertyNode> _properties;
        public ObservableAdapterCollection<PropertyNode> Properties
        {
            get
            {
                return _properties;
            }
        }
        private ObservableAdapterCollection<LeftRoleNode> _leftRoles;
        private ObservableAdapterCollection<RightRoleNode> _rightRoles;

        private void AttatchEntity(BusinessClass entity)
        {
           
            EditingState state = this.Entity.EntityState == EntityState.Clean ? EditingState.Clean : EditingState.Dirty;
            EntityStateChangedEventManager.AddListener(this.Entity, this._classListener);


            this._properties = new ObservableAdapterCollection<PropertyNode>(entity.Properties, p => { return new PropertyNode((BusinessProperty)p) { Site = this.Site }; });
            this._leftRoles = new ObservableAdapterCollection<LeftRoleNode>(entity.RightAssociations, a => { return new LeftRoleNode((BusinessAssociation)a) { Site = this.Site }; });
            this._rightRoles = new ObservableAdapterCollection<RightRoleNode>(entity.LeftAssociations, a => { return new RightRoleNode((BusinessAssociation)a){ Site = this.Site }; });

            this.Members.Lock();
            try
            {
                this.Members.AddChildCollection(this._properties);
                this.Members.AddChildCollection(this._leftRoles);
                this.Members.AddChildCollection(this._rightRoles);

                if (this.ShowInheritedMembers)
                {
                    this.AttachInheritedData();
                }
            }
            finally
            {
                this.Members.Unlock();
            }

            CollectionChangedEventManager.AddListener(this._properties, this._memberCollectionListener);
            CollectionChangedEventManager.AddListener(this._leftRoles, this._memberCollectionListener);
            CollectionChangedEventManager.AddListener(this._rightRoles, this._memberCollectionListener);

           

            foreach (MemberNode member in this._properties.Union<MemberNode>(this._leftRoles).Union(this._rightRoles))
            {
                PropertyChangedEventManager.AddListener(member, this._memberListener, "EditingState");
                if (member.EditingState == Studio.EditingState.Dirty)
                {
                    state = EditingState.Dirty;
                }
            }

            this.EditingState = state;


        }

        private WeakEventListener _inheritanceChangedListener;


        private void DetachInheritedData()
        {
            this.Members.Lock();
            PropertyChangedEventManager.RemoveListener(this.Entity, this._inheritanceChangedListener, "ParentClass");
            try
            {
                foreach (InheritedData data in this._inheritedData)
                {
                    PropertyChangedEventManager.RemoveListener(data.Class, this._inheritanceChangedListener, "ParentClass");
                    this.Members.RemoveChildCollection(data.Properties);
                    this.Members.RemoveChildCollection(data.LeftRoles);
                    this.Members.RemoveChildCollection(data.RightRoles);
                }
            }
            finally
            {
                this.Members.Unlock();
            }
        }

        private void AttachInheritedData()
        {
            PropertyChangedEventManager.AddListener(this.Entity, this._inheritanceChangedListener, "ParentClass");
            this.Members.Lock();
            try
            {
                BusinessClass @class = this.Entity.ParentClass;
                while (@class != null)
                {
                    InheritedData data = new InheritedData()
                    {
                        Class = @class,
                        Properties = new ObservableAdapterCollection<PropertyNode>(@class.Properties, p => new PropertyNode((BusinessProperty)p) { Site = this.Site, IsInherited = true }),
                        LeftRoles = new ObservableAdapterCollection<LeftRoleNode>(@class.RightAssociations, a => new LeftRoleNode((BusinessAssociation)a) { Site = this.Site, IsInherited = true }),
                        RightRoles = new ObservableAdapterCollection<RightRoleNode>(@class.LeftAssociations, a => new RightRoleNode((BusinessAssociation)a) { Site = this.Site, IsInherited = true })

                    };
                    
                    this.Members.AddChildCollection(data.Properties);
                    this.Members.AddChildCollection(data.LeftRoles);
                    this.Members.AddChildCollection(data.RightRoles);
                    PropertyChangedEventManager.AddListener(@class, this._inheritanceChangedListener, "ParentClass");

                    this._inheritedData.Add(data);
                    @class = @class.ParentClass;
                }
            }
            finally
            {
                this.Members.Unlock();
            }
            
        }

        private List<InheritedData> _inheritedData = new List<InheritedData>();

        private class InheritedData
        {
            public BusinessClass Class { get; set; }

            public ObservableAdapterCollection<PropertyNode> Properties { get; set; }

            public ObservableAdapterCollection<LeftRoleNode> LeftRoles { get; set; }

            public ObservableAdapterCollection<RightRoleNode> RightRoles { get; set; }
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
            "EditingState", "DisplayIndex"
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
            if (this.DisplayIndex <= 1)
            {
                IEntityService es = this.Site.GetRequiredService<IEntityService>();


                es.BeginUpdate();
                try
                {


                    es.SaveOrUpdate(this.Entity);
                    foreach (MemberNode member in this.Members)
                    {
                        member.SaveEntity();
                    }

                    es.EndUpdate(true);
                    base.SaveEntity();
                }
                catch
                {
                    es.EndUpdate(false);
                    throw;
                }
            }
        }


        private MemaberNodeCollection _members = new MemaberNodeCollection();

        public MemaberNodeCollection Members
        {
            get { return _members; }
            
        }

        IBusinessEntity IBusinessEntityGlyph.Entity
        {
            get { return this.Entity; }
        }

    }
}
