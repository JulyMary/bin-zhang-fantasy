using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.BusinessEngine.Collections
{
    public class BusinessObjectCollection<TChild> : ObservableListView<TChild> 
      
        where TChild : BusinessObject
    {
        public BusinessObjectCollection(IObservableList<TChild> initialSource)
            : base(initialSource)
        {
            
        }

       
    }
}
