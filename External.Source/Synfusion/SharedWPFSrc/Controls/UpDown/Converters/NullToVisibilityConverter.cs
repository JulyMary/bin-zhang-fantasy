using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;
using System.Windows;
using System.Text;

namespace Syncfusion.Windows.Shared
{
    public class NullToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return Visibility.Visible;
            }
            else
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            //throw new NotImplementedException();
            return value;
        }
    }
}
