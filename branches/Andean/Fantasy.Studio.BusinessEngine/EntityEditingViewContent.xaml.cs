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
    public abstract partial class EntityEditingViewContent : UserControl, IViewContent, IEditingViewContent, IObjectWithSite, IEntityEditingPanelContainer
    {
        public EntityEditingViewContent()
        {
            InitializeComponent();
        }

        public abstract string EditingPanelPath { get; }

        public abstract string CommandBindingPath { get;}
            

        public IBusinessEntity Entity { get; private set; }

        EntityEditingViewContentModel _model;


        public ICollection<IEntityEditingPanel> Panels
        {
            get { return this._model.EditingPanels; }
        }


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
                _model.EditingPanels.AddRange(AddInTree.Tree.GetTreeNode(this.EditingPanelPath).BuildChildItems<IEntityEditingPanel>(this,this.Site));
            }

            if (this.CommandBindingPath != null)
            {
                foreach (CommandBinding cb in AddInTree.Tree.GetTreeNode(this.CommandBindingPath).BuildChildItems(this, this.Site))
                {
                    this.CommandBindings.Add(cb);
                }
            }

            foreach (IEntityEditingPanel panel in _model.EditingPanels)
            {
                panel.Initialize();
                panel.Load(entity);
                panel.DirtyStateChanged += new EventHandler(EditingPanel_DirtyStateChanged);
            }

            this.DataContext = this._model;
            if (this._model.EditingPanels.Count > 0)
            {
                this.ActivePanel = this._model.EditingPanels[0];
                this.panelContainer.SelectedIndex = 0;
            }

           

            EvalDirtyState();
        }




        public IEntityEditingPanel ActivePanel
        {
            get { return (IEntityEditingPanel)GetValue(ActivePanelProperty); }
            set { SetValue(ActivePanelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ActivePanel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ActivePanelProperty =
            DependencyProperty.Register("ActivePanel", typeof(IEntityEditingPanel), typeof(EntityEditingViewContent), new UIPropertyMetadata(null, ActivePanelChangedCallback));

        private static void ActivePanelChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((EntityEditingViewContent)d).panelContainer.SelectedItem = e.NewValue;

            ((EntityEditingViewContent)d).OnActivePanelChanged(EventArgs.Empty);
        }

        private void panelContainer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.ActivePanel = (IEntityEditingPanel)this.panelContainer.SelectedItem;
        }


        public event EventHandler ActivePanelChanged;

        protected virtual void OnActivePanelChanged(EventArgs e)
        {
            if (this.ActivePanelChanged != null)
            {
                this.ActivePanelChanged(this, e);
            }
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

        public UIElement Element
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

            foreach (IEntityEditingPanel panel in this._model.EditingPanels)
            {
                panel.ViewContentSelected();
            }
        }

        public virtual void Deselected()
        {
            foreach (IEntityEditingPanel panel in this._model.EditingPanels)
            {
                panel.ViewContentDeselected();
            }
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
            //this.EvalDirtyState();
        }

        #endregion

       



       
        public IServiceProvider Site { get; set; }






       
       
    }
}
