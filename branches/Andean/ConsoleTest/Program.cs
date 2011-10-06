using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.AddIns;
using System.Xml;
using System.Xaml;
using System.Diagnostics;
using System.IO;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            FileStream fs = new FileStream(@"e:\Bin\Desktop\ClassLibrary1\ClassLibrary1.sln", FileMode.Open);
            BinaryReader r = new BinaryReader(fs);
            byte[] b = r.ReadBytes((int)fs.Length);
            fs.Close();


        }
    }
}
