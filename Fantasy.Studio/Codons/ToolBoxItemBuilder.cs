using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.AddIns;
using System.Windows.Markup;

namespace Fantasy.Studio.Codons
{
    [ContentProperty("Instance")]
    public class ToolBoxItemBuilder : CodonBase
    {
        public override object BuildItem(object owner, System.Collections.IEnumerable subItems, ConditionCollection condition, IServiceProvider services)
        {
            return Instance.BuildItems(owner);
        }
        
        public IToolBoxItesBuilder Instance { get; set; }

    }
}
