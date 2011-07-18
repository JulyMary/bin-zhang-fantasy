using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Fantasy.Studio
{
    public interface IWorkbench
    {
        ViewCollection Views { get; }

        PadCollection Pads { get; }

        string ApplicationTitle { get; set; }

        string WindowTitle { get; set; }

        ContentControl ContentContainer { get; }

        IWorkbenchLayout Layout { get; set; }


        /// <summary>
        /// The active workbench window.
        /// </summary>
        IWorkbenchWindow ActiveWorkbenchWindow
        {
            get;
        }

      

        /// <summary>
        /// Inserts a new <see cref="IViewContent"/> object in the workspace.
        /// </summary>
        void ShowView(IViewContent content);



        /// <summary>
        /// Inserts a new <see cref="IPadContent"/> object in the workspace.
        /// </summary>
        void ShowPad(IPadContent content);

        /// <summary>
        /// Returns a pad from a specific type.
        /// </summary>
        IPadContent GetPad(Type type);

        T GetPad<T>() where T : IPadContent ;


        /// <summary>
        /// Returns visibility of given <code>padContent</code>;
        /// </summary>
        bool IsVisible(IPadContent padContent);

        /// <summary>
        /// Closes the IViewContent m_content when m_content is open.
        /// </summary>
        void CloseContent(IViewContent content);

        ///// <summary>
        ///// Closes all views inside the workbench.
        ///// </summary>
        //void CloseAllViews();

        /// <summary>
        /// Is called, when a workbench view was opened
        /// </summary>
        event EventHandler<ViewContentEventArgs> ViewOpened;

        /// <summary>
        /// Is called, when a workbench view was closed
        /// </summary>
        event EventHandler<ViewContentEventArgs> ViewClosed;

        /// <summary>
        /// Is called, when the workbench window which the user has into
        /// the foreground (e.g. editable) changed to a new one.
        /// </summary>
        event EventHandler ActiveWorkbenchWindowChanged;

        bool CloseAllViews();

    }
}
