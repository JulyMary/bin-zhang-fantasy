using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Drawing;
using System.Drawing;
using System.IO;

namespace ExportIcons
{
    class Program
    {
        static void Main(string[] args)
        {

            foreach (string ext in args)
            {
                foreach(IconReader.IconSize size in Enum.GetValues(typeof(IconReader.IconSize)))
                {
                    foreach(bool overlay in new bool[] {false, true})
                    {
                        string file = string.Format("{0}.{1}{2}.ico", ext, size, overlay ? ".overlay" : String.Empty);
                        Icon icon = IconReader.GetFileIcon("." + ext, size, overlay);
                        FileStream fs = new FileStream(file, FileMode.Create);
                        icon.Save(fs);
                        fs.Close();
                    }
                }
            }

        }
    }
}
