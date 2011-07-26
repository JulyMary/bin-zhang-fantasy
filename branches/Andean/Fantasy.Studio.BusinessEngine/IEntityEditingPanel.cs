using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine;
using System.Windows;
using System.ComponentModel;

namespace Fantasy.Studio.BusinessEngine
{
    public interface IEntityEditingPanel
    {
        void Initialize();

        void Load(IBusinessEntity entity);

        EditingState DirtyState { get;}

        event EventHandler DirtyStateChanged;

        void Save();

        UIElement Content { get; }

        string Title { get; }

        void Closing(CancelEventArgs e);

        void Closed();
    }
}
