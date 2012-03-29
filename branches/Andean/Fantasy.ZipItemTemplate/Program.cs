using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Ionic.Zip;

namespace Fantasy.ZipItemTemplate
{
    class Program
    {

        static void Main(string[] args)
        {
            string sourceDir = args[0];
            string destDir = args[1];

            if (!Directory.Exists(destDir))
            {
                Directory.CreateDirectory(destDir);
            }

            DirectoryInfo src = new DirectoryInfo(sourceDir);
            foreach (DirectoryInfo dir in src.EnumerateDirectories())
            {
                string targetName = Path.Combine(destDir, dir.Name + ".zip");
                ZipFile zip = new ZipFile();

                zip.AddDirectory(dir.FullName, "");

                zip.Save(targetName);
                
            }
           

        }

       

      
    }
}
