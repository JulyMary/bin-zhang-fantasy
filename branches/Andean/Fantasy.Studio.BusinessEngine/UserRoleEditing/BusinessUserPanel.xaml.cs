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
using Fantasy.AddIns;
using Fantasy.Windows;
using System.Collections.Specialized;
using Fantasy.Utils;

namespace Fantasy.Studio.BusinessEngine.UserRoleEditing
{
    /// <summary>
    /// Interaction logic for BusinessUserPanel.xaml
    /// </summary>
    public partial class BusinessUserPanel : UserControl, IDocumentEditingPanel, IObjectWithSite
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

      
        public BusinessUser Entity { get; private set; }

        public void Load(object entity)
        {
            this.Entity = (BusinessUser)entity;
            this.Entity.EntityStateChanged += new EventHandler(EntityStateChanged);
            this.DataContext = this.Entity;



            this.DirtyState = this.Entity.EntityState == EntityState.Clean ? EditingState.Clean : EditingState.Dirty;
            this._selection.SetSelectedComponents(new object[] {this.Entity});
            this._selection.IsReadOnly = true;

            this._rolesChangedListener = new WeakEventListener((t, sender, e) => 
            {
                this.DirtyState = EditingState.Dirty;
                return true;
            });

            CollectionChangedEventManager.AddListener(this.Entity.Roles, this._rolesChangedListener);
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

        public bool IsPasswordChanged
        {
            get { return (bool)GetValue(IsPasswordChangedProperty); }
            set { SetValue(IsPasswordChangedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsPasswordChanged.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsPasswordChangedProperty =
            DependencyProperty.Register("IsPasswordChanged", typeof(bool), typeof(BusinessUserPanel), new UIPropertyMetadata(false));

        public event EventHandler DirtyStateChanged;


        private WeakEventListener _rolesChangedListener;

        public void Save()
        {
            IEntityService es = this.Site.GetRequiredService<IEntityService>();

            es.BeginUpdate();
            try
            {
                bool canSave = true;
                if (this.IsPasswordChanged)
                {
                    if (this.PasswordBox.Password == this.ConfirmPasswordBox.Password)
                    {
                        this.Entity.SetPassword(this.PasswordBox.Password);
                    }
                    else
                    {
                        canSave = false;
                    }
                }

                if (canSave && this.Entity.EntityState != EntityState.Clean)
                {
                    es.SaveOrUpdate(this.Entity);
                }
                
                es.EndUpdate(true);
            }
            catch
            {
                es.EndUpdate(false);
                throw;
            }
            this.PasswordBox.Password = this.ConfirmPasswordBox.Password = string.Empty;
            this.IsPasswordChanged = false;
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
        private void EntityStateChanged(object sender, EventArgs e)
        {
            if (this.Entity.EntityState != EntityState.Clean)
            {
                this.DirtyState = EditingState.Dirty;
            }
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            this.IsPasswordChanged = true;
            this.DirtyState = EditingState.Dirty; 
        }

      
        private void AddRoleButton_Click(object sender, RoutedEventArgs e)
        {
            BusinessRolePikcerModel model = new BusinessRolePikcerModel(this.Site, true);
            BusinessRolePicker picker = new BusinessRolePicker() { DataContext = model };
            if ((bool)picker.ShowDialog())
            {
                BusinessRole role = model.SelectedItem;
                if (this.Entity.Roles.IndexOf(role) < 0)
                {
                    this.Entity.Roles.Add(role);
                    role.Users.Add(this.Entity);
                    
                }
            }
        }

        private void RemoveRoleButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (BusinessRole role in this.MemberOfListView.SelectedItems.Cast<BusinessRole>().ToArray())
            {
                role.Users.Remove(this.Entity);
                this.Entity.Roles.Remove(role);

            }
        }

        private void MemberOfListView_DragEnter(object sender, DragEventArgs e)
        {
            IEnumerable<IEventHandler<DragEventArgs>> handlers = AddInTree.Tree.GetTreeNode("fantasy/studio/businessengine/businessuserpanel/memberof/dragdrop/enterhandlers").BuildChildItems<IEventHandler<DragEventArgs>>(this, this.GetChildSite());
            foreach (IEventHandler<DragEventArgs> handler in handlers)
            {
                handler.HandleEvent(this, e);
                if (e.Handled)
                {
                    break;
                }
            }

            if (!e.Handled)
            {
                e.Effects = DragDropEffects.None;
                e.Handled = true;
            }
        }

        private void MemberOfListView_DragLeave(object sender, DragEventArgs e)
        {
            IEnumerable<IEventHandler<DragEventArgs>> handlers = AddInTree.Tree.GetTreeNode("fantasy/studio/businessengine/businessuserpanel/memberof/dragdrop/leavehandlers").BuildChildItems<IEventHandler<DragEventArgs>>(this, this.GetChildSite());
            foreach (IEventHandler<DragEventArgs> handler in handlers)
            {
                handler.HandleEvent(this, e);
                if (e.Handled)
                {
                    break;
                }
            }

            if (!e.Handled)
            {
                e.Effects = DragDropEffects.None;
                e.Handled = true;
            }
        }

        private ServiceModel.ServiceContainer _childSite;

        private IServiceProvider GetChildSite()
        {
            if (_childSite == null)
            {
                _childSite = new ServiceModel.ServiceContainer(this.Site);
                
                _childSite.AddService(this);

                _childSite.AddService(this.Entity);
            }

            return _childSite;
        }

        private void MemberOfListView_DragOver(object sender, DragEventArgs e)
        {
            IEnumerable<IEventHandler<DragEventArgs>> handlers = AddInTree.Tree.GetTreeNode("fantasy/studio/businessengine/businessuserpanel/memberof/dragdrop/overhandlers").BuildChildItems<IEventHandler<DragEventArgs>>(this, this.GetChildSite());
            foreach (IEventHandler<DragEventArgs> handler in handlers)
            {
                handler.HandleEvent(this, e);
                if (e.Handled)
                {
                    break;
                }
            }

            if (!e.Handled)
            {
                e.Effects = DragDropEffects.None;
                e.Handled = true;
            }
        }

        private void MemberOfListView_Drop(object sender, DragEventArgs e)
        {
            IEnumerable<IEventHandler<DragEventArgs>> handlers = AddInTree.Tree.GetTreeNode("fantasy/studio/businessengine/businessuserpanel/memberof/dragdrop/drophandlers").BuildChildItems<IEventHandler<DragEventArgs>>(this, this.GetChildSite());
            foreach (IEventHandler<DragEventArgs> handler in handlers)
            {
                handler.HandleEvent(this, e);
                if (e.Handled)
                {
                    break;
                }
            }

            if (!e.Handled)
            {
                e.Effects = DragDropEffects.None;
                e.Handled = true;
            }
        }

        private void AutoSelectAll(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (sender is PasswordBox)
            {
                ((PasswordBox)sender).SelectAll();
            }
            else if(sender is TextBox) 
            {
                ((TextBox)sender).SelectAll();
            }
        }

        private void AutoSelectAllByMouse(object sender, MouseEventArgs e)
        {
            if (sender is PasswordBox)
            {
                ((PasswordBox)sender).SelectAll();
            }
            else if (sender is TextBox)
            {
                ((TextBox)sender).SelectAll();
            }
        }

        private void NameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string oname = this.Entity.Name;
            this.Entity.Name = ((TextBox)sender).Text; 
            if (this.Entity.FullName == oname)
            {
                this.Entity.FullName = this.Entity.Name;
            }

            string ocname = UniqueNameGenerator.GetCodeName(oname) + Properties.Resources.DefaultUserRoleCodeNameSuffix;
            if (ocname == this.Entity.CodeName)
            {
                this.Entity.CodeName = UniqueNameGenerator.GetCodeName(this.Entity.Name) + Properties.Resources.DefaultUserRoleCodeNameSuffix;
            }
        }

       
       
    }
}
