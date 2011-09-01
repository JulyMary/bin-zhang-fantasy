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

        UIElement Element { get; }

        string Title { get; }

        void Closing(CancelEventArgs e);

        void Closed();

        /// <summary>
        /// Is called when the view m_content is selected inside the window
        /// tab. NOT when the windows is selected.
        /// </summary>
        void ViewContentSelected();

        /// <summary>
        /// Is called when the view m_content is deselected inside the window
        /// tab before the other window is selected. NOT when the windows is deselected.
        /// </summary>
        void ViewContentDeselected();
    }
}
