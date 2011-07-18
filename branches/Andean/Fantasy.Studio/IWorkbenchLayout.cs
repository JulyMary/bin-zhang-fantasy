﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.Studio
{
    public interface IWorkbenchLayout
    {
        /// <summary>
        /// The active workbench window.
        /// </summary>
        IWorkbenchWindow ActiveWorkbenchwindow
        {
            get;
        }

        /// <summary>
        /// Attaches this layout manager to a workbench object.
        /// </summary>
        void Attach(IWorkbench workbench);

        /// <summary>
        /// Detaches this layout manager from the current workspace.
        /// </summary>
        void Detach();

        /// <summary>
        /// Shows a new <see cref="IPadContent"/>.
        /// </summary>
        void ShowPad(IPadContent content);

        /// <summary>
        /// Activates a pad (Show only makes it visible but Activate does
        /// bring it to foreground)
        /// </summary>
        void ActivatePad(IPadContent content);

        /// <summary>
        /// Hides a new <see cref="IPadContent"/>.
        /// </summary>
        void HidePad(IPadContent content);

        /// <summary>
        /// returns true, if padContent is visible;
        /// </summary>
        bool IsVisible(IPadContent padContent);

        /// <summary>
        /// Shows a new <see cref="IViewContent"/>.
        /// </summary>
        IWorkbenchWindow ShowView(IViewContent content);

        /// <summary>
        /// Is called, when the workbench window which the user has into
        /// the foreground (e.g. editable) changed to a new one.
        /// </summary>
        event EventHandler ActiveWorkbenchWindowChanged;

        // only needed in the workspace window when the 'secondary view m_content' changed
        // it is somewhat like 'active workbench window changed'
        void OnActiveWorkbenchWindowChanged(EventArgs e);

        void Save();

       
    }
}
