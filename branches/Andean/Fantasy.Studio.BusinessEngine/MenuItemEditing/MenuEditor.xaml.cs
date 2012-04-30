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
using Microsoft.Win32;
using Fantasy.Studio.Services;
using Fantasy.Studio.BusinessEngine.UserRoleEditing;
using System.ComponentModel.Design;
using Fantasy.Windows;

namespace Fantasy.Studio.BusinessEngine.MenuItemEditing
{
    /// <summary>
    /// Interaction logic for MenuEditor.xaml
    /// </summary>
    public partial class MenuEditor : UserControl, IViewContent, IEditingViewContent, IObjectWithSite
    {
        public MenuEditor()
        {
            InitializeComponent();
        }

        public IServiceProvider Site { get; set; }

        #region IEditingViewContent Members

        private ISelectionService _selectionService = new SelectionService(null);



        protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            if (this.Site != null)
            {
                IMonitorSelectionService monitor = this.Site.GetRequiredService<IMonitorSelectionService>();
                monitor.CurrentSelectionService = _selectionService;
            }
            if (this._model != null)
            {
                this._model.Refresh();
            }
            base.OnGotKeyboardFocus(e);
        }

        public void Load(object data)
        {
            
            this.Data = data;
            _model = new MenuEditorModel(this.Site);
            this.DataContext = _model;
            _model.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(Model_PropertyChanged);
           
            
        }

