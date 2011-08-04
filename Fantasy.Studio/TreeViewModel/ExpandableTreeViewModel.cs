using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Fantasy.AddIns;
using System.Windows.Input;
using Fantasy.Adaption;
using Fantasy.Studio.Services;

namespace Fantasy.Studio.TreeViewModel
{
    public class ExpandableTreeViewModel : ObjectWithSite
    {
        public ExpandableTreeViewModel(string path, object owner, IServiceProvider site)
        {
            this.Site = site;

            this._itemBuilders = AddInTree.Tree.GetTreeNode(path).BuildChildItems<TreeViewItemBuilder>(owner, site).ToList();
            this.RootItems = new ObservableCollection<object>();
            this.RootTreeViewItems = new ObservableCollection<TreeViewItem>();

            this.RootItems.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(RootItems_CollectionChanged);
       
        }

        void RootItems_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    this.RootTreeViewItems.Insert(e.NewStartingIndex, this.CreateTreeViewItem(e.NewItems[0]));
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    {
                        this.RootTreeViewItems.RemoveAt(e.OldStartingIndex);
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
                    this.RootTreeViewItems.Clear();
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Replace:
                    {
                        TreeViewItem newNode = this.CreateTreeViewItem(e.NewItems[0]);
                        this.RootTreeViewItems[e.OldStartingIndex] = newNode; 
                    }
                    break;
            }
        }

        private TreeViewItem CreateTreeViewItem(object item)
        {
            TreeViewItemBuilder builder = this._itemBuilders.First(b => b.Codon.TargetType.IsInstanceOfType(item));
            TreeViewItem rs = new TreeViewItem();
            rs.Text = builder.Codon._text != null ? builder.Codon._text.Build<IValueProvider>() : builder.Codon.Text;
            if (rs.Text is IObjectWithSite)
            {
                ((IObjectWithSite)rs).Site = this.Site;
            }

            rs.Icon = builder.Codon._icon != null ? builder.Codon._text.Build<IValueProvider>() : builder.Codon.Icon;
            if (rs.Text is IObjectWithSite)
            {
                ((IObjectWithSite)rs).Site = this.Site;
            }

            rs.Selected = this.CreateCommand(builder.Codon._selected, builder.Codon.Selected);
            rs.Unselected = this.CreateCommand(builder.Codon._unselected, builder.Codon.Unselected);
            rs.DoubleClick = this.CreateCommand(builder.Codon._doubleClick, builder.Codon.DoubleClick);
            rs.Expanded = this.CreateCommand(builder.Codon._expanded, builder.Codon.Expanded);
            rs.Collapsed = this.CreateCommand(builder.Codon._collapsed, builder.Codon.Collapsed);

            if (!string.IsNullOrEmpty(builder.Codon.ContextMenu))
            {
                rs.ContextMenu = this.Site.GetRequiredService<IMenuService>().CreateContextMenu(builder.Codon.ContextMenu, item, this.Site);
            }

            foreach (IChildrenProvider provider in builder.ChildProviders)
            {
                //rs.ChildItems.Union(provider.GetChildren(item));
            }


            
        }

        private ICommand CreateCommand(ObjectBuilder builder, ICommand command)
        {
            ICommand rs = null;
            if (builder != null)
            {
                rs = this.Site.GetRequiredService<IAdapterManager>().GetAdapter<ICommand>(builder.Build<object>());
                if (rs is IObjectWithSite)
                {
                    ((IObjectWithSite)Site).Site = this.Site;
                }

            }
            else
            {
                rs = command;
            }
            return rs;
        }


        private List<TreeViewItemBuilder> _itemBuilders = new List<TreeViewItemBuilder>();

        public ObservableCollection<object> RootItems { get; private set; }

        ObservableCollection<TreeViewItem> RootTreeViewItems { get; private set; }


    }
}
