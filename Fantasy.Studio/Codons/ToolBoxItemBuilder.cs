using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.AddIns;
using System.Windows.Markup;

namespace Fantasy.Studio.Codons
{
    [ContentProperty("Builder")]
    public class ToolBoxItemBuilder : CodonBase
    {
        public override object BuildItem(object owner, System.Collections.IEnumerable subItems, ConditionCollection condition, IServiceProvider services)
        {
            if (Builder is IObjectWithSite)
            {
                ((IObjectWithSite)Builder).Site = services;
            }

            return Builder.BuildItems(owner);
        }
        
        public IToolBoxItemsBuilder Builder { get; set; }

    }
}
