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

        private SelectionService _selectionService = new SelectionService(null);

        protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            if (this.Site != null)
            {
                IMonitorSelectionService monitor = this.Site.GetRequiredService<IMonitorSelectionService>();
                monitor.CurrentSelectionService = _selectionService;
            }
            base.OnGotKeyboardFocus(e);
        }

        public BusinessClass Entity { get; private set; }

        #region IEntityEditingPanel Members

        public void Initialize()
        {
            foreach (ToolBar bar in AddInTree.Tree.GetTreeNode("fantasy/studio/businessengine/classeditor/propertyeditor/toolbars").BuildChildItems(this, this.Site))
            {
                this.ToolBarTray.ToolBars.Add(bar);
            }

            foreach (CommandBinding cb in AddInTree.Tree.GetTreeNode("fantasy/studio/businessengine/propertyeditor/commandbindings").BuildChildItems(this, this.Site))
            {
                this.CommandBindings.Add(cb);
            }
        }

        public void Load(IBusinessEntity entity)
        {

            this.Entity = (BusinessClass)entity; 
            this.DataContext = entity;
            foreach (BusinessProperty prop in this.Entity.Properties)
            {
                this.HandlePropertyEvents(prop);
            }

            
            Properties.Settings.Default.PropertyEditorGridViewLayout.LoadLayout(this.PropertyGridView);

        }

        private void HandlePropertyEvents(BusinessProperty prop)
        {
            
        }

        public EditingState DirtyState
        {
            get { return EditingState.Clean; }
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
            Properties.Settings.Default.PropertyEditorGridViewLayout = this._propertyGridLayout;
        }

        #endregion
    }
}
