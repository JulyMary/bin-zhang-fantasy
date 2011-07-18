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
using System.Windows.Shapes;
using Fantasy.ServiceModel;
using Fantasy.Studio.Services;
using Fantasy.AddIns;
using Fantasy.Studio.Properties;
using System.ComponentModel;

namespace Fantasy.Studio
{
    /// <summary>
    /// Interaction logic for DefaultWorkbench.xaml
    /// </summary>
    public partial class DefaultWorkbench : Window, IObjectWithSite, IWorkbench, IService, IEditingService
    {
        public DefaultWorkbench()
        {
            InitializeComponent();
        }

        private IServiceProvider _site;

        public IServiceProvider Site
        {
            get { return _site; }
            set { _site = value; }
        }


        public virtual void InitializeService()
        {
          

            this._messageBox = this.Site.GetRequiredService<IMessageBoxService>();

            IMenuService menuSvc = this.Site.GetRequiredService<IMenuService>();
            UIElement menu = menuSvc.CreateMainMenu("fantasy/studio/workbench/mainmenu", this);
            this.MainMenuTray.Child = menu;
            foreach (ToolBar bar in AddInTree.Tree.GetTreeNode("fantasy/studio/workbench/toolbars").BuildChildItems(this))
            {
                this.ToolBarTray.ToolBars.Add(bar);
            }

            ContextMenu ctx = menuSvc.CreateContextMenu("fantasy/studio/toolbartray/contextmenu", this.ToolBarTray);
            this.ToolBarTray.ContextMenu = ctx;
            if (menu is FrameworkElement)
            {
                ((FrameworkElement)menu).ContextMenu = ctx;
            }
            foreach (IPadContent pad in AddInTree.Tree.GetTreeNode("fantasy/studio/workbench/pads").BuildChildItems(this))
            {
                this.Pads.Add(pad);
            }
            foreach (IPadContent pad in this.Pads)
            {
                pad.Initialize();
            }

            if (this.Initialize != null)
            {
                this.Initialize(this, EventArgs.Empty);
            }

        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);

            e.Cancel = ! this.CloseAllViews();

            if (!e.Cancel && this.Layout != null)
            {
                this.Layout.Save();
            }
        }



        public virtual void UninitializeService()
        {
            if (this.Uninitialize != null)
            {
                this.Uninitialize(this, EventArgs.Empty);
            }
        }

        public event EventHandler Initialize;

        public event EventHandler Uninitialize;

        private ViewCollection _views = new ViewCollection();

        public ViewCollection Views
        {
            get { return _views; }
        }

        private PadCollection _pads = new PadCollection();
        public PadCollection Pads
        {
            get { return _pads;  }
        }

        #region IWorkbench Members

        private string _applicationTitle = "Fantasy Studio";
        public string ApplicationTitle
        {
            get
            {
                return _applicationTitle;
            }
            set
            {
                _applicationTitle = value;
            }
        }

        private string _windowTitle = null;
        public string WindowTitle
        {
            get
            {
                return _windowTitle;
            }
            set
            {
                _windowTitle = value;
            }
        }

        private void UpdateTitle()
        {
            string t = this.ApplicationTitle;
            if (!string.IsNullOrWhiteSpace(this.WindowTitle))
            {
                t = this.WindowTitle + " - " + ApplicationTitle;
            }

            this.Title = t;
        }

        ContentControl IWorkbench.ContentContainer
        {
            get
            {
                return this.ContentContainer;
            }
        }

        #endregion



        private IWorkbenchLayout _layout = null;
        public IWorkbenchLayout Layout
        {
            get
            {
                return _layout;
            }
            set
            {
                if (_layout != value)
                {
                    IWorkbenchWindow oldActive = null;
                    if (_layout != null)
                    {
                        oldActive = _layout.ActiveWorkbenchwindow;
                        _layout.Detach();
                    }
                    _layout = value;

                    if (_layout != null)
                    {
                        _layout.Attach(this);
                        _layout.ActiveWorkbenchWindowChanged += new EventHandler(LayoutActiveWorkbenchWindowChanged);
                        if (oldActive != _layout.ActiveWorkbenchwindow)
                        {
                            this.OnActiveWorkbenchWindowChanged(EventArgs.Empty);
                        }
                    }
                }
            }


        }

