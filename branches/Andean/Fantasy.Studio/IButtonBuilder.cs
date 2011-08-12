using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Fantasy.AddIns;
using Fantasy.Studio.Controls;

namespace Fantasy.Studio
{
    public interface IButtonBuilder
    {
        ButtonModel[] Build(object caller);
    }
}
