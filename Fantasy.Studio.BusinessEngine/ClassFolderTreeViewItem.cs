using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Studio.BusinessEngine.Properties;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using Fantasy.ServiceModel;
using Fantasy.Studio.Services;
using System.Collections.ObjectModel;
using Fantasy.BusinessEngine.Collections;
using Fantasy.BusinessEngine;

namespace Fantasy.Studio.BusinessEngine
{
    public class ClassFolderTreeViewItem : IDocumentTreeViewItem
    {
        private Fantasy.BusinessEngine.BusinessPackage _package;

        public ClassFolderTreeViewItem(Fantasy.BusinessEngine.BusinessPackage package)
        {
            // TODO: Complete member initialization
            this._package = package;
            this._childItems = new ObservableAdapterCollection<IDocumentTreeViewItem>(package.Classes, c => new ClassTreeViewItem((BusinessClass)c));
          
        }

        #region IDocumentTreeViewItem Members

        public string Name
        {
            get { return Resources.PakcageClassFolderName; }
        }

        private ImageSource _icon;

        public ImageSource Icon
        {
            get
            {
                if (_icon == null)
                {
                    _icon = new BitmapImage(new Uri("/Fantasy.Studio.BusinessEngine;component/images/folder.png", UriKind.Relative));
                }
                return _icon;
            }
        }


        private ObservableAdapterCollection<IDocumentTreeViewItem> _childItems;
      
        public IEnumerable<IDocumentTreeViewItem> ChildItems
        {
            get { return _childItems; }
        }

        private ContextMenu _contextMenu;
        public ContextMenu ContextMenu
        {
            get
            {
                if (this._contextMenu == null)
                {
                    this._contextMenu = ServiceManager.Services.GetRequiredService<IMenuService>().CreateContextMenu("fantasy/studio/businessengine/documentpad/classfolder/contextmenu", this._package, null);
                }

                return _contextMenu;
            }
        }

        public void Refresh()
        {
            
        }

        public void Open()
        {
            
        }

        #endregion
    }
}
