using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Fantasy.Collection;
using System.Collections.ObjectModel;
using Fantasy.AddIns;


namespace Fantasy.Studio.TreeViewModel
{
    public class TreeViewItem : IDisposable, Fantasy.Studio.TreeViewModel.ITreeViewItem 
    {
        public TreeViewItem(ExtendableTreeViewModel model)
        {
            this._model = model;
            this.Items = new UnionedObservableCollection<object>();
            this.TreeViewItems = new ObservableCollection<TreeViewItem>();
            this.Items.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(ChildItems_CollectionChanged);
        }

        private ExtendableTreeViewModel _model;

        void ChildItems_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    for (int i = 0; i < e.NewItems.Count; i++)
                    {
                        TreeViewItem child = _model.CreateTreeViewItem(e.NewItems[i]);
                        this.TreeViewItems.Insert(i + e.NewStartingIndex, child);
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Move:
                    {
                        if (e.OldStartingIndex < e.NewStartingIndex)
                        {
                            for (int i = e.OldItems.Count - 1; i >= 0; i--)
                            {
                                this.TreeViewItems.Move(e.OldStartingIndex + i, e.NewStartingIndex + i);
                            }
                        }
                        else
                        {
                            for (int i = 0; i < e.OldItems.Count; i++)
                            {
                                this.TreeViewItems.Move(e.OldStartingIndex + i, e.NewStartingIndex + i);
                            }
                        }
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    {
                        for (int i = e.OldItems.Count - 1 + e.OldStartingIndex; i >= e.OldStartingIndex; i--)
                        {
                            TreeViewItem item = this.TreeViewItems[i];
                            this.TreeViewItems.RemoveAt(i);
                            item.Dispose();
                        }
                    }

                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Replace:
                    {
                        for (int i = 0; i < e.OldItems.Count; i++)
                        {
                            int index = i + e.OldStartingIndex; 
                            TreeViewItem old = this.TreeViewItems[index];
                            TreeViewItem item = _model.CreateTreeViewItem(e.NewItems[i]);
                            this.TreeViewItems[index] = item;
                            old.Dispose();
                        }
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
                    {
                        foreach (TreeViewItem ti in this.TreeViewItems)
                        {
                            ti.Dispose();
                        }
                        this.TreeViewItems.Clear();
                        foreach (object o in this.Items)
                        {
                            this.TreeViewItems.Add(_model.CreateTreeViewItem(o));
                        }
                    }
                    break;
            }
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

        public UnionedObservableCollection<object> Items { get; private set; }

        public ObservableCollection<TreeViewItem> TreeViewItems { get; private set; }

        #region IDisposable Members

        public void Dispose()
        {
            this.Items.CollectionChanged -= new System.Collections.Specialized.NotifyCollectionChangedEventHandler(ChildItems_CollectionChanged);
            this.Items.Clear();
            foreach (TreeViewItem ti in this.TreeViewItems)
            {
                ti.Dispose();
            }
            this.TreeViewItems.Clear();
        }

        #endregion
    }
}
