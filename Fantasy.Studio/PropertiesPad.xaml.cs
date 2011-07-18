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
using Fantasy.Adaption;
using System.ComponentModel;

namespace Fantasy.Studio
{
    /// <summary>
    /// Interaction logic for PropertiesPad.xaml
    /// </summary>
    public partial class PropertiesPad : UserControl, IPadContent
    {
        public PropertiesPad()
        {
            InitializeComponent();
        }

        #region IPadContent Members

        public string Title
        {
            get
            {
                return Properties.Resources.PropertiesPadTitle;
            }
        }

        private ImageSource _icon;
        public ImageSource Icon
        {
            get
            {
                if (_icon == null)
                {
                    _icon = new BitmapImage(new Uri("/Fantasy.Studio;component/images/Properties.ico", UriKind.Relative)); 
                }
                return _icon;
            }
        }

        public string Category { get; set; }

        private InputGestureCollection _inputGestures = new InputGestureCollection();
        public InputGestureCollection InputGestures
        {
            get { return _inputGestures; }
        }

        UIElement IPadContent.Content
        {
            get { return this; }
        }

        public void BringPadToFront()
        {
            
        }

        string IPadContent.Name { get { return "Properties"; } }

        #endregion

        #region IPadContent Members

        private IMonitorSelectionService _monitorSelectionService;
        private IAdapterManager _adapterManager;

        public void Initialize()
        {
            this._monitorSelectionService = ServiceManager.Services.GetRequiredService<IMonitorSelectionService>();
            this._monitorSelectionService.SelectionChanged += new EventHandler(MonitorSelectionServiceSelectionChanged);
            _adapterManager = ServiceManager.Services.GetRequiredService<IAdapterManager>();
            SetSelection();
        }

        void MonitorSelectionServiceSelectionChanged(object sender, EventArgs e)
        {
            SetSelection();
        }

        private void SetSelection()
        {
            if (this._monitorSelectionService.CurrentSelectionService != null)
            {
                object[] selected = this._monitorSelectionService.CurrentSelectionService.GetSelectedComponents().Cast<object>().Select(o => _adapterManager.GetAdapter(o, typeof(ICustomTypeDescriptor))).ToArray();
                this.PropertyGrid.SelectedObjects = selected;
            }
            else
            {
                this.PropertyGrid.SelectedObjects = null;
            }
        }

        #endregion
    }
}
