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
using System.ComponentModel.Design;
using Fantasy.Studio.Services;
using Fantasy.BusinessEngine;
using Fantasy.Studio.Controls;
using Fantasy.AddIns;
using NHibernate;
using Fantasy.BusinessEngine.Services;
using System.Collections.Specialized;

namespace Fantasy.Studio.BusinessEngine.EnumEditing
{
    /// <summary>
    /// Interaction logic for EnumValuesPanel.xaml
    /// </summary>
    public partial class EnumValuesPanel : UserControl, IEntityEditingPanel, IObjectWithSite
    {
        public EnumValuesPanel()
        {
            InitializeComponent();
           
        }

        public IServiceProvider Site { get; set; }

        private ISelectionService _selectionService = new SelectionService(null);

     
        protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            if (this.Site != null)
            {
                IMonitorSelectionService monitor = this.Site.GetRequiredService<IMonitorSelectionService>();
                monitor.CurrentSelectionService = _selectionService;
            }
            base.OnGotKeyboardFocus(e);
        }


        private EnumValuesPanelModel _model;

        #region IEntityEditingPanel Members

        public void Initialize()
        {
            
        }

        public void Load(IBusinessEntity entity)
        {
            this._model = new EnumValuesPanelModel((BusinessEnum)entity);
            this._model.Enum.EnumValues.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(EnumValuesChanged);

            foreach (ToolBar bar in AddInTree.Tree.GetTreeNode("fantasy/studio/businessengine/enumeditor/valuespanel/toolbars").BuildChildItems(this._model, this.Site))
            {
                this.ToolBarTray.ToolBars.Add(bar);
            }

            foreach (CommandBinding cb in AddInTree.Tree.GetTreeNode("fantasy/studio/businessengine/enumeditor/valuespanel/commandbindings").BuildChildItems(this._model, this.Site))
            {
                this.CommandBindings.Add(cb);
            }

            
            this.DataContext = this._model;
            foreach (BusinessEnumValue value in this._model.Enum.EnumValues)
            {
                if (value.EntityState != EntityState.Clean)
                {
                    this.DirtyState = EditingState.Dirty;
                }
                this.HandleEnumEvents(value);
            }


            Properties.Settings.Default.EnumValuesPanelGridViewLayout.LoadLayout(this.PropertyGridView);

        }

        void EnumValuesChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            this.DirtyState = EditingState.Dirty;
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Move)
            {
                foreach (BusinessEnumValue value in e.OldItems)
                {
                    this.RemoveEnumValueEvents(value);
                }
            }
        }

        private void HandleEnumEvents(BusinessEnumValue value)
        {
            value.EntityStateChanged += new EventHandler(EnumValueStateChanged); 
        }

        void EnumValueStateChanged(object sender, EventArgs e)
        {
            this.DirtyState =  EditingState.Dirty;
            BusinessEnumValue prop = (BusinessEnumValue)sender;
            if(prop.EntityState == EntityState.Deleted)
            {
                RemoveEnumValueEvents(prop);
            }
        }

        private void RemoveEnumValueEvents(BusinessEnumValue value)
        {
            value.EntityStateChanged -= new EventHandler(EnumValueStateChanged); 
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

                foreach (BusinessEnumValue value in this._model.Enum.EnumValues)
                {
                    session.SaveOrUpdate(value);
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

        public UIElement Element
        {
            get { return this; }
        }

        public string Title
        {
            get { return Properties.Resources.EnumValuesPanelTitle; }
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
            this._model.Enum.EnumValues.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.EnumValuesChanged);
            foreach (BusinessEnumValue value in this._model.Enum.EnumValues)
            {
                this.RemoveEnumValueEvents(value);
            }
            Properties.Settings.Default.EnumValuesPanelGridViewLayout = this._propertyGridLayout;
        }

        #endregion

        private void propertyListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this._selectionService.SetSelectedComponents(this.propertyListView.SelectedItems);
            foreach (BusinessEnumValue value in e.RemovedItems)
            {
                this._model.Selected.Remove(value);
            }
            foreach (BusinessEnumValue value in e.AddedItems)
            {
                this._model.Selected.Add(value);
            }
        }

       

        public void ViewContentSelected()
        {
           
        }

        public void ViewContentDeselected()
        {
           
        }


       
    }
}
