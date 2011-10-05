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
using Fantasy.Studio.Controls;

namespace Fantasy.Studio
{
    /// <summary>
    /// Interaction logic for OptionsWindow.xaml
    /// </summary>
    public partial class OptionsWindow : Window
    {
        public OptionsWindow()
        {
            InitializeComponent();
            this.Owner = Application.Current.MainWindow;
            OptionsWindowModel model = new OptionsWindowModel();
            this.DataContext = model;
        }

        private void OptionTreeView_Loaded(object sender, RoutedEventArgs e)
        {
           
               
           
        }

        private void OptionTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            OptionNode node = e.NewValue as OptionNode;
            if (node != null)
            {
                OptionsWindowModel model = (OptionsWindowModel)this.DataContext;
                model.SetSelectedNode(node);
            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            OptionsWindowModel model = (OptionsWindowModel)this.DataContext;
            model.Save();
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            OptionsWindowModel model = (OptionsWindowModel)this.DataContext;
            model.Save();
        }
    }
}
