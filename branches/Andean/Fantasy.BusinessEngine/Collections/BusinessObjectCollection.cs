using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.BusinessEngine.Collections
{
    public class BusinessObjectCollection<TParent, TChild> : ObservableListView<TChild> 
        where TParent : BusinessObject 
        where TChild : BusinessObject
    {
        public BusinessObjectCollection(TParent parent, IObservableList<TChild> initialSource)
            : base(initialSource)
        {
            this.Owner = parent;
        }

        public virtual TParent Owner { get; protected set; }
    }
}
