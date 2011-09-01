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
using Fantasy.BusinessEngine.Services;

namespace Fantasy.Studio.BusinessEngine
{
    /// <summary>
    /// Interaction logic for PropertyGridEditor.xaml
    /// </summary>
    public partial class PropertyGridEditor : UserControl, IEntityEditingPanel, IObjectWithSite
    {
        public PropertyGridEditor()
        {
            InitializeComponent();
            this.Title = Properties.Resources.PropertyGridEditorDefaultTitle;
        }

        public IServiceProvider Site { get; set; }


        private SelectionService _selection = new SelectionService(null);
       
        #region IEntityEditingPanel Members

        protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            base.OnGotKeyboardFocus(e);
            if (this.Site != null)
            {
                IMonitorSelectionService monitor = this.Site.GetRequiredService<IMonitorSelectionService>();
                monitor.CurrentSelectionService = this._selection;
            }
        }


        private IBusinessEntity _entity;

        public void Load(Fantasy.BusinessEngine.IBusinessEntity data)
        {
            this._entity = data;
            this._entity.EntityStateChanged += new EventHandler(EntityStateChanged);
            this._entity.PropertyChanged += new EventHandler<Fantasy.BusinessEngine.Events.EntityPropertyChangedEventArgs>(Entity_PropertyChanged);
            object descriptor = this.Site.GetRequiredService<IAdapterManager>().GetAdapter(this._entity, typeof(ICustomTypeDescriptor));
            this.propertyGrid.SelectedObject = descriptor;
            this.DirtyState = this._entity.EntityState == EntityState.Clean ? EditingState.Clean : EditingState.Dirty; 
        }

        void Entity_PropertyChanged(object sender, Fantasy.BusinessEngine.Events.EntityPropertyChangedEventArgs e)
        {
            this.propertyGrid.Refresh();
        }

        void EntityStateChanged(object sender, EventArgs e)
        {
            object descriptor = this.Site.GetRequiredService<IAdapterManager>().GetAdapter(this._entity, typeof(ICustomTypeDescriptor));
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
            

            ISession session = this.Site.GetRequiredService<IEntityService>().DefaultSession;

            session.BeginUpdate();
            try
            {

                session.SaveOrUpdate(this._entity);
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

        public string Title { get; set; }


        public void Closing(CancelEventArgs e)
        {
            
        }

        public void Closed()
        {
            this._entity.EntityStateChanged -= new EventHandler(EntityStateChanged);
            this._entity.PropertyChanged -= new EventHandler<Fantasy.BusinessEngine.Events.EntityPropertyChangedEventArgs>(Entity_PropertyChanged);
        }

        #endregion

        #region IEntityEditingPanel Members

        public void Initialize()
        {
            
        }

        #endregion



        public void ViewContentSelected()
        {
        }

        public void ViewContentDeselected()
        {
            
        }

       
    }
}
