using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.Studio.TreeViewModel
{
    public interface IValueProvider
    {
        object Source { get; set; }

        object Value { get; }

        event EventHandler ValueChanged;
    }
}
