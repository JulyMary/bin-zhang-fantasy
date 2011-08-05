﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Data;
using System.Collections;

namespace Syncfusion.Windows.Tools.Controls
{
    public class MultiLineConverter : IValueConverter
    {

        #region IValueConverter Members


        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (!(String.IsNullOrEmpty(value as string)))
            {
                var items = new List<FrameworkElement>();
                var sender = parameter as IButtonAdv;
                string label = value.ToString(), text1 = String.Empty, text2 = String.Empty;

                int index = label.IndexOf(' ');
                if (index != -1)
                {
                    text1 = label.Substring(0, index);
                    text2 = label.Substring(index, label.Length - index);
                }
                else
                {
                    text1 = label.Substring(0, label.Length);
                }

                if (sender != null)
                {
                    if (sender.IsMultiLine && index != -1)
                    {
                        var text_1 = new TextBlock() { HorizontalAlignment = HorizontalAlignment.Center };
                        text_1.Text = text1.TrimStart().TrimEnd();
                        items.Add(text_1);

                        var text_2 = new TextBlock();

                        text_2.Text = text2.Trim();

                        if (!(sender is DropDownButtonAdv))
                        {
                            text_1.Foreground = ((ButtonAdv)sender).Foreground;
                            text_1.FontFamily = ((ButtonAdv)sender).FontFamily;
                            text_1.FontSize = ((ButtonAdv)sender).FontSize;
                            text_2.Foreground = ((ButtonAdv)sender).Foreground;
                            items.Add(text_2);
                        }
                        else
                        {
                            if (sender is DropDownButtonAdv)
                            {
                                var dropdown = sender as DropDownButtonAdv;
                                StackPanel panel = new StackPanel() { Orientation = Orientation.Horizontal };
                                text_2.Foreground = ((DropDownButtonAdv)sender).Foreground;
                                text_1.Foreground = ((DropDownButtonAdv)sender).Foreground;
                                //panel.Children.Add(text_1);
                                panel.Children.Add(text_2);
                                TextBlock text_arrow = new TextBlock() { Text = "6", FontFamily = new FontFamily("Webdings"), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center, FontSize = 14 };
                                text_arrow.Foreground = ((DropDownButtonAdv)sender).Foreground;
                                text_arrow.FontSize = ((DropDownButtonAdv)sender).FontSize;
                                panel.Children.Add(text_arrow);
                                items.Add(panel);
                            }
                        }
                    }
                    else
                    {
                        var text = new TextBlock();
                        text.Text = label;
                        if (sender is ButtonAdv)
                            text.Foreground = ((ButtonAdv)sender).Foreground;
                        else if (sender is DropDownButtonAdv)
                            text.Foreground = ((DropDownButtonAdv)sender).Foreground;
                        items.Add(text);
                        if (sender is DropDownButtonAdv)
                        {
                            TextBlock text_arrow = new TextBlock() { Text = "6", FontFamily = new FontFamily("Webdings"), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center, FontSize = 14 };
                            text_arrow.Foreground = ((DropDownButtonAdv)sender).Foreground;
                            text_arrow.FontSize = ((DropDownButtonAdv)sender).FontSize;
                            items.Add(text_arrow);
                        }
                        else
                        {

                            var text_2 = new TextBlock() { Text = " " };
                            text_2.Foreground = ((ButtonAdv)sender).Foreground;
                            items.Add(text_2);
                        }
                    }
                }
                return items;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }


        #endregion
    }
}
