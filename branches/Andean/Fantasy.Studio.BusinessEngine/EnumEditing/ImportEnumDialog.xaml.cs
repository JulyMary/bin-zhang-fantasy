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

namespace Fantasy.Studio.BusinessEngine.EnumEditing
{
    /// <summary>
    /// Interaction logic for ImportEnumDialog.xaml
    /// </summary>
    public partial class ImportEnumDialog : Window
    {
        public ImportEnumDialog()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void EnumsTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            ImportEnumDialogModel model = (ImportEnumDialogModel)this.DataContext;
            model.SelectedEnum = this.EnumsTreeView.SelectedItem as EnumNode;
        }
    }
}
