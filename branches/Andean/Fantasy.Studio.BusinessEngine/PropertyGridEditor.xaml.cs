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
using System.ComponentModel;
using Fantasy.Adaption;
using Fantasy.ServiceModel;
using NHibernate;
using Fantasy.Studio.Services;

namespace Fantasy.Studio.BusinessEngine
{
    /// <summary>
    /// Interaction logic for PropertyGridEditor.xaml
    /// </summary>
    public partial class PropertyGridEditor : UserControl, IEntityEditingPanel
    {
        public PropertyGridEditor()
        {
            InitializeComponent();
            this.Title = Properties.Resources.PropertyGridEditorDefaultTitle;
        }


        private SelectionService _selection = new SelectionService(null);
       
        #region IEntityEditingPanel Members

        protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            base.OnGotKeyboardFocus(e);
            IMonitorSelectionService monitor = ServiceManager.Services.GetRequiredService<IMonitorSelectionService>();
            monitor.CurrentSelectionService = this._selection;
        }


        private IBusinessEntity _entity;

        public void Load(Fantasy.BusinessEngine.IBusinessEntity data)
        {
            this._entity = data;
            this._entity.EntityStateChanged += new EventHandler(EntityStateChanged);
            object descriptor = ServiceManager.Services.GetRequiredService<IAdapterManager>().GetAdapter(this._entity, typeof(ICustomTypeDescriptor));
            this.propertyGrid.SelectedObject = descriptor;
            this.DirtyState = this._entity.EntityState == EntityState.Clean ? EditingState.Clean : EditingState.Dirty; 
        }

        void EntityStateChanged(object sender, EventArgs e)
        {
            object descriptor = ServiceManager.Services.GetRequiredService<IAdapterManager>().GetAdapter(this._entity, typeof(ICustomTypeDescriptor));
            this.propertyGrid.SelectedObject = descriptor;
            this.DirtyState = this._entity.EntityState == EntityState.Clean ? EditingState.Clean : EditingState.Dirty; 
            CommandManager.InvalidateRequerySuggested();
        }

        private EditingState _dirtyState = EditingState.Clean;
        public EditingState DirtyState
        {
            get { return _dirtyState; }
            set
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


        public void Save()
        {
            ISession session = ServiceManager.Services.GetRequiredService<IEntityService>().DefaultSession;
            session.SaveOrUpdate(this._entity);
        }

        public new UIElement Content
        {
            get { return this; }
        }

        public string Title { get; set; }


        public void Closing(CancelEventArgs e)
        {
            
        }

        public void Closed()
        {
            
        }

        #endregion
    }
}
