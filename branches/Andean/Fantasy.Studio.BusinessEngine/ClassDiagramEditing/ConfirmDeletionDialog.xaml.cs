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
using System.Windows.Shapes;

namespace Fantasy.Studio.BusinessEngine.ClassDiagramEditing
{
    /// <summary>
    /// Interaction logic for DeleteDialog.xaml
    /// </summary>
    public partial class ConfirmDeletionDialog : Window
    {
        public ConfirmDeletionDialog()
        {
            InitializeComponent();
        }

        public bool IsDeletingObject
        {
            get { return (bool)GetValue(IsDeletingObjectProperty); }
            set { SetValue(IsDeletingObjectProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsDeletingObject.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsDeletingObjectProperty =
            DependencyProperty.Register("IsDeletingObject", typeof(bool), typeof(ConfirmDeletionDialog), new UIPropertyMetadata(true));

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }



      

    }
}
