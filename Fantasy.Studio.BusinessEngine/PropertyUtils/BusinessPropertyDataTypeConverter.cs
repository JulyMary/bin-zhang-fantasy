using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using Fantasy.BusinessEngine;

namespace Fantasy.Studio.BusinessEngine
{
    class BusinessPropertyDataTypeConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            BusinessProperty prop = (BusinessProperty)value;
            if (prop.DataType == null)
            {
                return null;
            }
            else if (prop.DataType.Id == BusinessDataType.WellknownIds.Class)
            {
                return prop.DataClassType.FullName;
            }
            else if (prop.DataType.Id == BusinessDataType.WellknownIds.Enum)
            {
                return prop.DataEnumType.FullName;
            }
            else
            {
                return prop.DataType.Name;
            }
          
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}
