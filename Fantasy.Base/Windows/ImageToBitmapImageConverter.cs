using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.IO;
using System.Windows.Media.Imaging;


namespace Fantasy.Windows
{
    public class ImageToBitmapImageConverter : IValueConverter
    {

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                System.Drawing.Image img = (System.Drawing.Image)value;
                MemoryStream ms = new MemoryStream();
                img.Save(ms, img.RawFormat);
                BitmapImage rs = new BitmapImage();
                rs.BeginInit();
                rs.StreamSource = ms;
                rs.EndInit();
                return rs;
            }
            else
            {
                return null;
            }
           
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
