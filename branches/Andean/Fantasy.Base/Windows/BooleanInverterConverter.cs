using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace Fantasy.Windows
{
    public class BooleanInverterConverter : IValueConverter
    {
        static  BooleanInverterConverter()
        {
            Default = new BooleanInverterConverter();
        }

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool)
            {
                return !(bool)value;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool)
            {
                return !(bool)value;
            }
            return value;
        }

        #endregion


        public static BooleanInverterConverter Default { get; private set; }
    } 

}
