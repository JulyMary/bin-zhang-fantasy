﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using Fantasy.AddIns;

namespace Fantasy.Studio
{
    public class AddInDataTemplateSelector : DataTemplateSelector
    {

        public override System.Windows.DataTemplate SelectTemplate(object item, System.Windows.DependencyObject container)
        {
            System.Windows.DataTemplate rs = null;
            IServiceProvider site = null;
            if (this.Owner is IObjectWithSite)
            {
                site = ((IObjectWithSite)this.Owner).Site;
            }
            foreach (DataTemplateSelector chain in AddInTree.Tree.GetTreeNode(this.Path).BuildChildItems(this.Owner, site))
            {
                rs = chain.SelectTemplate(item, container);
                if (rs != null)
                {
                    break;
                }
            }

            return rs;
        }

        public string Path { get; set; }

        public object Owner { get; set; }

    }
}
