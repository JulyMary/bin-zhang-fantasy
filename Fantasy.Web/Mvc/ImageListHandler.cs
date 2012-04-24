using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using Fantasy.BusinessEngine.Services;
using Fantasy.BusinessEngine;
using System.Drawing;
using System.Drawing.Imaging;

namespace Fantasy.Web.Mvc
{
    public class ImageListHandler : IHttpHandler
    {
        public ImageListHandler(RequestContext requestContext)
        {
            this.RequestContext = requestContext;
        }


        public RequestContext RequestContext { get; private set; }

        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            string imageKey = this.RequestContext.RouteData.GetRequiredString("key");

            IImageListService imageList = BusinessEngineContext.Current.GetRequiredService<IImageListService>();
            Image image = imageList.GetImage(imageKey);


            string extension = this.GetExtension(image);
            if (!string.IsNullOrEmpty(extension))
            {
                context.Response.ContentType = MIMETypes.GetMIMETypeForExtension(extension);
                image.Save(context.Response.OutputStream, image.RawFormat);
            }
            else
            {
                context.Response.ContentType = MIMETypes.GetMIMETypeForExtension(".png");
                image.Save(context.Response.OutputStream, ImageFormat.Png);
            }

            
        }



        private string GetExtension(Image image)
        {
            ImageFormat format = image.RawFormat;

          
            if (format == ImageFormat.Bmp)
            {
                return ".bmp";
            }
            if (format == ImageFormat.Emf)
            {
                return ".emf";
            }
            if (format == ImageFormat.Wmf)
            {
                return ".wmf";
            }
            if (format == ImageFormat.Gif)
            {
                return "gif";
            }
            if (format == ImageFormat.Jpeg)
            {
                return ".jpeg";
            }
            if (format == ImageFormat.Png)
            {
                return ".png";
            }
            if (format == ImageFormat.Tiff)
            {
                return "tiff";
            }
            if (format == ImageFormat.Exif)
            {
                return ".exif";
            }
            if (format == ImageFormat.Icon)
            {
                return "ico";
            }

            return null;

        }
    }
}