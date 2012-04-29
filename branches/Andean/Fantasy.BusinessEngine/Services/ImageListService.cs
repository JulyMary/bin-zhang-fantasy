using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.ServiceModel;
using System.Drawing;
using Fantasy.BusinessEngine.Properties;

namespace Fantasy.BusinessEngine.Services
{
    public class ImageListService : ServiceBase, IImageListService
    {

        private object _syncRoot = new object();
        public void Register(string key, Image image)
        {
            lock (this._syncRoot)
            {
                this._images.Add(key, image);
            }
        }

        public void Register(string key, Func<Image> imageLoader)
        {
            lock (this._syncRoot)
            {
                this._images.Add(key, imageLoader);
            }

            
        }

        public Image GetImage(string key)
        {
            lock (this._syncRoot)
            {
                object img = this._images[key];
                if (img is Image)
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
        
        }

        public bool ContainsImage(string key)
        {
            lock (_syncRoot)
            {
                return this._images.ContainsKey(key);
            }
        }

        Dictionary<string,object> _images = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        public string GetFolderKey(string key)
        {
            string rs =  key + "__folder";

            lock (_syncRoot)
            {
                if (!this.ContainsImage(rs))
                {
                    Image content = this.GetImage(key);
                    Bitmap folder = (Bitmap)Resources.FolderTemplate.Clone();
                    using (Graphics g = Graphics.FromImage(folder))
                    {
                        g.DrawImage(content, new RectangleF(10, 10, 32, 32));
                    }

                    this._images[rs] = folder;
                   
                }
            }

            return rs;
            
        }

      
    }
}
