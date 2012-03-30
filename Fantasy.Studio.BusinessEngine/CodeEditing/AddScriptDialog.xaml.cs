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

namespace Fantasy.Studio.BusinessEngine.CodeEditing
{
    /// <summary>
    /// Interaction logic for AddScriptDialog.xaml
    /// </summary>
    public partial class AddScriptDialog : Window
    {
        public AddScriptDialog()
        {
            InitializeComponent();
        }

        private void OKButtonClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void TemplatesListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            AddScriptDialogModel model = (AddScriptDialogModel)this.DataContext;
            if (model.IsValid)
            {
                this.DialogResult = true;
            }
        }

       



       
    }
}
