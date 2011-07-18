using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Windows;
using Fantasy.BusinessEngine;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using Fantasy.ServiceModel;
using Fantasy.Studio.Services;
using System.Windows.Input;

namespace Fantasy.Studio.BusinessEngine
{
    public class ClassTreeViewItem : NotifyPropertyChangedObject, IDocumentTreeViewItem
    {
        public ClassTreeViewItem(BusinessClass @class)
        {
            this.Class = @class;
            this.Class.PropertyChanged += new EventHandler<Fantasy.BusinessEngine.Events.EntityPropertyChangedEventArgs>(Class_PropertyChanged);
        }

        void Class_PropertyChanged(object sender, Fantasy.BusinessEngine.Events.EntityPropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Name")
            {
                this.OnPropertyChanged("Name");
            }
        }

        public BusinessClass Class { get; private set; }

        public string Name
        {
            get { return this.Class.Name; }
        }

        public Guid Id
        {
            get
            {
                return this.Class.Id;
            }
        }

        public string FullName
        {
            get
            {
                return this.Class.FullName;
            }
        }

        public string CodeName
        {
            get
            {
                return this.Class.CodeName;
            }
        }

        public string FullCodeName
        {
            get
            {
                return this.Class.FullCodeName;
            }
        }

        private ImageSource _icon;

        public ImageSource Icon
        {
            get
            {
                if (_icon == null)
                {
                    _icon = new BitmapImage(new Uri("/Fantasy.Studio.BusinessEngine;component/images/class.png", UriKind.Relative));
                }
                return _icon;
            }
        }

        #region IDocumentTreeViewItem Members

        private static IDocumentTreeViewItem[] _children = new IDocumentTreeViewItem[0];


        public IEnumerable<IDocumentTreeViewItem> ChildItems
        {
            get { return _children; }
        }

        private ContextMenu _contextMenu;
        public ContextMenu ContextMenu
        {
            get
            {
                if (this._contextMenu == null)
                {
                    this._contextMenu = ServiceManager.Services.GetRequiredService<IMenuService>().CreateContextMenu("fantasy/studio/businessengine/documentpad/class/contextmenu", this.Class);
                }

                return _contextMenu;
            }
        }


        public void Refresh()
        {
            
        }

        public void Open()
        {
            IEditingService documentService = ServiceManager.Services.GetRequiredService<IEditingService>();
            
            documentService.OpenView(this.Class); 
        }

        #endregion
    }
}
