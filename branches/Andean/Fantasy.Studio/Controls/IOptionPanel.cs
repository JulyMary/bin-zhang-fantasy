using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.Studio.Controls
{
    public interface IOptionPanel
    {
        void Save();

        EditingState DirtyState { get; }

        event EventHandler DirtyStateChanged;
    }
}
