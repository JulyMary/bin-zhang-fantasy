using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Fantasy.BusinessEngine.Services
{
    public interface IImageListService 
    {

        void Register(string key, byte[] rawImage);

        void Register(string key, Func<byte[]> _imageLoader);

        byte[] GetRawImage(string key);

        bool ContainsImage(string key);
    }
}
