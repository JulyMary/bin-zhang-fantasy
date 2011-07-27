using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows;

namespace Fantasy.Studio.Controls
{
    public class AutoGrayImage : Image
    {

        protected override void OnRender(System.Windows.Media.DrawingContext dc)
        {
            if (!this.IsEnabled && this.Source is BitmapSource)
            {
                BitmapSource imageSource = (BitmapSource)this.Source;
                BitmapSource greySource = new FormatConvertedBitmap((BitmapSource)imageSource, PixelFormats.Gray32Float, null, 0);
                ImageBrush opacityMask = new ImageBrush(imageSource);
                dc.PushOpacityMask(opacityMask);
                try
                {
                    dc.DrawImage(greySource, new Rect(new Point(), base.RenderSize));
                }
                finally
                {
                    dc.Pop();
                }
            }
            else
            {
                base.OnRender(dc);
            }
        }
    }
}
