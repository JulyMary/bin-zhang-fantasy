﻿using System;
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

namespace Fantasy.Studio.Controls
{
    /// <summary>
    /// Interaction logic for ToolBox.xaml
    /// </summary>
    public partial class ToolBox : UserControl
    {
        public ToolBox()
        {
            InitializeComponent();
            
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (ToolBoxItemModel item in e.RemovedItems)
            {
                if (item.Unselected != null && item.Unselected.CanExecute(item.CommandParameter))
                {
                    item.Unselected.Execute(item.CommandParameter);
                }
            }

            foreach (ToolBoxItemModel item in e.AddedItems)
            {
                if (item.Selected != null && item.Selected.CanExecute(item.CommandParameter))
                {
                    item.Selected.Execute(item.CommandParameter);
                }
            }
        }

        

       
        
    }
}
