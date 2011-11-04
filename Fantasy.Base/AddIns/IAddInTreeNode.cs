using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Fantasy.AddIns
{
    public interface IAddInTreeNode
    {
        IAddInTreeNode[] ChildNodes { get; }

        ICodon Codon { get; }

        ConditionCollection Condition { get; }

        ConditionFailedAction GetCurrentConditionFailedAction(object caller);

        IEnumerable BuildChildItems(object caller, IServiceProvider site );

        IEnumerable<T> BuildChildItems<T>(object caller, IServiceProvider site);

        object BuildItem(object caller, IServiceProvider site);

        T BuildItem<T>(object caller, IServiceProvider site);

        
    }
}
