using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.AddIns;
using Fantasy.Studio.Controls;

namespace Fantasy.Studio.Codons
{
    [System.Windows.Markup.ContentProperty("Provider")]
    public class TreeNodeChildProvider : CodonBase, ICodon
    {
        private ObjectBuilder _provider = null;

        [Template("_provider")]
        public IChildItemsProvider Provider { get; set; }

        public override object BuildItem(object owner, System.Collections.IEnumerable subItems, ConditionCollection condition, IServiceProvider services)
        {
            return this._provider != null ? _provider.Build<IChildItemsProvider>() : this.Provider;
        }
    }
}
