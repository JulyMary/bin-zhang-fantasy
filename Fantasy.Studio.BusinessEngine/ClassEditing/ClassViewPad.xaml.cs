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
using System.ComponentModel;
using Fantasy.Studio.Services;
using System.ComponentModel.Design;
using Fantasy.Studio.Controls;
using Fantasy.Adaption;
using Fantasy.Studio.Descriptor;

namespace Fantasy.Studio.BusinessEngine.ClassEditing
{
    /// <summary>
    /// Interaction logic for ClassViewPad.xaml
    /// </summary>
    public partial class ClassViewPad : UserControl, IObjectWithSite, IPadContent
    {
        public ClassViewPad()
        {
            InitializeComponent();
        }

        private ISelectionService _selectionService = null;


        protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            base.OnGotKeyboardFocus(e);
            IMonitorSelectionService monitor = this.Site.GetRequiredService<IMonitorSelectionService>();
            monitor.CurrentSelectionService = this._selectionService;
        }

        #region IPadContent Members

        private void treeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            ITreeViewItem item = (ITreeViewItem)this.treeView.SelectedItem;
            object data = this.Site.GetRequiredService<IAdapterManager>().GetAdapter<IReadOnlyAdapter>(item.DataContext);

            if (data == null)
            {
                data = item.DataContext;
            }

            _selectionService.SetSelectedComponents(new object[] { data }, SelectionTypes.Primary);
        }

        UIElement IPadContent.Content
        {
            get { return this; }
        }

        public string Title
        {
            get { return Properties.Resources.ClassViewPadTitle; }
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

        public string Category
        {
            get;
            set;
        }

        public void Initialize()
        {
            this._selectionService = new SelectionService(this.Site);
            ClassViewPadModel model = new ClassViewPadModel() { Site = this.Site };
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

        [Browsable(false)]
        public IServiceProvider Site { get; set; }

        string IPadContent.Name { get { return "ClassView"; } }

    }
}
