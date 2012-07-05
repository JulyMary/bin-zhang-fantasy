using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using Fantasy.IO;

namespace Fantasy.Jobs.Utils
{
    public static class MD5HashCodeHelper
    {
        public static string GetMD5HashCode(Stream stream)
        {
            string rs;
            using (MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider())
            {

                byte[] bytes = provider.ComputeHash(stream);
                rs = BitConverter.ToString(bytes).Replace("-", "");
            }
           
            return rs;
        }

        public static string GetMD5HashCode(string filePath)
        {
            string rs;
            FileStream stream = LongPathFile.Open(filePath, FileMode.Open, FileAccess.Read);
            try
            {
                rs = GetMD5HashCode(stream);
            }
            finally
            {
                stream.Close();
            }
            return rs;
        }
    }
}
