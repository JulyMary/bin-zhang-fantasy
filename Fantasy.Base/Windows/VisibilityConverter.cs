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
        public VisibilityConverter(Visibility falseValue, bool invert)
        {
            this._falseValue = falseValue;
            this._invert = invert;
        }

        #region IValueConverter Members

        private bool _invert = false;

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {

            bool b = System.Convert.ToBoolean(value);
            if (this._invert)
            {
                b = !b;
            }

            if (b)
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
            bool rs = (Visibility)value == Visibility.Visible;

            if (_invert)
            {
                rs = !rs;
            }

            return rs;
             
        }

        #endregion

        public readonly static VisibilityConverter Hidden = new VisibilityConverter(Visibility.Hidden, false);

        public readonly static VisibilityConverter Collapsed = new VisibilityConverter(Visibility.Collapsed, false);

        public readonly static VisibilityConverter InvertHidden = new VisibilityConverter(Visibility.Hidden, true);

        public readonly static VisibilityConverter InvertCollapsed = new VisibilityConverter(Visibility.Collapsed, true);

        public readonly static VisibilityConverter Visible = new VisibilityConverter(Visibility.Visible, false);
    }
}
