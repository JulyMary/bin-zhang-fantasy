using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Fantasy.AddIns;
using System.Windows.Input;
using Fantasy.Adaption;
using Fantasy.Studio.Services;
using System.Windows;
using System.Collections.Specialized;
using Fantasy.Collections;

namespace Fantasy.Studio.TreeViewModel
{
    public class ExtendableTreeViewModel : ObjectWithSite
    {
        public ExtendableTreeViewModel(string path, object owner, IServiceProvider site)
        {
            this.Site = site;

            this._itemBuilders = AddInTree.Tree.GetTreeNode(path).BuildChildItems<TreeViewItemBuilder>(owner, site).ToList();
            this.Items = new ObservableCollection<object>();
            this.TreeViewItems = new ObservableAdapterCollection<TreeViewItem>(this.Items, this.CreateTreeViewItem);
           
           
               
        }
 
        internal TreeViewItem CreateTreeViewItem(object item)
        {
            TreeViewItemBuilder builder = this._itemBuilders.First(b => b.Codon.TargetType.IsInstanceOfType(item));
            TreeViewItem rs = new TreeViewItem(this);
            rs.DataContext = item;
            rs.Text = builder.Codon._text != null ? builder.Codon._text.Build<IValueProvider>() : builder.Codon.Text;
            if (rs.Text != null)
            {
                rs.Text.Source = item;
            }
            if (rs.Text is IObjectWithSite)
            {
                ((IObjectWithSite)rs).Site = this.Site;
            }

            rs.Icon = builder.Codon._icon != null ? builder.Codon._text.Build<IValueProvider>() : builder.Codon.Icon;
            if (rs.Icon != null)
            {
                rs.Icon.Source = item;
            }
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
                rs.Items.Union(provider.GetChildren(item));
            }

            return rs;

            
        }

        private ICommand CreateCommand(ObjectBuilder builder, ICommand command)
        {
            ICommand rs = null;
            if (builder != null)
            {
                rs = this.Site.GetRequiredService<IAdapterManager>().GetAdapter<ICommand>(builder.Build<object>());
                if (rs is IObjectWithSite)
                {
                    ((IObjectWithSite)rs).Site = this.Site;
                }

            }
            else
            {
                rs = command;
            }
            return rs;
        }


        private List<TreeViewItemBuilder> _itemBuilders = new List<TreeViewItemBuilder>();

        public ObservableCollection<object> Items { get; private set; }

        public IEnumerable<TreeViewItem> TreeViewItems { get; private set; }



       
    }
}
