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
using Fantasy.Studio.Services;
using Fantasy.BusinessEngine;
using Fantasy.BusinessEngine.Services;

namespace Fantasy.Studio.BusinessEngine.UserRoleEditing
{
    /// <summary>
    /// Interaction logic for BusinessRolePanel.xaml
    /// </summary>
    public partial class BusinessRolePanel : UserControl, IEntityEditingPanel, IObjectWithSite
    {
        public BusinessRolePanel()
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


        public BusinessRole Entity { get; private set; }

        public void Load(Fantasy.BusinessEngine.IBusinessEntity entity)
        {
            this.Entity = (BusinessRole)entity;
            this.Entity.EntityStateChanged += new EventHandler(EntityStateChanged);
            this.DataContext = this.Entity;
            this.DirtyState = entity.EntityState == EntityState.Clean ? EditingState.Clean : EditingState.Dirty;

            this._selection.SetSelectedComponents(new object[] { this.Entity });
            this._selection.IsReadOnly = true;

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
                es.SaveOrUpdate(this.Entity);
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
            get { return Properties.Resources.BusinessRolePanelTitle; }
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



        private void EntityStateChanged(object sender, EventArgs e)
        {
            if (this.Entity.EntityState != EntityState.Clean)
            {
                this.DirtyState = EditingState.Dirty;
            }
        }

       

    }
}
