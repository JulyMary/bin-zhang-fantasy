using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Fantasy.Studio
{
    public interface ICommandTargetProvider
    {
        IInputElement CommandTarget { get; }
    }
}
