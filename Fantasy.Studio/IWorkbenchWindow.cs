using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Fantasy.Studio
{
    public interface IWorkbenchWindow
    {
        /// <summary>
        /// The window title.
        /// </summary>
        string Title
        {
            get;
            set;
        }

        /// <summary>
        /// The current view m_content which is shown inside this window.
        /// </summary>
        IViewContent ViewContent
        {
            get;
        }

        /// <summary>
        /// Brings this window to front and sets the user focus to this
        /// window.
        /// </summary>
        void Select();

        bool IsVisible { get; set; }

        void Close();

        /// <summary>
        /// Is called when the window is selected.
        /// </summary>
        event EventHandler Selected;

        /// <summary>
        /// Is called when the window is deselected.
        /// </summary>
        event EventHandler Deselected;

        /// <summary>
        /// Is called when the title of this window has changed.
        /// </summary>
        event EventHandler TitleChanged;


        event CancelEventHandler Closing;

        event EventHandler Closed;
    }
}
