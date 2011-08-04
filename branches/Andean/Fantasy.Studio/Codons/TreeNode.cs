using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.AddIns;
using Fantasy.Studio.TreeViewModel;
using System.Windows.Input;

namespace Fantasy.Studio.Codons
{
    public class TreeNode : CodonBase
    {
        public Type TargetType { get; set; }

        internal ObjectBuilder _text;
        [Template("_text")]
        public IValueProvider Text { get; set; }

        internal ObjectBuilder _icon;
        [Template("_icon")]
        public IValueProvider Icon { get; set; }

        internal ObjectBuilder _doubleClick;
        [Template("_doubleClick")]
        public ICommand DoubleClick { get; set; }

        internal ObjectBuilder _selected;
        [Template("_selected")]
        public ICommand Selected { get; set; }

        internal ObjectBuilder _unselected;
        [Template("_unselected")]
        public ICommand Unselected { get; set; }


        internal ObjectBuilder _expanded;
        [Template("_expanded")]
        public ICommand Expanded { get; set; }

        internal ObjectBuilder _collapsed;
        [Template("_collapsed")]
        public ICommand Collapsed { get; set; }


        public string ContextMenu { get; set; }

        public override object BuildItem(object owner, System.Collections.IEnumerable subItems, ConditionCollection condition, IServiceProvider services)
        {
            TreeViewItemBuilder rs = new TreeViewItemBuilder() { Codon = this, ChildProviders = subItems.Cast<IChildrenProvider>().ToList() };
            return rs;
        }

       
    }
}
