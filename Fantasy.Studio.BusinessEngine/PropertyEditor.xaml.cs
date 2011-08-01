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
using Fantasy.Studio.Controls;
using Fantasy.AddIns;
using Fantasy.Studio.Services;
using Fantasy.ServiceModel;
using System.ComponentModel.Design;
using NHibernate;
using Fantasy.BusinessEngine.Services;
using System.Collections.Specialized;

namespace Fantasy.Studio.BusinessEngine
{
    /// <summary>
    /// Interaction logic for BusinessPropertyEditor.xaml
    /// </summary>
    public partial class PropertyEditor : UserControl, IEntityEditingPanel, IObjectWithSite
    {
        public PropertyEditor()
        {
            InitializeComponent();
           
        }

        public IServiceProvider Site { get; set; }

        private ISelectionService _selectionService = new SelectionService(null);

        private PropertyEditorModel _model;

        protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            if (this.Site != null)
            {
                IMonitorSelectionService monitor = this.Site.GetRequiredService<IMonitorSelectionService>();
                monitor.CurrentSelectionService = _selectionService;
            }
            base.OnGotKeyboardFocus(e);
        }

       
        #region IEntityEditingPanel Members

        public void Initialize()
        {
            
        }

        public void Load(IBusinessEntity entity)
        {
            this._model = new PropertyEditorModel((BusinessClass)entity);

            this._model.Class.Properties.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(PropertiesChanged);

            foreach (ToolBar bar in AddInTree.Tree.GetTreeNode("fantasy/studio/businessengine/classeditor/propertyeditor/toolbars").BuildChildItems(this._model, this.Site))
            {
                this.ToolBarTray.ToolBars.Add(bar);
            }

            foreach (CommandBinding cb in AddInTree.Tree.GetTreeNode("fantasy/studio/businessengine/propertyeditor/commandbindings").BuildChildItems(this._model, this.Site))
            {
                this.CommandBindings.Add(cb);
            }

            
            this.DataContext = this._model;
            foreach (BusinessProperty prop in this._model.Class.Properties)
            {
                this.HandlePropertyEvents(prop);
            }

            
            Properties.Settings.Default.PropertyEditorGridViewLayout.LoadLayout(this.PropertyGridView);

        }

        void PropertiesChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            this.DirtyState = EditingState.Dirty;
        }

        private void HandlePropertyEvents(BusinessProperty prop)
        {
            prop.EntityStateChanged += new EventHandler(PropertyStateChanged); 
        }

        void PropertyStateChanged(object sender, EventArgs e)
        {
            this.DirtyState =  EditingState.Dirty;
            BusinessProperty prop = (BusinessProperty)sender;
            if(prop.EntityState == EntityState.Deleted)
            {
                RemovePropertyEvents(prop);
            }
        }

        private void RemovePropertyEvents(BusinessProperty prop)
        {
            prop.EntityStateChanged -= new EventHandler(PropertyStateChanged); 
        }


        private EditingState _dirtyState = EditingState.Clean;
        public EditingState DirtyState
        {
            get { return _dirtyState; }
            private set
            {
                if (this._dirtyState != value)
                {
                    this._dirtyState = value;
                    this.OnDirtyStateChanged(EventArgs.Empty);
                }
            }
        }


        protected virtual void OnDirtyStateChanged(EventArgs e)
        {
            if (this.DirtyStateChanged != null)
            {
                this.DirtyStateChanged(this, e);
            }
        }


        public event EventHandler DirtyStateChanged;

        public void Save()
        {
            ISession session = this.Site.GetRequiredService<IEntityService>().DefaultSession;

            session.BeginUpdate();
            try
            {

                foreach (BusinessProperty prop in this._model.Class.Properties)
                {
                    session.SaveOrUpdate(prop);
                }
                session.EndUpdate(true);
            }
            catch
            {
                session.EndUpdate(false);
                throw;
            }
            this.DirtyState = EditingState.Clean;
            
        }

        UIElement IEntityEditingPanel.Content
        {
            get { return this; }
        }

        public string Title
        {
            get { return Properties.Resources.BusinessPropertyEditorTitle; }
        }

        #endregion

        #region IEntityEditingPanel Members


        private GridViewLayoutSetting _propertyGridLayout = new GridViewLayoutSetting();

        public void Closing(System.ComponentModel.CancelEventArgs e)
        {
            this._propertyGridLayout.SaveLayout(this.PropertyGridView);
        }

        public void Closed()
        {
            this._model.Class.Properties.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.PropertiesChanged);
            foreach (BusinessProperty prop in this._model.Class.Properties)
            {
                this.RemovePropertyEvents(prop);
            }
            Properties.Settings.Default.PropertyEditorGridViewLayout = this._propertyGridLayout;
        }

        #endregion

        private void propertyListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this._selectionService.SetSelectedComponents(this.propertyListView.SelectedItems);
            foreach (BusinessProperty property in e.RemovedItems)
            {
                this._model.Selected.Remove(property);
            }
            foreach (BusinessProperty property in e.AddedItems)
            {
                this._model.Selected.Add(property);
            }
           
        }
    }
}
