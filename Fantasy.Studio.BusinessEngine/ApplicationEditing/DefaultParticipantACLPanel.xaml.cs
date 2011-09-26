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
using Fantasy.Windows;
using System.Collections.Specialized;
using Fantasy.BusinessEngine.Services;
using Fantasy.Studio.Services;
using Fantasy.Studio.BusinessEngine.UserRoleEditing;

namespace Fantasy.Studio.BusinessEngine.ApplicationEditing
{
    /// <summary>
    /// Interaction logic for DefaultParticipantACLPanel.xaml
    /// </summary>
    public partial class DefaultParticipantACLPanel : UserControl, IObjectWithSite, IDocumentEditingPanel
    {
        public DefaultParticipantACLPanel()
        {
            InitializeComponent();
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

            if (this.Entity != null)
            {
                foreach (BusinessApplicationACL acl in this.Entity.ACLs)
                {
                    acl.Security.Sync(this.Entity.Class, null);
                   
                }
            }
        }

        #region IDocumentEditingPanel Members

        public void Initialize()
        {
            
        }


        public BusinessApplicationParticipant Entity { get; private set; }

        public void Load(object document)
        {
            this.Entity = ((ParticipantACL)document).Entity;


            EditingState editingState = this.Entity.EntityState == EntityState.Clean ? EditingState.Clean : EditingState.Dirty; 

            this._aclChangedListener = new WeakEventListener(ACLStateChanged);
            this._aclCollectionChangedListener = new WeakEventListener(ACLCollectionChanged); 

            foreach (BusinessApplicationACL acl in this.Entity.ACLs)
            {
                if (acl.EntityState != EntityState.Clean)
                {
                    editingState = EditingState.Dirty;
                }
                EntityStateChangedEventManager.AddListener(acl, this._aclChangedListener);
            }

            CollectionChangedEventManager.AddListener(this.Entity.ACLs, this._aclCollectionChangedListener);

            DefaultParticipantACLModel model = new DefaultParticipantACLModel(this.Entity);
            this.DataContext = model;
           

        }



        private WeakEventListener _aclChangedListener;
        private WeakEventListener _aclCollectionChangedListener;

        private bool ACLStateChanged(Type type, object sender, EventArgs e)
        {
            if (((BusinessApplicationACL)sender).EntityState != EntityState.Clean)
            {
                this.DirtyState = EditingState.Dirty;
            }
            return true;
        }

        private bool ACLCollectionChanged(Type type, object sender, EventArgs e)
        {
            NotifyCollectionChangedEventArgs args = (NotifyCollectionChangedEventArgs)e;
            if (args.Action != NotifyCollectionChangedAction.Move)
            {
                if (args.OldItems != null)
                {
                    foreach (BusinessApplicationACL acl in args.OldItems)
                    {
                        
                        EntityStateChangedEventManager.RemoveListener(acl, this._aclChangedListener);
                    }
                }

                if (args.NewItems != null)
                {
                    foreach (BusinessApplicationACL acl in args.NewItems)
                    {

                        EntityStateChangedEventManager.AddListener(acl, this._aclChangedListener);
                    }
                }
            }
            this.DirtyState = EditingState.Dirty;
            return true;
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
            IEntityService es = this.Site.GetRequiredService<IEntityService>();

            es.BeginUpdate();
            try
            {

                foreach (BusinessApplicationACL acl in this.Entity.ACLs)
                {
                    acl.Security.Sync(this.Entity.Class, null);
                    es.SaveOrUpdate(acl);
                }
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
            get { return Properties.Resources.DefaultParticipantACLPanelTitle; }
        }

        public void Closing(System.ComponentModel.CancelEventArgs e)
        {
           
        }

        public void Closed()
        {
           
        }

        public void ViewContentSelected()
        {
            
        }

        public void ViewContentDeselected()
        {
           
        }

        #endregion

        private void ACLListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this._selection.IsReadOnly = true;
            DefaultParticipantACLModel model = this.DataContext as DefaultParticipantACLModel;
            if (model != null)
            {
                this._selection.SetSelectedComponents(new object[] { model.SelectedItem.Role });
            }
        }

       
       

        private void AddRoleButton_Click(object sender, RoutedEventArgs e)
        {

            DefaultParticipantACLModel model = this.DataContext as DefaultParticipantACLModel;

            BusinessRolePikcerModel rm = new BusinessRolePikcerModel(this.Site, false);
            rm.SelectedItemChanging += (s, arg) =>
            {
                arg.Cancel = Entity.ACLs.Any(a => a.Role == arg.Role);

            };
            BusinessRolePicker rolePicker = new BusinessRolePicker() { DataContext = rm };

            if ((bool)rolePicker.ShowDialog())
            {
                BusinessRole role = rm.SelectedItem;
                IEntityService es = this.Site.GetRequiredService<IEntityService>();
                BusinessApplicationACL acl = es.AddBusinessApplicationACL(this.Entity, role, null);
                BusinessObjectSecurity sec = BusinessObjectSecurity.Create(this.Entity.Class, BusinessObjectAccess.None, BusinessObjectAccess.None);
                acl.Security = sec;
                model.SelectedItem = acl;
            }
        }

        private void RemoveRoleButton_Click(object sender, RoutedEventArgs e)
        {
            DefaultParticipantACLModel model = this.DataContext as DefaultParticipantACLModel;
            this.Entity.ACLs.Remove(model.SelectedItem);
        }

       
    }
}
