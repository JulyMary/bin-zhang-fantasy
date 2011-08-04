using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Studio.Codons;

namespace Fantasy.Studio.TreeViewModel
{
    class TreeViewItemBuilder : ObjectWithSite
    {
        public TreeNode Codon { get; set; }

        public List<IChildrenProvider> ChildProviders { get; set; } 
    }
}
