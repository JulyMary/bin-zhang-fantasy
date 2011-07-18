using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows;

namespace Fantasy.Windows
{
    public class VisibilityConverter : IValueConverter
    {
        private Visibility _falseValue;
        public VisibilityConverter(Visibility falseValue)
        {
            this._falseValue = falseValue; 
        }

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (System.Convert.ToBoolean(value))
            {
                return Visibility.Visible;
            }
            else
            {
                return _falseValue;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if((Visibility)value == Visibility.Visible )
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        public readonly static VisibilityConverter Hidden = new VisibilityConverter(Visibility.Hidden);

        public readonly static VisibilityConverter Collapsed = new VisibilityConverter(Visibility.Collapsed);

        public readonly static VisibilityConverter Visible = new VisibilityConverter(Visibility.Visible);
    }
}
