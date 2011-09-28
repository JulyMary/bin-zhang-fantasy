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
using Fantasy.Windows;
using Fantasy.Studio.Controls;
using Fantasy.BusinessEngine;

namespace Fantasy.Studio.BusinessEngine.ObjectSecurityEditing
{
    /// <summary>
    /// Interaction logic for ObjectSecurityEditor.xaml
    /// </summary>
    public partial class ObjectSecurityEditor : UserControl
    {
        public ObjectSecurityEditor()
        {
            InitializeComponent();
        }


        private bool _updating = false;

        public void SaveLayout()
        {
            GridViewLayoutSetting layout = new GridViewLayoutSetting();
            layout.SaveLayout(this.PropertyGridView);
            Properties.Settings.Default.ObjectSecurityPropertyGridViewLayout = layout;

        }

       

        private void PropertyWriteChecked(object sender, RoutedEventArgs e)
        {
            if (!this._updating)
            {
                this._updating = true;
                try
                {
                    CheckBox checkBox = (CheckBox)sender;

                    foreach (BusinessObjectMemberSecurity prop in this.PropertyListView.SelectedItems)
                    {
                        prop.CanWrite = checkBox.IsChecked;
                    }
                }
                finally
                {
                    this._updating = false;
                }
            }
        }

        private void PropertyReadChecked(object sender, RoutedEventArgs e)
        {
            if (!this._updating)
            {
                this._updating = true;
                try
                {
                    CheckBox checkBox = (CheckBox)sender;

                    foreach (BusinessObjectMemberSecurity prop in this.PropertyListView.SelectedItems)
                    {
                        prop.CanRead = checkBox.IsChecked;
                    }
                }
                finally
                {
                    this._updating = false;
                }
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.ObjectSecurityPropertyGridViewLayout.LoadLayout(this.PropertyGridView);
        }

        private void PropertyListView_SelectAll(object sender, ExecutedRoutedEventArgs e)
        {
            this.PropertyListView.SelectAll();
        }

       
    }
}
