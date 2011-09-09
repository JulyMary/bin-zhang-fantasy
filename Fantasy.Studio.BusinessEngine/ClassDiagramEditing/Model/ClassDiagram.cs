using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.XSerialization;
using Fantasy.Windows;
using System.Collections.ObjectModel;
using Fantasy.BusinessEngine;
using System.Xml.Linq;
using Fantasy.BusinessEngine.Services;
using NHibernate;

namespace Fantasy.Studio.BusinessEngine.ClassDiagramEditing.Model
{

    [XSerializable("diagram", NamespaceUri=Consts.ClassDiagramNamespace)]
    public class ClassDiagram : NotifyPropertyChangedObject, IObjectWithSite
    {
        public ClassDiagram()
        {
            this.Classes = new ObservableCollection<ClassGlyph>();

            this.Classes.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(ClassesCollectionChanged);
            this.Inheritances = new ObservableCollection<InheritanceGlyph>();
            this.Inheritances.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(InheritancesCollectionChanged);

            this.Enums = new ObservableCollection<EnumGlyph>();
            this.Enums.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(EnumsCollectionChanged);

        }

        void EnumsCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            HashSet<BusinessEnum> entities = new HashSet<BusinessEnum>();

            if (e.Action != System.Collections.Specialized.NotifyCollectionChangedAction.Move)
            {
                if (e.OldItems != null)
                {
                    foreach (EnumGlyph node in e.OldItems)
                    {
                        entities.Add(node.Entity);

                        node.Diagram = null;
                        node.PropertyChanged -= new System.ComponentModel.PropertyChangedEventHandler(GlyphPropertyChanged);
                    }
                }
            }
            if (e.NewItems != null)
            {
                foreach (EnumGlyph node in e.NewItems)
                {
                    entities.Add(node.Entity);
                    node.Diagram = this;
                    node.Site = this.Site;
                    node.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(GlyphPropertyChanged);
                }
            }

            if (!this._deserializing)
            {
                this.EditingState = EditingState.Dirty;
                foreach (BusinessEnum @enum in entities)
                {
                    UpdateDisplayIndex(@enum);
                }
            }
        }

        private void UpdateDisplayIndex(BusinessEnum @enum)
        {
            EnumGlyph [] nodes = this.Enums.Where(n => n.Entity == @enum).ToArray();
            if (nodes.Length == 1)
            {
                nodes[0].DisplayIndex = 0;
            }
            else if (nodes.Length > 1)
            {
                for (int i = 0; i < nodes.Length; i++)
                {
                    nodes[i].DisplayIndex = i + 1;
                }
            }
        }

