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

namespace Fantasy.Studio
{
    /// <summary>
    /// Interaction logic for ToolBoxPad.xaml
    /// </summary>
    public partial class ToolBoxPad : UserControl, IPadContent
    {
        public ToolBoxPad()
        {
            InitializeComponent();
        }

        public string Title
        {
            get
            {
                return Properties.Resources.ToolBoxPadTitle;
            }
        }

        private ImageSource _icon;
        public ImageSource Icon
        {
            get
            {
                if (_icon == null)
                {
                    _icon = new BitmapImage(new Uri("/Fantasy.Studio;component/images/toolbox.png", UriKind.Relative));
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

        string IPadContent.Name { get { return "ToolBox"; } }

        [Browsable(false)]
        public IServiceProvider Site { get; set; }

        #region IPadContent Members


        public void Initialize()
        {
            
        }

        #endregion
    }
}
