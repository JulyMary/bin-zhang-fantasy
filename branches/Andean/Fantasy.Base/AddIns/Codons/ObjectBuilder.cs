using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.AddIns
{
    public class ObjectBuilder : CodonBase
    {
        public override object BuildItem(object owner, System.Collections.IEnumerable subItems, ConditionCollection condition)
        {
            return Activator.CreateInstance(this.Type);
        }

        public Type Type { get; set; }
    }
}
