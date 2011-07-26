using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Windows;
using Fantasy.BusinessEngine;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Fantasy.Collection;
using System.Windows;
using Fantasy.AddIns;
using Fantasy.Studio.Services;
using Fantasy.ServiceModel;
using System.Windows.Input;
using System.Windows.Controls;

namespace Fantasy.Studio.BusinessEngine
{
    public class PackageTreeViewItem : NotifyPropertyChangedObject, IObjectWithSite, IDocumentTreeViewItem
    {
        public BusinessPackage Package {get;private set;}

        private bool _childLoaded = false;
        public PackageTreeViewItem(BusinessPackage package)
        {
            this.Package = package;
            this.Package.PropertyChanged += new EventHandler<Fantasy.BusinessEngine.Events.EntityPropertyChangedEventArgs>(PackagePropertyChanged);
        }

        void PackagePropertyChanged(object sender, Fantasy.BusinessEngine.Events.EntityPropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Name")
            {
                this.OnPropertyChanged("Name");
            }
        }

        public string Name
        {
            get { return Package.Name; }
        }

        public Guid Id
        {
            get
            {
                return Package.Id;
            }
        }

        public string FullName
        {
            get
            {
                return Package.FullName;
            }
        }

        public string CodeName
        {
            get
            {
                return Package.CodeName;
            }
        }

        public string FullCodeName
        {
            get
            {
                return Package.FullCodeName;
            }
        }

        private ImageSource _icon;

        public ImageSource Icon
        {
            get
            {
                if (_icon == null)
                {
                    _icon = new BitmapImage(new Uri("/Fantasy.Studio.BusinessEngine;component/images/package.png", UriKind.Relative));
                }
                return _icon;
            }
        }


        private UnionedObservableCollection<IDocumentTreeViewItem> _children = new UnionedObservableCollection<IDocumentTreeViewItem>();

        public IEnumerable<IDocumentTreeViewItem> ChildItems
        {
            get 
            {
                if (!_childLoaded)
                {
                    foreach(IPackageChildrenProvider provider in AddInTree.Tree.GetTreeNode("fantasy/studio/businessengine/documentpad/package/childprovider").BuildChildItems<IPackageChildrenProvider>(this.Package, this.Site))
                    {
                        this._children.Union(provider.GetItems(this.Package) );
                    }
                    this._childLoaded = true;
                }

                return _children;
            }
        }

        public void Refresh()
        {

            this._children.Clear();
            foreach (IPackageChildrenProvider provider in AddInTree.Tree.GetTreeNode("fantasy/studio/businessengine/documentpad/package/childprovider").BuildChildItems<IPackageChildrenProvider>(this.Package, this.Site))
            {
                this._children.Union(provider.GetItems(this.Package));
            }
            this._childLoaded = true;
        }

        public void Open()
        {
            IEditingService documentService = ServiceManager.Services.GetRequiredService<IEditingService>();

            documentService.OpenView(this.Package); 
        }

        #region IDocumentTreeViewItem Members


        private ContextMenu _contextMenu;
        public ContextMenu ContextMenu
        {
            get
            {
                if (this._contextMenu == null)
                {
                    this._contextMenu = ServiceManager.Services.GetRequiredService<IMenuService>().CreateContextMenu("fantasy/studio/businessengine/documentpad/package/contextmenu", this, this.Site);
                }

                return _contextMenu;
            }
        }

        #endregion


        public IServiceProvider Site { get; set; }
    }
}
