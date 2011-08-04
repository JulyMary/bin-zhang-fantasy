using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Fantasy.Collection;


namespace Fantasy.Studio.TreeViewModel
{
    class TreeViewItem 
    {
        public TreeViewItem()
        {
            this.ChildItems = new UnionedObservableCollection<TreeViewItem>();
        }

        public object DataContext { get; set; }

        public IValueProvider Text { get; set; }

        public IValueProvider Icon { get; set; }

        public ContextMenu ContextMenu { get; set; }

        public ICommand DoubleClick  { get; set; }

        public ICommand Selected { get; set; }

        public ICommand Unselected { get; set; }

        public ICommand Expanded { get; set; }

        public ICommand Collapsed { get; set; }

        public UnionedObservableCollection<TreeViewItem> ChildItems { get; private set; }
    }
}
