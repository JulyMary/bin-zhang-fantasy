using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Fantasy.BusinessEngine;
using Fantasy.AddIns;
using System.ComponentModel;

namespace Fantasy.Studio.BusinessEngine
{
    /// <summary>
    /// Interaction logic for EntityEditingViewContent.xaml
    /// </summary>
    public abstract partial class EntityEditingViewContent : UserControl, IViewContent, IEditingViewContent, IObjectWithSite
    {
        public EntityEditingViewContent()
        {
            InitializeComponent();
        }

        public abstract string EditingPanelPath { get; }

        public abstract string CommandBindingPath { get;}
            

        public IBusinessEntity Entity { get; private set; }

        EntityEditingViewContentModel _model;

        public virtual void Load(IBusinessEntity entity)
        {
            this.Entity = entity;
            if ((entity is INamedBusinessEntity) && (entity is INotifyPropertyChanged))
            {
                ((INotifyPropertyChanged)entity).PropertyChanged += new PropertyChangedEventHandler(Entity_PropertyChanged);
            }
            _model = new EntityEditingViewContentModel();
            if (this.EditingPanelPath != null)
            {
                _model.EditingPanels.AddRange(AddInTree.Tree.GetTreeNode(this.EditingPanelPath).BuildChildItems<IEntityEditingPanel>(this,this._services));
            }

            if (this.CommandBindingPath != null)
            {
                foreach (CommandBinding cb in AddInTree.Tree.GetTreeNode(this.CommandBindingPath).BuildChildItems(this, this._services))
                {
                    this.CommandBindings.Add(cb);
                }
            }

            foreach (IEntityEditingPanel panel in _model.EditingPanels)
            {
                panel.Load(entity);

                TabItem item = new TabItem() { Header = panel.Title, Content = panel.Content };
                this.panelContainer.Items.Add(item);

                item.Visibility = _model.TabStripVisibility;

                panel.DirtyStateChanged += new EventHandler(EditingPanel_DirtyStateChanged);
            }

            EvalDirtyState();
        }

        void EditingPanel_DirtyStateChanged(object sender, EventArgs e)
        {
            EvalDirtyState();
        }

        private void EvalDirtyState()
        {
            EditingState  state = EditingState.Clean;
            foreach (IEntityEditingPanel panel in _model.EditingPanels)
            {
                if (panel.DirtyState == EditingState.Dirty)
                {
                    state = EditingState.Dirty;
                    break;
                }
            }
            this.DirtyState = state;
        }

        void Entity_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Name")
            {
                this.OnTitleChanged(EventArgs.Empty);
            }
        }


        #region IViewContent Members

        UIElement IViewContent.Content
        {
            get { return this; }
        }

        IWorkbenchWindow IViewContent.WorkbenchWindow { get; set; }
        

        public virtual string Title
        {
            get { return (Entity is INamedBusinessEntity) ? ((INamedBusinessEntity)Entity).Name : string.Empty; }
        }

        public virtual void Selected()
        {
            
        }

        public virtual void Deselected()
        {
           
        }

        public virtual void Closing(System.ComponentModel.CancelEventArgs e)
        {
            foreach (IEntityEditingPanel panel in this._model.EditingPanels)
            {
                panel.Closing(e);
            }
        }

        public virtual void Closed()
        {
            foreach (IEntityEditingPanel panel in this._model.EditingPanels)
            {
                panel.Closed();
            }
        }

        public event EventHandler TitleChanged;

        protected virtual void OnTitleChanged(EventArgs e)
        {
            if (this.TitleChanged != null)
            {
                this.TitleChanged(this, e);
            }
        }


        public abstract string DocumentName { get; }
       

        public abstract string DocumentType {get;} 
       
        public virtual void Dispose()
        {
            
        }

        #endregion

        #region IEditingViewContent Members

        void IEditingViewContent.Load(object data)
        {
            this.Load((IBusinessEntity)data);
        }

        object IEditingViewContent.Data
        {
            get { return this.Entity; }
        }

        private EditingState _dirtyState =  EditingState.Clean;
        public virtual EditingState DirtyState
        {
            get
            {
                return _dirtyState;
            }
            protected set
            {
                if (_dirtyState != value)
                {
                    this._dirtyState = value;
                    this.OnDirtyStateChanged(EventArgs.Empty);
                }
            }
        }

        public event EventHandler DirtyStateChanged;

        protected virtual void OnDirtyStateChanged(EventArgs e)
        {
            if (this.DirtyStateChanged != null)
            {
                this.DirtyStateChanged(this, e);
            }
        }


        public virtual void Save()
        {
            foreach (IEntityEditingPanel panel in this._model.EditingPanels)
            {
                if (panel.DirtyState == EditingState.Dirty)
                {
                    panel.Save();
                }
            }
        }

        #endregion

        Enum IStateObject.State
        {
            get { return this.DirtyState; }
        }



        #region IObjectWithSite Members
        private IServiceProvider _services;
        IServiceProvider IObjectWithSite.Site
        {
            get
            {
                return _services;
            }
            set
            {
                _services = value;
            }
        }

        #endregion
    }
}
