using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Studio.Controls;
using System.Collections.ObjectModel;
using Fantasy.AddIns;

namespace Fantasy.Studio
{
    public class ToolBoxPadModel : ToolBoxModel, IObjectWithSite
    {

        public ToolBoxPadModel(IServiceProvider services)
        {
            this.Site = services;
            this.Items = _items;
            IWorkbench w = this.Site.GetRequiredService<IWorkbench>();
            w.ActiveWorkbenchWindowChanged += new EventHandler(ActiveWorkbenchWindowChanged);
            this.RefreshItems();

        }


        private ObservableCollection<ToolBoxItemModel> _items = new ObservableCollection<ToolBoxItemModel>();


        void ActiveWorkbenchWindowChanged(object sender, EventArgs e)
        {
            this.RefreshItems();
        }

        private void RefreshItems()
        {
            this._items.Clear();

            foreach(object obj in AddInTree.Tree.GetTreeNode("fantasy/studio/workbench/toolbox/items").BuildChildItems(this, this.Site))
            {
                if(obj is ToolBoxItemModel)
                {
                    this._items.Add((ToolBoxItemModel)obj);
                }
                else if(obj is ToolBoxItemModel[])
                {
                    foreach(ToolBoxItemModel item in (ToolBoxItemModel[])obj)
                    {
                        this._items.Add(item);
                    }
                }
            }
        }

        public IServiceProvider Site { get; set; }
    }
}
