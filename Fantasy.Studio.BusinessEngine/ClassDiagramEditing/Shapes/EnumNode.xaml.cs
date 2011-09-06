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

namespace Fantasy.Studio.BusinessEngine.ClassDiagramEditing.Shapes
{
    /// <summary>
    /// Interaction logic for EnumNode.xaml
    /// </summary>
    public partial class EnumNode : UserControl
    {
        public EnumNode()
        {
            InitializeComponent();
        }

        private void EnumValueListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