        void InheritancesCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action != System.Collections.Specialized.NotifyCollectionChangedAction.Move)
            {
                if (e.OldItems != null)
                {
                    foreach (InheritanceGlyph node in e.OldItems)
                    {
                        node.Diagram = null;
                        node.PropertyChanged -= new System.ComponentModel.PropertyChangedEventHandler(GlyphPropertyChanged);
                    }
                }
            }
            if (e.NewItems != null)
            {
                foreach (InheritanceGlyph node in e.NewItems)
                {
                    
                    node.Diagram = this;
                    node.Site = this.Site;
                    node.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(GlyphPropertyChanged);
                }
            }

            if (!this._deserializing)
            {
                this.EditingState = EditingState.Dirty;
               
            }
        }


        private bool _deserializing = false;

        public void LoadDiagram(BusinessClassDiagram entity)
        {
            this.Entity = entity;

            this.EditingState = Entity.EntityState == EntityState.Clean ? EditingState.Clean : Studio.EditingState.Dirty;

            this.Entity.EntityStateChanged += new EventHandler(Entity_EntityStateChanged);

            if (!string.IsNullOrEmpty(this.Entity.Diagram))
            {
                _deserializing = true;
                try
                {
                    XSerializer ser = new XSerializer(typeof(ClassDiagram));
                    XElement element = XElement.Parse(this.Entity.Diagram);
                    ser.Deserialize(element, this);
                }
                finally
                {
                    _deserializing = false;
                }

                IEntityService es = this.Site.GetRequiredService<IEntityService>();
                foreach (ClassGlyph node in this.Classes.ToArray())
                {
                    BusinessClass @class = es.DefaultSession.Get<BusinessClass>(node.ClassId);
                    if (@class == null)
                    {
                        this.Classes.Remove(node);
                    }
                    else
                    {
                        node.Entity = @class;
                    }
                }

                foreach (InheritanceGlyph inheritance in this.Inheritances.ToArray())
                {
                    ClassGlyph child = this.Classes.SingleOrDefault(c => c.Id == inheritance.DerivedGlyphId);
                    ClassGlyph parent = this.Classes.SingleOrDefault(c => c.Id == inheritance.BaseGlyphId);

                    if (child != null && parent != null && child.Entity.ParentClass == parent.Entity)
                    {
                        inheritance.BaseClass = parent;
                        inheritance.DerivedClass = child;
                    }
                    else
                    {
                        this.Inheritances.Remove(inheritance);
                    }
                }

                foreach (EnumGlyph node in this.Enums.ToArray())
                {
                    BusinessEnum @enum = es.DefaultSession.Get<BusinessEnum>(node.EnumId);
                    if (@enum == null)
                    {
                        this.Enums.Remove(node);
                    }
                    else
                    {
                        node.Entity = @enum;
                    }
                }

            }

            foreach (BusinessClass @class in this.Classes.Select(n => n.Entity).Distinct())
            {
                UpdateDisplayIndex(@class);
            }

            foreach (BusinessEnum @enum in this.Enums.Select(n => n.Entity).Distinct())
            {
                UpdateDisplayIndex(@enum);
            }

        }


        private void UpdateDisplayIndex(BusinessClass @class)
        {
            ClassGlyph[] nodes = this.Classes.Where(n => n.Entity == @class).ToArray();
            if (nodes.Length == 1)
            {
                nodes[0].DisplayIndex = 0;
            }
            else if (nodes.Length > 1)
            {
                for (int i = 0; i < nodes.Length; i++)
                {
                    nodes[i].DisplayIndex = i + 1;
                }
            }


        }


        void Entity_EntityStateChanged(object sender, EventArgs e)
        {
            if (this.Entity.EntityState != EntityState.Clean)
            {
                this.EditingState = Studio.EditingState.Dirty;
            }
        }

        public void SyncEntities()
        {

            foreach (ClassGlyph node in this.Classes.ToArray())
            {
                if (node.Entity.EntityState == EntityState.Deleted)
                {
                    this.Classes.Remove(node);
                }
            }

            foreach (InheritanceGlyph inheritance in this.Inheritances.ToArray())
            {
                ClassGlyph child = this.Classes.SingleOrDefault(c => c.Id == inheritance.DerivedGlyphId);
                ClassGlyph parent = this.Classes.SingleOrDefault(c => c.Id == inheritance.BaseGlyphId);

                if (child == null || parent == null || child.Entity.ParentClass != parent.Entity)
                {
                    this.Inheritances.Remove(inheritance);
                }
            }

            foreach (EnumGlyph node in this.Enums.ToArray())
            {
                if (node.Entity.EntityState == EntityState.Deleted)
                {
                    this.Enums.Remove(node);
                }
            }

        }

        public void Unload()
        {
            this.Entity.EntityStateChanged -= new EventHandler(Entity_EntityStateChanged);

        }

        public void Save()
        {
            ISession session = this.Site.GetRequiredService<IEntityService>().DefaultSession;
            IDDLService ddl = this.Site.GetRequiredService<IDDLService>();

            session.BeginUpdate();
            try
            {
                XSerializer ser = new XSerializer(typeof(ClassDiagram));
                this.Entity.Diagram = ser.Serialize(this).ToString(SaveOptions.OmitDuplicateNamespaces);

                session.SaveOrUpdate(this.Entity);

                foreach (IBusinessEntity entity in this.DeletingEntities)
                {
                    if (entity is BusinessClass)
                    {
                        BusinessClass cls = (BusinessClass)entity;
                        ddl.DeleteClassTable((BusinessClass)entity);
                        if (entity.EntityState != EntityState.New && entity.EntityState != EntityState.Deleted)
                        {
                            session.Delete(entity);
                        }
                    }
                }

                this.DeletingEntities.Clear();
               
                foreach (ClassGlyph cls in this.Classes)
                {
                    cls.SaveEntity();
                }

                foreach (InheritanceGlyph i in this.Inheritances)
                {
                    i.SaveEntity();
                }

                foreach (EnumGlyph e in this.Enums)
                {
                    e.SaveEntity();
                }

                session.EndUpdate(true);
                this.EditingState = EditingState.Clean;
            }
            catch
            {
                session.EndUpdate(false);
                throw;
            }
        }

        public BusinessClassDiagram Entity
        {
            get;
            private set;
        }

        void ClassesCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            HashSet<BusinessClass> entities = new HashSet<BusinessClass>();
            
            if (e.Action != System.Collections.Specialized.NotifyCollectionChangedAction.Move)
            {
                if (e.OldItems != null)
                {
                    foreach (ClassGlyph node in e.OldItems)
                    {
                        entities.Add(node.Entity);

                        node.Diagram = null;
                        node.PropertyChanged -= new System.ComponentModel.PropertyChangedEventHandler(GlyphPropertyChanged);
                    }
                }
            }
            if (e.NewItems != null)
            {
                foreach (ClassGlyph node in e.NewItems)
                {
                    entities.Add(node.Entity);
                    node.Diagram = this;
                    node.Site = this.Site;
                    node.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(GlyphPropertyChanged);
                }
            }
                 
            if (!this._deserializing)
            {
                this.EditingState = EditingState.Dirty;
                foreach (BusinessClass @class in entities)
                {
                    UpdateDisplayIndex(@class);
                }
            }
        }

        void GlyphPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "EditingState" && ((ClassDiagramGlyph)sender).EditingState == EditingState.Dirty)
            {
                this.EditingState = Studio.EditingState.Dirty;
            }
        }

        [XArray(Name="classes"),
        XArrayItem(Name = "class", Type = typeof(ClassGlyph))]
        public ObservableCollection<ClassGlyph> Classes { get; private set; }

        [XArray(Name = "enums"),
        XArrayItem(Name = "enum", Type = typeof(EnumGlyph))]
        public ObservableCollection<EnumGlyph> Enums { get; private set; }


        [XArray(Name="inheritances"),
        XArrayItem(Name = "inheritance", Type = typeof(InheritanceGlyph))]
        public ObservableCollection<InheritanceGlyph> Inheritances { get; private set; }

        public IServiceProvider Site { get; set; }


        private EditingState _editingState = EditingState.Clean;

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


        private ISet<IBusinessEntity> _deletingEntities = new HashSet<IBusinessEntity>() ;
        public ISet<IBusinessEntity> DeletingEntities
        {
            get
            {
                return _deletingEntities;
            }
        } 
  
       


       
    }
}