        void LayoutActiveWorkbenchWindowChanged(object sender, EventArgs e)
        {
            this.OnActiveWorkbenchWindowChanged(e);
        }

      

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (this.WindowState != System.Windows.WindowState.Minimized)
            {
                Settings.Default.WorkbenchState.WindowState = this.WindowState;
            }
        }



        #region IWorkbench Members


        public IWorkbenchWindow ActiveWorkbenchWindow
        {
            get { return this._layout != null ? this._layout.ActiveWorkbenchwindow : null; }
        }

       

        public void ShowView(IViewContent content)
        {
            bool openning = this._views.IndexOf(content) <= 0;
            if (openning)
            {
                this.Views.Add(content);
            }
            this._layout.ShowView(content);
            if (openning)
            {
                content.WorkbenchWindow.Closed += (sender, e) => { this.WorkbenchWindowClosed(content); };
                this.OnViewOpened(new ViewContentEventArgs(content));
            }
           
        }

       

        public void ShowPad(IPadContent content)
        {
            if (this._layout != null)
            {
                this._layout.ShowPad(content);
            }
        }

        public IPadContent GetPad(Type type)
        {
            return this.Pads.FirstOrDefault(p => type.IsInstanceOfType(p));
        }

        public T GetPad<T>() where T : IPadContent
        {
            return (T)GetPad(typeof(T));
        }

        bool IWorkbench.IsVisible(IPadContent padContent)
        {
            return this._layout != null ? this._layout.IsVisible(padContent) : false;
        }


        public void CloseContent(IViewContent content)
        {
            if (content.WorkbenchWindow != null)
            {
                content.WorkbenchWindow.Close();
            }
            else
            {
                CancelEventArgs e = new CancelEventArgs();
                content.Closing(e);
                if (!e.Cancel)
                {
                    content.Closed();
                }
                WorkbenchWindowClosed(content);
            }
        }

        private void WorkbenchWindowClosed(IViewContent content)
        {
            if (this.Views.Contains(content))
            {
                this._views.Remove(content);

                this.OnViewClosed(new ViewContentEventArgs(content));

                content.Dispose();
            }

        }

        private bool _closingAllViews = false;
        public bool CloseAllViews()
        {
            bool rs = true;
            if (this.Views.Count > 0)
            {
                this._closingAllViews = true;
                try
                {
                    var query = from v in this.Views where (v is IEditingViewContent) && ((IEditingViewContent)v).DirtyState == EditingState.Dirty  select (IEditingViewContent)v;
                    IEditingViewContent[] dirties = query.ToArray();
                    if (dirties.Length > 0)
                    {
                        string text = "\n" + string.Join("\n", dirties.Select(v => v.Title));
                       

                        switch (_messageBox.Show(string.Format(Properties.Resources.SaveChangesText, text), button: MessageBoxButton.YesNoCancel, defaultResult: MessageBoxResult.Yes))
                        {
                            case MessageBoxResult.Cancel:
                                rs = false;
                                break;
                            case MessageBoxResult.Yes:
                                foreach (IEditingViewContent view in dirties)
                                {
                                    view.Save();
                                }
                                break;

                        }
                    }
                    if (rs)
                    {
                        foreach (IViewContent content in this.Views.ToArray())
                        {
                            if (content.WorkbenchWindow != null)
                            {
                                content.WorkbenchWindow.Close();
                            }
                        }
                    }
                }
                finally
                {
                    this._closingAllViews = false;
                }
                this.OnActiveWorkbenchWindowChanged(EventArgs.Empty);
                
            }
            return rs;
        }


        public IEditingViewContent OpenView(object data)
        {
            IEditingViewContent rs = null;

            var query = from v in this.Views where (v is IEditingViewContent) && ((IEditingViewContent)v).Data == data select v;

            rs = (IEditingViewContent)query.SingleOrDefault();
            if (rs == null)
            {

                foreach (IEditingViewBuilder builder in AddInTree.Tree.GetTreeNode("fantasy/studio/documents/builders").BuildChildItems(data))
                {
                    rs = builder.CreateView(data);
                    rs.Load(data);
                    if (rs != null)
                    {
                        this.ShowView(rs);
                        if (rs.WorkbenchWindow != null)
                        {
                            rs.WorkbenchWindow.Closing += new System.ComponentModel.CancelEventHandler(WorkbenchWindowClosing);
                        }
                        break;
                    }
                }
            }
            return rs;
        }

        private IMessageBoxService _messageBox;

        private void WorkbenchWindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            IEditingViewContent content = (IEditingViewContent)((IWorkbenchWindow)sender).ViewContent;
            if (content.DirtyState == EditingState.Dirty  && !this._closingAllViews)
            {
                string text = "\n" + content.Title;
                switch (_messageBox.Show(string.Format(Properties.Resources.SaveChangesText, text), button: MessageBoxButton.YesNoCancel, defaultResult: MessageBoxResult.Yes))
                {
                    case MessageBoxResult.Cancel:
                        e.Cancel = true;
                        break;
                    case MessageBoxResult.Yes:
                        content.Save();
                        break;

                }
            }

        }

        public event EventHandler<ViewContentEventArgs> ViewOpened;

        protected virtual void OnViewOpened(ViewContentEventArgs e)
        {
            if (this.ViewOpened != null)
            {
                this.ViewOpened(this, e);
            }
        }

        public event EventHandler<ViewContentEventArgs> ViewClosed;

        protected virtual void OnViewClosed(ViewContentEventArgs e)
        {
            if (this.ViewClosed != null)
            {
                this.ViewClosed(this, e);
            }
        }

        public event EventHandler ActiveWorkbenchWindowChanged;

        protected virtual void OnActiveWorkbenchWindowChanged(EventArgs e)
        {
            if (this.ActiveWorkbenchWindowChanged != null)
            {
                this.ActiveWorkbenchWindowChanged(this, e);
            }
        }

        #endregion
    }
}
