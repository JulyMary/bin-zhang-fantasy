using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AvalonDock;
using System.ComponentModel;

namespace Fantasy.Studio.Workbench.Layout
{
    /// <summary>
    /// Interaction logic for WorkbenchWindow.xaml
    /// </summary>
    public partial class WorkbenchWindow : DocumentContent, IWorkbenchWindow
    {
        private IViewContent _view;
       
        public WorkbenchWindow(IViewContent view)
        {
            InitializeComponent();
            this._view = view;
            this.Title = GetDisplayTitle();
            view.TitleChanged += new EventHandler(ViewTitleChanged);

            view.IconChanged += new EventHandler(ViewIconChanged);
            this.Icon = view.Icon;
            if (view is IEditingViewContent)
            {
                ((IEditingViewContent)view).DirtyStateChanged += new EventHandler(ViewTitleChanged);
            }
            this.Content = view.Element;
            this.IsActiveDocumentChanged +=new EventHandler(OnIsActiveDocumentChanged);
            view.WorkbenchWindow = this;
        }

        void ViewIconChanged(object sender, EventArgs e)
        {
            this.Icon = this._view.Icon;
        }

        void ViewTitleChanged(object sender, EventArgs e)
        {


            this.Title = GetDisplayTitle();
            this.OnTitleChanged(EventArgs.Empty);

        }

        private string GetDisplayTitle()
        {
            string title = this.ViewContent.Title;
            IEditingViewContent edit = this.ViewContent as IEditingViewContent;
            if (edit != null && edit.DirtyState == EditingState.Dirty)
            {
                title += "*";
            }
            return title;
        }




        

        public event EventHandler TitleChanged;

        protected virtual void OnTitleChanged(EventArgs e)
        {
            if (this.TitleChanged != null)
            {
                this.OnTitleChanged(e);
            }
        }


        private void OnIsActiveDocumentChanged(object sender, EventArgs e)
        {
            if (this.IsActiveDocument)
            {
                this.OnSelected(EventArgs.Empty);
            }
            else
            {
                this.OnDeselected(EventArgs.Empty);
            }
        }

        #region IWorkbenchWindow Members

        public IViewContent ViewContent
        {
            get { return _view; }
        }

        public void Select()
        {
            this.Activate();
        }

        bool IWorkbenchWindow.IsVisible
        {
            get
            {
                return this.IsVisible;
            }
            set
            {
                this.Hide();
            }
        }

        void IWorkbenchWindow.Close()
        {
            this.Close();
        }

        

        public event EventHandler Deselected;
        protected virtual void OnDeselected(EventArgs e)
        {
            if (this.ViewContent != null)
            {
                this.ViewContent.Deselected();
            }
            if (this.Deselected != null)
            {
                this.Deselected(this, e);
            }

        }

        public event EventHandler Selected;
        protected virtual void OnSelected(EventArgs e)
        {
            if (this.ViewContent != null)
            {
                this.ViewContent.Selected();
            }

            if (this.Selected != null)
            {
                this.Selected(this, e);
            }

        }


        #endregion

        #region IWorkbenchWindow Members


        protected override void OnClosing(CancelEventArgs e)
        {
            if (this._closing != null)
            {
                this._closing(this, e);
            }
            this._view.Closing(e);
            base.OnClosing(e);
            
        }

        protected override void OnClosed()
        {
            this._view.Closed();
            base.OnClosed();
        }
       
        private CancelEventHandler _closing;


        event CancelEventHandler IWorkbenchWindow.Closing
        {
            add
            {
                _closing += value;
            }
            remove
            {
                _closing -= value;
            }
        }

        #endregion
    }
}
