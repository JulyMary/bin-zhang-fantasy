using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.IO;
using System.IO;

namespace EncodingConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 3)
            {
                Console.WriteLine("EncodingConverter.exe DirectoryName SearchPattern OriginEncoding [DestinationEncoding]");
            }

            string dir = args[0];
            string pattern = args[1];
            Encoding origin = Encoding.GetEncoding(args[2]);
            Encoding dest = args.Length > 3 ? Encoding.GetEncoding(args[3]) : Encoding.Unicode;

            foreach (string file in LongPathDirectory.EnumerateAllFiles(dir, pattern))
            {
                Console.WriteLine(LongPath.GetRelativePath(dir + "\\", file));
                StreamReader reader = new StreamReader(file, origin, true);
                string content = reader.ReadToEnd();
                reader.Close();
                StreamWriter writer = new StreamWriter(file, false, dest);
                writer.Write(content);
                writer.Close();
            }


        }


       


    }
}
