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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            ToolBoxItemModel item = (ToolBoxItemModel)btn.DataContext;
            if (item.Click != null && item.Click.CanExecute(item.CommandParameter) )
            {
                item.Click.Execute(item.CommandParameter);
            }
        }

       
       

        private bool _startDraging = false;
        private void ListView_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ToolBoxItemModel itemModel = (ToolBoxItemModel)this.ListView.GetObjectAtPoint<ListViewItem>(e.GetPosition(this.ListView));

            if (itemModel != null && itemModel.Click != null && itemModel.Click.CanExecute(itemModel.CommandParameter))
            {
                itemModel.Click.Execute(itemModel.CommandParameter);
            }


            _startDraging = itemModel != null && itemModel.DoDragDrop != null;
            this.ListView.SelectedItem = itemModel;
        }

        private void ListView_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _startDraging = false;
        }

        private void ListView_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (_startDraging && e.LeftButton == MouseButtonState.Pressed)
                {

                    ToolBoxItemModel itemModel = (ToolBoxItemModel)this.ListView.SelectedItem;
                    DoDragDropEventArgs args = new DoDragDropEventArgs(itemModel.CommandParameter);
                    itemModel.DoDragDrop.HandleEvent(this.ListView, args);
                    if (args.AllowedEffects != DragDropEffects.None && args.Data != null)
                    {
                        DragDrop.DoDragDrop(this.ListView, args.Data, args.AllowedEffects);
                    }
                }

            }
            finally
            {
                _startDraging = false;
            }
        }

       


    }
}
