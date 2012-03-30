using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using Fantasy.Drawing;
using System.Diagnostics;
using System.IO;

namespace Fantasy.Windows
{
    public static class CurrentProcessIcon
    {
        private static BitmapSource _icon = null;
        public static BitmapSource Icon
        {
            get
            {
                if (_icon == null)
                {
                    System.Drawing.Icon ico = System.Drawing.Icon.ExtractAssociatedIcon(Process.GetCurrentProcess().MainModule.FileName);
                    _icon = System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(ico.Handle, System.Windows.Int32Rect.Empty, System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
                }
                return _icon;
            }
        }
    }
}
