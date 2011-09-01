using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows;

namespace Fantasy.Studio
{
    public interface IViewContent : IDisposable
    {
       
        UIElement Element
        {
            get;
        }

        /// <summary>
        /// The workbench window in which this view is displayed.
        /// </summary>
        IWorkbenchWindow WorkbenchWindow
        {
            get;
            set;
        }

        /// <summary>
        /// The text on the tab page when more than one view m_content
        /// is attached to a single window.
        /// </summary>
        string Title
        {
            get;
        }

        /// <summary>
        /// Is called when the view m_content is selected inside the window
        /// tab. NOT when the windows is selected.
        /// </summary>
        void Selected();

        /// <summary>
        /// Is called when the view m_content is deselected inside the window
        /// tab before the other window is selected. NOT when the windows is deselected.
        /// </summary>
        void Deselected();


        /// <summary>
        /// Is called when the view before close;
        /// </summary>
        /// <returns><code>True</code> for close view and false for cancel the operation.</returns>
        void Closing(CancelEventArgs e);

        void Closed();

        event EventHandler TitleChanged;

        string DocumentName { get;}

        string DocumentType { get;}
    }
}
