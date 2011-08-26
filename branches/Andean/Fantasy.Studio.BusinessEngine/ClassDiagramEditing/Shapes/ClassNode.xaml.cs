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
    /// Interaction logic for ClassNode.xaml
    /// </summary>
    public partial class ClassNode : Syncfusion.Windows.Diagram.Node
    {
        static ClassNode()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ClassNode), new FrameworkPropertyMetadata(typeof(ClassNode)));

        }


        public ClassNode()
        {
            InitializeComponent();

        }

       
    }
}
