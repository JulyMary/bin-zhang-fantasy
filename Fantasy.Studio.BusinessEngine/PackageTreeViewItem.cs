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
    public class PackageTreeViewItem : NotifyPropertyChangedObject, IDocumentTreeViewItem
    {
        private BusinessPackage _package;

        private bool _childLoaded = false;
        public PackageTreeViewItem(BusinessPackage package)
        {
            this._package = package;
            this._package.PropertyChanged += new EventHandler<Fantasy.BusinessEngine.Events.EntityPropertyChangedEventArgs>(PackagePropertyChanged);
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
            get { return _package.Name; }
        }

        public Guid Id
        {
            get
            {
                return _package.Id;
            }
        }

        public string FullName
        {
            get
            {
                return _package.FullName;
            }
        }

        public string CodeName
        {
            get
            {
                return _package.CodeName;
            }
        }

        public string FullCodeName
        {
            get
            {
                return _package.FullCodeName;
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
                    foreach(IPackageChildrenProvider provider in AddInTree.Tree.GetTreeNode("fantasy/studio/businessengine/documentpad/package/childprovider").BuildChildItems<IPackageChildrenProvider>(this._package))
                    {
                        this._children.Union(provider.GetItems(this._package) );
                    }
                    this._childLoaded = true;
                }

                return _children;
            }
        }

        public void Refresh()
        {

            this._children.Clear();
            foreach (IPackageChildrenProvider provider in AddInTree.Tree.GetTreeNode("fantasy/studio/businessengine/documentpad/package/childprovider").BuildChildItems<IPackageChildrenProvider>(this._package))
            {
                this._children.Union(provider.GetItems(this._package));
            }
            this._childLoaded = true;
        }

        public void Open()
        {
            IEditingService documentService = ServiceManager.Services.GetRequiredService<IEditingService>();
            
            documentService.OpenView(this._package); 
        }

        #region IDocumentTreeViewItem Members


        private ContextMenu _contextMenu;
        public ContextMenu ContextMenu
        {
            get
            {
                if (this._contextMenu == null)
                {
                    this._contextMenu = ServiceManager.Services.GetRequiredService<IMenuService>().CreateContextMenu("fantasy/studio/businessengine/documentpad/package/contextmenu", this._package);
                }

                return _contextMenu;
            }
        }

        #endregion
    }
}
