using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.AddIns;
using System.Windows.Markup;
using Fantasy.Studio.Controls;

namespace Fantasy.Studio.Codons
{
    [ContentProperty("Instance")]
    public class ButtonBuilder : CodonBase
    {
        public override object BuildItem(object owner, System.Collections.IEnumerable subItems, ConditionCollection condition, IServiceProvider services)
        {
            ButtonSourceCollections rs = new ButtonSourceCollections() { Builder = this.Instance, Conditions = condition };
            rs.Update(owner);
            return rs;
        }
        public IButtonBuilder Instance { get; set; }
    }
}
