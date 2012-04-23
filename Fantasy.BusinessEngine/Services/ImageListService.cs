using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.ServiceModel;

namespace Fantasy.BusinessEngine.Services
{
    public class ImageListService : ServiceBase, IImageListService
    {
        #region IImageListService Members

        public void Register(string key, byte[] rawImage)
        {
            this._images.Add(key, rawImage);
        }

        public void Register(string key, Func<byte[]> imageLoader)
        {
            this._images.Add(key, imageLoader);

            
        }

        public byte[] GetRawImage(string key)
        {
            object img = this._images[key];
            if(img is byte[])
            {
                return (byte[])img;
            }
            else
            {
                byte[] raw = ((Func<byte[]>)img)(); 
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
