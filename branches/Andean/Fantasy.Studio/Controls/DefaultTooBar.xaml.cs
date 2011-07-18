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
using System.Windows.Threading;

namespace Fantasy.Studio.Controls
{
    /// <summary>
    /// Interaction logic for DefaultTooBar.xaml
    /// </summary>
    public partial class DefaultTooBar : System.Windows.Controls.ToolBar
    {
        public DefaultTooBar()
        {
            InitializeComponent();
        }

        private delegate void IvalidateMeasureJob();

        public override void OnApplyTemplate()
        {
            Dispatcher.BeginInvoke(new IvalidateMeasureJob(InvalidateMeasure), DispatcherPriority.Background, null);
            base.OnApplyTemplate();
        }


        public string Caption { get; set; }

        

    }
}
