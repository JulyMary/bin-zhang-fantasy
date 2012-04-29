using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;

namespace Fantasy.BusinessEngine.Services
{
    public interface IImageListService 
    {

        void Register(string key, Image image);

        void Register(string key, Func<Image> _imageLoader);




        Image GetImage(string key);

       

        string GetFolderKey(string key);

       

        bool ContainsImage(string key);
    }
}
