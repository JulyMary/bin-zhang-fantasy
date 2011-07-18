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
using Fantasy.Studio.Services;
using Fantasy.ServiceModel;
using System.ComponentModel.Design;

namespace Fantasy.Studio.BusinessEngine
{
    /// <summary>
    /// Interaction logic for PackagePad.xaml
    /// </summary>
    public partial class DocumentPad : UserControl, IPadContent
    {
        public DocumentPad()
        {
            InitializeComponent();
        }


        private ISelectionService _selectionService = null;


        protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            base.OnGotKeyboardFocus(e);
            IMonitorSelectionService monitor = ServiceManager.Services.GetRequiredService<IMonitorSelectionService>();
            monitor.CurrentSelectionService = this._selectionService;
        }

        #region IPadContent Members

        private void treeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            object tvi = this.treeView.SelectedItem;

            _selectionService.SetSelectedComponents(new object[] { tvi }, SelectionTypes.Primary);
        }

        UIElement IPadContent.Content
        {
            get { return this; }
        }

        public string Title
        {
            get { return Properties.Resources.DocumentPadTitle; }
        }

        private ImageSource _icon;
        public ImageSource Icon
        {
            get
            {
                if (_icon == null)
                {
                    _icon = new BitmapImage(new Uri("/Fantasy.Studio.BusinessEngine;component/images/model.ico", UriKind.Relative));
                }
                return _icon;
            }
        }

        public string Category
        {
            get;
            set;
        }

        public void Initialize()
        {
            this._selectionService = new SelectionService(ServiceManager.Services);
            DocumentPadModel model = new DocumentPadModel();
            this.DataContext = model;
        }

        private InputGestureCollection _inputGestures = new InputGestureCollection();
        public InputGestureCollection InputGestures
        {
            get { return _inputGestures; }
        }

        public void BringPadToFront()
        {
            
        }

        #endregion

        private void TreeViewItemDoubleClick(object sender, MouseEventArgs e)
        {

                TreeViewItem item = (TreeViewItem)e.Source;

                TreeViewItem origin = FindTreeViewItem((DependencyObject)e.OriginalSource);
                if (origin != null && item == origin)
                {
                    IDocumentTreeViewItem data = (IDocumentTreeViewItem)item.DataContext;
                    data.Open();
                   
                }

        }


        private TreeViewItem FindTreeViewItem(DependencyObject source)
        {
            if (source == null)
            {
                return null;
            }
            if (source is TreeViewItem)
            {
                return (TreeViewItem)source;
            }

            return FindTreeViewItem(VisualTreeHelper.GetParent(source));
        }

       

       
    }
}
