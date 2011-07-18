using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Windows.Markup;

namespace Fantasy.AddIns
{
   
    public interface ICodon
    {
        ICondition Conditional { get;}

        bool HandleCondition { get; }

        string ID { get; }

        string InsertAfter { get; }

        string InsertBefore { get; }

        object BuildItem(object owner, IEnumerable subItems, ConditionCollection condition);

        IList<ICodon> Codons { get;}
    }
}
