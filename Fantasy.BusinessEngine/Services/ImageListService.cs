using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.ServiceModel;
using System.Drawing;

namespace Fantasy.BusinessEngine.Services
{
    public class ImageListService : ServiceBase, IImageListService
    {
        #region IImageListService Members

        public void Register(string key, Image image)
        {
            this._images.Add(key, image);
        }

        public void Register(string key, Func<Image> imageLoader)
        {
            this._images.Add(key, imageLoader);

            
        }

        public Image GetImage(string key)
        {
            object img = this._images[key];
            if(img is Image)
            {
                return (Image)img;
            }
            else
            {
               Image raw = ((Func<Image>)img)(); 
               this._images[key] = raw;
               return raw;
            }
        
        }

        public bool ContainsImage(string key)
        {
            return this._images.ContainsKey(key);
        }

        #endregion


        Dictionary<string,object> _images = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);



        
    }
}
