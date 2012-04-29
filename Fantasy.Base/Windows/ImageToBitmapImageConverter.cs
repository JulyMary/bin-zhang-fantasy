using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;

namespace Fantasy.Windows
{
    public class ImageToBitmapImageConverter : IValueConverter
    {

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return null;
            Image img = (Image)value;
            MemoryStream stream = new MemoryStream();
            img.Save(stream, img.RawFormat);


            BitmapImage rs = new BitmapImage(); 
            rs.BeginInit();
            rs.StreamSource = stream; 
            rs.EndInit(); 
            return rs;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
