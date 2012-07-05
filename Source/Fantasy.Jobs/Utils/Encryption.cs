using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Fantasy.Jobs.Utils
{

    public static class Encryption
    {

        private const int encryptLength = 65536;
        private static byte[] key = {
		87,
		26,
		33,
		147,
		52,
		64,
		172,
		83,
		19,
		107,
		113,
		12,
		30,
		149,
		5,
		201
	};
        private static byte[] iv = {
		65,
		93,
		7,
		43,
		155,
		63,
		154,
		80,
		32,
		219,
		1,
		55,
		67,
		13,
		247,
		16
	};
        private static RijndaelManaged crypto = new System.Security.Cryptography.RijndaelManaged();


        public static byte[] EncryptData(byte[] data)
        {

            MemoryStream dataStream = new MemoryStream();
            crypto.Padding = PaddingMode.None;

            CryptoStream cryptoStream = new CryptoStream(dataStream, crypto.CreateEncryptor(key, iv), CryptoStreamMode.Write);
            BinaryWriter cryptoStreamWriter = new BinaryWriter(cryptoStream);

            cryptoStreamWriter.Write(data);

            if ((cryptoStreamWriter != null))
            {
                cryptoStreamWriter.Close();
            }

            if ((dataStream != null))
            {
                dataStream.Close();
            }

            data = dataStream.ToArray();

            return data;

        }

        public static byte[] DecryptData(byte[] data)
        {

           
            MemoryStream dataStream = new MemoryStream(data);

            crypto.Padding = PaddingMode.None;

            CryptoStream cryptoStream = new CryptoStream(dataStream, crypto.CreateDecryptor(key, iv), CryptoStreamMode.Read);
            BinaryReader cryptoStreamReader = new BinaryReader(cryptoStream);

            data = cryptoStreamReader.ReadBytes(data.Length);

            if ((dataStream != null))
            {
                dataStream.Close();
            }

            if ((cryptoStream != null))
            {
                cryptoStream.Close();
            }

            return data;

        }

        public static string EncryptString(string str)
        {

            byte[] data = Encoding.Unicode.GetBytes(str);

            byte[] encrypted = EncryptData(data);

            return Convert.ToBase64String(encrypted);

        }

        public static string DecryptString(string s)
        {

            byte[] data = Convert.FromBase64String(s);
            byte[] decrypted = DecryptData(data);

            return Encoding.Unicode.GetString(decrypted);


        }

       

    }

}
