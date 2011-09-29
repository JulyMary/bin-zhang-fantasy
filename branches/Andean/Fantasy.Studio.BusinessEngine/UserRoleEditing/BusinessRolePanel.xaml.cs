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
using Fantasy.Windows;
using System.Collections.Specialized;
using Fantasy.AddIns;
using Fantasy.Utils;

namespace Fantasy.Studio.BusinessEngine.UserRoleEditing
{
    /// <summary>
    /// Interaction logic for BusinessRolePanel.xaml
    /// </summary>
    public partial class BusinessRolePanel : UserControl, IDocumentEditingPanel, IObjectWithSite
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

        private WeakEventListener _rolesChangedListener;

        public void Load(object entity)
        {
            this.Entity = (BusinessRole)entity;
            this.Entity.EntityStateChanged += new EventHandler(EntityStateChanged);
            this.DataContext = this.Entity;
            this.DirtyState = this.Entity.EntityState == EntityState.Clean ? EditingState.Clean : EditingState.Dirty;

            this._selection.SetSelectedComponents(new object[] { this.Entity });
            this._selection.IsReadOnly = true;

            this._rolesChangedListener = new WeakEventListener((t, sender, e) =>
            {
                this.DirtyState = EditingState.Dirty;
                return true;
            });

            CollectionChangedEventManager.AddListener(this.Entity.Users, this._rolesChangedListener);

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

        private void AddUserButton_Click(object sender, RoutedEventArgs e)
        {
            BusinessUserPikcerModel model = new BusinessUserPikcerModel(this.Site);
            BusinessUserPicker picker = new BusinessUserPicker() { DataContext = model };
            if ((bool)picker.ShowDialog())
            {
                BusinessUser user = model.SelectedItem;
                if (this.Entity.Users.IndexOf(user) < 0)
                {
                    this.Entity.Users.Add(user);
                    user.Roles.Add(this.Entity);
                }
            }
        }

        private void RemoveUserButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (BusinessUser user in this.MembersListView.SelectedItems.Cast<BusinessUser>().ToArray())
            {
                user.Roles.Remove(this.Entity);
                this.Entity.Users.Remove(user);

            }
        }

        private void MembersListView_DragEnter(object sender, DragEventArgs e)
        {
            IEnumerable<IEventHandler<DragEventArgs>> handlers = AddInTree.Tree.GetTreeNode("fantasy/studio/businessengine/businessrolepanel/members/dragdrop/enterhandlers").BuildChildItems<IEventHandler<DragEventArgs>>(this, this.GetChildSite());
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

        private void MembersListView_DragLeave(object sender, DragEventArgs e)
        {
            IEnumerable<IEventHandler<DragEventArgs>> handlers = AddInTree.Tree.GetTreeNode("fantasy/studio/businessengine/businessrolepanel/members/dragdrop/leavehandlers").BuildChildItems<IEventHandler<DragEventArgs>>(this, this.GetChildSite());
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

        private void MembersListView_DragOver(object sender, DragEventArgs e)
        {
            IEnumerable<IEventHandler<DragEventArgs>> handlers = AddInTree.Tree.GetTreeNode("fantasy/studio/businessengine/businessrolepanel/members/dragdrop/overhandlers").BuildChildItems<IEventHandler<DragEventArgs>>(this, this.GetChildSite());
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

        private void MembersListView_Drop(object sender, DragEventArgs e)
        {
            IEnumerable<IEventHandler<DragEventArgs>> handlers = AddInTree.Tree.GetTreeNode("fantasy/studio/businessengine/businessrolepanel/members/dragdrop/drophandlers").BuildChildItems<IEventHandler<DragEventArgs>>(this, this.GetChildSite());
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

        private void NameTextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            ((TextBox)sender).SelectAll();
        }

        private void NameTextBox_GotMouseCapture(object sender, MouseEventArgs e)
        {
            ((TextBox)sender).SelectAll();
        }

        private void NameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string oname = this.Entity.Name;
            this.Entity.Name = ((TextBox)sender).Text;
           

            string ocname = UniqueNameGenerator.GetCodeName(oname) + Properties.Resources.DefaultUserRoleCodeNameSuffix;
            if (ocname == this.Entity.CodeName)
            {
                this.Entity.CodeName = UniqueNameGenerator.GetCodeName(this.Entity.Name) + Properties.Resources.DefaultUserRoleCodeNameSuffix;
            }
        }

       

    }
}