        void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "EditingState":
                    this.OnDirtyStateChanged(EventArgs.Empty);
                    break;
                case "SelectedItem":
                    this._selectionService.SetSelectedComponents(new object[] { this._model.SelectedItem.Entity });
                    break;
            }
           
        }

        private MenuEditorModel _model;

        public object Data { get; private set; }
       

        public EditingState DirtyState
        {
            get { return this._model != null ? _model.EditingState : EditingState.Clean; }
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
            this._model.Save();
        }

        #endregion

        #region IViewContent Members

        public UIElement Element
        {
            get { return this; }
        }

        public IWorkbenchWindow WorkbenchWindow {get;set;}
       

        public string Title
        {
            get { return Properties.Resources.MenuEditorName; }
        }

        public void Selected()
        {
           
        }

        public void Deselected()
        {
            
        }

        public void Closing(System.ComponentModel.CancelEventArgs e)
        {
            
        }

        public void Closed()
        {
            
        }

        event EventHandler IViewContent.TitleChanged {add{} remove{}}

        public string DocumentName
        {
            get { return Properties.Resources.MenuEditorName; }
        }

        public string DocumentType
        {
            get { return "menueditor"; }
        }

        private ImageSource _icon;
        public ImageSource Icon
        {
            get
            {
                if (_icon == null)
                {
                    _icon = new BitmapImage(new Uri("/Fantasy.Studio.BusinessEngine;component/images/menuedit.png", UriKind.Relative));
                }
                return _icon;
            }
        }

        event EventHandler IViewContent.IconChanged { add { } remove { } }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            
        }

        #endregion

        private void MemberOfListView_DragEnter(object sender, DragEventArgs e)
        {
            BusinessRoleData data = e.Data.GetDataByType<BusinessRoleData>();
            if (data != null && !data.IsComputed)
            {
                e.Effects = DragDropEffects.Copy;
                e.Handled = true;
            }
        }

        private void MemberOfListView_DragLeave(object sender, DragEventArgs e)
        {

        }

        private void MemberOfListView_DragOver(object sender, DragEventArgs e)
        {
            BusinessRoleData data = e.Data.GetDataByType<BusinessRoleData>();
            if (data != null )
            {
                e.Effects = DragDropEffects.Copy;
                e.Handled = true;
            }
        }

        private void MemberOfListView_Drop(object sender, DragEventArgs e)
        {
            BusinessRoleData role = e.Data.GetDataByType<BusinessRoleData>();
            if (role != null)
            {
                e.Effects = DragDropEffects.Copy;
                e.Handled = true;

                if (this._model.SelectedItem.Entity.Roles.IndexOf(role.Id) < 0)
                {

                    this._model.SelectedItem.Roles.Add(role);
                    this._model.SelectedItem.Entity.Roles.Add(role.Id);
                }
            }
        }

        private void AddRoleButton_Click(object sender, RoutedEventArgs e)
        {
            BusinessRolePikcerModel model = new BusinessRolePikcerModel(this.Site, false);
            BusinessRolePicker picker = new BusinessRolePicker() { DataContext = model };
            if ((bool)picker.ShowDialog())
            {
                BusinessRoleData role = model.SelectedItem;
                if (this._model.SelectedItem.Roles.IndexOf(role) < 0)
                {
                    this._model.SelectedItem.Roles.Add(role);
                    this._model.SelectedItem.Entity.Roles.Add(role.Id);
                }
            }
        }

        private void RemoveRoleButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (BusinessRoleData role in this.MemberOfListView.SelectedItems.Cast<BusinessRoleData>().ToArray())
            {
                this._model.SelectedItem.Roles.Remove(role);
                this._model.SelectedItem.Entity.Roles.Remove(role.Id);
            }
        }

        private void AutoSelectAllByMouse(object sender, MouseEventArgs e)
        {
            ((TextBox)sender).SelectAll();
        }

        private void AutoSelectAll(object sender, KeyboardFocusChangedEventArgs e)
        {
            ((TextBox)sender).SelectAll();
        }

        private void AddMenuItem(object sender, ExecutedRoutedEventArgs e)
        {
            BusinessMenuItem item = this.MenuTreeView.SelectedItem as BusinessMenuItem;
            if (item != null)
            {
                TreeViewItem node = (TreeViewItem)GetTreeViewItem(this.MenuTreeView ,item);
                if (node != null)
                {
                    node.IsExpanded = true;
                }

                _model.Add(item);
            }



        }

        private TreeViewItem GetTreeViewItem(ItemsControl container, object item)
        {
            if (container != null)
            {
                if (container.DataContext == item)
                {
                    return container as TreeViewItem;
                }

                // Expand the current container
                if (container is TreeViewItem && !((TreeViewItem)container).IsExpanded)
                {
                    container.SetValue(TreeViewItem.IsExpandedProperty, true);
                }

                // Try to generate the ItemsPresenter and the ItemsPanel.
                // by calling ApplyTemplate.  Note that in the 
                // virtualizing case even if the item is marked 
                // expanded we still need to do this step in order to 
                // regenerate the visuals because they may have been virtualized away.

                container.ApplyTemplate();
                ItemsPresenter itemsPresenter =
                    (ItemsPresenter)container.Template.FindName("ItemsHost", container);
                if (itemsPresenter != null)
                {
                    itemsPresenter.ApplyTemplate();
                }
                else
                {
                    // The Tree template has not named the ItemsPresenter, 
                    // so walk the descendents and find the child.
                    itemsPresenter = FindVisualChild<ItemsPresenter>(container);
                    if (itemsPresenter == null)
                    {
                        container.UpdateLayout();

                        itemsPresenter = FindVisualChild<ItemsPresenter>(container);
                    }
                }

                Panel itemsHostPanel = (Panel)VisualTreeHelper.GetChild(itemsPresenter, 0);


                // Ensure that the generator for this panel has been created.
                UIElementCollection children = itemsHostPanel.Children;

                //MyVirtualizingStackPanel virtualizingPanel =
                //    itemsHostPanel as MyVirtualizingStackPanel;

                for (int i = 0, count = container.Items.Count; i < count; i++)
                {
                    TreeViewItem subContainer;
                    
                    subContainer =
                        (TreeViewItem)container.ItemContainerGenerator.
                        ContainerFromIndex(i);

                    // Bring the item into view to maintain the 
                    // same behavior as with a virtualizing panel.
                    subContainer.BringIntoView();
                   

                    if (subContainer != null)
                    {
                        // Search the next level for the object.
                        TreeViewItem resultContainer = GetTreeViewItem(subContainer, item);
                        if (resultContainer != null)
                        {
                            return resultContainer;
                        }
                        else
                        {
                            // The object is not under this TreeViewItem
                            // so collapse it.
                            subContainer.IsExpanded = false;
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Search for an element of a certain type in the visual tree.
        /// </summary>
        /// <typeparam name="T">The type of element to find.</typeparam>
        /// <param name="visual">The parent element.</param>
        /// <returns></returns>
        private T FindVisualChild<T>(Visual visual) where T : Visual
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(visual); i++)
            {
                Visual child = (Visual)VisualTreeHelper.GetChild(visual, i);
                if (child != null)
                {
                    T correctlyTyped = child as T;
                    if (correctlyTyped != null)
                    {
                        return correctlyTyped;
                    }

                    T descendent = FindVisualChild<T>(child);
                    if (descendent != null)
                    {
                        return descendent;
                    }
                }
            }

            return null;
        }


        private void CanRemoveMenuItem(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this._model.SelectedItem != null && ! this._model.SelectedItem.Entity.IsSystem;
        }

        private void RemoveMenuItem(object sender, ExecutedRoutedEventArgs e)
        {
            _model.Remove(this._model.SelectedItem.Entity);
        }

        private void MenuTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            BusinessMenuItem item = this.MenuTreeView.SelectedItem as BusinessMenuItem;
            if (item != null)
            {
                this._model.SetSelectedMenuItem(item);
            }
           
        }

        private void CanAddMenuItem(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this._model.SelectedItem != null;
        }

        private void BrowseImageButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog()
            {
                DefaultExt = ".png",
                Filter = Properties.Resources.OpenFileDialogImageFilter,
                Title = "Select Icon",
                Multiselect = false

            };

            if (dlg.ShowDialog() == true)
            {
                try
                {
                     System.Drawing.Image image = System.Drawing.Image.FromFile(dlg.FileName);
                     this._model.SelectedItem.Entity.Icon = image;
                }
                catch
                {
                     IMessageBoxService msgBox = this.Site.GetRequiredService<IMessageBoxService>();

                     msgBox.Show(string.Format(Properties.Resources.InvalidImageFileMessage, dlg.FileName), icon: MessageBoxImage.Error);
                }
               
            }
        }

        private void CleanupImageButton_Click(object sender, RoutedEventArgs e)
        {
            this._model.SelectedItem.Entity.Icon = null;
        }
    }
}
