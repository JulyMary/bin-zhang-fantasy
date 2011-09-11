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
using Fantasy.Windows;

namespace Fantasy.Studio.Controls
{
    /// <summary>
    /// Interaction logic for ExpandableTreeView.xaml
    /// </summary>
    public partial class ExtendableTreeView : TreeView
    {
        public ExtendableTreeView()
        {
            InitializeComponent();
        }


        private void treeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TreeViewItem old = e.OldValue as TreeViewItem;
            if (old != null && old.Unselected != null && old.Unselected.CanExecute(old.DataContext))
            {
                old.Unselected.Execute(old.DataContext);
            }

            TreeViewItem @new = e.NewValue as TreeViewItem;

            if (@new != null && @new.Selected != null && @new.Selected.CanExecute(@new.DataContext))
            {
                @new.Selected.Execute(@new.DataContext);
            }
        }



        private void TreeViewItemDoubleClick(object sender, MouseEventArgs e)
        {

            System.Windows.Controls.TreeViewItem item = (System.Windows.Controls.TreeViewItem)e.Source;

            System.Windows.Controls.TreeViewItem origin = FindTreeViewItem((DependencyObject)e.OriginalSource);
            if (origin != null && item == origin)
            {
                TreeViewItem data = (TreeViewItem)item.DataContext;
                if(data.DoubleClick != null && data.DoubleClick.CanExecute(data.DataContext))
                {
                    data.DoubleClick.Execute(data.DataContext);
                }

            }

        }

        private System.Windows.Controls.TreeViewItem FindTreeViewItem(DependencyObject source)
        {
            if (source == null)
            {
                return null;
            }
            if (source is System.Windows.Controls.TreeViewItem)
            {
                return (System.Windows.Controls.TreeViewItem)source;
            }

            return FindTreeViewItem(VisualTreeHelper.GetParent(source));
        }

       

        private bool _startDraging = false;
        private void TreeView_PreviewMouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Controls.TreeViewItem container = (System.Windows.Controls.TreeViewItem)this.GetContainerAtPoint<System.Windows.Controls.TreeViewItem>(e.GetPosition(this));

            if(container != null)
            {
               
                TreeViewItem data = (TreeViewItem)container.DataContext;
                if(data.DoDragDrop != null)
                {
                    _startDraging = true;
                    container.IsSelected = true;


                }
            }
        }

        private void TreeView_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (_startDraging && e.LeftButton == MouseButtonState.Pressed)
                {

                    TreeViewItem itemModel = (TreeViewItem)this.SelectedItem;
                    DoDragDropEventArgs args = new DoDragDropEventArgs(itemModel.DataContext);
                   
                    itemModel.DoDragDrop.HandleEvent(this, args);
                    if (args.AllowedEffects != DragDropEffects.None && args.Data != null)
                    {
                        
                        DragDrop.DoDragDrop(this, args.Data, args.AllowedEffects);
                    }
                }

            }
            finally
            {
                _startDraging = false;
            }
        }

        private void TreeView_PreviewMouseButtonUp(object sender, MouseButtonEventArgs e)
        {
            _startDraging = false;
        }
    }
}
