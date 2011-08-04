using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.AddIns;

namespace Fantasy.Studio
{
    public interface IEditingViewContent : IViewContent
    {
        void Load(object data);

        object Data { get; }

        EditingState DirtyState { get; }

        event EventHandler DirtyStateChanged;

        void Save();
    }
}
