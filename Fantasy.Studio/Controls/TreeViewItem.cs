using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Fantasy.Collections;
using System.Collections.ObjectModel;
using Fantasy.AddIns;
using System.Windows;
using System.Collections.Specialized;



namespace Fantasy.Studio.Controls
{
    public class TreeViewItem : Fantasy.Studio.Controls.ITreeViewItem
    {
        public TreeViewItem(ExtendableTreeViewModel model)
        {
            this._model = model;
            this.Items = new UnionedObservableCollection();
            this.TreeViewItems = new ObservableAdapterCollection<TreeViewItem>(this.Items, this._model.CreateTreeViewItem);
        }

        private ExtendableTreeViewModel _model;

        public object DataContext { get; set; }

        public IValueProvider Text { get; set; }

        public IValueProvider Icon { get; set; }

        public ContextMenu ContextMenu { get; set; }

        public ICommand DoubleClick  { get; set; }

        public ICommand Selected { get; set; }

        public ICommand Unselected { get; set; }

        public ICommand Expanded { get; set; }

        public ICommand Collapsed { get; set; }

        public UnionedObservableCollection Items { get; private set; }

        public IEnumerable<TreeViewItem> TreeViewItems { get; private set; }

        public IEventHandler<DoDragDropEventArgs> DoDragDrop { get; set; }
    }
}
