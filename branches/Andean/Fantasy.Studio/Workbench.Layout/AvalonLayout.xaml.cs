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
using System.IO;
using Fantasy.Studio.Properties;
using System.Xml.Linq;

namespace Fantasy.Studio.Workbench.Layout
{
    /// <summary>
    /// Interaction logic for AvalonLayout.xaml
    /// </summary>
    public partial class AvalonLayout : UserControl, IWorkbenchLayout
    {
        public AvalonLayout()
        {
            InitializeComponent();
        }

        #region IWorkbenchLayout Members

        public IWorkbenchWindow ActiveWorkbenchwindow
        {
            get { return this.dockingManager.ActiveDocument as IWorkbenchWindow; }
        }

        private IWorkbench _workbench;

        private List<DockableContent> _padContainers = new List<DockableContent>();

        public void Attach(IWorkbench workbench)
        {
            workbench.ContentContainer.Content = this;
            this._workbench = workbench;
            if (dockingManager.IsLoaded)
            {
                LoadLayout();
            }
            else
            {
                dockingManager.Loaded += (sender, e) => { LoadLayout(); };
            }
        }

        private void LoadLayout()
        {
            
            List<IPadContent> unattachedPads = new List<IPadContent>(_workbench.Pads);

            dockingManager.DeserializationCallback += (sender, e) =>
            {
                int index = unattachedPads.FindIndex(p => p.Name == e.Name);
                if (index >= 0)
                {
                    IPadContent pad = unattachedPads[index];
                    e.Content = CreateDockableContentForPad(pad);
                    unattachedPads.RemoveAt(index);
                }

            };

            if (Settings.Default.AvalonWorkbenchLayout != null)
            {
                dockingManager.RestoreLayout(Settings.Default.AvalonWorkbenchLayout.CreateReader());
            }

            foreach (IPadContent pad in unattachedPads)
            {
                DockableContent content = CreateDockableContentForPad(pad);
                content.Show(dockingManager, AnchorStyle.Left);
            }

            foreach (IViewContent view in _workbench.Views)
            {
                this.ShowView(view);
            }
        }

        private DockableContent CreateDockableContentForPad(IPadContent pad)
        {
            DockableContent rs = new DockableContent();
            rs.Content = pad.Content;
            rs.Name = pad.Name;
            rs.Title = pad.Title;
            rs.Icon = pad.Icon;
            rs.IsCloseable = true;
            rs.HideOnClose = true;
            rs.IsActiveContentChanged += new EventHandler(PadIsActiveContentChanged);
            _padContainers.Add(rs);
           
            return rs;
        }

        public void Detach()
        {
            if (this._workbench.ContentContainer.Content  == this)
            {

                this._workbench.ContentContainer.Content  = null;
            }
        }

        public void ShowPad(IPadContent content)
        {
            DockableContent container = this._padContainers.Find(c => c.Name == content.Name);
            if (container != null)
            {
                container.Show();
            }
        }

        public void ActivatePad(IPadContent content)
        {
            DockableContent container = this._padContainers.Find(c => c.Name == content.Name);
            if (container != null)
            {
                container.Activate();
            }
        }

        public void HidePad(IPadContent content)
        {
            DockableContent container = this._padContainers.Find(c => c.Name == content.Name);
            if (container != null)
            {
                container.Hide();
            }
        }

        bool IWorkbenchLayout.IsVisible(IPadContent padContent)
        {
            DockableContent container = this._padContainers.Find(c => c.Name == padContent.Name);
            if (container != null)
            {
                return container.IsVisible;
            }
            else
            {
                return false;
            }
        }

        public IWorkbenchWindow ShowView(IViewContent content)
        {
            WorkbenchWindow rs = (WorkbenchWindow)this.dockingManager.Documents.FirstOrDefault(d => ((WorkbenchWindow)d).ViewContent == content);
            if (rs == null)
            {
                rs = new WorkbenchWindow(content);
                rs.Show(dockingManager, false);
            }
           
            rs.Activate();
            
            return rs;
        }

        public event EventHandler ActiveWorkbenchWindowChanged;

        public void OnActiveWorkbenchWindowChanged(EventArgs e)
        {
            if (this.ActiveWorkbenchWindowChanged != null)
            {
                this.ActiveWorkbenchWindowChanged(this, e);
            }
        }

        public void Save()
        {
            MemoryStream stream = new MemoryStream();
            this.dockingManager.SaveLayout(stream);
            stream.Seek(0, SeekOrigin.Begin);

            XElement x = XElement.Load(stream);
            Settings.Default.AvalonWorkbenchLayout = x;
        }

        #endregion

        private void PadIsActiveContentChanged(object sender, EventArgs e)
        {
            DockableContent container = (DockableContent)sender;
            IPadContent pad = this._workbench.Pads.Single(p => p.Content == container.Content);
            pad.BringPadToFront();
        }

        private void dockingManager_ActiveDocumentChanged(object sender, EventArgs e)
        {
            this.OnActiveWorkbenchWindowChanged(e);
        }
    }
}
