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
            TreeViewItem old = (TreeViewItem)e.OldValue;
            if (old != null && old.Unselected != null && old.Unselected.CanExecute(old.DataContext))
            {
                old.Unselected.Execute(old.DataContext);
            }

            TreeViewItem @new = (TreeViewItem)e.NewValue;

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
    }
}
