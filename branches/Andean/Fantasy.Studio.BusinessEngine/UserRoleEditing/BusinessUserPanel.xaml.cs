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
using Fantasy.Studio.Services;
using Fantasy.BusinessEngine.Services;

namespace Fantasy.Studio.BusinessEngine.UserRoleEditing
{
    /// <summary>
    /// Interaction logic for BusinessUserPanel.xaml
    /// </summary>
    public partial class BusinessUserPanel : UserControl, IEntityEditingPanel
    {
        public BusinessUserPanel()
        {
            InitializeComponent();
        }


        public void Initialize()
        {

        }


        public IServiceProvider Site { get; set; }


        private SelectionService _selection = new SelectionService(null);

      

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

        public BusinessUser Entity { get; private set; }

        public void Load(Fantasy.BusinessEngine.IBusinessEntity entity)
        {
            this.Entity = (BusinessUser)entity;
            this.Entity.EntityStateChanged += new EventHandler(EntityStateChanged);
            this.DirtyState = entity.EntityState == EntityState.Clean ? EditingState.Clean : EditingState.Dirty;

        }

       

        private EditingState _dirtyState;
        public EditingState DirtyState
        {
            get
            {
                return _dirtyState;
            }
            set
            {
                if (this._dirtyState != value)
                {
                    this._dirtyState = value;
                    if (this.DirtyStateChanged != null)
                    {
                        this.DirtyStateChanged(this, EventArgs.Empty);
                    }
                }
            }
        }

        public event EventHandler DirtyStateChanged;

        public void Save()
        {
            IEntityService es = this.Site.GetRequiredService<IEntityService>();

            es.BeginUpdate();
            try
            {
                if (this.IsPasswordChanged)
                {
                    this.Entity.SetPassword(this.Password);
                }
                es.SaveOrUpdate(this._entity);
                es.EndUpdate(true);
            }
            catch
            {
                es.EndUpdate(false);
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
            get { return Properties.Resources.BusinessUserPanelTitle; }
        }

        public void Closing(System.ComponentModel.CancelEventArgs e)
        {
           
        }

        public void Closed()
        {
            this.Entity.EntityStateChanged -= new EventHandler(EntityStateChanged);
        }

        public void ViewContentSelected()
        {
            
        }

        public void ViewContentDeselected()
        {
           
        }

       

        public EventHandler EntityStateChanged { get; set; }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {

        }

       
    }
}
