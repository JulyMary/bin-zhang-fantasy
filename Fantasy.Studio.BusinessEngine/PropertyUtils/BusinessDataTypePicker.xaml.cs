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
using Fantasy.BusinessEngine;
using Fantasy.Studio.TreeViewModel;

namespace Fantasy.Studio.BusinessEngine
{
    /// <summary>
    /// Interaction logic for BusinessDataTypePicker.xaml
    /// </summary>
    public partial class BusinessDataTypePicker : Window
    {
        public BusinessDataTypePicker()
        {
            InitializeComponent();
            //this.DialogResult = false;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

       

        private void extendableTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            ITreeViewItem item = (ITreeViewItem)e.NewValue;
            IBusinessEntity entity = item != null ? (IBusinessEntity)item.DataContext : null;
            ((BusinessDataTypePikcerModel)this.DataContext).SelectedItem = entity;
        }

        private void extendableTreeView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (this.OkButton.IsEnabled)
            {
                this.DialogResult = true;
            }
        }

    }
}
