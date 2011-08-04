using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Fantasy.AddIns
{
    [TypeConverter(typeof(StringValueProviderConverter))]
    public interface IValueProvider
    {
        object Source { get; set; }

        object Value { get; }

        event EventHandler ValueChanged;
    }
}
