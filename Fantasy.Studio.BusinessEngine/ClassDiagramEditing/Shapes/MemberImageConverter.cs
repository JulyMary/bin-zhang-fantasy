using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace Fantasy.Studio.BusinessEngine.ClassDiagramEditing.Shapes
{
    public class MemberImageConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is Model.PropertyNode)
            {
                return "/Fantasy.Studio;component/images/Properties.ico";
            }
            else
            {
                return "/Fantasy.Studio.BusinessEngine;component/images/association.png";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}
