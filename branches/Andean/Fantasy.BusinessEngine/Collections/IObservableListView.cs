using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.BusinessEngine.Collections
{
    public interface IObservableListView : IObservableList
    {
        IObservableList Source { get; set; }
    }

    public interface IObservableListView<T> : IObservableList<T>
    {

        IObservableList<T> Source { get; set; }
        
    }
}
