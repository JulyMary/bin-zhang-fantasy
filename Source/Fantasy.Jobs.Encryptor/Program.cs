using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Jobs;

namespace Fantasy.Jobs.Encryptor
{
    class Program
    {
        static void Main(string[] args)
        {

            foreach(string plainText in CommandArgumentsHelper.Arguments.GetValueOrDefault("e", new List<string>()))
            {
                Console.WriteLine("Plain: {0}", plainText);
                Console.WriteLine("Encrypted: {0}", Fantasy.Jobs.Encryption.Encrypt(plainText));
                Console.WriteLine("");
            }

            foreach(string encrypted in CommandArgumentsHelper.Arguments.GetValueOrDefault("d", new List<string>()))
            {
                Console.WriteLine("Encrypted: {0}", encrypted);

                try
                {
                    Console.WriteLine("Plain: {0}", Fantasy.Jobs.Encryption.Decrypt(encrypted));
                }
                catch
                {
                    Console.WriteLine("Invalid encrypted text."); 
                }
                Console.WriteLine("");
            }
        }

      
    }
}
