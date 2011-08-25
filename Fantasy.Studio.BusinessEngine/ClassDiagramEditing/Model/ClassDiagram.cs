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
            this.Classes = new ObservableCollection<ClassNode>();

            this.Classes.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(ClassesCollectionChanged);

        }


        private bool _deserializing = false;

        public void LoadDiagram(BusinessClassDiagram entity)
        {
            this.Entity = entity;
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
                foreach (ClassNode node in this.Classes.ToArray())
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
            }
        }

        public void SyncEntities()
        {

            foreach (ClassNode node in this.Classes.ToArray())
            {
                if (node.Entity.EntityState == EntityState.Deleted)
                {
                    this.Classes.Remove(node);
                }
            }
        }


        public void Save()
        {
            ISession session = this.Site.GetRequiredService<IEntityService>().DefaultSession;


            session.BeginUpdate();
            try
            {
                XSerializer ser = new XSerializer(typeof(ClassDiagram));
                this.Entity.Diagram = ser.Serialize(this).ToString(SaveOptions.OmitDuplicateNamespaces);

                session.SaveOrUpdate(this.Entity);
                foreach (ClassNode cls in this.Classes)
                {
                    cls.SaveEntity();
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

            if (e.Action != System.Collections.Specialized.NotifyCollectionChangedAction.Move)
            {
                foreach (ClassNode node in e.OldItems)
                {
                    node.Diagram = null;
                    node.PropertyChanged -= new System.ComponentModel.PropertyChangedEventHandler(ClassNodePropertyChanged);
                }
            }
            foreach (ClassNode node in e.NewItems)
            {
                node.Diagram = this;
                node.Site = this.Site;
                node.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(ClassNodePropertyChanged);
            }
                 
            if (!this._deserializing)
            {
                this.EditingState = EditingState.Dirty;
            }
        }

        void ClassNodePropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "EditingState" && ((ClassNode)sender).EditingState == EditingState.Dirty)
            {
                this.EditingState = Studio.EditingState.Dirty;
            }
        }

        [XArray(Name="classes"),
        XArrayItem(Name = "class", Type = typeof(ClassNode))]
        public ObservableCollection<ClassNode> Classes { get; private set; }

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


       
      
    }
}
